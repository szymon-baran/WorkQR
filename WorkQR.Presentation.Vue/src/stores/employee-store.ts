import { defineStore } from 'pinia';
import { GetUserDetailsVM } from '../components/models';

export const useEmployeeStore = defineStore('employee', {
  state: () => ({
    userDetailsVM: new GetUserDetailsVM(),
  }),
});
