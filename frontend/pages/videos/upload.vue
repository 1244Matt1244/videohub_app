<template>
  <div class="max-w-2xl mx-auto py-6">
    <h1 class="text-3xl font-bold mb-6">Upload Video</h1>

    <div class="card bg-base-100 shadow-xl">
      <div class="card-body">
        <input v-model="title" placeholder="Title" class="input input-bordered w-full mb-3" />
        <textarea v-model="description" placeholder="Description"
                  class="textarea textarea-bordered w-full mb-3"></textarea>

        <label class="label cursor-pointer justify-start gap-2 mb-3">
          <input type="checkbox" v-model="isPremium" class="checkbox" />
          <span>Premium video</span>
        </label>

        <input v-if="isPremium" v-model.number="price" type="number"
               placeholder="Price (USD)" class="input input-bordered w-full mb-3" />

        <input type="file" accept="video/*" @change="onFile"
               class="file-input file-input-bordered w-full mb-3" />

        <button class="btn btn-primary w-full"
                :disabled="uploading || !file || !title" @click="upload">
          <span v-if="uploading" class="loading loading-spinner"></span>
          {{ uploading ? 'Uploading…' : 'Upload' }}
        </button>

        <div v-if="progress > 0 && progress < 100" class="mt-3">
          <progress class="progress progress-primary w-full" :value="progress" max="100"></progress>
        </div>

        <div v-if="error" class="alert alert-error mt-3 text-sm">{{ error }}</div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const api = useApi();
const title = ref('');
const description = ref('');
const isPremium = ref(false);
const price = ref(0);
const file = ref<File | null>(null);
const uploading = ref(false);
const progress = ref(0);
const error = ref('');

const onFile = (e: Event) => {
  file.value = (e.target as HTMLInputElement).files?.[0] ?? null;
};

const upload = async () => {
  if (!file.value || !title.value) return;
  uploading.value = true;
  error.value = '';
  progress.value = 0;

  try {
    const { uploadUrl, uploadId } = await api<{ uploadUrl: string; uploadId: string }>(
      '/api/videos/upload-url', { method: 'POST' }
    );

    await new Promise<void>((resolve, reject) => {
      const xhr = new XMLHttpRequest();
      xhr.open('PUT', uploadUrl);
      xhr.upload.onprogress = (e) => {
        if (e.lengthComputable) progress.value = Math.round((e.loaded / e.total) * 100);
      };
      xhr.onload = () => (xhr.status >= 200 && xhr.status < 300 ? resolve() : reject());
      xhr.onerror = () => reject();
      xhr.send(file.value);
    });

    await api('/api/videos', {
      method: 'POST',
      body: {
        title: title.value,
        description: description.value,
        muxUploadId: uploadId,
        isPremium: isPremium.value,
        price: price.value,
      },
    });

    await navigateTo('/videos/mine');
  } catch (e: any) {
    error.value = e?.message || 'Upload failed';
  } finally {
    uploading.value = false;
  }
};
</script>
