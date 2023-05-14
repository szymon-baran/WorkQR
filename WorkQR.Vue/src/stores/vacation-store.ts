import { defineStore } from 'pinia';
import { api } from 'boot/axios';

const addFormDefaultState = {
  Description: null,
  DateFrom: '',
  DateTo: '',
  VacationType: null,
};

export const useVacationStore = defineStore('vacation', {
  state: () => ({
    vacationRequests: [],
    addForm: { ...addFormDefaultState },
  }),
  actions: {
    async getVacationRequests() {
      const response = await api.get('companyEmployee/getVacationRequests');
      this.vacationRequests = response.data;
    },
    async getVacationTypes() {
      const response = await api.get('companyEmployee/getVacationTypes');
      return response.data;
    },
    async addVacationRequest() {
      const response = await api.post(
        'companyEmployee/addVacationRequest',
        this.addForm
      );
      return response;
    },
    reset() {
      Object.assign(this.addForm, addFormDefaultState);
    },
  },
});
