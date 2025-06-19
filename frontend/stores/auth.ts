import { defineStore } from 'pinia'

interface User {
  email: string
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null as User | null,
    token: '',
    darkMode: false
  }),

  actions: {
    async login(email: string, password: string) {
      const config = useRuntimeConfig()
      const { data, error } = await useFetch<{ token: string }>('/api/auth/login', {
        baseURL: config.public.apiBase,
        method: 'POST',
        body: { email, password }
      })

      if (error.value) {
        console.error('Login failed:', error.value)
        throw createError({ statusCode: 401, statusMessage: 'Unauthorized' })
      }

      if (!data.value?.token) {
        throw createError({ statusCode: 500, statusMessage: 'No token returned from server.' })
      }

      this.token = data.value.token
      this.user = { email }

      // Optional: persist token in localStorage (only on client)
      if (process.client) {
        localStorage.setItem('auth_token', this.token)
      }
    },

    logout() {
      this.user = null
      this.token = ''

      if (process.client) {
        localStorage.removeItem('auth_token')
      }
    },

    toggleDarkMode() {
      this.darkMode = !this.darkMode
    },

    initializeFromLocalStorage() {
      if (process.client) {
        const token = localStorage.getItem('auth_token')
        if (token) {
          this.token = token
          // Optional: call backend to fetch user info
        }
      }
    }
  }
})
