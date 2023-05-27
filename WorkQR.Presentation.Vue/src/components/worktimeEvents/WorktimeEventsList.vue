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
  >
    <template v-slot:body="props">
      <q-tr :props="props">
        <q-td key="eventTypeName" :props="props">{{
          props.row.eventTypeName
        }}</q-td>
        <q-td key="eventTime" :props="props">{{
          new Date(props.row.eventTime).toLocaleString()
        }}</q-td>
        <q-td key="durationInSecs" :props="props">{{
          secondsToWorkedTime(props.row.durationInSecs)
        }}</q-td>
        <q-td key="durationInSecs" :props="props"
          >{{ props.row.description
          }}<q-popup-edit
            title="Zmień opis"
            buttons
            v-model="props.row.description"
            v-slot="scope"
            @save="onDescriptionChange($event, props.row.id)"
          >
            <q-input v-model="scope.value" dense autofocus /> </q-popup-edit
        ></q-td>
      </q-tr>
    </template>
  </q-table>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useWorktimeEventStore } from 'stores/worktime-event-store';
import { Notify } from 'quasar';

export default defineComponent({
  name: 'WorktimeEventsList',
  props: {
    title: {
      type: String,
      required: false,
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
      },
      {
        name: 'durationInSecs',
        field: 'durationInSecs',
        align: 'center',
        label: 'Czas trwania',
        sortable: false,
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
    const onDescriptionChange = async (description: string, id: string) => {
      await worktimeEventStore.updateTodayEventDescription(description, id);
      Notify.create({
        type: 'positive',
        message: 'Opis został zaktualizowany pomyślnie!',
        icon: 'check_circle',
      });
    };
    return {
      worktimeEventStore,
      columns,
      secondsToWorkedTime,
      onDescriptionChange,
    };
  },
});
</script>
