<template>
  <div>
    <div class="row q-col-gutter-md q-mt-xs">
      <div class="col-lg-3 col-xs-12">
        <q-input
          filled
          :model-value="`${reportForm.DatesRange.from} - ${reportForm.DatesRange.to}`"
          label="Przedział czasu *"
        >
          <template v-slot:append>
            <q-icon name="event" class="cursor-pointer">
              <q-popup-proxy
                cover
                transition-show="scale"
                transition-hide="scale"
              >
                <q-date
                  v-model="reportForm.DatesRange"
                  range
                  mask="YYYY-MM-DD"
                  filled
                  flat
                  bordered
                  @update:model-value="onChange"
                  :options="disableDatesAfterToday"
                >
                  <div class="row items-center justify-end">
                    <q-btn v-close-popup label="Zamknij" color="primary" flat />
                  </div>
                </q-date>
              </q-popup-proxy>
            </q-icon>
          </template>
        </q-input>
      </div>
    </div>
    <div class="row q-mt-md q-mb-sm q-col-gutter-lg">
      <div class="col-xs-12 col-md-3">
        <p class="text-subtitle1 text-center">
          Dni w pracy w wybranym przedziale czasu
        </p>
        <presence-doughnut ref="presenceDoughnut" />
      </div>
      <div class="col-xs-12 col-md-3">
        <p class="text-subtitle1 text-center">
          Porównanie godzin pracy/przerwy
        </p>
        <worktime-comparison-polar-area ref="worktimeComparisonPolarArea" />
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { ref } from 'vue';
import { useReportStore } from 'stores/report-store';
import { storeToRefs } from 'pinia';
import { date } from 'quasar';
import PresenceDoughnut from './PresenceDoughnut.vue';
import WorktimeComparisonPolarArea from './WorktimeComparisonPolarArea.vue';

const reportStore = useReportStore();
const { reportForm } = storeToRefs(reportStore);
const employees = ref([]);
const presenceDoughnut = ref(null);
const worktimeComparisonPolarArea = ref(null);

const onChange = () => {
  presenceDoughnut.value.loadCurrentEmployeePresenceData();
  worktimeComparisonPolarArea.value.loadEmployeeWorkTimeComparisonData();
};

const disableDatesAfterToday = (d: string) => {
  return d <= date.formatDate(Date.now(), 'YYYY/MM/DD');
};
</script>
