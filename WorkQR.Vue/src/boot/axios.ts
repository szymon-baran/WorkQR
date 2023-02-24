import { boot } from 'quasar/wrappers';
import axios, { AxiosInstance } from 'axios';
import { useAuthStore } from 'stores/auth-store';

declare module '@vue/runtime-core' {
  interface ComponentCustomProperties {
    $axios: AxiosInstance;
  }
}

// Be careful when using SSR for cross-request state pollution
// due to creating a Singleton instance here;
// If any client changes this (global) instance, it might be a
// good idea to move this instance creation inside of the
// "export default () => {}" function below (which runs individually
// for each client)
const api = axios.create({ baseURL: 'https://localhost:5001' });

api.interceptors.request.use(function (config) {
  const authStore = useAuthStore();
  debugger;
  if (config.url) {
    const controller = config.url.substring(0, config.url.indexOf('/'));
    if (controller !== 'auth')
      config.headers['Authorization'] = `Bearer ${authStore.getToken}`;
    return config;
  }
});

api.interceptors.response.use(
  (response) => {
    return response;
  },
  async function (error) {
    const authStore = useAuthStore();
    const originalRequest = error.config;
    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      await authStore.refreshAccessToken();
      return api(originalRequest);
    }
    return Promise.reject(error);
  }
);

export default boot(({ app }) => {
  // for use inside Vue files (Options API) through this.$axios and this.$api

  app.config.globalProperties.$axios = axios;
  // ^ ^ ^ this will allow you to use this.$axios (for Vue Options API form)
  //       so you won't necessarily have to import axios in each vue file

  app.config.globalProperties.$api = api;
  // ^ ^ ^ this will allow you to use this.$api (for Vue Options API form)
  //       so you can easily perform requests against your app's API
});

export { api };
