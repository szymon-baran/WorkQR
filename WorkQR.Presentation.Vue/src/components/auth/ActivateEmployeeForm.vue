<template>
  <q-form
    @submit="onRegCodeSubmit"
    class="q-gutter-md"
    v-if="!isRegCodeValidated"
  >
    <div class="row">
      <div class="col">
        <q-input
          filled
          v-model="regCode"
          label="Kod rejestracyjny"
          lazy-rules
          :rules="[
            (val) => (val && val.length > 0) || 'Wprowadź kod rejestracyjny',
          ]"
        />
      </div>
    </div>
    <div class="row q-col-gutter-xs q-mt-md">
      <div class="col">
        <q-btn label="Dalej" type="submit" color="primary" />
      </div>
    </div>
  </q-form>
  <q-form @submit="onSubmit" class="q-gutter-md" v-else>
    <div class="row q-col-gutter-xs">
      <div class="col">
        <q-input
          filled
          v-model="activateEmployeeForm.Username"
          label="Nazwa użytkownika *"
          lazy-rules
          readonly
        />
      </div>
    </div>
    <div class="row q-col-gutter-xs">
      <div class="col-lg-6 col-xs-12">
        <q-input
          filled
          v-model="activateEmployeeForm.FirstName"
          label="Imię *"
          lazy-rules
          :rules="[(val) => (val && val.length > 0) || 'Wprowadź imię']"
        />
      </div>
      <div class="col-lg-6 col-xs-12">
        <q-input
          filled
          v-model="activateEmployeeForm.LastName"
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
          v-model="activateEmployeeForm.Password"
          label="Hasło *"
          lazy-rules
          :rules="[
            (val) =>
              (val && val.length > 0 && passwordPattern.test(val)) ||
              'Hasło musi zawierać conajmniej 7 znaków, w tym małe i wielkie litery, cyfrę i znak specjalny!',
          ]"
          :type="isPwd ? 'password' : 'text'"
        >
          <template v-slot:append>
            <q-icon
              :name="isPwd ? 'visibility_off' : 'visibility'"
              class="pointer"
              @click="isPwd = !isPwd"
            />
          </template>
        </q-input>
      </div>
      <div class="col-lg-6 col-xs-12">
        <q-input
          filled
          v-model="repeatPassword"
          label="Powtórz hasło *"
          lazy-rules
          :rules="[
            (val) =>
              (val && val.length > 0 && val == activateEmployeeForm.Password) ||
              'Hasła muszą być identyczne!',
          ]"
          :type="isPwd ? 'password' : 'text'"
        >
          <template v-slot:append>
            <q-icon
              :name="isPwd ? 'visibility_off' : 'visibility'"
              class="pointer"
              @click="isPwd = !isPwd"
            />
          </template>
        </q-input>
      </div>
    </div>

    <div class="row q-col-gutter-xs q-mt-sm">
      <div class="col">
        <q-toggle
          v-model="isLicenceAccepted"
          label="Akceptuję regulamin aplikacji workQR"
        />
      </div>
    </div>

    <div class="row q-col-gutter-xs q-mt-md">
      <div class="col">
        <q-btn label="Aktywuj konto" type="submit" color="primary" />
      </div>
    </div>
  </q-form>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import { useQuasar, Notify } from 'quasar';
import { useRoute } from 'vue-router';
import { useUserStore } from 'stores/user-store';

export default defineComponent({
  name: 'ActivateEmployeeForm',
  setup(props, context) {
    const q = useQuasar();
    const userStore = useUserStore();
    const route = useRoute();
    const passwordPattern =
      /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{7,}$/;
    const isLicenceAccepted = ref(false);
    const isPwd = ref(true);
    const repeatPassword = ref('');
    const regCode = ref('');
    const isRegCodeValidated = ref(false);
    const { activateEmployeeForm } = storeToRefs(userStore);
    const onSubmit = async () => {
      if (isLicenceAccepted.value !== true) {
        Notify.create({
          type: 'negative',
          message: 'Musisz zaakceptować regulamin!',
          icon: 'error',
        });
      } else {
        await userStore.activateEmployee();
        Notify.create({
          type: 'positive',
          message: 'Konto aktywowane pomyślnie! Możesz się teraz zalogować.',
          icon: 'check_circle',
        });
        context.emit('success');
      }
    };
    const onRegCodeSubmit = async () => {
      let response = await userStore.getUserDataByRegistrationCode(
        regCode.value
      );
      isRegCodeValidated.value = true;
      activateEmployeeForm.value.Username = response.username;
      activateEmployeeForm.value.FirstName = response.firstName;
      activateEmployeeForm.value.LastName = response.lastName;
      activateEmployeeForm.value.RegistrationCode = regCode.value;
    };
    onMounted(() => {
      if (
        route.params['regCode'] &&
        typeof route.params['regCode'] === 'string'
      ) {
        regCode.value = route.params['regCode'];
      }
    });
    return {
      userStore,
      passwordPattern,
      isLicenceAccepted,
      isPwd,
      repeatPassword,
      activateEmployeeForm,
      regCode,
      isRegCodeValidated,
      onSubmit,
      onRegCodeSubmit,
    };
  },
});
</script>
