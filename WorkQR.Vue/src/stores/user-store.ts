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
    registerEmployeeForm: {
      Username: null,
      Email: null,
      FirstName: null,
      LastName: null,
      PositionId: null,
    },
    activateEmployeeForm: {
      Username: null,
      Password: null,
      FirstName: null,
      LastName: null,
      RegistrationCode: null,
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
    async addEmployee() {
      const response = await api.post(
        'companyModeration/addEmployee',
        this.registerEmployeeForm
      );
      return response.data;
    },
    async getUserDataByRegistrationCode(regCode: string) {
      const response = await api.get('auth/getUserDataByRegistrationCode', {
        params: {
          registrationCode: regCode,
        },
      });
      return response.data;
    },
    async activateEmployee() {
      const response = await api.post(
        'auth/activateEmployee',
        this.activateEmployeeForm
      );
      return response.data;
    },
  },
});
