<template>
  <q-page padding>
    <div class="row">
      <span class="text-h4">Strona główna</span>
      <q-space />
      <q-btn
        color="primary"
        icon="qr_code_2"
        :label="$q.screen.lt.sm ? '' : 'Kod QR'"
        :round="$q.screen.lt.sm ? true : false"
        @click="showQrCodeDialog()"
      />
    </div>
    <worktime-events-calendar></worktime-events-calendar>
  </q-page>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useQuasar } from 'quasar';
import WorktimeEventsCalendar from 'components/worktimeEvents/WorktimeEventsCalendar.vue';
import QrCodeDialog from 'components/qr/QrCodeDialog.vue';

export default defineComponent({
  name: 'IndexPage',
  components: { WorktimeEventsCalendar },
  setup() {
    const $q = useQuasar();
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
      showQrCodeDialog,
    };
  },
});
</script>
