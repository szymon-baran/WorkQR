<template>
  <q-form @submit="onSubmit" class="q-gutter-md">
    <div class="row q-col-gutter-xs">
      <div class="col">
        <q-input
          filled
          v-model="registerCompanyForm.CompanyName"
          label="Nazwa firmy *"
          lazy-rules
          :rules="[(val) => (val && val.length > 0) || 'Wprowadź nazwę firmy']"
        />
      </div>
    </div>
    <div class="row q-col-gutter-xs">
      <div class="col-lg-6 col-xs-12">
        <q-input
          filled
          v-model="registerCompanyForm.ModeratorUsername"
          label="Login moderatora *"
          lazy-rules
          :rules="[userStore.validateUsername]"
        />
      </div>
      <div class="col-lg-6 col-xs-12">
        <q-input
          filled
          v-model="registerCompanyForm.ModeratorEmail"
          label="E-mail moderatora *"
          type="email"
          lazy-rules
          :rules="[(val) => (val && val.length > 0) || 'Wprowadź adres e-mail']"
        />
      </div>
    </div>
    <div class="row q-col-gutter-xs">
      <div class="col-lg-6 col-xs-12">
        <q-input
          filled
          v-model="registerCompanyForm.ModeratorPassword"
          label="Hasło moderatora *"
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
              (val &&
                val.length > 0 &&
                val == registerCompanyForm.ModeratorPassword) ||
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

    <div class="row q-col-gutter-xs q-mt-sm">
      <div class="col">
        <Checkbox v-model="captchaResponse" theme="dark" />
      </div>
    </div>

    <div class="row q-col-gutter-xs q-mt-md">
      <div class="col">
        <q-btn label="Zarejestruj" type="submit" color="primary" />
      </div>
    </div>
  </q-form>
</template>
<script lang="ts">
import { defineComponent, ref } from 'vue';
import { storeToRefs } from 'pinia';
import { useQuasar } from 'quasar';
import { Notify } from 'quasar';
import { useUserStore } from 'stores/user-store';
import { Checkbox } from 'vue-recaptcha/head';

export default defineComponent({
  name: 'RegisterCompanyForm',
  components: {
    Checkbox,
  },
  setup(props, context) {
    const q = useQuasar();
    const userStore = useUserStore();
    const passwordPattern =
      /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{7,}$/;
    const isLicenceAccepted = ref(false);
    const isPwd = ref(true);
    const repeatPassword = ref('');
    const { registerCompanyForm } = storeToRefs(userStore);
    const captchaResponse = ref(false);
    const onSubmit = async () => {
      if (isLicenceAccepted.value !== true) {
        Notify.create({
          type: 'negative',
          message: 'Musisz zaakceptować regulamin!',
          icon: 'error',
        });
      } else if (captchaResponse.value === false) {
        Notify.create({
          type: 'negative',
          message: 'Musisz potwierdzić, że jesteś człowiekiem!',
          icon: 'error',
        });
      } else {
        let response = await userStore.registerCompany();
        context.emit('success', response);
      }
    };
    return {
      userStore,
      passwordPattern,
      isLicenceAccepted,
      isPwd,
      repeatPassword,
      registerCompanyForm,
      captchaResponse,
      onSubmit,
    };
  },
});
</script>
