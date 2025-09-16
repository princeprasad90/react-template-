import React, { ReactNode } from 'react';
import { create } from 'zustand';

export interface AuthTokens {
  accessToken: string;
  refreshToken: string;
}

export interface UserProfile {
  id: string;
  name: string;
  email: string;
}

export interface MenuItem {
  title: string;
  url: string;
}

interface CurrentUserResponse {
  profile: UserProfile;
  menu?: MenuItem[];
}

interface AuthState {
  tokens: AuthTokens | null;
  profile: UserProfile | null;
  menu: MenuItem[];
  setTokens: (tokens: AuthTokens | null) => void;
  setProfile: (profile: UserProfile | null) => void;
  setMenu: (menu: MenuItem[]) => void;
  fetchCurrentUser: () => Promise<void>;
  login: (access: string, refresh: string) => Promise<void>;
  logout: () => void;
}

export const useAuthStore = create<AuthState>((set, get) => ({
  tokens: null,
  profile: null,
  menu: [],
  setTokens: tokens => set({ tokens }),
  setProfile: profile => set({ profile }),
  setMenu: menu => set({ menu }),
  fetchCurrentUser: async () => {
    const { tokens } = get();
    if (!tokens?.accessToken) {
      throw new Error('Missing access token');
    }

    const res = await fetch('/api/users/me', {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${tokens.accessToken}`
      }
    });

    if (!res.ok) {
      throw new Error('Failed to fetch user information');
    }

    const data: CurrentUserResponse = await res.json();
    set({
      profile: data.profile,
      menu: data.menu ?? []
    });
  },
  login: async (access, refresh) => {
    set({ tokens: { accessToken: access, refreshToken: refresh } });
    try {
      await get().fetchCurrentUser();
    } catch (error) {
      set({ tokens: null, profile: null, menu: [] });
      throw error;
    }
  },
  logout: () => {
    set({ tokens: null, profile: null, menu: [] });
  }
}));

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => <>{children}</>;

export const useAuth = () => {
  const { tokens, profile, menu, login, logout } = useAuthStore(state => ({
    tokens: state.tokens,
    profile: state.profile,
    menu: state.menu,
    login: state.login,
    logout: state.logout
  }));

  const loggedIn = !!tokens;
  const accessToken = tokens?.accessToken ?? null;
  const refreshToken = tokens?.refreshToken ?? null;

  return {
    loggedIn,
    accessToken,
    refreshToken,
    user: profile,
    menu,
    login,
    logout
  };
};
