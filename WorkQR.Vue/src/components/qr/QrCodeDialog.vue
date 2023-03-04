<template>
  <q-dialog>
    <q-card flat class="card q-pa-sm">
      <q-card-section class="row items-center q-pb-md">
        <div class="text-h6">Twój kod QR</div>
        <q-space />
        <q-btn icon="close" dense round color="primary" v-close-popup />
      </q-card-section>
      <q-card-section class="text-center">
        <qrcode-vue
          :value="value"
          :size="size"
          level="H"
          :foreground="foreground"
          :background="background"
        />
      </q-card-section>
      <q-card-section class="text-center">
        <p class="text-body-2 q-mb-none">
          Natknąłeś się na problem z działaniem kodu?
        </p>
        <p class="text-body-2">Skontaktuj się z prezesem swojej firmy.</p>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>
<script lang="ts">
import { defineComponent, ref } from 'vue';
import QrcodeVue from 'qrcode.vue';
import { colors } from 'quasar';
const { getPaletteColor } = colors;

export default defineComponent({
  name: 'QrCodeDialog',
  components: {
    QrcodeVue,
  },
  setup() {
    const size = 300;
    const foreground = getPaletteColor('primary');
    const background = getPaletteColor('dark');
    return {
      value: ref('https://google.com'),
      size,
      foreground,
      background,
    };
  },
});
</script>
<style lang="scss" scoped>
.card {
  width: 400px;
  max-width: 70vw;
}
</style>
