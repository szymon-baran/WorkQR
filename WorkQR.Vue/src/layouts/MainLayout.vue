<template>
  <q-layout view="hHh Lpr lff">
    <q-header>
      <q-toolbar
        class="bg-secondary q-px-md q-py-xs"
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
          ><q-img
            src="~assets/logo.png"
            width="3rem"
            class="logo-sm"
            no-spinner
          />
          <span class="header-font header-sm q-ml-sm text-primary text-h5"
            >workQR</span
          >
        </q-toolbar-title>
        <div
          v-if="authStore.isUserAuthenticated && $route.name !== 'QRScanner'"
        >
          <span class="pointer">
            <q-chip color="dark-page" :clickable="false">
              <q-avatar color="primary" text-color="white">{{
                authStore.getInitials
              }}</q-avatar>
              <q-btn-dropdown
                color="primary"
                size="md"
                dense
                flat
                :label="authStore.getFullName"
                cover
              >
                <div class="row q-pa-md">
                  <div class="column">
                    <div class="text-h6 q-mb-md text-center">Opcje</div>
                    <q-btn
                      color="primary"
                      icon="qr_code_2"
                      label="Kod QR"
                      size="sm"
                    />
                    <q-btn
                      color="primary"
                      icon="settings"
                      label="Ustawienia"
                      size="sm"
                      class="q-mt-sm"
                    />
                  </div>

                  <q-separator vertical inset class="q-mx-md" />

                  <div class="column items-center">
                    <q-avatar color="primary" text-color="white" size="4rem">{{
                      authStore.getInitials
                    }}</q-avatar>

                    <div class="text-subtitle1 q-mt-md">
                      {{ authStore.getUserName }}
                    </div>

                    <div class="text-subtitle2 q-mb-md text-center">
                      {{ authStore.getFullName }}
                    </div>

                    <q-btn
                      color="primary"
                      label="Wyloguj"
                      push
                      size="sm"
                      @click="logout"
                    />
                  </div>
                </div>
              </q-btn-dropdown>
            </q-chip>
          </span>
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
      :width="220"
    >
      <!-- <q-drawer
      v-model="leftDrawerOpen"
      show-if-above
      v-if="$route.name !== 'QRScanner'"
      :mini="miniState"
      @mouseover="miniState = false"
      @mouseout="miniState = true"
      mini-to-overlay
    > -->
      <q-list>
        <div></div>
        <EssentialLink
          v-for="link in essentialLinks"
          :key="link.title"
          v-bind="link"
        />
      </q-list>
    </q-drawer>

    <q-page-container>
      <router-view v-slot="{ Component }">
        <transition name="scale" mode="out-in">
          <component :is="Component" />
        </transition>
      </router-view>
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
    routerTo: 'Home',
    isBlank: false,
  },
  {
    title: 'Dane historyczne',
    caption: 'Sprawdź ile godzin przepracowałeś',
    icon: 'history',
    routerTo: 'MyEventsHistory',
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
    title: 'Ustawienia',
    caption: 'Zmień dane swojego konta',
    icon: 'settings',
    routerTo: '/',
    isBlank: false,
  },
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
    const miniState = ref(true);
    const isUserMenuVisible = ref(false);
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
      miniState,
      isUserMenuVisible,
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
