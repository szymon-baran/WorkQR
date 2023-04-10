<template>
  <div>
    <div style="width: 60vw; border: 1px solid black">
      <div class="row q-px-md q-py-sm head">
        <div class="col">
          <span
            class="header-font header-sm q-ml-sm text-dark text-h5 self-center"
            >workQR</span
          >
        </div>
        <div class="col text-right self-center text-dark text-subtitle2">
          IDENTYFIKATOR
        </div>
      </div>
      <div class="row q-px-lg q-py-md">
        <div class="col self-center">
          <div class="text-h4 text-dark">{{ authStore.getFullName }}</div>
          <div class="text-body2 text-dark">
            [Stanowisko] firmy [NazwaFirmy]
          </div>
        </div>
        <div class="col text-right">
          <img v-bind:src="image" />
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from 'vue';
import { useAuthStore } from 'stores/auth-store';

export default defineComponent({
  name: 'PrintQrCode',
  props: {
    image: {
      type: Object,
      required: true,
    },
  },
  setup() {
    const authStore = useAuthStore();
    return {
      authStore,
    };
  },
});
</script>
<style lang="scss" scoped>
.head {
  border-bottom: 1px solid black;
  text-shadow: none;
}
.head .header-font {
  text-shadow: none;
}
</style>
