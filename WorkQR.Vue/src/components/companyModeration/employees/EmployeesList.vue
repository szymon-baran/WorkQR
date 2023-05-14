<template>
  <div>
    <div class="row">
      <div class="col-7 text-subtitle1">
        Możesz edytować wartości poszczególnych rekordów poprzez kliknięcie
        wybranej komórki
      </div>
      <div
        class="col-5 text-subtitle2 text-right"
        v-if="editedRecords && editedRecords.length > 0"
      >
        Edytowane rekordy:
        {{ editedRecords }}
        <div>
          <q-btn
            color="primary"
            label="Zapisz zmiany"
            class="q-mt-sm"
            @click="saveChanges"
          />
        </div>
      </div>
    </div>
    <q-table
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
          <q-td key="index" :props="props">{{ props.row.index }}</q-td>
          <q-td key="lastName" :props="props" class="pointer">
            {{ props.row.lastName }}
            <q-popup-edit
              title="Edytuj nazwisko"
              auto-save
              v-model="props.row.lastName"
              v-slot="scope"
              @change="onValueUpdate(props.row)"
            >
              <q-input v-model="scope.value" dense autofocus />
            </q-popup-edit>
          </q-td>
          <q-td key="firstName" :props="props" class="pointer">
            {{ props.row.firstName }}
            <q-popup-edit
              title="Edytuj imię"
              auto-save
              v-model="props.row.firstName"
              v-slot="scope"
              @change="onValueUpdate(props.row)"
            >
              <q-input v-model="scope.value" dense autofocus />
            </q-popup-edit>
          </q-td>
          <q-td key="username" :props="props" class="pointer">
            {{ props.row.username }}
            <q-popup-edit
              title="Edytuj nazwę użytkownika"
              auto-save
              v-model="props.row.username"
              v-slot="scope"
              @change="onValueUpdate(props.row)"
            >
              <q-input v-model="scope.value" dense autofocus />
            </q-popup-edit>
          </q-td>
          <q-td key="positionId" :props="props" class="pointer">
            {{
              positions.find((x) => x.value == props.row.positionId)?.label ??
              'Brak'
            }}
            <q-popup-edit
              title="Edytuj typ konta"
              auto-save
              v-model="props.row.positionId"
              v-slot="scope"
            >
              <q-select
                v-model="scope.value"
                :options="positions"
                dense
                autofocus
                emit-value
              >
                <template v-slot:selected>
                  {{
                    positions.find((x) => x.value == scope.value)?.label ??
                    'Brak'
                  }}
                </template>
              </q-select>
            </q-popup-edit>
          </q-td>
          <q-td
            key="lastActivity"
            :props="props"
            class="pointer"
            :style="getLastActivityStyle(props.row.lastActivity)"
          >
            <q-icon name="schedule" size="1.3rem" />
            {{ getLastActivityText(props.row.lastActivity) }}
          </q-td>
          <q-td key="actions" :props="props">
            <q-btn
              dense
              round
              flat
              color="grey"
              icon="qr_code"
              @click="showQrCode(props.row)"
              ><q-tooltip anchor="bottom middle" self="top middle"
                >Pokaż kod QR</q-tooltip
              ></q-btn
            >
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
          </q-td>
        </q-tr>
      </template>
    </q-table>
  </div>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted, toRaw } from 'vue';
import { api } from 'boot/axios';
import { useQuasar, Notify } from 'quasar';
import EmployeeDetailsDialog from './EmployeeDetailsDialog.vue';
import QrCodeDialog from 'components/qr/QrCodeDialog.vue';

export default defineComponent({
  name: 'EmployeesModeratorList',
  props: {
    isInactiveView: {
      type: Boolean,
      default: false,
    },
  },
  setup(props) {
    const $q = useQuasar();
    const employees = ref([]);
    const positions = ref([]);
    const editedRecords = ref([]);
    const columns = [
      {
        name: 'index',
        label: '#',
        field: 'index',
      },
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
        name: 'positionId',
        field: 'positionId',
        align: 'center',
        label: 'Typ konta',
        sortable: false,
      },
      {
        name: 'lastActivity',
        field: 'lastActivity',
        align: 'center',
        label: 'Ostatnia aktywność',
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
    const showQrCode = (row: any) => {
      $q.dialog({
        component: QrCodeDialog,
        componentProps: {
          companyUser: toRaw(row),
          companyUserPositionName:
            positions.value.find((x) => x.value == row.positionId)?.label ??
            'Brak stanowiska',
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
    const onValueUpdate = (row: any) => {
      if (!editedRecords.value.includes(row.index))
        editedRecords.value.push(row.index);
    };
    const saveChanges = async () => {
      const recordsToSave = employees.value.filter((x) =>
        editedRecords.value.includes(x.index)
      );
      await api.post('companyModeration/updateCompanyEmployees', recordsToSave);
      Notify.create({
        type: 'positive',
        message: 'Zaktualizowano pomyślnie!',
        icon: 'check_circle',
      });
      editedRecords.value = [];
    };
    const setEmployees = async () => {
      const response = await api.get('companyModeration/getCompanyEmployees');
      employees.value = response.data;
      employees.value.forEach((row, index) => {
        row.index = index + 1;
      });
    };
    const setInactiveAccounts = async () => {
      const response = await api.get(
        'companyModeration/getCompanyInactiveAccounts'
      );
      employees.value = response.data;
      employees.value.forEach((row, index) => {
        row.index = index + 1;
      });
    };
    const getLastActivityText = (lastActivity: Date) => {
      if (lastActivity) {
        let diffMins = Math.floor(
          (new Date().getTime() - Date.parse(lastActivity)) / (1000 * 60)
        );
        if (diffMins <= 60) {
          return `${Math.round(diffMins)} minut temu`;
        } else if (diffMins / 60 <= 24) {
          return `${Math.round(diffMins / 60)} godzin temu`;
        } else {
          return `${Math.round(diffMins / (60 * 24))} dni temu`;
        }
      }
      return 'Nigdy';
    };
    const getLastActivityStyle = (lastActivity: Date) => {
      if (lastActivity) {
        let diffMins = Math.floor(
          (new Date().getTime() - Date.parse(lastActivity)) / (1000 * 60)
        );
        if (diffMins <= 60) {
          return 'color: green;';
        } else if (diffMins / 60 <= 24) {
          return 'color: yellow;';
        } else if (diffMins / (60 * 24) <= 28) {
          return 'color: orange;';
        }
      }
      return 'color: red;';
    };
    onMounted(async () => {
      if (props.isInactiveView) {
        await setInactiveAccounts();
      } else {
        await setEmployees();
      }
      const positionsResponse = await api.get(
        'companyModeration/getCompanyPositionsForUserToSelect'
      );
      positions.value = positionsResponse.data;
    });
    return {
      employees,
      positions,
      editedRecords,
      columns,
      showDetails,
      showQrCode,
      onValueUpdate,
      saveChanges,
      setEmployees,
      setInactiveAccounts,
      getLastActivityText,
      getLastActivityStyle,
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
