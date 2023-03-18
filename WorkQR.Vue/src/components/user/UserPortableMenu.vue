<template>
  <div class="row q-pa-md">
    <div class="column">
      <div class="text-h6 q-mb-md text-center">Akcje konta</div>
      <q-btn color="primary" icon="qr_code_2" @click="showQrCodeDialog()" />
      <q-btn color="primary" icon="settings" class="q-mt-sm" />
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

      <q-btn color="primary" label="Wyloguj" push size="sm" @click="logout" />
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from 'vue';
import { useQuasar } from 'quasar';
import { Notify } from 'quasar';
import { useRouter } from 'vue-router';
import { useAuthStore } from 'stores/auth-store';
import QrCodeDialog from 'components/qr/QrCodeDialog.vue';

export default defineComponent({
  name: 'UserPortableMenu',
  setup() {
    const $q = useQuasar();
    const router = useRouter();
    const authStore = useAuthStore();
    const logout = () => {
      Notify.create({
        type: 'positive',
        message: 'Wylogowano pomyślnie!',
        icon: 'check_circle',
      });
      authStore.logout();
      routerPushToLogin();
    };
    const routerPushToLogin = () => {
      router.push({ name: 'Login' });
    };
    const showQrCodeDialog = () => {
      $q.dialog({
        component: QrCodeDialog,
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
      authStore,
      router,
      logout,
      routerPushToLogin,
      showQrCodeDialog,
    };
  },
});
</script>
