<template>
  <div class="py-6">
    <h1 class="text-3xl font-bold mb-6">Video Dashboard</h1>

    <div v-if="pending" class="flex justify-center py-12">
      <span class="loading loading-spinner loading-lg"></span>
    </div>

    <div v-else-if="videos?.length" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div v-for="video in videos" :key="video.id" class="card bg-base-100 shadow-xl">
        <figure v-if="video.thumbnailUrl">
          <img :src="video.thumbnailUrl" :alt="video.title" />
        </figure>
        <div class="card-body">
          <h2 class="card-title">
            {{ video.title }}
            <span v-if="video.isPremium" class="badge badge-warning">Premium</span>
          </h2>
          <p class="text-sm opacity-70">{{ video.description || 'No description' }}</p>
          <div class="card-actions justify-end mt-4">
            <template v-if="video.isPremium && !video.playbackId && !video.isOwner">
              <button class="btn btn-warning btn-sm" @click="unlock(video.id)">
                Unlock ${{ video.price }}
              </button>
            </template>
            <NuxtLink v-else :to="`/videos/${video.id}`" class="btn btn-primary btn-sm">
              Watch
            </NuxtLink>
          </div>
        </div>
      </div>
    </div>

    <div v-else class="alert">
      <span>No videos yet. Upload your first one!</span>
    </div>
  </div>
</template>

<script setup lang="ts">
const api = useApi();

const { data: videos, pending } = await useAsyncData('videos', () =>
  api<any[]>('/api/videos')
);

const unlock = async (videoId: string) => {
  const { url } = await api<{ url: string }>('/api/payments/checkout', {
    method: 'POST',
    body: { userId: '00000000-0000-0000-0000-000000000000', videoId },
  });
  window.location.href = url;
};
</script>
