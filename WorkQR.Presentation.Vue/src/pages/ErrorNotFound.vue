<template>
  <div
    class="fullscreen bg-negative text-white text-center q-pa-md flex flex-center"
  >
    <div>
      <div style="font-size: 30vh">404</div>

      <div class="text-h2" style="opacity: 0.4">Nie znaleziono strony</div>

      <q-btn
        class="q-mt-xl"
        color="white"
        text-color="negative"
        unelevated
        :to="{ name: 'Home' }"
        label="Strona główna"
        no-caps
      />
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'ErrorNotFound',
});
</script>
