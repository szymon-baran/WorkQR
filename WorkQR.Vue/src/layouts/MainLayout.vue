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
          v-if="$q.screen.lt.md && $route.name !== 'QRScanner'"
        />

        <q-toolbar-title class="q-ml-sm q-mt-xs" shrink
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
        <q-space />
        <div class="row" v-if="!$q.screen.lt.md && $route.name !== 'QRScanner'">
          <EssentialLink
            v-for="link in essentialLinks"
            :key="link.title"
            v-bind="link"
          />
        </div>
        <q-space />
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
                :label="$q.screen.lt.md ? '' : authStore.getFullName"
                cover
              >
                <user-portable-menu />
              </q-btn-dropdown>
            </q-chip>
          </span>
        </div>
      </q-toolbar>
    </q-header>

    <q-drawer
      v-model="leftDrawerOpen"
      v-show="$route.name !== 'QRScanner'"
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
        <EssentialLinkMobile
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
import { useAuthStore } from '../stores/auth-store';
import EssentialLink from 'src/components/navigation/EssentialLink.vue';
import EssentialLinkMobile from 'src/components/navigation/EssentialLinkMobile.vue';
import UserPortableMenu from 'src/components/user/UserPortableMenu.vue';

const linksList = [
  {
    title: 'Strona główna',
    caption: 'Zarządzaj swoim czasem pracy',
    icon: 'home',
    routerTo: 'Home',
    isBlank: false,
  },
  {
    title: 'Historia',
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
    EssentialLinkMobile,
    UserPortableMenu,
  },

  setup() {
    const $q = useQuasar();
    const leftDrawerOpen = ref(false);
    const loginDialogOpen = ref(false);
    const miniState = ref(true);
    const authStore = useAuthStore();
    const essentialLinks = ref(linksList);
    onMounted(() => {
      if (authStore.isQRScanner) {
        const scannerLink = {
          title: 'Skaner QR',
          caption: 'Funkcjonalność administracyjna',
          icon: 'qr_code_scanner',
          routerTo: 'QRScanner',
          isBlank: false,
        };
        if (!essentialLinks.value.some((x) => x.routerTo === 'QRScanner')) {
          essentialLinks.value.push(scannerLink);
        }
      }
    });

    return {
      essentialLinks,
      leftDrawerOpen,
      loginDialogOpen,
      miniState,
      authStore,
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
