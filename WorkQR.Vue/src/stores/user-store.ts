import { defineStore } from 'pinia';
import { api } from 'boot/axios';
import { Notify } from 'quasar';
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
      try {
        const response = await api.post('auth/login', this.loginForm);
        authStore.saveLoginInfo(response.data);
        return true;
      } catch (error: any) {
        Notify.create({
          type: 'negative',
          message: error.response.data,
          icon: 'error',
        });
        return false;
      }
    },
  },
});
