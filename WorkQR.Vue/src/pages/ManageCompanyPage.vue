<template>
  <q-page padding>
    <q-card dark bordered flat>
      <q-card-section>
        <div class="row">
          <span class="text-h4">Zarządzenie firmą</span>
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
            <q-tab name="employees" icon="groups" label="Pracownicy" />
            <q-tab
              name="positions"
              icon="event_seat"
              label="Stanowiska pracy"
            />
            <q-tab
              name="created-accounts"
              icon="warning"
              label="Konta nieaktywowane"
            />
            <q-tab name="reports" icon="description" label="Raporty" />
          </q-tabs>
        </div>
        <q-tab-panels
          v-model="tab"
          animated
          swipeable
          transition-prev="jump-up"
          transition-next="jump-up"
        >
          <q-tab-panel name="employees">
            <div class="row q-mb-md">
              <div class="col text-h4">Pracownicy</div>
              <q-btn
                color="primary"
                label="Dodaj nowego pracownika"
                @click="showAddEmployeeDialog"
              />
            </div>
            <employees-list :is-inactive-view="false" />
          </q-tab-panel>

          <q-tab-panel name="positions">
            <div class="text-h4 q-mb-md">Stanowiska pracy</div>
            <positions-list />
          </q-tab-panel>

          <q-tab-panel name="created-accounts">
            <div class="text-h4 q-mb-md">Konta nieaktywowane</div>
            <employees-list :is-inactive-view="true" />
          </q-tab-panel>

          <q-tab-panel name="reports">
            <div class="text-h4 q-mb-md">Raporty</div>
            <download-report />
          </q-tab-panel>
        </q-tab-panels>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import { useQuasar } from 'quasar';
import EmployeesList from 'components/companyModeration/employees/EmployeesList.vue';
import EmployeeAddDialog from 'components/companyModeration/employees/EmployeeAddDialog.vue';
import PositionsList from 'components/companyModeration/positions/PositionsList.vue';
import DownloadReport from 'components/companyModeration/reports/DownloadReport.vue';

export default defineComponent({
  name: 'ManageCompanyPage',
  components: {
    EmployeesList,
    PositionsList,
    DownloadReport,
  },
  setup() {
    const $q = useQuasar();
    const showAddEmployeeDialog = async () => {
      $q.dialog({
        component: EmployeeAddDialog,
      });
    };
    return {
      tab: ref('employees'),
      splitterModel: ref(20),
      showAddEmployeeDialog,
    };
  },
});
</script>
