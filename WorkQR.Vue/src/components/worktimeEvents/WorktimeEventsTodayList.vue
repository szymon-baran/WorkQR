<template>
  <div class="row">
    <div class="col-5 q-mt-xl text-center text-h5 q-pa-lg">
      <span v-if="worktimeEvents.length > 0">
        <div>
          Aktualnie
          <span class="text-weight-bold">{{ getCurrentTaskName() }}</span>
        </div>
        <span v-if="worktimeEvents[0]['eventType'] !== eventTypes.END_WORK">
          <div>od</div>
          <div class="header-font header-md q-mt-sm">
            {{ hours }} <span class="time-word">godz.</span> {{ minutes }}
            <span class="time-word">min.</span> {{ seconds }}
            <span class="time-word">sek.</span>
          </div>
        </span>
      </span>
    </div>
    <div class="col-7">
      <q-table
        title="Dzisiejsze zdarzenia"
        :rows="worktimeEvents"
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
      />
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted, onBeforeUnmount } from 'vue';
import { api } from 'boot/axios';
import { eventTypeEnum } from '../../enums/eventTypeEnum';

export default defineComponent({
  name: 'WorktimeEventsTodayList',
  setup() {
    const worktimeEvents = ref([]);
    const columns = [
      {
        name: 'eventTypeName',
        field: 'eventTypeName',
        required: true,
        label: 'Typ zdarzenia',
        align: 'center',
        sortable: false,
      },
      {
        name: 'eventTime',
        field: 'eventTime',
        align: 'center',
        label: 'Godzina rozpoczęcia',
        sortable: false,
        format: (val: any) => `${new Date(val).toLocaleString()}`,
      },
      {
        name: 'durationInSecs',
        field: 'durationInSecs',
        align: 'center',
        label: 'Czas trwania',
        sortable: false,
        format: (val: any) => `${secondsToWorkedTime(val)}`,
      },
      {
        name: 'description',
        field: 'description',
        align: 'left',
        label: 'Opis',
        sortable: false,
      },
    ];
    const secondsToWorkedTime = (seconds: number) =>
      seconds
        ? `${Math.floor(seconds / 3600)}:${Math.floor((seconds % 3600) / 60)
            .toString()
            .padEnd(2, '0')}:${((seconds % 3600) % 60)
            .toFixed(0)
            .padEnd(2, '0')}`
        : '0:00:00';
    const interval = ref();
    const hours = ref();
    const minutes = ref();
    const seconds = ref();
    const eventTypes = eventTypeEnum;
    const calculateTimeSinceLastEvent = () => {
      if (
        worktimeEvents.value.length > 0 &&
        worktimeEvents.value[0]['eventType'] !== eventTypes.END_WORK
      ) {
        let difference =
          Date.now() - new Date(worktimeEvents.value[0]['eventTime']).getTime();

        seconds.value = Math.floor(difference / 1000);
        minutes.value = Math.floor(seconds.value / 60);
        hours.value = Math.floor(minutes.value / 60);

        hours.value %= 24;
        minutes.value %= 60;
        seconds.value %= 60;
      }
    };
    const getCurrentTaskName = () => {
      switch (worktimeEvents.value[0]['eventType']) {
        case eventTypes.START_WORK:
        case eventTypes.END_BREAK:
          return 'pracujesz';
        case eventTypes.START_BREAK:
          return 'jesteś na przerwie';
        case eventTypes.END_WORK:
          return 'nie pracujesz';
      }
    };
    onMounted(async () => {
      interval.value = setInterval(() => {
        calculateTimeSinceLastEvent();
      }, 1000);
      const response = await api.get(
        'worktimeEvent/getUserWorktimeEventsToday'
      );
      worktimeEvents.value = response.data;
    });
    onBeforeUnmount(() => {
      clearInterval(interval.value);
    });
    return {
      worktimeEvents,
      columns,
      interval,
      hours,
      minutes,
      seconds,
      eventTypes,
      calculateTimeSinceLastEvent,
      getCurrentTaskName,
    };
  },
});
</script>
<style lang="scss" scoped>
.time-word {
  opacity: 0.8;
  font-size: 70%;
  letter-spacing: 0.02rem;
}
</style>
