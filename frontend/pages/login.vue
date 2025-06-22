<template>
  <div class="p-6 max-w-md mx-auto">
    <h2 class="text-2xl mb-4">Login</h2>
    <form @submit.prevent="handleLogin">
      <input v-model="email" type="email" placeholder="Email" class="input input-bordered w-full mb-3" />
      <input v-model="password" type="password" placeholder="Password" class="input input-bordered w-full mb-3" />
      <button class="btn btn-primary w-full">Login</button>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '~/stores/auth'

const email = ref('')
const password = ref('')
const auth = useAuthStore()
const router = useRouter()

async function handleLogin() {
  try {
    await auth.login(email.value, password.value)
    router.push('/dashboard')
  } catch (err) {
    alert('Login failed!')
  }
}
</script>
