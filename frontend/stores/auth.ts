import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null as null | { email: string },
    token: '',
    darkMode: false
  }),
  actions: {
    async login(email: string, password: string) {
      const { data, error } = await useFetch('/api/auth/login', {
        method: 'POST',
        body: { email, password }
      })

      if (error.value) throw new Error('Login failed')
      this.token = data.value.token
      this.user = { email }
    },
    toggleDarkMode() {
      this.darkMode = !this.darkMode
    },
    logout() {
      this.user = null
      this.token = ''
    }
  }
})
