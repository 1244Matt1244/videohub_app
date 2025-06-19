<template>
  <div class="card shadow-md bg-base-100">
    <figure><img :src="video.thumbnail" alt="Video thumbnail" /></figure>
    <div class="card-body">
      <h2 class="card-title">{{ video.title }}</h2>
      <p>{{ video.description }}</p>
      <div class="card-actions justify-end">
        <button class="btn btn-accent" @click="watch">Watch</button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps<{ video: { id: string, title: string, thumbnail: string, description: string } }>()

function watch() {
  // redirect to video player
}
</script>
