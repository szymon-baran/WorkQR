<template>
  <q-dialog ref="dialog">
    <q-card flat class="card q-pa-sm">
      <q-card-section class="row items-center q-pb-md">
        <div class="text-h6">Dodaj wniosek urlopowy</div>
        <q-space />
        <q-btn icon="close" dense round color="primary" v-close-popup />
      </q-card-section>
      <q-card-section class="q-pb-none">
        <div class="row q-px-sm">
          <p class="text-subtitle2">
            Zaznacz cały okres czasu, razem z dniami wolnymi - nie zostaną
            wliczone do Twojego limitu wykorzystanych dni.
          </p>
        </div>
      </q-card-section>
      <q-card-section class="q-pb-lg">
        <vacation-request-add-form @success="onVacationRequestAdd" />
      </q-card-section>
    </q-card>
  </q-dialog>
</template>

<script lang="ts">
import { defineComponent, ref, onBeforeUnmount } from 'vue';
import { Notify } from 'quasar';
import { useVacationStore } from 'stores/vacation-store';
import VacationRequestAddForm from './VacationRequestAddForm.vue';

export default defineComponent({
  name: 'VacationRequestAddDialog',
  components: {
    VacationRequestAddForm,
  },
  setup() {
    const vacationStore = useVacationStore();
    const dialog = ref(null);
    const onVacationRequestAdd = async () => {
      dialog.value.hide();
      Notify.create({
        type: 'positive',
        message: 'Wniosek został wysłany pomyślnie!',
        icon: 'check_circle',
      });
      await vacationStore.getVacationRequests();
    };
    onBeforeUnmount(() => {
      vacationStore.reset();
    });
    return {
      vacationStore,
      dialog,
      onVacationRequestAdd,
    };
  },
});
</script>

<style lang="scss" scoped>
.card {
  width: 900px;
  max-width: 95vw;
}
</style>
