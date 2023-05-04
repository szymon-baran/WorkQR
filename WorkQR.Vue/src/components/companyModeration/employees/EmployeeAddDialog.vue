<template>
  <q-dialog ref="dialog">
    <q-card flat class="card q-pa-sm">
      <q-card-section class="row items-center q-pb-md">
        <div class="text-h6">Dodaj pracownika</div>
        <q-space />
        <q-btn icon="close" dense round color="primary" v-close-popup />
      </q-card-section>
      <q-card-section class="q-pb-none">
        <div class="row q-px-sm">
          <p class="text-subtitle2">
            Pracownik będzie musiał potwierdzić rejestrację kodem otrzymanym w
            wiadomości mailowej. Ustali też wtedy swoje hasło.
          </p>
        </div>
      </q-card-section>
      <q-card-section class="q-pb-lg">
        <employee-add-form @success="onEmployeeAdd" />
      </q-card-section>
    </q-card>
  </q-dialog>
</template>

<script lang="ts">
import { defineComponent, onBeforeUnmount, ref } from 'vue';
import { Notify } from 'quasar';
import EmployeeAddForm from './EmployeeAddForm.vue';
import { useUserStore } from 'stores/user-store';

export default defineComponent({
  name: 'EmployeeAddDialog',
  components: {
    EmployeeAddForm,
  },
  setup() {
    const userStore = useUserStore();
    const dialog = ref(null);
    const onEmployeeAdd = () => {
      dialog.value.hide();
      Notify.create({
        type: 'positive',
        message: 'Użytkownik został dodany pomyślnie!',
        icon: 'check_circle',
      });
    };
    onBeforeUnmount(() => {
      userStore.$reset();
    });
    return {
      userStore,
      dialog,
      onEmployeeAdd,
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
