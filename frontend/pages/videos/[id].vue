<template>
  <div class="max-w-4xl mx-auto py-6">
    <div v-if="pending" class="flex justify-center py-12">
      <span class="loading loading-spinner loading-lg"></span>
    </div>

    <div v-else-if="video" class="card bg-base-100 shadow-xl">
      <div class="card-body">
        <h1 class="card-title text-2xl">
          {{ video.title }}
          <span v-if="video.isPremium" class="badge badge-warning">Premium</span>
        </h1>
        <p class="text-sm opacity-70">{{ video.description }}</p>

        <div v-if="video.playbackId" class="mt-4 aspect-video">
          <iframe :src="`https://stream.mux.com/${video.playbackId}.m3u8`"
                  style="width: 100%; height: 100%;"
                  frameborder="0" allowfullscreen></iframe>
        </div>

        <div v-else-if="video.status === 'preparing'" class="alert mt-4">
          <span>Video is still processing. Check back in a moment.</span>
        </div>

        <div v-else class="alert alert-error mt-4">
          <span>Video is not available.</span>
        </div>
      </div>
    </div>

    <div v-else class="alert alert-error">
      <span>Video not found.</span>
    </div>
  </div>
</template>

<script setup lang="ts">
const api = useApi();
const route = useRoute();
const id = route.params.id as string;

const { data: video, pending } = await useAsyncData(`video-${id}`, () =>
  api<any>(`/api/videos/${id}`)
);
</script>
