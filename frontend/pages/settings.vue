<template>
  <div class="max-w-2xl mx-auto py-6">
    <h1 class="text-3xl font-bold mb-6">Settings</h1>

    <div class="card bg-base-100 shadow-xl mb-6">
      <div class="card-body">
        <h2 class="card-title">Profile</h2>
        <input v-model="fullName" class="input input-bordered w-full" />
        <input :value="auth.user?.email" disabled class="input input-bordered w-full mt-3" />
        <button class="btn btn-primary mt-4" @click="save">Save</button>
      </div>
    </div>

    <div class="card bg-base-100 shadow-xl">
      <div class="card-body">
        <h2 class="card-title">Appearance</h2>
        <label class="label cursor-pointer">
          <span>Dark Mode</span>
          <input type="checkbox" class="toggle toggle-primary"
                 :checked="colorMode.value === 'dark'"
                 @change="toggleDark" />
        </label>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const auth = useAuthStore();
const api = useApi();
const colorMode = useColorMode();

const fullName = ref(auth.user?.fullName || '');

const save = async () => {
  await api('/api/user/profile', {
    method: 'PUT',
    body: { fullName: fullName.value, userId: auth.user?.id },
  });
  if (auth.user) auth.user.fullName = fullName.value;
  alert('Saved!');
};

const toggleDark = async () => {
  const isDark = colorMode.value !== 'dark';
  colorMode.preference = isDark ? 'dark' : 'light';
  try {
    await api('/api/user/preferences/dark-mode', {
      method: 'PUT',
      body: { isDark, userId: auth.user?.id },
    });
  } catch {}
};
</script>
