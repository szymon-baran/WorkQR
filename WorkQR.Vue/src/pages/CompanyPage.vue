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
            <q-tab name="analytics" icon="analytics" label="Twoje statystyki" />
            <q-tab
              name="vacations"
              icon="beach_access"
              label="Wnioski urlopowe"
            />
            <q-tab
              name="insights"
              icon="insights"
              label="Statystyki na tle firmy"
            />
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
            <employees-grid />
          </q-tab-panel>

          <q-tab-panel name="analytics">
            <div class="text-h4 q-mb-md">Statystyki pracy</div>
            <p>
              Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quis
              praesentium cumque magnam odio iure quidem, quod illum numquam
              possimus obcaecati commodi minima assumenda consectetur culpa fuga
              nulla ullam. In, libero.
            </p>
            <p>
              Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quis
              praesentium cumque magnam odio iure quidem, quod illum numquam
              possimus obcaecati commodi minima assumenda consectetur culpa fuga
              nulla ullam. In, libero.
            </p>
            <p>
              Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quis
              praesentium cumque magnam odio iure quidem, quod illum numquam
              possimus obcaecati commodi minima assumenda consectetur culpa fuga
              nulla ullam. In, libero.
            </p>
          </q-tab-panel>

          <q-tab-panel name="vacations">
            <div class="text-h4 q-mb-md">Wnioski urlopowe</div>
            <vacation-requests-list />
          </q-tab-panel>

          <q-tab-panel name="insights">
            <div class="text-h4 q-mb-md">Statystyki pracy 2</div>
            <p>
              Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quis
              praesentium cumque magnam odio iure quidem, quod illum numquam
              possimus obcaecati commodi minima assumenda consectetur culpa fuga
              nulla ullam. In, libero.
            </p>
            <p>
              Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quis
              praesentium cumque magnam odio iure quidem, quod illum numquam
              possimus obcaecati commodi minima assumenda consectetur culpa fuga
              nulla ullam. In, libero.
            </p>
            <p>
              Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quis
              praesentium cumque magnam odio iure quidem, quod illum numquam
              possimus obcaecati commodi minima assumenda consectetur culpa fuga
              nulla ullam. In, libero.
            </p>
          </q-tab-panel>
        </q-tab-panels>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import { useQuasar } from 'quasar';
import EmployeesList from 'components/companyEmployee/EmployeesList.vue';
import VacationRequestsList from 'components/companyEmployee/VacationRequestsList.vue';

export default defineComponent({
  name: 'CompanyPage',
  components: {
    EmployeesList,
    VacationRequestsList,
  },
  setup() {
    const $q = useQuasar();
    return {
      tab: ref('employees'),
    };
  },
});
</script>
