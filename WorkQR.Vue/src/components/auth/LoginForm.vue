<template>
  <!-- <q-dialog position="top" v-if="notClosed"> -->
  <div class="q-pa-lg">
    <q-card :class="[route.name === 'Login' ? 'card' : 'dialog-card']">
      <q-card-section class="row" v-if="route.name === 'Login'">
        <router-link to="/"
          ><q-btn round color="primary" icon="arrow_back"
        /></router-link>
      </q-card-section>
      <q-card-section class="row">
        <div class="text-h6">Logowanie</div>
        <q-space />
        <div class="text-h6" v-if="route.name !== 'Login'">
          <router-link to="/login"
            ><q-btn round color="primary" icon="open_in_full"
          /></router-link>
        </div>
      </q-card-section>
      <q-separator inset class="q-mb-md" />
      <q-form @submit="onSubmit">
        <q-card-section>
          <q-input
            filled
            v-model="loginForm.Username"
            label="Nazwa użytkownika"
            lazy-rules
            :rules="[
              (val) => (val && val.length > 0) || 'Podaj nazwę użytkownika',
            ]"
            class="q-mb-sm"
          />
          <q-input
            filled
            v-model="loginForm.Password"
            label="Hasło"
            lazy-rules
            :rules="[(val) => (val && val.length > 0) || 'Podaj hasło']"
            :type="isPwd ? 'password' : 'text'"
          >
            <template v-slot:append>
              <q-icon
                :name="isPwd ? 'visibility_off' : 'visibility'"
                class="cursor-pointer"
                @click="isPwd = !isPwd"
              />
            </template>
          </q-input>
          <p
            class="login-text text-center text-weight-bold text-subtitle1 q-mt-sm"
          >
            <a href="/">Zapomniałeś hasła?</a>
          </p>
          <div class="row justify-center">
            <q-btn
              color="primary"
              label="Zaloguj"
              class="text-center"
              padding="10px 50px"
              type="submit"
            />
          </div>
          <q-separator class="q-my-xl" />
          <p class="login-text text-center text-subtitle1">
            Nie posiadasz konta?<br />
            <a href="/" class="text-weight-bold"
              >Wprowadź otrzymany kod tutaj.</a
            >
          </p>
        </q-card-section>
      </q-form>
    </q-card>
  </div>
  <!-- </q-dialog> -->
</template>
<script>
import { ref, defineComponent } from 'vue';
import { useAuthStore } from 'stores/auth-store';
import { useRoute } from 'vue-router';
import { storeToRefs } from 'pinia';

export default defineComponent({
  name: 'LoginForm',
  setup() {
    const store = useAuthStore();
    const { loginForm } = storeToRefs(store);
    return {
      isPwd: ref(true),
      notClosed: ref(true),
      loginForm,
      async onSubmit() {
        await store.login();
        debugger;
        notClosed = false;
      },
    };
  },
  computed: {
    route: () => useRoute(),
  },
});
</script>

<style scoped lang="scss">
.login-text {
  color: $primary;
}
a {
  text-decoration: none;
  color: $primary;
}
.card {
  width: 35rem;
  height: 45rem;
  background: rgba($dark, 0.7);
}
.dialog-card {
  width: 30rem;
  height: 40rem;
  background: rgba($dark, 0.95);
}
</style>
