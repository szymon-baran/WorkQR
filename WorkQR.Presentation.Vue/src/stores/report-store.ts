import { defineStore } from 'pinia';
import { api } from 'boot/axios';
const now = new Date();

const reportFormDefaultState = {
  Employees: [],
  DatesRange: {
    from: new Date(
      now.getFullYear() - (now.getMonth() > 0 ? 0 : 1),
      (now.getMonth() - 1 + 12) % 12,
      1
    ).toLocaleDateString('en-CA'),
    to: new Date(now.getFullYear(), now.getMonth(), 0).toLocaleDateString(
      'en-CA'
    ),
  },
};

export const useReportStore = defineStore('report', {
  state: () => ({
    reportForm: { ...reportFormDefaultState },
  }),
  getters: {
    getDateFrom: (state) => state.reportForm.DatesRange.from,
    getDateTo: (state) => state.reportForm.DatesRange.to,
  },
  actions: {
    async getReport() {
      const params = new URLSearchParams();
      params.append('DateFrom', this.getDateFrom);
      params.append('DateTo', this.getDateTo);
      this.reportForm.Employees.forEach((x) => {
        params.append('Employees', x);
      });
      const response = await api.get(
        'companyModeration/getCompanyRaportForDate',
        {
          params: params,
          responseType: 'blob',
        }
      );
      return response;
    },
    async getEmployees() {
      const response = await api.get(
        'companyModeration/getCompanyEmployeesToSelect'
      );
      return response.data;
    },
    async getEmployeesPresenceData() {
      const params = new URLSearchParams();
      params.append('DateFrom', this.getDateFrom);
      params.append('DateTo', this.getDateTo);
      this.reportForm.Employees.forEach((x) => {
        params.append('Employees', x);
      });
      const response = await api.get(
        'companyModeration/getEmployeesPresenceData',
        {
          params: params,
        }
      );
      return response.data;
    },
    async getCurrentEmployeePresenceData() {
      const params = new URLSearchParams();
      params.append('DateFrom', this.getDateFrom);
      params.append('DateTo', this.getDateTo);
      const response = await api.get(
        'companyEmployee/getEmployeePresenceData',
        {
          params: params,
        }
      );
      return response.data;
    },
    async getCurrentEmployeeWorkTimeComparisonData() {
      const params = new URLSearchParams();
      params.append('DateFrom', this.getDateFrom);
      params.append('DateTo', this.getDateTo);
      const response = await api.get(
        'companyEmployee/getEmployeeWorkTimeComparisonData',
        {
          params: params,
        }
      );
      return response.data;
    },
    async getEmployeesWorkedHoursData() {
      const params = new URLSearchParams();
      params.append('DateFrom', this.getDateFrom);
      params.append('DateTo', this.getDateTo);
      this.reportForm.Employees.forEach((x) => {
        params.append('Employees', x);
      });
      const response = await api.get(
        'companyModeration/getEmployeesWorkedHoursData',
        {
          params: params,
        }
      );
      return response.data;
    },
    async getModeratorWarningsList() {
      const params = new URLSearchParams();
      params.append('DateFrom', this.getDateFrom);
      params.append('DateTo', this.getDateTo);
      const response = await api.get('companyModeration/getEmployeesWarnings', {
        params: params,
      });
      return response.data;
    },
    reset() {
      Object.assign(this.reportForm, reportFormDefaultState);
    },
  },
});
