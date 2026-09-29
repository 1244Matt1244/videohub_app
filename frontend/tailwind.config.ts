import daisyui from 'daisyui';

export default {
  content: [
    './components/**/*.{vue,js,ts}',
    './layouts/**/*.vue',
    './pages/**/*.vue',
    './app.vue',
  ],
  plugins: [daisyui],
  daisyui: {
    themes: ['light', 'dark'],
    darkTheme: 'dark',
  },
};
