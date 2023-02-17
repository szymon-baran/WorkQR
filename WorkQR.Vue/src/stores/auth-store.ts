import { defineStore } from 'pinia';
import { UserDTO } from '../components/models';

export const useAuthStore = defineStore('auth', {
  state: () => ({
    isAuthenticated: false,
    userName: '',
    token: '',
    expiration: BigInt(0),
    roles: [''],
  }),
  persist: true,
  getters: {
    isUserAuthenticated: (state) =>
      state.isAuthenticated && new Date().getTime() < state.expiration,
    getUserName: (state) => state.userName ?? '',
    isQRScanner: (state) => state.roles.some((x) => x === 'QRScanner'),
  },
  actions: {
    saveLoginInfo(data: UserDTO) {
      this.isAuthenticated = true;
      this.userName = data.username;
      this.token = data.token;
      this.expiration = data.expiration;
      this.roles = data.roles;
    },
  },
});
