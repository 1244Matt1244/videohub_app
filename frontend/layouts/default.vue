<template>
  <div class="min-h-screen bg-base-100">
    <header v-if="auth.isAuthenticated" class="navbar bg-base-200 shadow">
      <div class="container mx-auto px-4 flex justify-between items-center w-full">
        <NuxtLink to="/dashboard" class="text-xl font-bold">VideoApp</NuxtLink>
        <nav class="flex items-center gap-4">
          <NuxtLink to="/dashboard" class="btn btn-ghost btn-sm">Dashboard</NuxtLink>
          <NuxtLink to="/videos/mine" class="btn btn-ghost btn-sm">My Videos</NuxtLink>
          <NuxtLink to="/videos/upload" class="btn btn-ghost btn-sm">Upload</NuxtLink>
          <NuxtLink to="/settings" class="btn btn-ghost btn-sm">Settings</NuxtLink>
          <button class="btn btn-ghost btn-sm" @click="toggleDark">
            {{ colorMode.value === 'dark' ? '‚òÄÔ∏è' : 'Ìºô' }}
          </button>
          <button class="btn btn-sm btn-outline" @click="logout">Logout</button>
        </nav>
      </div>
    </header>
    <main class="container mx-auto p-4">
      <slot />
    </main>
  </div>
</template>

<script setup lang="ts">
const auth = useAuthStore();
const colorMode = useColorMode();

const toggleDark = () => {
  colorMode.preference = colorMode.value === 'dark' ? 'light' : 'dark';
};

const logout = () => {
  auth.logout();
  navigateTo('/login');
};
</script>
