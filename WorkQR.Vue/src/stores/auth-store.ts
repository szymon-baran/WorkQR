import { defineStore } from 'pinia';
import { UserDTO, UserTokenDTO } from '../components/models';
import { api } from 'boot/axios';

export const useAuthStore = defineStore('auth', {
  state: () => ({
    isAuthenticated: false,
    userName: '',
    token: '',
    refreshToken: '',
    expiration: BigInt(0),
    roles: [''],
    qrAuthorizationKey: '',
  }),
  persist: true,
  getters: {
    isUserAuthenticated: (state) =>
      state.isAuthenticated && new Date().getTime() < state.expiration,
    getUserName: (state) => state.userName ?? '',
    isQRScanner: (state) => state.roles.some((x) => x === 'QRScanner'),
    getToken: (state) => state.token,
    getQrAuthorizationKey: (state) => state.qrAuthorizationKey,
  },
  actions: {
    saveLoginInfo(data: UserDTO) {
      this.isAuthenticated = true;
      this.userName = data.username;
      this.token = data.token;
      this.refreshToken = data.refreshToken;
      this.expiration = data.expiration;
      this.roles = data.roles;
    },
    refreshLoginInfo(data: UserTokenDTO) {
      this.token = data.accessToken;
      this.refreshToken = data.refreshToken;
      this.expiration = data.expiration;
    },
    setQrAuthorizationKey(data: string) {
      this.qrAuthorizationKey = data;
    },
    async refreshAccessToken() {
      const obj = {
        accessToken: this.token,
        refreshToken: this.refreshToken,
      };
      const response = await api.post('auth/refreshAccessToken', obj);
      this.refreshLoginInfo(response.data);
      return response.data.accessToken;
    },
    async setQRAuthorizationKey() {
      const response = await api.get('user/getQRAuthorizationKey');
      this.setQrAuthorizationKey(response.data);
    },
  },
});
