<template>
  <PolarArea :data="chartData" :options="chartOptions" v-if="loaded" />
</template>
<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { PolarArea } from 'vue-chartjs';
import {
  Chart as ChartJS,
  RadialLinearScale,
  ArcElement,
  Tooltip,
  Legend,
} from 'chart.js';
import { useReportStore } from 'stores/report-store';

ChartJS.register(RadialLinearScale, ArcElement, Tooltip, Legend);

export default defineComponent({
  name: 'WorktimeComparisonPolarArea',
  components: { PolarArea },
  setup(props, ctx) {
    const reportStore = useReportStore();
    const loaded = ref(false);
    const chartData = ref({
      labels: ['Ty', 'Twój oddział', 'Wszyscy pracownicy'],
      datasets: [
        {
          label: 'W pracy',
          data: [],
          backgroundColor: [
            'rgba(245, 164, 86, 0.7)',
            'rgba(245, 164, 86, 0.5)',
            'rgba(245, 164, 86, 0.3)',
          ],
          pointBackgroundColor: 'rgba(245, 164, 86,1)',
          pointBorderColor: '#fff',
          pointHoverBackgroundColor: '#fff',
          pointHoverBorderColor: 'rgba(245, 164, 86,1)',
        },
        {
          label: 'Na przerwie',
          data: [],
          backgroundColor: [
            'rgba(247, 51, 73,0.9)',
            'rgba(247, 51, 73,0.7)',
            'rgba(247, 51, 73,0.5)',
          ],
          pointBackgroundColor: 'rgba(247, 51, 73,1)',
          pointBorderColor: '#fff',
          pointHoverBackgroundColor: '#fff',
          pointHoverBorderColor: 'rgba(247, 51, 73,1)',
        },
      ],
    });
    const loadEmployeeWorkTimeComparisonData = async () => {
      loaded.value = false;
      const response =
        await reportStore.getCurrentEmployeeWorkTimeComparisonData();
      chartData.value.datasets[0].data[0] = response.workedHours;
      chartData.value.datasets[1].data[0] = response.breakHours;
      chartData.value.datasets[0].data[1] = response.positionWorkedHours;
      chartData.value.datasets[1].data[1] = response.positionBreakHours;
      chartData.value.datasets[0].data[2] = response.everyoneWorkedHours;
      chartData.value.datasets[1].data[2] = response.everyoneBreakHours;
      loaded.value = true;
    };
    ctx.expose({ loadEmployeeWorkTimeComparisonData });
    onMounted(async () => {
      await loadEmployeeWorkTimeComparisonData();
    });
    return {
      reportStore,
      loaded,
      chartData,
      chartOptions: {
        responsive: true,
      },
    };
  },
});
</script>
