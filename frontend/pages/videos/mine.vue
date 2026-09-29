<template>
  <div class="py-6">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-3xl font-bold">My Videos</h1>
      <NuxtLink to="/videos/upload" class="btn btn-primary">Upload Video</NuxtLink>
    </div>

    <div v-if="pending" class="flex justify-center py-12">
      <span class="loading loading-spinner loading-lg"></span>
    </div>

    <div v-else-if="videos?.length" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div v-for="video in videos" :key="video.id" class="card bg-base-100 shadow-xl">
        <div class="card-body">
          <h2 class="card-title">{{ video.title }}</h2>
          <p class="text-sm opacity-70">Status: {{ video.status }}</p>
          <div class="card-actions justify-end">
            <button class="btn btn-error btn-sm" @click="remove(video.id)">Delete</button>
          </div>
        </div>
      </div>
    </div>

    <div v-else class="alert">
      <span>You have no videos.</span>
    </div>
  </div>
</template>

<script setup lang="ts">
const api = useApi();
const { data: videos, pending, refresh } = await useAsyncData('my-videos', () =>
  api<any[]>('/api/videos?onlyMine=true')
);

const remove = async (id: string) => {
  if (!confirm('Delete this video?')) return;
  await api(`/api/videos/${id}`, { method: 'DELETE' });
  await refresh();
};
</script>
