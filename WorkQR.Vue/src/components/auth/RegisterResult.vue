<template>
  <div class="text-body1">Możesz teraz zalogować się na swoje konto.</div>
  <div v-if="isCompany">
    <div class="text-subtitle2 q-mt-sm">
      Najedź na pole poniżej aby zobaczyć dane do logowania się na konto skanera
      QR.
    </div>
    <div class="text-subtitle2">
      Zapisz je w bezpiecznym miejscu i nie udostępniaj niepowołanym osobom.
    </div>
    <div
      @mouseover="() => (isHovered = true)"
      @mouseleave="() => (isHovered = false)"
      class="bg-dark-page q-mt-xs"
    >
      <div :style="[isHovered ? '' : 'visibility: hidden;']">
        <div class="text-body-2">
          Nazwa użytkownika: {{ result.scannerUsername }}
        </div>
        <div class="text-body-2">Hasło: {{ result.scannerPassword }}</div>
      </div>
    </div>
  </div>
  <div class="text-body1 q-mt-sm">
    Życzymy przyjemnego korzystania z workQR!
  </div>
</template>
<script lang="ts">
import { defineComponent, ref, PropType } from 'vue';
import { CompanyRegisterResultDTO } from 'components/models';

export default defineComponent({
  name: 'RegisterResult',
  props: {
    result: {
      type: Object as PropType<CompanyRegisterResultDTO>,
      required: true,
    },
    isCompany: {
      type: Boolean,
      default: false,
    },
  },
  setup() {
    return {
      isHovered: ref(false),
    };
  },
});
</script>
