import { defineStore } from 'pinia';

interface User {
  id: string;
  email: string;
  fullName: string;
  isPremium: boolean;
  darkMode: boolean;
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: null as string | null,
    user: null as User | null,
  }),
  getters: {
    isAuthenticated: (state) => !!state.token,
    isPremium: (state) => state.user?.isPremium ?? false,
  },
  actions: {
    init() {
      if (import.meta.client) {
        const token = localStorage.getItem('token');
        const user = localStorage.getItem('user');
        if (token) this.token = token;
        if (user) this.user = JSON.parse(user);
      }
    },
    async login(email: string, password: string) {
      const { apiBase } = useRuntimeConfig().public;
      const res = await $fetch<{ token: string; user: User }>('/api/auth/login', {
        baseURL: apiBase,
        method: 'POST',
        body: { email, password },
      });
      this.setAuth(res.token, res.user);
    },
    async register(email: string, password: string, fullName: string) {
      const { apiBase } = useRuntimeConfig().public;
      const res = await $fetch<{ token: string; user: User }>('/api/auth/register', {
        baseURL: apiBase,
        method: 'POST',
        body: { email, password, fullName },
      });
      this.setAuth(res.token, res.user);
    },
    async googleLogin(idToken: string) {
      const { apiBase } = useRuntimeConfig().public;
      const res = await $fetch<{ token: string; user: User }>('/api/auth/google', {
        baseURL: apiBase,
        method: 'POST',
        body: { idToken },
      });
      this.setAuth(res.token, res.user);
    },
    setAuth(token: string, user: User) {
      this.token = token;
      this.user = user;
      if (import.meta.client) {
        localStorage.setItem('token', token);
        localStorage.setItem('user', JSON.stringify(user));
      }
    },
    logout() {
      this.token = null;
      this.user = null;
      if (import.meta.client) {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
      }
    },
  },
});
