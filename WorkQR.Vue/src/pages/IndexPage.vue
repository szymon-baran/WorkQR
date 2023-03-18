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
        <div>
          <worktime-events-today-list />
        </div>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useQuasar } from 'quasar';
import QrCodeDialog from 'components/qr/QrCodeDialog.vue';
import WorktimeEventsTodayList from 'components/worktimeEvents/WorktimeEventsTodayList.vue';

export default defineComponent({
  name: 'IndexPage',
  components: {
    WorktimeEventsTodayList,
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
