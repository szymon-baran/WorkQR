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
                  @update:model-value="setWarningsList"
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
    <div class="row q-mt-md">
      <div class="col-12">
        <q-table
          :rows="warnings"
          :columns="columns"
          row-key="id"
          dark
          color="primary"
          :pagination="{
            rowsPerPage: 0,
          }"
          :rows-per-page-options="[0]"
          class="q-mt-md"
          bordered
          flat
        >
          <template v-slot:body="props">
            <q-tr :props="props">
              <q-td key="fullName" :props="props">
                {{ props.row.fullName }}
              </q-td>
              <q-td key="overextendedBreaksCount" :props="props">
                {{ props.row.overextendedBreaksCount }}
              </q-td>
              <q-td key="notEnoughDailyWorkCount" :props="props">
                {{ props.row.notEnoughDailyWorkCount }}
              </q-td>
            </q-tr>
          </template>
        </q-table>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useReportStore } from 'stores/report-store';
import { storeToRefs } from 'pinia';
import { date } from 'quasar';

const reportStore = useReportStore();
const { reportForm } = storeToRefs(reportStore);

const warnings = ref([]);
const columns = [
  {
    name: 'fullName',
    field: 'fullName',
    align: 'center',
    label: 'Imię i nazwisko',
    sortable: false,
  },
  {
    name: 'overextendedBreaksCount',
    field: 'overextendedBreaksCount',
    align: 'right',
    label: 'Liczba dni z przekroczoną przerwą',
    sortable: false,
  },
  {
    name: 'notEnoughDailyWorkCount',
    field: 'notEnoughDailyWorkCount',
    align: 'right',
    label: 'Liczba dni z opuszczeniem pracy przed czasem',
    sortable: false,
  },
];

const setWarningsList = async () => {
  const responseData = await reportStore.getModeratorWarningsList();
  warnings.value = responseData;
};

const disableDatesAfterToday = (d: string) => {
  return d <= date.formatDate(Date.now(), 'YYYY/MM/DD');
};

onMounted(async () => {
  await setWarningsList();
});
</script>
