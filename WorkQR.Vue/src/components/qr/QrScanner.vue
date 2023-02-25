<template>
  <div class="background" style="height: 94vh; width: 100vw">
    <q-card class="card" flat>
      <q-card-section style="padding: 0">
        <div class="q-mt-xl">
          <qrcode-stream :camera="camera" @decode="onDecode" class="video">
            <div class="time q-pa-sm">
              <q-btn
                round
                color="primary"
                icon="cameraswitch"
                @click="switchCamera"
                class="q-ma-xs"
                style="position: absolute"
              />
              <p class="text-subtitle1 text-center text-primary q-mb-none">
                {{ date }}
              </p>
              <p class="text-h2 text-center text-primary q-mb-xs q-my-none">
                {{ timestamp }}
              </p>
              <p
                class="text-body1 text-center text-primary q-mb-none"
                style="margin-bottom: 0"
              >
                Zeskanuj swój kod QR
              </p>
            </div>
          </qrcode-stream>
        </div>
      </q-card-section>
    </q-card>
  </div>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { QrcodeStream } from 'vue-qrcode-reader';
import { api } from 'boot/axios';
import { Notify } from 'quasar';

const days = [
  'Niedziela',
  'Poniedziałek',
  'Wtorek',
  'Środa',
  'Czwartek',
  'Piątek',
  'Sobota',
];

export default defineComponent({
  name: 'EssentialLink',
  components: {
    QrcodeStream,
  },
  setup() {
    const weekDays = days;
    const timestamp = ref('');
    const date = ref('');
    const camera = ref('front');
    const onDecode = async (string: any) => {
      try {
        debugger;
        await api.post('QRScanner/scan', null, {
          params: {
            qrAuthorizationKey: string,
          },
        });
      } catch (error: any) {
        Notify.create({
          type: 'negative',
          message: error.response.data,
          icon: 'error',
        });
      }
    };
    const getNow = () => {
      const today = new Date();
      const dateNow = `${
        weekDays[new Date().getDay()]
      }, ${new Date().toLocaleDateString()}`;
      const time = today.toLocaleTimeString('en-US', {
        hour12: false,
        hour: 'numeric',
        minute: 'numeric',
      });
      timestamp.value = time;
      date.value = dateNow;
    };
    const switchCamera = () => {
      switch (camera.value) {
        case 'front':
          camera.value = 'rear';
          break;
        case 'rear':
          camera.value = 'front';
          break;
      }
    };
    onMounted(() => {
      getNow();
      setInterval(getNow, 100000);
    });

    return {
      weekDays,
      timestamp,
      date,
      camera,
      getNow,
      onDecode,
      switchCamera,
    };
  },
});
</script>
<style lang="scss" scoped>
.time {
  background-color: rgba(0, 0, 0, 0.4);
}
.card {
  width: 100%;
  height: 100%;
}
.video {
  height: 80vh;
  width: 94vw;
  margin: 0 auto;
  border: 2px solid $primary;
  border-radius: 4px;
}
.background {
  background-color: $secondary;
}
</style>
