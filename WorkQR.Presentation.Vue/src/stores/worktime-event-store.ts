import { defineStore } from 'pinia';
import { GetEventsVM } from '../components/models';
import { api } from 'boot/axios';

export const useWorktimeEventStore = defineStore('worktime-event', {
  state: () => ({
    worktimeEvents: [],
    settings: new GetEventsVM(),
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
    async setCompanyEmployeeWorktimeEvents() {
      const response = await api.get(
        'companyModeration/getEmployeeWorktimeEvents',
        {
          params: this.settings,
        }
      );
      this.worktimeEvents = response.data;
    },
  },
});
