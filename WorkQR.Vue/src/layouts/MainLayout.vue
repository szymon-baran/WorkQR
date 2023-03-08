<template>
  <q-layout view="hHh lpR fFf">
    <q-header>
      <q-toolbar
        class="bg-secondary q-pa-xs"
        style="height: 6vh; min-height: 62px"
      >
        <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="toggleLeftDrawer"
          color="primary"
          v-if="$q.screen.lt.sm && $route.name !== 'QRScanner'"
        />

        <q-toolbar-title class="q-ml-sm q-mt-xs"
          ><q-img src="~assets/logo.png" width="3rem" class="logo-sm" />
          <span class="header-font header-sm q-ml-sm text-primary text-h5"
            >workQR</span
          >
        </q-toolbar-title>
        <div
          v-if="authStore.isUserAuthenticated && $route.name !== 'QRScanner'"
        >
          Witaj, {{ authStore.getUserName }}!
          <q-btn
            flat
            dense
            round
            icon="logout"
            aria-label="Wyloguj"
            class="q-ml-sm"
            @click="logout"
          />
        </div>
        <div
          v-if="!authStore.isUserAuthenticated && $route.name !== 'QRScanner'"
        >
          <q-btn
            flat
            dense
            round
            icon="login"
            aria-label="Zaloguj"
            color="primary"
            @click="routerPushToLogin"
          />
        </div>
      </q-toolbar>
    </q-header>

    <q-drawer
      v-model="leftDrawerOpen"
      show-if-above
      v-if="$route.name !== 'QRScanner'"
    >
      <q-list>
        <div class="q-mt-md"></div>
        <EssentialLink
          v-for="link in essentialLinks"
          :key="link.title"
          v-bind="link"
        />
      </q-list>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { useQuasar } from 'quasar';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/auth-store';
import { Notify } from 'quasar';
import EssentialLink from 'components/EssentialLink.vue';

const linksList = [
  {
    title: 'Strona główna',
    caption: 'Zarządzaj swoim czasem pracy',
    icon: 'home',
    routerTo: '/',
    isBlank: false,
  },
  {
    title: 'Firma',
    caption: 'Informacje o współpracownikach',
    icon: 'work',
    routerTo: '/',
    isBlank: false,
  },
  {
    title: 'Twój kod QR',
    caption: 'Sprawdź swój kod QR',
    icon: 'qr_code_2',
    routerTo: '/',
    isBlank: false,
  },
  {
    title: 'Ustawienia',
    caption: 'Zmień dane swojego konta',
    icon: 'settings',
    routerTo: '/',
    isBlank: false,
  },
  // {
  //   title: 'Github',
  //   caption: 'Odwiedź stronę projektu',
  //   icon: 'code',
  //   link: 'https://github.com/szymon-baran/WorkQR',
  //   isExternalLink: true,
  //   separatorBefore: true,
  // },
];

export default defineComponent({
  name: 'MainLayout',

  components: {
    EssentialLink,
  },

  setup() {
    const $q = useQuasar();
    const router = useRouter();
    const leftDrawerOpen = ref(false);
    const loginDialogOpen = ref(false);
    const authStore = useAuthStore();
    const routerPushToLogin = () => {
      router.push({ name: 'Login' });
    };
    const logout = () => {
      Notify.create({
        type: 'positive',
        message: 'Wylogowano pomyślnie!',
        icon: 'check_circle',
      });
      authStore.logout();
      routerPushToLogin();
    };

    onMounted(() => {
      if (authStore.isQRScanner) {
        linksList.push({
          title: 'Skaner QR',
          caption: 'Funkcjonalność administracyjna',
          icon: 'qr_code_scanner',
          routerTo: '/qr-scanner',
          isBlank: false,
        });
      }
    });

    return {
      essentialLinks: linksList,
      leftDrawerOpen,
      loginDialogOpen,
      authStore,
      routerPushToLogin,
      logout,
      toggleLeftDrawer() {
        leftDrawerOpen.value = !leftDrawerOpen.value;
      },
    };
  },
});
</script>
<style lang="scss" scoped>
.qr-icon {
  color: rgba($accent, 0.7);
}
</style>
