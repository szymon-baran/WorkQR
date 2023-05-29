<template>
  <div style="height: 88vh; width: 100vw; margin-bottom: auto">
    <q-card class="card" flat>
      <q-card-section style="padding: 0">
        <p
          class="text-h4 text-center text-negative q-my-xs q-my-none"
          v-if="errorMsg"
        >
          {{ errorMsg }}
        </p>
        <div class="q-mt-xl">
          <qrcode-stream
            :camera="camera"
            @decode="onDecode"
            @init="onInit"
            class="video"
          >
            <div
              class="time q-pa-md"
              :class="[isResponseVisible ? 'response-color' : 'time']"
              style="height: 18%"
            >
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
            </div>
            <transition name="scale" mode="out-in">
              <div class="time q-pa-xs" v-if="!isResponseVisible">
                <p class="text-body1 text-center text-primary">
                  Zeskanuj swój kod QR
                </p>
              </div>
              <div class="response-color response q-pa-sm" v-else>
                <div>
                  <p class="text-h4 text-center text-primary q-my-lg">
                    Cześć, {{ responseEvent?.fullName }}!
                  </p>
                  <p
                    class="text-body1 text-center text-primary q-mb-xs q-mt-none"
                    v-if="responseEvent?.isOnVacation"
                  >
                    Aktualnie jesteś na urlopie, który potrwa do
                    {{
                      new Date(responseEvent?.vacationTo).toLocaleDateString()
                    }}. Miłego odpoczynku!
                  </p>
                </div>
                <div v-if="!responseEvent?.isOnVacation">
                  <p class="text-h5 text-center text-primary q-mb-xs q-mt-none">
                    {{ getResponseEventName() }}
                  </p>
                  <p
                    class="text-body1 text-center text-primary q-mb-xs q-mt-none"
                    v-if="responseEvent?.eventType !== eventTypes.END_WORK"
                  >
                    Pozostało Ci dzisiaj
                    {{
                      Math.round(responseEvent?.breakMinutesLeftToday * 1) / 1
                    }}
                    minut przerwy.
                  </p>
                </div>
                <div
                  class="row text-center"
                  v-if="!responseEvent?.isOnVacation"
                >
                  <div class="col-6 col-xs-12 q-mb-lg">
                    <q-btn
                      icon="cancel"
                      label="Anuluj wydarzenie"
                      size="lg"
                      color="primary"
                      outline
                      @click="cancelEvent()"
                    />
                  </div>
                  <div class="col-6 col-xs-12 q-mb-lg">
                    <q-btn
                      icon="house"
                      v-if="
                        responseEvent?.eventType === eventTypes.START_BREAK ||
                        responseEvent?.eventType === eventTypes.END_BREAK
                      "
                      label="Zakończ dzień pracy"
                      size="lg"
                      color="primary"
                      outline
                      @click="endWork()"
                    />
                  </div>
                </div>
              </div>
            </transition>
          </qrcode-stream>
        </div>
      </q-card-section>
    </q-card>
  </div>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted, onBeforeUnmount } from 'vue';
import { QrcodeStream } from 'vue-qrcode-reader';
import { api } from 'boot/axios';
import { Notify } from 'quasar';
import { eventTypeEnum } from '../../enums/eventTypeEnum';
import { EventScanDTO } from 'components/models';

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
    const isResponseVisible = ref(false);
    const intervalTimer = ref();
    const intervalResponse = ref();
    const responseEvent = ref<EventScanDTO>();
    const eventTypes = eventTypeEnum;
    const errorMsg = ref('');

    const onDecode = async (string: any) => {
      try {
        camera.value = 'off';
        const response = await api.post('QRScanner/scan', null, {
          params: {
            qrAuthorizationKey: string,
          },
        });
        isResponseVisible.value = true;
        responseEvent.value = response.data;
        intervalResponse.value = setInterval(() => {
          hideResponse();
        }, 7500);
      } catch (error: any) {
        Notify.create({
          type: 'negative',
          message: error.response.data,
          icon: 'error',
        });
      }
    };
    const onInit = async (promise: any) => {
      try {
        await promise;
      } catch (error) {
        if (error.name === 'NotAllowedError') {
          errorMsg.value = 'Brak uprawnień';
        } else if (error.name === 'NotFoundError') {
          errorMsg.value = 'Urządzenie nie posiada kamery';
        } else if (error.name === 'NotSupportedError') {
          errorMsg.value = 'Błąd HTTPS';
        } else if (error.name === 'NotReadableError') {
          errorMsg.value = 'Kamera w użyciu';
        } else if (error.name === 'OverconstrainedError') {
          errorMsg.value = 'Błąd kamery';
        } else if (error.name === 'StreamApiNotSupportedError') {
          errorMsg.value = 'Nieobsługiwana przeglądarka';
        } else if (error.name === 'InsecureContextError') {
          errorMsg.value = 'Błąd kontekstu HTTPS';
        } else {
          errorMsg.value = `Błąd: (${error.name})`;
        }
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
    const hideResponse = () => {
      clearInterval(intervalResponse.value);
      camera.value = 'front';
      isResponseVisible.value = false;
    };
    const getResponseEventName = () => {
      switch (responseEvent.value?.eventType) {
        case eventTypes.START_WORK:
          return 'Rozpoczęto nowy dzień pracy. Dobrego dnia!';
        case eventTypes.START_BREAK:
          return 'Rozpoczęto przerwę.';
        case eventTypes.END_BREAK:
          return 'Zakończono przerwę. Owocnej pracy!';
        case eventTypes.END_WORK:
          return 'Zakończono dzień pracy.';
      }
    };
    const cancelEvent = async () => {
      await api.post('QRScanner/cancelEvent', null, {
        params: {
          id: responseEvent.value?.id,
        },
      });
      hideResponse();
      Notify.create({
        type: 'positive',
        message: 'Operacja została anulowana pomyślnie!',
        icon: 'check_circle',
      });
    };
    const endWork = async () => {
      await api.post('QRScanner/endWork', null, {
        params: {
          id: responseEvent.value?.id,
        },
      });
      hideResponse();
      Notify.create({
        type: 'positive',
        message: 'Dzień pracy został zakończony!',
        icon: 'check_circle',
      });
    };
    onMounted(() => {
      getNow();
      intervalTimer.value = setInterval(() => {
        getNow();
      }, 1000);
    });
    onBeforeUnmount(() => {
      clearInterval(intervalTimer.value);
      clearInterval(intervalResponse.value);
    });

    return {
      weekDays,
      timestamp,
      date,
      camera,
      isResponseVisible,
      intervalTimer,
      intervalResponse,
      responseEvent,
      eventTypes,
      errorMsg,
      getNow,
      onDecode,
      onInit,
      switchCamera,
      getResponseEventName,
      cancelEvent,
      endWork,
    };
  },
});
</script>
<style lang="scss" scoped>
.time {
  background-color: rgba(0, 0, 0, 0.4);
}
.response-color {
  background-color: rgba(0, 0, 0, 0);
}
.response {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  height: 82%;
}
.card {
  width: 100%;
  height: 100%;
}
.video {
  height: 82vh;
  width: 100vw;
  margin: 0 auto;
  border: 3px solid $primary;
  border-radius: 4px;
}
</style>
