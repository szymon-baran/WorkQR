<template>
  <div>
    <div style="width: 85.6mm; height: 54mm; border: 1px solid black">
      <div class="row q-px-sm q-py-xs head">
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
      <div class="row q-pt-xs">
        <div class="col q-px-md self-center">
          <div class="text-h5 text-dark q-mb-sm">
            {{
              companyUser
                ? `${companyUser.firstName} ${companyUser.lastName}`
                : authStore.getFullName
            }}
          </div>
          <div class="text-body2 text-dark">{{ companyUserPositionName }}</div>
          <div class="text-body2 text-dark">Januszex Industries</div>
        </div>
        <div style="border-left: 1px solid black"></div>
        <div class="col q-px-sm q-pt-sm self-center text-right">
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
    companyUser: {
      type: Object,
    },
    companyUserPositionName: {
      type: String,
      default: '',
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
.text-body2 {
  font-size: 0.8rem;
}
.head {
  border-bottom: 1px solid black;
  text-shadow: none;
}
.head .header-font {
  text-shadow: none;
}
</style>
