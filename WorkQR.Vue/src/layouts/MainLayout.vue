<template>
  <q-layout view="hHh lpR fFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="toggleLeftDrawer"
        />

        <q-toolbar-title class="q-ml-sm"
          ><q-icon name="qr_code_2" size="2.8rem" class="text-accent" />
          WorkQR
        </q-toolbar-title>

        <div>
          Witaj, Admin!
          <q-btn
            flat
            dense
            round
            icon="logout"
            aria-label="Wyloguj"
            class="q-ml-sm"
            @click="toggleLoginDialog"
          />
        </div>
      </q-toolbar>
    </q-header>

    <q-drawer v-model="leftDrawerOpen" show-if-above bordered>
      <q-list>
        <q-item-label header> Menu aplikacji </q-item-label>

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
import { defineComponent, ref } from 'vue';
import { useQuasar } from 'quasar';
import EssentialLink from 'components/EssentialLink.vue';
import LoginDialog from 'components/auth/LoginDialog.vue';

const linksList = [
  {
    title: 'Strona główna',
    caption: 'Zarządzaj swoim czasem pracy!',
    icon: 'home',
    link: '/',
    isBlank: false,
  },
  {
    title: 'Firma',
    caption: 'Informacje o współpracownikach',
    icon: 'work',
    link: '/',
    isBlank: false,
  },
  {
    title: 'Ustawienia',
    caption: 'Konfiguracja kodów QR',
    icon: 'settings',
    link: '/',
    isBlank: false,
  },
  {
    title: 'Github',
    caption: 'Odwiedź stronę projektu',
    icon: 'code',
    link: 'https://github.com/szymon-baran/WorkQR',
    separatorBefore: true,
  },
];

export default defineComponent({
  name: 'MainLayout',

  components: {
    EssentialLink,
  },

  setup() {
    const $q = useQuasar();
    const leftDrawerOpen = ref(false);
    const loginDialogOpen = ref(false);

    return {
      essentialLinks: linksList,
      leftDrawerOpen,
      loginDialogOpen,
      toggleLoginDialog() {
        $q.dialog({
          component: LoginDialog,

          // props forwarded to your custom component
          componentProps: {
            text: 'something',
            // ...more..props...
          },
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
      },
      toggleLeftDrawer() {
        leftDrawerOpen.value = !leftDrawerOpen.value;
      },
    };
  },
});
</script>
