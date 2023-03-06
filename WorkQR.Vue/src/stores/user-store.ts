import { defineStore } from 'pinia';
import { api } from 'boot/axios';
import { useAuthStore } from './auth-store';

export const useUserStore = defineStore('user', {
  state: () => ({
    loginForm: {
      Username: null,
      Password: null,
      RememberMe: false,
    },
  }),
  actions: {
    async login() {
      const authStore = useAuthStore();
      const response = await api.post('auth/login', this.loginForm);
      authStore.saveLoginInfo(response.data);
    },
  },
});
