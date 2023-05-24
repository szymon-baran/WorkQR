<template>
  <q-page padding>
    <q-card dark bordered flat>
      <q-card-section>
        <div class="row">
          <span class="text-h4 col-lg-11 col-xs-12">Strona główna</span>
          <q-btn
            color="primary"
            icon="qr_code_2"
            :label="$q.screen.lt.md ? '' : 'Kod QR'"
            :round="$q.screen.lt.md ? true : false"
            @click="showQrCodeDialog()"
            class="col-lg-1"
            :class="$q.screen.lt.md ? 'q-mt-sm' : ''"
          />
        </div>
      </q-card-section>
      <q-separator dark inset />
      <q-card-section
        style="max-width: 95vw; width: 100%; height: 44rem; max-height: 71vh"
      >
        <div>
          <worktime-events-today />
        </div>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useQuasar } from 'quasar';
import QrCodeDialog from 'components/qr/QrCodeDialog.vue';
import WorktimeEventsToday from 'components/worktimeEvents/WorktimeEventsToday.vue';

export default defineComponent({
  name: 'IndexPage',
  components: {
    WorktimeEventsToday,
  },
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
