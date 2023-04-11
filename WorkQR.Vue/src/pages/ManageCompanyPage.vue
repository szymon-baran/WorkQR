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
        <q-splitter v-model="splitterModel" dark>
          <template v-slot:before>
            <div>
              <q-tabs
                v-model="tab"
                vertical
                class="text-off-white"
                active-class="text-primary"
              >
                <q-tab name="employees" icon="groups" label="Pracownicy" />
                <q-tab
                  name="positions"
                  icon="event_seat"
                  label="Stanowiska pracy"
                />
                <q-tab name="movies" icon="movie" label="Movies" />
              </q-tabs>
            </div>
          </template>

          <template v-slot:after>
            <q-tab-panels
              v-model="tab"
              animated
              swipeable
              vertical
              transition-prev="jump-up"
              transition-next="jump-up"
            >
              <q-tab-panel name="employees">
                <div class="text-h4 q-mb-md">Pracownicy</div>
                <employees-list />
              </q-tab-panel>

              <q-tab-panel name="positions">
                <div class="text-h4 q-mb-md">Stanowiska pracy</div>
                <positions-list />
              </q-tab-panel>

              <q-tab-panel name="movies">
                <div class="text-h4 q-mb-md">Movies</div>
                <p>
                  Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quis
                  praesentium cumque magnam odio iure quidem, quod illum numquam
                  possimus obcaecati commodi minima assumenda consectetur culpa
                  fuga nulla ullam. In, libero.
                </p>
                <p>
                  Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quis
                  praesentium cumque magnam odio iure quidem, quod illum numquam
                  possimus obcaecati commodi minima assumenda consectetur culpa
                  fuga nulla ullam. In, libero.
                </p>
                <p>
                  Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quis
                  praesentium cumque magnam odio iure quidem, quod illum numquam
                  possimus obcaecati commodi minima assumenda consectetur culpa
                  fuga nulla ullam. In, libero.
                </p>
              </q-tab-panel>
            </q-tab-panels>
          </template>
        </q-splitter>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import EmployeesList from 'components/company/EmployeesList.vue';
import PositionsList from 'components/company/PositionsList.vue';

export default defineComponent({
  name: 'ManageCompanyPage',
  components: {
    EmployeesList,
    PositionsList,
  },
  setup() {
    return {
      tab: ref('employees'),
      splitterModel: ref(20),
    };
  },
});
</script>
