export const useApi = () => {
  const { apiBase } = useRuntimeConfig().public;
  const auth = useAuthStore();

  return $fetch.create({
    baseURL: apiBase,
    onRequest({ options }) {
      if (auth.token) {
        options.headers = {
          ...options.headers,
          Authorization: `Bearer ${auth.token}`,
        };
      }
    },
  });
};
