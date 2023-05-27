<template>
  <Bar
    id="my-chart-id"
    :options="chartOptions"
    :data="chartData"
    v-if="loaded"
  />
</template>
<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
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
  name: 'WorkedHoursChart',
  components: { Bar },
  setup(props, ctx) {
    const reportStore = useReportStore();
    const loaded = ref(false);
    const chartData = ref({
      labels: [],
      datasets: [
        {
          label: 'Przepracowane',
          data: [],
          backgroundColor: '#21ba45',
          order: 3,
          stack: 'stack1',
        },
        {
          label: 'Na przerwie',
          data: [],
          backgroundColor: '#f73349',
          order: 3,
          stack: 'stack1',
        },
      ],
    });
    const loadEmployeesWorkedHoursData = async () => {
      loaded.value = false;
      const response = await reportStore.getEmployeesWorkedHoursData();
      chartData.value.labels = response.map((x) => x.fullName);
      chartData.value.datasets[0].data = response.map((x) => x.workedHours);
      chartData.value.datasets[1].data = response.map((x) => x.breakHours);
      loaded.value = true;
    };
    ctx.expose({ loadEmployeesWorkedHoursData });
    onMounted(async () => {
      await loadEmployeesWorkedHoursData();
    });
    return {
      reportStore,
      loaded,
      chartData,
      chartOptions: {
        plugins: {
          title: {
            display: true,
            text: 'Dni przepracowane/spędzone na przerwie przez pracowników',
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
