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
          <q-td key="lastName" :props="props" class="pointer">
            {{ props.row.lastName }}
          </q-td>
          <q-td key="firstName" :props="props" class="pointer">
            {{ props.row.firstName }}
          </q-td>
          <q-td key="username" :props="props" class="pointer">
            {{ props.row.username }}
          </q-td>
          <q-td key="dateFrom" :props="props">
            {{ new Date(props.row.dateFrom).toLocaleDateString() }}
          </q-td>
          <q-td key="dateTo" :props="props">
            {{ new Date(props.row.dateTo).toLocaleDateString() }}
          </q-td>
          <q-td key="requestDescription" :props="props" class="pointer">
            {{ props.row.requestDescription }}
          </q-td>
          <q-td key="vacationType" :props="props">
            {{
              vacationTypes.find((x) => x.value == props.row.vacationType)
                ?.label ?? 'Brak'
            }}
          </q-td>
          <q-td key="actions" :props="props">
            <q-btn
              dense
              round
              flat
              color="amber"
              icon="done"
              @click="acceptRequest(props.row.id)"
              ><q-tooltip anchor="bottom middle" self="top middle"
                >Potwierdź</q-tooltip
              >
            </q-btn>
            <q-btn dense round flat color="amber" icon="close"
              ><q-tooltip anchor="bottom middle" self="top middle"
                >Odrzuć</q-tooltip
              >
              <q-popup-edit
                title="Odrzuć wniosek"
                buttons
                v-model="rejectionDescription"
                v-slot="scope"
                @save="rejectRequest($event, props.row.id)"
                label-set="Odrzuć"
              >
                <q-input
                  filled
                  v-model="scope.value"
                  label="Powód odrzucenia wniosku"
                />
              </q-popup-edit>
            </q-btn>
          </q-td>
        </q-tr>
      </template>
    </q-table>
  </div>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted, onBeforeUnmount } from 'vue';
import { useQuasar } from 'quasar';
import { useVacationStore } from 'stores/vacation-store';
import { Notify } from 'quasar';

export default defineComponent({
  name: 'VacationRequestsModList',
  setup() {
    const vacationStore = useVacationStore();
    const $q = useQuasar();
    const vacationTypes = ref([]);
    const rejectionDescription = ref('');
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
        name: 'requestDescription',
        field: 'requestDescription',
        align: 'left',
        label: 'Opis',
        sortable: false,
      },
      {
        name: 'vacationType',
        field: 'vacationType',
        align: 'center',
        label: 'Typ',
        sortable: false,
      },
      { name: 'actions', label: 'Akcje', field: '', align: 'right' },
    ];

    const acceptRequest = async (id: string) => {
      await vacationStore.acceptRequest(id);
      Notify.create({
        type: 'positive',
        message: 'Wniosek urlopowy został zatwierdzony pomyślnie!',
        icon: 'check_circle',
      });
      await vacationStore.getModeratorVacationRequests();
    };

    const rejectRequest = async (event: string, id: string) => {
      debugger;
      await vacationStore.rejectRequest(id, event);
      Notify.create({
        type: 'positive',
        message: 'Wniosek urlopowy został odrzucony pomyślnie!',
        icon: 'check_circle',
      });
      await vacationStore.getModeratorVacationRequests();
    };

    onMounted(async () => {
      await vacationStore.getModeratorVacationRequests();

      const vacationTypesResponse = await vacationStore.getVacationTypes();
      vacationTypes.value = vacationTypesResponse;
    });

    onBeforeUnmount(() => {
      vacationStore.$reset();
    });

    return {
      vacationStore,
      vacationTypes,
      rejectionDescription,
      columns,
      acceptRequest,
      rejectRequest,
    };
  },
});
</script>
