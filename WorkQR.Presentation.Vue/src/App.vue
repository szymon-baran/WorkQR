<template>
  <router-view v-slot="{ Component }">
    <transition name="scale" mode="out-in">
      <component :is="Component" />
    </transition>
  </router-view>
</template>

<script setup lang="ts">
import 'cal-sans';
import { useRecaptchaProvider } from 'vue-recaptcha/head';
useRecaptchaProvider();
</script>
