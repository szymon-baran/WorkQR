<template>
  <q-table
    :title="title"
    :rows="worktimeEventStore.getWorktimeEvents"
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
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useWorktimeEventStore } from 'stores/worktime-event-store';

export default defineComponent({
  name: 'WorktimeEventsList',
  props: {
    title: {
      type: String,
      required: true,
    },
  },
  setup() {
    const worktimeEventStore = useWorktimeEventStore();
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

    return {
      worktimeEventStore,
      columns,
      secondsToWorkedTime,
    };
  },
});
</script>
