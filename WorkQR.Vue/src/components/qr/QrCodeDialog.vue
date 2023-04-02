<template>
  <q-dialog>
    <q-card flat class="card q-pa-sm">
      <q-card-section class="row items-center q-pb-md">
        <div class="text-h6">{{ getTitle() }}</div>
        <q-space />
        <q-btn icon="close" dense round color="primary" v-close-popup />
      </q-card-section>
      <q-card-section class="text-center">
        <div class="row q-mb-md flex-center">
          <q-btn
            icon="refresh"
            color="primary"
            label="Przeładuj kod"
            @click="authStore.setQRAuthorizationKey()"
          />
        </div>
        <div class="row q-mb-md flex-center" v-if="companyUsername">
          <q-btn
            icon="restart_alt"
            color="primary"
            label="Wygeneruj nowy kod"
            @click="confirmCodeReset()"
          />
        </div>
        <qrcode-vue
          :value="getQrCode"
          :size="size"
          level="H"
          :foreground="foreground"
          :background="background"
          v-if="authStore.getQrAuthorizationKey || companyUserCode"
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
        <transition
          enter-active-class="animated fadeIn"
          leave-active-class="animated fadeOut"
          appear
          :duration="200"
        >
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
        </transition>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted, computed } from 'vue';
import QrcodeVue from 'qrcode.vue';
import { useQuasar, colors } from 'quasar';
import { useAuthStore } from 'stores/auth-store';
const { getPaletteColor } = colors;

export default defineComponent({
  name: 'QrCodeDialog',
  props: {
    companyUserId: {
      type: String,
      default: '',
    },
    companyUsername: {
      type: String,
      default: '',
    },
    companyUserCode: {
      type: String,
      default: '',
    },
  },
  components: {
    QrcodeVue,
  },
  setup(props) {
    const size = 260;
    const foreground = getPaletteColor('primary');
    const background = getPaletteColor('dark');
    const authStore = useAuthStore();
    const isHelpOpen = ref(false);
    const qrCode = ref();
    const $q = useQuasar();
    const getQrCode = computed(
      () => qrCode.value ?? authStore.getQrAuthorizationKey
    );
    const getTitle = () => {
      return props.companyUsername
        ? `Kod QR ${props.companyUsername}`
        : 'Twój kod QR';
    };
    const confirmCodeReset = () => {
      $q.dialog({
        title: 'Wygeneruj nowy kod',
        message:
          'Czy na pewno chcesz wygenerować nowy kod? Po wykonaniu tej operacji stary kod przestanie działać!',
        cancel: true,
        persistent: true,
        focus: 'cancel',
      }).onOk(async () => {
        const response = await authStore.resetQRAuthorizationKey(
          props.companyUserId
        );
        qrCode.value = response;
      });
    };
    onMounted(() => {
      if (props.companyUserCode) {
        qrCode.value = props.companyUserCode;
      } else if (!authStore.getQrAuthorizationKey) {
        authStore.setQRAuthorizationKey();
      }
    });
    return {
      size,
      foreground,
      background,
      authStore,
      isHelpOpen,
      qrCode,
      getQrCode,
      getTitle,
      confirmCodeReset,
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
