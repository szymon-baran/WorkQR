import { defineStore } from 'pinia';
import { api } from 'boot/axios';
import { Notify } from 'quasar';

export const useAuthStore = defineStore('auth', {
  state: () => ({
    isAuthenticated: false,
    loginForm: {
      Username: null,
      Password: null,
      RememberMe: false,
    },
  }),
  getters: {
    isUserAuthenticated: (state) => state.isAuthenticated,
  },
  actions: {
    async login() {
      try {
        const response = await api.post('auth/login', this.loginForm);
        sessionStorage.setItem('userName', response.data.username);
        sessionStorage.setItem('userToken', response.data.token);
        sessionStorage.setItem('userExpiration', response.data.expiration);
        return true;
      } catch (error: any) {
        Notify.create({
          type: 'negative',
          message: error.response.data,
        });
        return false;
      }
    },
  },
});
