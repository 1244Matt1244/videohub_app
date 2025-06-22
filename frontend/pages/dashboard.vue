<script setup lang="ts">
const { $api } = useNuxtApp()
const route = useRoute()
const token = ref(route.query.token || '')

// Dohvati sve videe
const { data: videos } = await useFetch('/api/videos/list', {
  headers: { Authorization: `Bearer ${token.value}` }
})

// Stripe payment handler
const handlePayment = async (videoId: string) => {
  const { clientSecret } = await $api('/stripe/payment', {
    method: 'POST',
    body: { videoId }
  })
  
  const stripe = await loadStripe('your_pk_key')
  await stripe.confirmCardPayment(clientSecret)
}
</script>

<template>
  <div class="container mx-auto p-4">
    <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
      <div v-for="video in videos" :key="video.id" class="card bg-base-100 shadow-xl">
        <div class="card-body">
          <h2 class="card-title">{{ video.title }}</h2>
          <p>{{ video.description }}</p>
          
          <div v-if="video.isPremium" class="card-actions justify-end">
            <button @click="handlePayment(video.id)" class="btn btn-primary">
              Unlock Premium - \$9.99
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>