<template>
  <q-page class="row items-center justify-evenly">
    <qr-scanner />
  </q-page>
</template>
<script lang="ts">
import QrScanner from 'components/qr/QrScanner.vue';
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'ScannerPage',
  components: { QrScanner },
});
</script>
