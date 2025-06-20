<template>
  <div class="p-6">
    <h1 class="text-3xl mb-4">Dashboard</h1>
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
      <VideoCard v-for="vid in videos" :key="vid.id" :video="vid" />
    </div>
  </div>
</template>

<script setup lang="ts">
import VideoCard from '~/components/VideoCard.vue'
import { useAuthStore } from '~/stores/auth'

const auth = useAuthStore()

// Učitavanje videa preko API-ja, prosljeđujemo auth header ako postoji
const { data: videos } = await useFetch('/api/videos', {
  headers: auth.token ? { Authorization: `Bearer ${auth.token}` } : {}
})
</script>
