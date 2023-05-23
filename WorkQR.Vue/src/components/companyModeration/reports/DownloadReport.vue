<template>
  <div>
    <q-form class="q-mt-sm" @submit="onSubmit">
      <div class="row">
        <div class="col text-right">
          <q-btn label="Pobierz raport" type="submit" color="primary" />
        </div>
      </div>
      <div class="row q-col-gutter-md q-mt-xs">
        <div class="col-md-6 col-sm-12">
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
                      <q-btn
                        v-close-popup
                        label="Zamknij"
                        color="primary"
                        flat
                      />
                    </div>
                  </q-date>
                </q-popup-proxy>
              </q-icon>
            </template>
          </q-input>
        </div>
        <div class="col-md-6 col-sm-12">
          <q-select
            v-model="reportForm.Employees"
            label="Pracownicy *"
            :options="employees"
            filled
            lazy-rules
            emit-value
            :rules="[(val) => val || val === 0 || 'Wybierz pracowników']"
            multiple
            @update:model-value="onChange"
          >
            <template v-slot:selected>
              <span v-for="employee in reportForm.Employees" :key="employee">
                {{
                  employees.find((x) => x.value == employee)?.label ?? ''
                }},&nbsp;
              </span>
            </template>
          </q-select>
        </div>
      </div>
    </q-form>
    <div class="row">
      <div class="col">
        <presence-chart ref="presenceChart" />
      </div>
      <div class="col">a</div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useReportStore } from 'stores/report-store';
import { storeToRefs } from 'pinia';
import { Notify, date } from 'quasar';
import PresenceChart from './PresenceChart.vue';

const reportStore = useReportStore();
const { reportForm } = storeToRefs(reportStore);
const employees = ref([]);
const presenceChart = ref(null);

const onSubmit = async () => {
  if (
    !reportForm.value.DatesRange ||
    !reportForm.value.Employees ||
    reportForm.value.Employees.length === 0
  ) {
    Notify.create({
      type: 'negative',
      message: 'Wypełnij dane formularza',
      icon: 'error',
    });
    return;
  }

  const response = await reportStore.getReport();

  var fileURL = window.URL.createObjectURL(new Blob([response.data]));
  var fileLink = document.createElement('a');
  fileLink.href = fileURL;
  fileLink.setAttribute('download', 'raport.pdf');
  document.body.appendChild(fileLink);
  fileLink.click();
};

const onChange = () => {
  presenceChart.value.loadEmployeesPresenceData();
};

const disableDatesAfterToday = (d: string) => {
  return d <= date.formatDate(Date.now(), 'YYYY/MM/DD');
};

onMounted(async () => {
  const responseData = await reportStore.getEmployees();
  employees.value = responseData;
  reportForm.value.Employees = responseData.map((x) => x.value);
});
</script>
