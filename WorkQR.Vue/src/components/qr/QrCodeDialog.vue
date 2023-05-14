<template>
  <q-dialog>
    <q-card flat class="card q-pa-sm">
      <q-card-section class="row items-center q-pb-md">
        <div class="text-h6">{{ getTitle() }}</div>
        <q-space />
        <q-btn icon="close" dense round color="primary" v-close-popup />
      </q-card-section>
      <q-card-section class="text-center">
        <div class="row q-mb-md">
          <div class="col">
            <q-btn
              icon="refresh"
              color="primary"
              label="Odnów"
              @click="authStore.setQRAuthorizationKey()"
            />
          </div>
          <div class="col">
            <q-btn
              icon="print"
              color="primary"
              label="Drukuj"
              @click="printPdf()"
            />
          </div>
        </div>
        <div class="row q-mb-md flex-center" v-if="companyUser">
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
          v-if="
            authStore.getQrAuthorizationKey ||
            (companyUser && companyUser.qrAuthorizationKey)
          "
        />
        <div ref="qrCodeImage">
          <qrcode-vue
            :value="getQrCode"
            :size="size * 0.53"
            level="H"
            v-if="
              authStore.getQrAuthorizationKey ||
              (companyUser && companyUser.qrAuthorizationKey)
            "
            v-show="false"
          />
        </div>
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
              <span class="text-weight-bold">Krok 1.</span> Dostosuj jasność na
              swoim urządzeniu, tak aby kod był wyraźnie widoczny na podglądzie.
            </p>
            <p class="text-body-2 q-mb-none">
              <span class="text-weight-bold">Krok 2.</span> Odnów kod
              przyciskiem znajdującym się powyżej.
            </p>
            <p class="text-body-2">
              <span class="text-weight-bold">Krok 3.</span> Skontaktuj się z
              moderatorem strony w swojej firmie.
            </p>
          </div>
        </transition>
      </q-card-section>
    </q-card>
    <print-qr-code
      :image="getImage"
      :companyUser="companyUser"
      :companyUserPositionName="companyUserPositionName"
      ref="printComponent"
      v-show="false"
    />
  </q-dialog>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted, computed } from 'vue';
import QrcodeVue from 'qrcode.vue';
import { useQuasar, colors } from 'quasar';
import { useAuthStore } from 'stores/auth-store';
import PrintQrCode from './PrintQrCode.vue';

const { getPaletteColor } = colors;

export default defineComponent({
  name: 'QrCodeDialog',
  props: {
    companyUser: {
      type: Object,
    },
    companyUserPositionName: {
      type: String,
    },
  },
  components: {
    QrcodeVue,
    PrintQrCode,
  },
  setup(props) {
    const size = 260;
    const foreground = getPaletteColor('primary') as string;
    const background = getPaletteColor('dark');
    const authStore = useAuthStore();
    const isHelpOpen = ref(false);
    const qrCode = ref();
    const qrCodeImage = ref(null);
    const printComponent = ref(null);
    const $q = useQuasar();
    const getQrCode = computed(
      () => qrCode.value ?? authStore.getQrAuthorizationKey
    );
    const getTitle = () => {
      return props.companyUser
        ? `Kod QR ${props.companyUser.username}`
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
        if (props.companyUser) {
          const response = await authStore.resetQRAuthorizationKey(
            props.companyUser.id
          );
          qrCode.value = response;
        }
      });
    };
    const getImage = computed(() => {
      return qrCodeImage.value
        ? qrCodeImage.value
            .getElementsByTagName('canvas')[0]
            .toDataURL('image/png')
        : '';
    });
    const printPdf = () => {
      let stylesHtml = '';
      for (const node of [
        ...document.querySelectorAll('link[rel="stylesheet"], style'),
      ]) {
        stylesHtml += node.outerHTML;
      }
      let w = window.open(
        '',
        '',
        'left=0,top=0,width=800,height=900,toolbar=0,scrollbars=0,status=0'
      );
      if (w && printComponent.value) {
        //w.document.write(printComponent.value.$el.innerHTML);
        w.document.write(`<!DOCTYPE html>
          <html>
            <head>
              ${stylesHtml}
            </head>
            <body>
              ${printComponent.value.$el.innerHTML}
            </body>
          </html>`);
        w.document.close();
        w.setTimeout(function () {
          if (w) w.print();
        }, 1000);
      }
    };
    onMounted(() => {
      if (props.companyUser) {
        qrCode.value = props.companyUser.qrAuthorizationKey;
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
      qrCodeImage,
      printComponent,
      getQrCode,
      getImage,
      getTitle,
      confirmCodeReset,
      printPdf,
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
