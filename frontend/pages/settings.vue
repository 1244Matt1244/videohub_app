<template>
  <div class="p-6 max-w-lg mx-auto">
    <h1 class="text-2xl mb-4">Settings</h1>
    <button class="btn btn-outline w-full" @click="auth.toggleDarkMode()">
      Toggle {{ auth.darkMode ? 'Light' : 'Dark' }} Mode
    </button>
    <button class="btn btn-error mt-4 w-full" @click="auth.logout(); router.push('/login')">
      Logout
    </button>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '~/stores/auth'
const auth = useAuthStore()
const router = useRouter()
</script>
