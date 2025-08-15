export type CacheKey = string;

const cache = new Map<CacheKey, any>();

function buildKey(url: string, options?: RequestInit): CacheKey {
  return `${url}:${JSON.stringify(options || {})}`;
}

export async function apiGet<T>(url: string, options?: RequestInit): Promise<T> {
  const key = buildKey(url, options);
  if (cache.has(key)) {
    return cache.get(key);
  }
  const res = await fetch(url, {
    credentials: 'include',
    headers: { 'Content-Type': 'application/json', ...(options?.headers || {}) },
    ...options,
    method: 'GET'
  });
  if (!res.ok) throw new Error(res.statusText);
  const data = await res.json();
  cache.set(key, data);
  return data as T;
}

export async function apiPost<T>(url: string, body: any, options?: RequestInit): Promise<T> {
  const res = await fetch(url, {
    credentials: 'include',
    headers: { 'Content-Type': 'application/json', ...(options?.headers || {}) },
    body: JSON.stringify(body),
    ...options,
    method: 'POST'
  });
  if (!res.ok) throw new Error(res.statusText);
  invalidate(url);
  return res.json();
}

export function invalidate(url?: string) {
  if (!url) {
    cache.clear();
    return;
  }
  [...cache.keys()].forEach(key => {
    if (key.startsWith(url)) cache.delete(key);
  });
}
