import { defineStore } from 'pinia'
export const useAuthStore = defineStore('auth', {
  state: () => ({ user: null, token: '', darkMode: false }),
  actions: {
    async login(email, pwd) {
      const { data } = await useFetch('/api/auth/login', {method:'POST', body:{email, pwd}})
      this.token = data.value.token
    },
    toggleDarkMode() { this.darkMode = !this.darkMode }
  }
})
