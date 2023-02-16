<template>
  <div class="q-pa-lg">
    <q-card class="card" flat>
      <q-card-section style="height: 100%" horizontal>
        <q-card-section
          style="
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            width: 100%;
          "
          class="background"
        >
          <q-card-section class="row">
            <q-img src="~assets/logo.png" width="6.75rem" class="logo" />
            <span
              class="text-h4 q-ml-sm text-accent header-font letter-spacing-md self-center"
              >workQR</span
            >
            <q-space />
            <q-btn
              round
              color="primary"
              icon="login"
              @click="() => (isLoginOpen = !isLoginOpen)"
              v-if="!isLoginOpen"
              class="self-start"
            />
          </q-card-section>
          <q-card-section
            class="row"
            style="display: flex; align-items: center; justify-content: center"
          >
            <p class="text-h6">Witamy w workQR,</p>
            <div style="width: 100%; height: 0; margin: 0; border: 0"></div>
            <p class="text-subtitle1">
              najlepszej aplikacji do zarządzania czasem pracy Twojej firmy.
            </p>
          </q-card-section>
          <q-card-section
            class="row q-mt-xl"
            style="display: flex; align-items: center; justify-content: center"
          >
            <p class="text-body2">Masz pytania odnośnie działania systemu?</p>
            <div style="width: 100%; height: 0; margin: 0; border: 0"></div>
            <p class="text-body2">
              Zachęcamy do kontaktu
              <a href="mailto:szymon.w.baran@gmail.com">tutaj</a>!
            </p>
          </q-card-section>
        </q-card-section>
        <transition
          enter-active-class="animated fadeInRight"
          leave-active-class="animated fadeOutRight"
          appear
          :duration="200"
        >
          <q-card-section v-if="isLoginOpen" style="width: 45%">
            <q-card-section class="row">
              <div></div>
              <q-space />
              <q-btn
                round
                color="primary"
                icon="close"
                @click="() => (isLoginOpen = !isLoginOpen)"
              />
            </q-card-section>
            <q-card-section class="row">
              <div class="text-h6">Logowanie</div>
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
                    (val) =>
                      (val && val.length > 0) || 'Podaj nazwę użytkownika',
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
                  class="text-primary text-center text-weight-bold text-subtitle1 q-mt-sm"
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
                <p class="text-center text-subtitle1 q-mb-none">
                  Posiadasz kod rejestracyjny?
                </p>
                <p class="text-primary text-center text-subtitle1 q-mt-none">
                  <a href="/" class="text-weight-bold">Załóż konto</a>
                </p>
              </q-card-section>
            </q-form>
          </q-card-section>
        </transition>
      </q-card-section>
    </q-card>
  </div>
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
      isLoginOpen: ref(false),
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
a {
  text-decoration: none;
  color: $primary;
}
.card {
  width: 60vw;
  height: 70vh;
  background: rgba($dark, 0.95);
}
.background {
  background-image: linear-gradient(
    to left top,
    rgba(#be8902, 0.25),
    rgba(#976d03, 0.25),
    rgba(#755401, 0.25),
    rgba(#4e3701, 0.25),
    rgba(#332501, 0.25)
  );
}
</style>
