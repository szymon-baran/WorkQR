<template>
  <q-form @submit="onSubmit" class="q-gutter-md">
    <div class="row q-col-gutter-sm">
      <div class="col-12">
        <q-input
          filled
          v-model="addForm.Description"
          label="Opis *"
          lazy-rules
          :rules="[
            (val) => (val && val.length > 0) || 'Opisz swój wniosek urlopowy',
          ]"
        />
      </div>
      <div class="col-xs-12 col-sm-6">
        <q-select
          v-model="addForm.VacationType"
          label="Typ *"
          :options="vacationTypes"
          filled
          lazy-rules
          emit-value
          :rules="[(val) => val || val === 0 || 'Wybierz typ']"
        >
          <template v-slot:selected>
            {{
              vacationTypes.find((x) => x.value == addForm.VacationType)
                ?.label ?? ''
            }}
          </template>
        </q-select>
      </div>
      <div class="col-xs-12 col-sm-6">
        <q-input
          filled
          :model-value="`${datesRange.from} - ${datesRange.to}`"
          :rules="[vacationStore.validateVacationRequest(datesRange)]"
          label="Przedział czasu"
          lazy-rules
        >
          <template v-slot:append>
            <q-icon name="event" class="cursor-pointer">
              <q-popup-proxy
                cover
                transition-show="scale"
                transition-hide="scale"
              >
                <q-date
                  v-model="datesRange"
                  range
                  mask="YYYY-MM-DD"
                  filled
                  flat
                  bordered
                  :options="disableDatesBeforeToday"
                >
                  <div class="row items-center justify-end">
                    <q-btn v-close-popup label="Zamknij" color="primary" flat />
                  </div>
                </q-date>
              </q-popup-proxy>
            </q-icon>
          </template>
        </q-input>
      </div>
    </div>
    <div class="row">
      <p class="text-subtitle2">
        Zwróć uwagę na poprawność wypełnionego wniosku. Po jego złożeniu, nie
        będzie mógł zostać edytowany!
      </p>
    </div>
    <div class="row q-col-gutter-xs q-mt-md">
      <div class="col">
        <q-btn label="Złóż wniosek" type="submit" color="primary" />
      </div>
    </div>
  </q-form>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import { date } from 'quasar';
import { useVacationStore } from 'stores/vacation-store';

export default defineComponent({
  name: 'VacationRequestAddForm',
  setup(props, context) {
    const vacationStore = useVacationStore();
    const vacationTypes = ref([]);
    const { addForm } = storeToRefs(vacationStore);
    const now = new Date();
    const datesRange = ref({
      from: new Date(now.setDate(now.getDate() + 7)).toLocaleDateString(
        'en-CA'
      ),
      to: new Date(now.setDate(now.getDate() + 7)).toLocaleDateString('en-CA'),
    });
    const disableDatesBeforeToday = (d: string) => {
      return d >= date.formatDate(Date.now(), 'YYYY/MM/DD');
    };
    const onSubmit = async () => {
      addForm.value.DateFrom = datesRange.value.from;
      addForm.value.DateTo = datesRange.value.to;
      let validationAsync = await vacationStore.validateVacationRequest(
        datesRange.value
      );
      if (validationAsync) {
        let response = await vacationStore.addVacationRequest();
        context.emit('success', response);
      }
    };
    onMounted(async () => {
      const vacationTypesResponse = await vacationStore.getVacationTypes();
      vacationTypes.value = vacationTypesResponse;
    });
    return {
      vacationStore,
      vacationTypes,
      addForm,
      now,
      datesRange,
      disableDatesBeforeToday,
      onSubmit,
    };
  },
});
</script>
