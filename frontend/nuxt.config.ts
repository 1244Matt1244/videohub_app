export default defineNuxtConfig({
  modules: ['@pinia/nuxt', '@nuxtjs/color-mode', '@nuxtjs/tailwindcss'],
  css: ['~/assets/css/main.css'],
  colorMode: {
    classSuffix: '',
    preference: 'system',
    fallback: 'light',
  },
  runtimeConfig: {
    public: {
      apiBase: process.env.NUXT_PUBLIC_API_BASE || 'http://localhost:5000',
    },
  },
  devtools: { enabled: true },
});
