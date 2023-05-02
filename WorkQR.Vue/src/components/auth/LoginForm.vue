<template>
  <div>
    <q-card :class="[$q.screen.lt.md ? 'card-mobile' : 'card-large']" flat>
      <q-card-section style="height: 100%" horizontal>
        <transition
          enter-active-class="animated fadeIn"
          leave-active-class="animated fadeOut"
          appear
          :duration="200"
        >
          <q-card-section
            style="display: flex; flex-direction: column; width: 100%"
            class="background"
            v-show="!isLoginOpen || !$q.screen.lt.md"
          >
            <q-card-section class="row q-pb-none">
              <q-img
                src="~assets/logo-new2.png"
                width="6.5rem"
                class="logo-md q-ml-sm"
                no-spinner
              />
              <span
                class="text-h4 q-ml-sm text-accent header-font header-md self-center"
                v-if="!$q.screen.lt.md"
                >workQR</span
              >
              <q-space />
              <q-btn
                round
                color="primary"
                icon="login"
                @click="() => (isLoginOpen = !isLoginOpen)"
                v-show="!isLoginOpen"
                class="self-start"
              />
            </q-card-section>
            <q-card-section class="row q-pt-none" v-if="$q.screen.lt.md">
              <span class="text-h4 text-accent header-font header-md"
                >workQR</span
              >
            </q-card-section>
            <q-card-section
              class="row q-mt-sm"
              style="
                display: flex;
                align-items: center;
                justify-content: center;
              "
            >
              <q-img
                src="~assets/clock-people-2.png"
                width="38rem"
                class="q-mb-sm"
                no-spinner
              />
              <div style="width: 100%; height: 0; margin: 0; border: 0"></div>
              <p class="text-h6 q-mb-xs text-center">Zarządzaj czasem pracy</p>
              <div style="width: 100%; height: 0; margin: 0; border: 0"></div>
              <p class="text-subtitle1 q-mb-none text-center">
                w swojej firmie z wykorzystaniem kodów QR.
              </p>
              <div style="width: 100%; height: 0; margin: 0; border: 0"></div>
              <p class="text-subtitle1 text-center">Wygodnie i za darmo.</p>
              <div style="width: 100%; height: 0; margin: 0; border: 0"></div>
              <q-btn
                round
                color="primary"
                icon="fa-brands fa-github"
                href="https://github.com/szymon-baran/WorkQR"
                target="_blank"
              />
            </q-card-section>
          </q-card-section>
        </transition>
        <transition
          enter-active-class="animated fadeIn"
          leave-active-class="animated fadeOut"
          appear
          :duration="200"
        >
          <q-card-section
            v-show="isLoginOpen"
            :style="[$q.screen.lt.md ? 'width: 100%' : 'width: 45%']"
          >
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
                      class="pointer"
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
                  Nie posiadasz konta?
                </p>
                <p
                  class="text-primary text-center text-subtitle1 q-mt-none q-mb-md"
                >
                  <span class="text-weight-bold pointer" @click="register"
                    >Zarejestruj się</span
                  >
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
import { useUserStore } from 'stores/user-store';
import { useRoute } from 'vue-router';
import { useRouter } from 'vue-router';
import { storeToRefs } from 'pinia';
import { useQuasar, Notify } from 'quasar';
import RegisterDialog from './RegisterDialog.vue';

export default defineComponent({
  name: 'LoginForm',
  setup() {
    const store = useUserStore();
    const router = useRouter();
    const { loginForm } = storeToRefs(store);
    const $q = useQuasar();
    const register = () => {
      $q.dialog({
        component: RegisterDialog,
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
    return {
      isPwd: ref(true),
      isLoginOpen: ref(false),
      loginForm,
      register,
      async onSubmit() {
        await store.login();
        Notify.create({
          type: 'positive',
          message: 'Zalogowano pomyślnie!',
          icon: 'check_circle',
        });
        router.push({ name: 'Home' });
        store.$reset();
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
.card-large {
  width: 75rem;
  max-width: 94vw;
  height: 43rem;
  max-height: 100vh;
  background: rgba($dark, 0.95);
}
.card-mobile {
  width: 100vw;
  height: 100vh;
  background: rgba($dark, 0.95);
}
.background {
  // background-image: linear-gradient(
  //   to left top,
  //   rgba(#be8902, 0.25),
  //   rgba(#976d03, 0.25),
  //   rgba(#755401, 0.25),
  //   rgba(#4e3701, 0.25),
  //   rgba(#332501, 0.25)
  // );
  background: rgba($primary, 0.12);
}
</style>
