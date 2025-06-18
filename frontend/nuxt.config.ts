import { defineNuxtConfig } from 'nuxt'
export default defineNuxtConfig({
  modules: ['@nuxtjs/tailwindcss', '@pinia/nuxt'],
  runtimeConfig: { public: { apiBase: process.env.NUXT_PUBLIC_API_BASE } }
})
