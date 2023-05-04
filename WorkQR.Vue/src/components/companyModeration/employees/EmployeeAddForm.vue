<template>
  <q-form @submit="onSubmit" class="q-gutter-md">
    <div class="row q-col-gutter-xs">
      <div class="col">
        <q-input
          filled
          v-model="registerEmployeeForm.FirstName"
          label="Imię *"
          lazy-rules
          :rules="[(val) => (val && val.length > 0) || 'Wprowadź imię']"
        />
      </div>
      <div class="col">
        <q-input
          filled
          v-model="registerEmployeeForm.LastName"
          label="Nazwisko *"
          lazy-rules
          :rules="[(val) => (val && val.length > 0) || 'Wprowadź nazwisko']"
        />
      </div>
    </div>
    <div class="row q-col-gutter-xs">
      <div class="col-lg-6 col-xs-12">
        <q-input
          filled
          v-model="registerEmployeeForm.Username"
          label="Login*"
          lazy-rules
          :rules="[userStore.validateUsername]"
        />
      </div>
      <div class="col-lg-6 col-xs-12">
        <q-input
          filled
          v-model="registerEmployeeForm.Email"
          label="E-mail *"
          type="email"
          lazy-rules
          :rules="[(val) => (val && val.length > 0) || 'Wprowadź adres e-mail']"
        />
      </div>
    </div>
    <div class="row q-col-gutter-xs">
      <div class="col-lg-12 col-xs-12">
        <q-select
          v-model="registerEmployeeForm.PositionId"
          :options="positions"
          filled
          lazy-rules
          emit-value
          :rules="[(val) => (val && val.length > 0) || 'Wybierz stanowisko']"
        >
          <template v-slot:selected>
            {{
              positions.find((x) => x.value == registerEmployeeForm.PositionId)
                ?.label ?? 'Brak'
            }}
          </template>
        </q-select>
      </div>
    </div>
    <div class="row q-col-gutter-xs q-mt-md">
      <div class="col">
        <q-btn label="Dodaj pracownika" type="submit" color="primary" />
      </div>
    </div>
  </q-form>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { api } from 'boot/axios';
import { storeToRefs } from 'pinia';
import { useUserStore } from 'stores/user-store';

export default defineComponent({
  name: 'EmployeeAddForm',
  setup(props, context) {
    const userStore = useUserStore();
    const positions = ref([]);
    const { registerEmployeeForm } = storeToRefs(userStore);
    const onSubmit = async () => {
      let response = await userStore.addEmployee();
      context.emit('success', response);
    };
    onMounted(async () => {
      const positionsResponse = await api.get(
        'companyModeration/getCompanyPositionsForUserToSelect'
      );
      positions.value = positionsResponse.data;
    });
    return {
      userStore,
      positions,
      registerEmployeeForm,
      onSubmit,
    };
  },
});
</script>
