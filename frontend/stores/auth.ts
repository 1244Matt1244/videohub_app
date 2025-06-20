import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(null)
  const darkMode = ref(false)

  function login(email: string, password: string) {
    // Implementiraj login koji postavlja token, npr. fetch + save token
    // Za primjer:
    token.value = 'fake-jwt-token'
  }

  function logout() {
    token.value = null
  }

  function toggleDarkMode() {
    darkMode.value = !darkMode.value
    if (darkMode.value) {
      document.documentElement.classList.add('dark')
      localStorage.setItem('dark-mode', 'true')
    } else {
      document.documentElement.classList.remove('dark')
      localStorage.setItem('dark-mode', 'false')
    }
  }

  return { token, darkMode, login, logout, toggleDarkMode }
})
