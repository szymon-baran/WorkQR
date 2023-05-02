<template>
  <div>
    <q-table
      grid
      flat
      bordered
      :card-container-class="cardContainerClass"
      :rows="employees"
      :columns="columns"
      row-key="name"
      :filter="filter"
      hide-header
      v-model:pagination="pagination"
      :rows-per-page-options="rowsPerPageOptions"
    >
      <template v-slot:top-right>
        <q-input
          borderless
          dense
          debounce="300"
          v-model="filter"
          placeholder="Wyszukaj"
        >
          <template v-slot:append>
            <q-icon name="search" />
          </template>
        </q-input>
      </template>

      <template v-slot:item="props">
        <div class="q-pa-xs col-sm-4 col-md-3 employee-card">
          <q-card flat bordered>
            <q-card-section class="flex flex-center">
              <q-avatar color="primary" text-color="white" size="4rem">{{
                getInitials(props.row)
              }}</q-avatar>
            </q-card-section>
            <q-separator inset />
            <q-card-section class="flex flex-center">
              <div class="text-body1">
                {{ props.row.lastName }} {{ props.row.firstName }}
              </div>
            </q-card-section>
            <q-card-section class="flex flex-center q-pt-none">
              <div class="text-subtitle2">{{ props.row.positionName }}</div>
            </q-card-section>
          </q-card>
        </div>
      </template>
    </q-table>
  </div>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted, computed } from 'vue';
import { api } from 'boot/axios';
import { useQuasar } from 'quasar';

export default defineComponent({
  name: 'EmployeesGrid',
  setup() {
    const $q = useQuasar();
    const employees = ref([]);
    const filter = ref('');
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
        label: 'Nazwa pozycji',
        sortable: false,
      },
    ];
    const getItemsPerPage = () => {
      if ($q.screen.lt.md) {
        return 6;
      }
      return 8;
    };
    const getInitials = (row: any) => {
      return (
        `${row.firstName.substring(0, 1)}${row.lastName.substring(0, 1)}` ?? ''
      );
    };
    const pagination = ref({
      page: 1,
      rowsPerPage: getItemsPerPage(),
    });
    const cardContainerClass = computed(() => {
      return $q.screen.gt.xs
        ? 'grid-masonry grid-masonry--' + ($q.screen.gt.sm ? '3' : '2')
        : null;
    });
    const rowsPerPageOptions = computed(() => {
      return $q.screen.gt.md ? [4, 8, 12] : [3, 6, 9];
    });

    onMounted(async () => {
      const employeesResponse = await api.get('company/getCompanyEmployees');
      employees.value = employeesResponse.data;
    });
    return {
      employees,
      filter,
      columns,
      cardContainerClass,
      pagination,
      rowsPerPageOptions,
      getItemsPerPage,
      getInitials,
    };
  },
});
</script>
<style lang="scss" scoped>
.employee-card :hover {
  filter: brightness(110%);
}
</style>
