<template>
  <div class="min-h-screen flex items-center justify-center bg-base-200">
    <div class="card w-96 bg-base-100 shadow-xl">
      <div class="card-body">
        <h2 class="card-title text-2xl mb-4">Welcome to VideoApp</h2>

        <div class="tabs tabs-boxed mb-4">
          <a class="tab" :class="{ 'tab-active': mode === 'login' }" @click="mode = 'login'">Login</a>
          <a class="tab" :class="{ 'tab-active': mode === 'register' }" @click="mode = 'register'">Register</a>
        </div>

        <input v-if="mode === 'register'" v-model="fullName" type="text"
               placeholder="Full Name" class="input input-bordered w-full mb-3" />
        <input v-model="email" type="email" placeholder="Email" class="input input-bordered w-full mb-3" />
        <input v-model="password" type="password" placeholder="Password" class="input input-bordered w-full mb-3" />

        <div v-if="error" class="alert alert-error text-sm mb-3">{{ error }}</div>

        <button class="btn btn-primary w-full" :disabled="loading" @click="submit">
          <span v-if="loading" class="loading loading-spinner"></span>
          {{ mode === 'login' ? 'Login' : 'Register' }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const auth = useAuthStore();
const email = ref('');
const password = ref('');
const fullName = ref('');
const mode = ref<'login' | 'register'>('login');
const error = ref('');
const loading = ref(false);

const submit = async () => {
  error.value = '';
  loading.value = true;
  try {
    if (mode.value === 'login') {
      await auth.login(email.value, password.value);
    } else {
      await auth.register(email.value, password.value, fullName.value);
    }
    await navigateTo('/dashboard');
  } catch (e: any) {
    error.value = e?.data?.error || 'Authentication failed';
  } finally {
    loading.value = false;
  }
};
</script>
