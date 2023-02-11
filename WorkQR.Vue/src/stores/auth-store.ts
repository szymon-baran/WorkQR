import { defineStore } from 'pinia';

export const useAuthStore = defineStore('auth', {
  state: () => ({
    isAuthenticated: false,
    loginForm: {
      username: null,
      password: null,
    },
  }),
  getters: {
    isUserAuthenticated: (state) => state.isAuthenticated,
  },
});
