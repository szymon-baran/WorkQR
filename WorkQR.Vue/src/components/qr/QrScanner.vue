<template>
  <div class="background" style="height: 94vh; width: 100vw">
    <q-card class="card" flat>
      <q-card-section style="padding: 0">
        <div class="time q-pa-sm q-mt-xs">
          <p class="text-h1 text-center text-primary">
            {{ timestamp }}
          </p>
          <p class="text-h5 text-center text-primary">
            {{ date }}
          </p>
        </div>
        <div class="q-mt-md">
          <qrcode-stream :camera="camera" @decode="onDecode" class="video">
            <q-btn
              round
              color="primary"
              icon="cameraswitch"
              @click="switchCamera"
              class="q-ma-sm"
            />
          </qrcode-stream>
        </div>
      </q-card-section>
    </q-card>
  </div>
</template>
<script lang="ts">
import { defineComponent, ref } from 'vue';
import { QrcodeStream } from 'vue-qrcode-reader';

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
  methods: {
    onDecode(string: any) {
      debugger;
      let str = string;
    },
    getNow() {
      const today = new Date();
      const date = `${
        this.weekDays[new Date().getDay()]
      }, ${new Date().toLocaleDateString()}`;
      const time = today.toLocaleTimeString('en-US', {
        hour12: false,
        hour: 'numeric',
        minute: 'numeric',
        second: 'numeric',
      });
      this.timestamp = time;
      this.date = date;
    },
    switchCamera() {
      switch (this.camera) {
        case 'front':
          this.camera = 'rear';
          break;
        case 'rear':
          this.camera = 'front';
          break;
      }
    },
  },
  setup() {
    const weekDays = days;
    const timestamp = ref('');
    const date = ref('');
    const camera = ref('front');
    return {
      weekDays,
      timestamp,
      date,
      camera,
    };
  },
  mounted() {
    setInterval(this.getNow, 1000);
  },
});
</script>
<style lang="scss" scoped>
.card {
  width: 100%;
  height: 100%;
}
.video {
  height: 64.5vh;
  width: 86vw;
  margin: 0 auto;
  border: 2px solid $primary;
  border-radius: 4px;
}
.background {
  background-color: $secondary;
}
</style>
