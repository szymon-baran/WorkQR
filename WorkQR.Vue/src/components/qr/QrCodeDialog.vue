<template>
  <q-dialog>
    <q-card flat class="card q-pa-sm">
      <q-card-section class="row items-center q-pb-md">
        <div class="text-h6">Twój kod QR</div>
        <q-space />
        <q-btn icon="close" dense round color="primary" v-close-popup />
      </q-card-section>
      <q-card-section class="text-center">
        <div class="row q-mb-md flex-center">
          <q-btn
            icon="refresh"
            color="primary"
            label="Odnów kod"
            @click="authStore.setQRAuthorizationKey()"
          />
        </div>
        <qrcode-vue
          :value="authStore.getQrAuthorizationKey"
          :size="size"
          level="H"
          :foreground="foreground"
          :background="background"
          v-if="authStore.getQrAuthorizationKey"
        />
      </q-card-section>
      <q-card-section>
        <div class="row q-mb-md flex-center">
          <q-btn
            :icon="isHelpOpen ? 'expand_less' : 'expand_more'"
            color="primary"
            label="Kod nie działa?"
            :outline="isHelpOpen"
            @click="() => (isHelpOpen = !isHelpOpen)"
          />
        </div>
        <div class="" v-show="isHelpOpen">
          <p class="text-body-2 q-mb-none">
            <span class="text-weight-bold">Krok 1.</span> Spróbuj odnowić kod
            przyciskiem znajdującym się powyżej.
          </p>
          <p class="text-body-2 q-mb-none">
            <span class="text-weight-bold">Krok 2.</span> Jeżeli przeładowanie
            nie pomogło, wprowadź na skanerze następujący kod: 000 000.
          </p>
          <p class="text-body-2">
            <span class="text-weight-bold">Krok 3.</span> Skontaktuj się z
            moderatorem strony w swojej firmie.
          </p>
        </div>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import QrcodeVue from 'qrcode.vue';
import { colors } from 'quasar';
import { useAuthStore } from 'stores/auth-store';
const { getPaletteColor } = colors;

export default defineComponent({
  name: 'QrCodeDialog',
  components: {
    QrcodeVue,
  },
  setup() {
    const size = 260;
    const foreground = getPaletteColor('primary');
    const background = getPaletteColor('dark');
    const authStore = useAuthStore();
    const isHelpOpen = ref(false);
    onMounted(() => {
      if (!authStore.getQrAuthorizationKey) {
        authStore.setQRAuthorizationKey();
      }
    });
    return {
      size,
      foreground,
      background,
      authStore,
      isHelpOpen,
    };
  },
});
</script>
<style lang="scss" scoped>
.card {
  width: 400px;
  max-width: 90vw;
}
</style>
