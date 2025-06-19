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

const { data: videos } = await useFetch('/api/videos', {
  headers: useAuthStore().token
    ? { Authorization: `Bearer ${useAuthStore().token}` }
    : {}
})
</script>
