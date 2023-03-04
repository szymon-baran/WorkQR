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
    <example-component
      title="Example component"
      active
      :todos="todos"
      :meta="meta"
    ></example-component>
  </q-page>
</template>

<script lang="ts">
import { Todo, Meta } from 'components/models';
import { defineComponent, ref } from 'vue';
import { useQuasar } from 'quasar';
import ExampleComponent from 'components/ExampleComponent.vue';
import QrCodeDialog from 'components/qr/QrCodeDialog.vue';

export default defineComponent({
  name: 'IndexPage',
  components: { ExampleComponent },
  setup() {
    const $q = useQuasar();
    const todos = ref<Todo[]>([
      {
        id: 1,
        content: 'ct1',
      },
      {
        id: 2,
        content: 'ct2',
      },
      {
        id: 3,
        content: 'ct3',
      },
      {
        id: 4,
        content: 'ct4',
      },
      {
        id: 5,
        content: 'ct5',
      },
    ]);
    const meta = ref<Meta>({
      totalCount: 1200,
    });
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
      todos,
      meta,
      showQrCodeDialog,
    };
  },
});
</script>
