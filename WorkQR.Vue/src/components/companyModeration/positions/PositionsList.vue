<template>
  <div>
    <q-table
      :rows="positions"
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
          <q-td key="name" :props="props">
            {{ props.row.name }}
          </q-td>
          <q-td key="timeBasis" :props="props">
            {{ props.row.timeBasis }}
          </q-td>
          <q-td key="breakMinsPerDay" :props="props">
            {{ props.row.breakMinsPerDay }}
          </q-td>
          <q-td key="isSystemPosition" :props="props">
            <q-icon
              :name="props.row.isSystemPosition ? 'check' : 'close'"
              size="1.3rem"
            />
          </q-td>
          <q-td key="actions" :props="props">
            <q-btn
              dense
              round
              flat
              color="grey"
              icon="delete"
              v-if="!props.row.isSystemPosition"
              ><q-tooltip anchor="bottom middle" self="top middle"
                >Usuń</q-tooltip
              ></q-btn
            >
          </q-td>
        </q-tr>
      </template>
    </q-table>
  </div>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { api } from 'boot/axios';
import { useQuasar } from 'quasar';

export default defineComponent({
  name: 'PositionsList',
  setup() {
    const $q = useQuasar();
    const positions = ref([]);
    const columns = [
      {
        name: 'name',
        field: 'name',
        align: 'center',
        label: 'Nazwa',
        sortable: false,
      },
      {
        name: 'timeBasis',
        field: 'timeBasis',
        align: 'center',
        label: 'Wymiar czasu pracy',
        sortable: false,
      },
      {
        name: 'breakMinsPerDay',
        field: 'breakMinsPerDay',
        align: 'center',
        label: 'Czas przerwy/dzień [min]',
        sortable: false,
      },
      {
        name: 'isSystemPosition',
        field: 'isSystemPosition',
        align: 'center',
        label: 'Stanowisko systemowe',
        sortable: false,
      },
      { name: 'actions', label: 'Akcje', field: '', align: 'right' },
    ];
    onMounted(async () => {
      const response = await api.get('position/getCompanyPositionsForUser');
      positions.value = response.data;
    });
    return {
      positions,
      columns,
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
