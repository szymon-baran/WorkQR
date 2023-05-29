<template>
  <Doughnut :data="chartData" :options="chartOptions" v-if="loaded" />
</template>
<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { Doughnut } from 'vue-chartjs';
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js';
import { useReportStore } from 'stores/report-store';

ChartJS.register(ArcElement, Tooltip, Legend);

export default defineComponent({
  name: 'PresentAbsentChart',
  components: { Doughnut },
  setup(props, ctx) {
    const reportStore = useReportStore();
    const loaded = ref(false);
    const chartData = ref({
      labels: ['Obecny', 'Na urlopie', 'Nieobecny'],
      datasets: [
        {
          data: [],
          backgroundColor: ['#21ba45', '#f5a456', '#f73349'],
        },
      ],
    });
    const loadCurrentEmployeePresenceData = async () => {
      loaded.value = false;
      const response = await reportStore.getCurrentEmployeePresenceData();
      chartData.value.datasets[0].data[0] = response.daysPresent;
      chartData.value.datasets[0].data[1] = response.daysOnVacation;
      chartData.value.datasets[0].data[2] =
        response.allDaysCount - response.daysPresent - response.daysOnVacation;
      loaded.value = true;
    };
    ctx.expose({ loadCurrentEmployeePresenceData });
    onMounted(async () => {
      await loadCurrentEmployeePresenceData();
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
