import { defineStore } from 'pinia';
import { api } from 'boot/axios';
import { useEmployeeStore } from './employee-store';

export const useWorktimeEventStore = defineStore('worktime-event', {
  state: () => ({
    worktimeEvents: [],
  }),
  getters: {
    getWorktimeEvents: (state) => state.worktimeEvents,
  },
  actions: {
    async setUserWorktimeEventsToday() {
      const response = await api.get(
        'worktimeEvent/getUserWorktimeEventsToday'
      );
      this.worktimeEvents = response.data;
    },
    async updateTodayEventDescription(description: string, id: string) {
      const object = {
        Id: id,
        Description: description,
      };
      await api.post('worktimeEvent/updateTodayEventDescription', object);
    },
    async setCompanyEmployeeWorktimeEvents() {
      const employeeStore = useEmployeeStore();
      const response = await api.get(
        'companyModeration/getEmployeeWorktimeEvents',
        {
          params: employeeStore.userDetailsVM,
        }
      );
      this.worktimeEvents = response.data;
    },
  },
});
