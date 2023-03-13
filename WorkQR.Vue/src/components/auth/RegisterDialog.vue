<template>
  <q-dialog>
    <q-card flat class="card q-pa-sm">
      <q-card-section class="row items-center q-pb-md">
        <div class="text-h6">Rejestracja</div>
        <q-space />
        <q-btn icon="close" dense round color="primary" v-close-popup />
      </q-card-section>
      <q-card-section class="q-pb-lg">
        <div class="row q-col-gutter-md items-stretch q-px-sm">
          <div
            class="col-6 pointer"
            @mouseover="() => (isLeftHovered = true)"
            @mouseleave="() => (isLeftHovered = false)"
          >
            <q-img src="~assets/employer.jpeg" height="100%">
              <div class="absolute-bottom text-subtitle1 text-center item-card">
                <p class="text-subtitle1 q-mt-md">Firma</p>
                <div class="text-body2" v-if="isLeftHovered">
                  <p class="q-my-none">
                    Chcesz wykorzystać możliwości aplikacji w swojej firmie?
                  </p>
                  <p class="q-mt-none q-mb-xl">Załóż konto tutaj!</p>
                </div>
              </div>
            </q-img>
          </div>
          <div
            class="col-6 pointer"
            @mouseover="() => (isRightHovered = true)"
            @mouseleave="() => (isRightHovered = false)"
          >
            <q-img src="~assets/worker.jpeg" height="100%">
              <div class="absolute-bottom text-subtitle1 text-center item-card">
                <p class="text-subtitle1 q-mt-md">Pracownik</p>
                <div class="text-body2" v-if="isRightHovered">
                  <p class="q-my-none">
                    Posiadasz kod rejestracyjny od swojego pracodawcy?
                  </p>
                  <p class="q-mt-none q-mb-xl">Załóż konto tutaj!</p>
                </div>
              </div>
            </q-img>
          </div>
        </div>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>
<script lang="ts">
import { defineComponent, ref } from 'vue';
export default defineComponent({
  name: 'RegisterDialog',
  setup() {
    return {
      isLeftHovered: ref(false),
      isRightHovered: ref(false),
    };
  },
});
</script>
<style lang="scss" scoped>
.card {
  width: 900px;
  max-width: 95vw;
}
</style>
