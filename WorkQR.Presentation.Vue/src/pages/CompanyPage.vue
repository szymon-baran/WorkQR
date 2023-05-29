<template>
  <q-page padding>
    <q-card dark bordered flat>
      <q-card-section>
        <div class="row">
          <span class="text-h4">Twoja firma</span>
        </div>
      </q-card-section>
      <q-separator dark inset />
      <q-card-section style="min-height: 44rem">
        <div>
          <q-tabs
            v-model="tab"
            class="text-off-white"
            active-class="text-primary"
            narrow-indicator
            align="justify"
          >
            <q-tab
              name="employees"
              icon="group"
              label="Lista współpracowników"
            />
            <q-tab name="analytics" icon="analytics" label="Statystyki" />
            <q-tab name="vacations" icon="beach_access" label="Urlopy" />
          </q-tabs>
        </div>
        <q-tab-panels
          v-model="tab"
          animated
          swipeable
          vertical
          transition-prev="jump-up"
          transition-next="jump-up"
        >
          <q-tab-panel name="employees">
            <div class="text-h5 q-mb-md">Lista współpracowników</div>
            <employees-list />
          </q-tab-panel>

          <q-tab-panel name="analytics">
            <div class="text-h5 q-mb-md">Statystyki</div>
            <employee-report />
          </q-tab-panel>

          <q-tab-panel name="vacations">
            <div class="row q-mb-md">
              <div class="col-xs-12 col-lg-9 text-h5">Urlopy</div>
              <div class="col-xs-12 col-lg-3 text-right">
                <q-btn
                  color="primary"
                  label="Nowy wniosek urlopowy"
                  @click="showAddVacationRequestDialog"
                />
              </div>
            </div>
            <vacation-requests-list />
          </q-tab-panel>
        </q-tab-panels>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { useQuasar } from 'quasar';
import EmployeesList from 'components/companyEmployee/EmployeesList.vue';
import EmployeeReport from 'components/companyEmployee/reports/EmployeeReport.vue';
import VacationRequestsList from 'components/companyEmployee/vacationRequests/VacationRequestsList.vue';
import VacationRequestAddDialog from 'components/companyEmployee/vacationRequests/VacationRequestAddDialog.vue';
import { useVacationStore } from 'src/stores/vacation-store';

export default defineComponent({
  name: 'CompanyEmployeePage',
  components: {
    EmployeesList,
    EmployeeReport,
    VacationRequestsList,
  },
  setup() {
    const $q = useQuasar();
    const showAddVacationRequestDialog = async () => {
      $q.dialog({
        component: VacationRequestAddDialog,
      });
    };
    const vacationStore = useVacationStore();
    onMounted(async () => {
      await vacationStore.getVacationRequests();
    });
    return {
      tab: ref('employees'),
      vacationStore,
      showAddVacationRequestDialog,
    };
  },
});
</script>
