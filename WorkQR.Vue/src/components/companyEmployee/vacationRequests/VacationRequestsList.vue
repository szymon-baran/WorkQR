<template>
  <div>
    <q-table
      :rows="vacationStore.vacationRequests"
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
          <q-td key="isApproved" :props="props" auto-width>
            <q-icon
              :name="props.row.isApproved ? 'check' : 'close'"
              :color="props.row.isApproved ? 'green' : 'red'"
              size="1.2rem"
            ></q-icon>
          </q-td>
          <q-td key="dateFrom" :props="props">
            {{ new Date(props.row.dateFrom).toLocaleDateString() }}
          </q-td>
          <q-td key="dateTo" :props="props">
            {{ new Date(props.row.dateTo).toLocaleDateString() }}
          </q-td>
          <q-td key="vacationType" :props="props">
            {{
              vacationTypes.find((x) => x.value == props.row.vacationType)
                ?.label ?? 'Brak'
            }}
          </q-td>
          <!-- <q-td key="actions" :props="props">
            <q-btn
              dense
              round
              flat
              :color="props.row.isDisabled ? 'red' : 'green'"
              :icon="props.row.isDisabled ? 'close' : 'check'"
              v-if="!isInactiveView"
              ><q-tooltip anchor="bottom middle" self="top middle">{{
                props.row.isDisabled ? 'Odblokuj konto' : 'Zablokuj konto'
              }}</q-tooltip>
              <q-popup-edit
                title="Zablokuj konto"
                auto-save
                v-model="props.row.isDisabled"
                v-slot="scope"
              >
                <q-checkbox
                  v-model="scope.value"
                  label="Konto zablokowane"
                /> </q-popup-edit
            ></q-btn>
            <q-btn
              dense
              round
              flat
              color="primary"
              icon="arrow_forward_ios"
              @click="showDetails(props.row)"
              ><q-tooltip anchor="bottom middle" self="top middle"
                >Szczegóły</q-tooltip
              ></q-btn
            >
          </q-td> -->
        </q-tr>
      </template>
    </q-table>
  </div>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { useQuasar } from 'quasar';
import { useVacationStore } from 'stores/vacation-store';

export default defineComponent({
  name: 'VacationRequestsList',
  setup() {
    const vacationStore = useVacationStore();
    const $q = useQuasar();
    const vacationTypes = ref([]);
    const columns = [
      {
        name: 'isApproved',
        field: 'isApproved',
        align: 'center',
        label: 'Zatwierdzone',
        sortable: false,
      },
      {
        name: 'dateFrom',
        field: 'dateFrom',
        align: 'center',
        label: 'Od',
        sortable: false,
      },
      {
        name: 'dateTo',
        field: 'dateTo',
        align: 'center',
        label: 'Do',
        sortable: false,
      },
      {
        name: 'vacationType',
        field: 'vacationType',
        align: 'center',
        label: 'Typ',
        sortable: false,
      },
    ];

    onMounted(async () => {
      await vacationStore.getVacationRequests();

      const vacationTypesResponse = await vacationStore.getVacationTypes();
      vacationTypes.value = vacationTypesResponse;
    });
    return {
      vacationStore,
      vacationTypes,
      columns,
    };
  },
});
</script>
