<template>
  <transition
    enter-active-class="animated fadeIn"
    leave-active-class="animated fadeOut"
    appear
    :duration="300"
  >
    <div :key="$route.path">
      <router-view />
    </div>
  </transition>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'App',
});
</script>
