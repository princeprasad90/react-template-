import React, { createContext, useContext, useState, ReactNode } from 'react';

interface AuthContextValue {
  loggedIn: boolean;
  accessToken: string | null;
  refreshToken: string | null;
  login: (access: string, refresh: string) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

let tokensRef: { accessToken: string; refreshToken: string } | null = null;
export const getAuthTokens = () => tokensRef;
export const setAuthTokens = (tokens: { accessToken: string; refreshToken: string } | null) => {
  tokensRef = tokens;
};

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [tokens, setTokens] = useState<{ accessToken: string; refreshToken: string } | null>(null);

  const login = (access: string, refresh: string) => {
    const t = { accessToken: access, refreshToken: refresh };
    setTokens(t);
    setAuthTokens(t);
  };
  const logout = () => {
    setTokens(null);
    setAuthTokens(null);
  };

  return (
    <AuthContext.Provider value={{
      loggedIn: !!tokens,
      accessToken: tokens?.accessToken || null,
      refreshToken: tokens?.refreshToken || null,
      login,
      logout
    }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used within AuthProvider');
  return ctx;
};
