import { defineStore } from 'pinia';
import { api } from 'boot/axios';
import { useEmployeeStore } from './employee-store';

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
    async getModeratorVacationRequests() {
      const response = await api.get('companyModeration/getVacationRequests');
      this.vacationRequests = response.data;
    },
    async getModeratorCompanyEmployeeVacationRequests() {
      const employeeStore = useEmployeeStore();
      const response = await api.get(
        'companyModeration/getCompanyEmployeeVacationRequests',
        {
          params: employeeStore.userDetailsVM,
        }
      );
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
    async acceptRequest(id: string) {
      const response = await api.post(
        'companyModeration/acceptVacationRequest',
        null,
        {
          params: {
            id: id,
          },
        }
      );
      return response;
    },
    async rejectRequest(id: string, rejectionDescription: string) {
      const model = {
        Id: id,
        RejectionDescription: rejectionDescription,
      };
      const response = await api.post(
        'companyModeration/rejectVacationRequest',
        model
      );
      return response;
    },
    async validateVacationRequest(value: any) {
      const model = {
        DateFrom: value.from,
        DateTo: value.to,
        VacationType: this.addForm.VacationType,
      };
      return await api.get('companyEmployee/validateVacationRequest', {
        params: model,
      });
    },
    reset() {
      Object.assign(this.addForm, addFormDefaultState);
    },
  },
});
