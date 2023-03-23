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
    registerCompanyForm: {
      ModeratorUsername: null,
      ModeratorPassword: null,
      ModeratorEmail: null,
      CompanyName: null,
    },
  }),
  actions: {
    async login() {
      const authStore = useAuthStore();
      const response = await api.post('auth/login', this.loginForm);
      authStore.saveLoginInfo(response.data);
    },
    async validateUsername(value: string) {
      return await api.get('auth/validateUsername', {
        params: {
          username: value,
        },
      });
    },
    async registerCompany() {
      const response = await api.post(
        'auth/registerCompany',
        this.registerCompanyForm
      );
      return response.data;
    },
  },
});
