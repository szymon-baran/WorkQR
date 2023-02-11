<template>
  <q-dialog position="top">
    <div class="q-pa-lg">
      <q-card style="width: 500px" class="dialog">
        <q-card-section class="row">
          <div class="text-h6">Logowanie</div>
        </q-card-section>
        <q-separator inset class="q-mb-md" />
        <q-form @submit="onSubmit" @reset="onReset">
          <q-card-section>
            <q-input
              filled
              v-model="loginForm.username"
              label="Nazwa użytkownika"
              lazy-rules
              :rules="[
                (val) => (val && val.length > 0) || 'Podaj nazwę użytkownika',
              ]"
              class="q-mb-sm"
            />
            <q-input
              filled
              v-model="loginForm.password"
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
            <p class="login-text text-center text-weight-bold text-subtitle1">
              <a href="/">Zapomniałeś hasła?</a>
            </p>
            <div class="row justify-center">
              <q-btn
                color="primary"
                label="Zaloguj"
                class="text-center"
                padding="10px 50px"
              />
            </div>
            <q-separator class="q-my-lg" />
            <p class="login-text text-center text-subtitle1">
              Nie posiadasz konta?
              <a href="/" class="text-weight-bold">Zarejestruj się.</a>
            </p>
          </q-card-section>
        </q-form>
      </q-card>
    </div>
  </q-dialog>
</template>
<script>
import { ref, defineComponent } from 'vue';
import { useAuthStore } from 'stores/auth-store';
import { storeToRefs } from 'pinia';

export default defineComponent({
  name: 'LoginDialog',
  setup() {
    const store = useAuthStore();
    const { loginForm } = storeToRefs(store);
    return {
      isPwd: ref(true),
      loginForm,
    };
  },
});
</script>

<style scoped lang="scss">
.dialog {
  border-top: 8px solid $secondary;
}
.login-text {
  color: $primary;
}
a {
  text-decoration: none;
  color: $primary;
}
</style>
