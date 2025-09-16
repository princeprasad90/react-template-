import { AuthTokens, useAuthStore } from './store/auth';

export async function api<T>(url: string, options: RequestInit = {}, retry = true): Promise<T> {
  const { tokens, setTokens } = useAuthStore.getState();
  const res = await fetch(url, {
    headers: {
      'Content-Type': 'application/json',
      ...(tokens?.accessToken ? { Authorization: `Bearer ${tokens.accessToken}` } : {}),
      ...(options.headers || {})
    },
    ...options
  });

  if (res.status === 401 && retry && tokens?.refreshToken) {
    const refreshRes = await fetch('/api/auth/refresh', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ refreshToken: tokens.refreshToken })
    });
    if (refreshRes.ok) {
      const newTokens: AuthTokens = await refreshRes.json();
      setTokens(newTokens);
      return api<T>(url, options, false);
    }
  }

  if (!res.ok) throw new Error(res.statusText);
  const text = await res.text();
  return text ? JSON.parse(text) : (undefined as unknown as T);
}
