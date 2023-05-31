<template>
  <Bar
    id="my-chart-id"
    :options="chartOptions"
    :data="chartData"
    v-if="loaded"
  />
</template>
<script lang="ts">
import { defineComponent, ref } from 'vue';
import { Bar } from 'vue-chartjs';
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
} from 'chart.js';
import { useReportStore } from 'stores/report-store';

ChartJS.register(
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale
);

export default defineComponent({
  name: 'PresentAbsentChart',
  components: { Bar },
  setup(props, ctx) {
    const reportStore = useReportStore();
    const loaded = ref(false);
    const chartData = ref({
      labels: [],
      datasets: [
        {
          label: 'Obecny [dni]',
          data: [],
          backgroundColor: '#21ba45',
          order: 3,
          stack: 'stack1',
        },
        {
          label: 'Na urlopie [dni]',
          data: [],
          backgroundColor: '#f5a456',
          order: 3,
          stack: 'stack1',
        },
        {
          label: 'Nieobecny [dni]',
          data: [],
          backgroundColor: '#f73349',
          order: 3,
          stack: 'stack1',
        },
      ],
    });
    const loadEmployeesPresenceData = async () => {
      loaded.value = false;
      const response = await reportStore.getEmployeesPresenceData();
      chartData.value.labels = response.map((x) => x.fullName);
      chartData.value.datasets[0].data = response.map((x) => x.daysPresent);
      chartData.value.datasets[1].data = response.map((x) => x.daysOnVacation);
      chartData.value.datasets[2].data = response.map(
        (x) => x.allDaysCount - x.daysPresent - x.daysOnVacation
      );
      loaded.value = true;
    };
    ctx.expose({ loadEmployeesPresenceData });
    return {
      reportStore,
      loaded,
      chartData,
      chartOptions: {
        plugins: {
          title: {
            display: true,
            text: 'Porównanie dni obecnych do nieobecnych i urlopów pracowników',
          },
        },
        responsive: true,
        scales: {
          x: {
            stacked: true,
          },
          y: {
            stacked: true,
          },
        },
      },
    };
  },
});
</script>
