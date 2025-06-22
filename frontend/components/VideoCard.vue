<template>
  <div class="card shadow-md bg-base-100">
    <figure>
      <img :src="video.thumbnail" :alt="`Thumbnail for ${video.title}`" />
    </figure>
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
// Definiramo tip video objekta
interface Video {
  id: string
  title: string
  thumbnail: string
  description: string
}

const props = defineProps<{ video: Video }>()

const router = useRouter()

function watch() {
  // Preusmjeri na stranicu videoplayer, npr. /videos/:id
  router.push(`/videos/${props.video.id}`)
}
</script>
