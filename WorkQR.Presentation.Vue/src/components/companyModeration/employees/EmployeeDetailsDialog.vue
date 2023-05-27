<template>
  <q-dialog>
    <q-card flat class="card q-pa-sm">
      <q-card-section class="items-center q-pb-sm">
        <div class="text-h6">Szczegóły użytkownika {{ username }}</div>
        <p class="text-subtitle2 q-mt-sm">
          Poniższe filtry mają wpływ na wszystkie tabele z danymi!
        </p>
      </q-card-section>
      <q-card-section class="row items-center q-pb-md q-col-gutter-sm">
        <q-input
          dense
          filled
          v-model="employeeStore.userDetailsVM.DateFrom"
          mask="####-##-##"
          class="col"
          label="Data od"
          clearable
          @blur="reloadData()"
        >
          <template v-slot:append>
            <q-icon name="event" class="pointer">
              <q-popup-proxy
                cover
                transition-show="scale"
                transition-hide="scale"
              >
                <q-date
                  v-model="employeeStore.userDetailsVM.DateFrom"
                  mask="YYYY-MM-DD"
                >
                  <div class="row items-center justify-end">
                    <q-btn v-close-popup label="Zamknij" color="primary" flat />
                  </div>
                </q-date>
              </q-popup-proxy>
            </q-icon>
          </template>
        </q-input>
        <q-input
          dense
          filled
          v-model="employeeStore.userDetailsVM.DateTo"
          mask="####-##-##"
          class="col"
          label="Data do"
          clearable
          @blur="reloadData()"
        >
          <template v-slot:append>
            <q-icon name="event" class="pointer">
              <q-popup-proxy
                cover
                transition-show="scale"
                transition-hide="scale"
              >
                <q-date
                  v-model="employeeStore.userDetailsVM.DateTo"
                  mask="YYYY-MM-DD"
                >
                  <div class="row items-center justify-end">
                    <q-btn v-close-popup label="Zamknij" color="primary" flat />
                  </div>
                </q-date>
              </q-popup-proxy>
            </q-icon>
          </template>
        </q-input>
        <q-input
          dense
          filled
          v-model="employeeStore.userDetailsVM.Description"
          label="Opis"
          class="col"
          clearable
          @blur="reloadData()"
        />
      </q-card-section>
      <q-card-section class="q-pb-md">
        <div class="row q-col-gutter-xs">
          <div class="col-xs-12 col-lg-7">
            <div class="text-h5 q-mb-md">
              Wydarzenia spełniające wybrane filtry
            </div>
            <worktime-events-list />
          </div>
          <div class="col-xs-12 col-lg-5">
            <div class="text-h5 q-mb-md">Urlopy</div>
            <vacation-requests-list />
          </div>
        </div>
      </q-card-section>
      <q-card-section class="q-pb-md"> </q-card-section>
    </q-card>
  </q-dialog>
</template>
<script lang="ts">
import { defineComponent, onBeforeUnmount, onMounted } from 'vue';
import { useWorktimeEventStore } from 'stores/worktime-event-store';
import { useVacationStore } from 'stores/vacation-store';
import { useEmployeeStore } from 'stores/employee-store';
import WorktimeEventsList from '../../worktimeEvents/WorktimeEventsList.vue';
import VacationRequestsList from 'src/components/companyEmployee/vacationRequests/VacationRequestsList.vue';

export default defineComponent({
  name: 'EmployeeDetailsDialog',
  props: {
    userId: {
      type: String,
      required: true,
    },
    username: {
      type: String,
      required: true,
    },
  },
  components: {
    WorktimeEventsList,
    VacationRequestsList,
  },
  setup(props) {
    const worktimeEventStore = useWorktimeEventStore();
    const vacationStore = useVacationStore();
    const employeeStore = useEmployeeStore();

    const reloadData = async () => {
      await worktimeEventStore.setCompanyEmployeeWorktimeEvents();
      await vacationStore.getModeratorCompanyEmployeeVacationRequests();
    };

    onMounted(async () => {
      employeeStore.userDetailsVM.UserId = props.userId;
      await worktimeEventStore.setCompanyEmployeeWorktimeEvents();
      await vacationStore.getModeratorCompanyEmployeeVacationRequests();
    });

    onBeforeUnmount(() => {
      vacationStore.reset();
    });

    return {
      worktimeEventStore,
      vacationStore,
      employeeStore,
      reloadData,
    };
  },
});
</script>
<style lang="scss" scoped>
.card {
  width: 120em;
  height: 60em;
  max-width: 95vw;
  max-height: 80vh;
}
</style>
