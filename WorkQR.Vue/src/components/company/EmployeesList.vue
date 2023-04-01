<template>
  <div>
    <q-table
      title="Pracownicy"
      :rows="employees"
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
          <q-td key="lastName" :props="props">
            {{ props.row.lastName }}
          </q-td>
          <q-td key="firstName" :props="props">
            {{ props.row.firstName }}
          </q-td>
          <q-td key="username" :props="props">
            {{ props.row.username }}
          </q-td>
          <q-td key="positionName" :props="props">
            {{ props.row.positionName }}
          </q-td>
          <q-td key="isDisabled" :props="props">
            <q-icon
              :name="props.row.isDisabled ? 'close' : 'check'"
              size="1.3rem"
            />
          </q-td>
          <q-td key="actions" :props="props">
            <q-btn dense round flat color="grey" icon="qr_code"
              ><q-tooltip anchor="bottom middle" self="top middle"
                >Pokaż kod QR</q-tooltip
              ></q-btn
            >
            <q-btn
              dense
              round
              flat
              color="grey"
              :icon="props.row.isDisabled ? 'check' : 'block'"
              ><q-tooltip anchor="bottom middle" self="top middle">{{
                props.row.isDisabled ? 'Odblokuj' : 'Zablokuj'
              }}</q-tooltip></q-btn
            >
            <q-btn dense round flat color="grey" icon="delete"
              ><q-tooltip anchor="bottom middle" self="top middle"
                >Usuń</q-tooltip
              ></q-btn
            >
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
import EmployeeDetailsDialog from './EmployeeDetailsDialog.vue';

export default defineComponent({
  name: 'EmployeesList',
  setup() {
    const $q = useQuasar();
    const employees = ref([]);
    const columns = [
      {
        name: 'lastName',
        field: 'lastName',
        align: 'center',
        label: 'Nazwisko',
        sortable: false,
      },
      {
        name: 'firstName',
        field: 'firstName',
        align: 'center',
        label: 'Imię',
        sortable: false,
      },
      {
        name: 'username',
        field: 'username',
        align: 'center',
        label: 'Nazwa użytkownika',
        sortable: false,
      },
      {
        name: 'positionName',
        field: 'positionName',
        align: 'center',
        label: 'Typ konta',
        sortable: false,
      },
      {
        name: 'isDisabled',
        field: 'isDisabled',
        align: 'center',
        label: 'Konto aktywne',
        sortable: false,
      },
      { name: 'actions', label: 'Akcje', field: '', align: 'right' },
    ];
    const showDetails = (row: any) => {
      $q.dialog({
        component: EmployeeDetailsDialog,
        componentProps: {
          userId: row.id,
          username: row.username,
        },
      })
        .onOk(() => {
          console.log('OK');
        })
        .onCancel(() => {
          console.log('Cancel');
        })
        .onDismiss(() => {
          console.log('Called on OK or Cancel');
        });
    };
    onMounted(async () => {
      const response = await api.get('company/getCompanyEmployees');
      employees.value = response.data;
    });
    return {
      employees,
      columns,
      showDetails,
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
