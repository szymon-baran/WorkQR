<template>
  <q-page class="row items-center justify-evenly background">
    <LoginForm />
  </q-page>
</template>

<script lang="ts">
import LoginForm from 'components/auth/LoginForm.vue';
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'LoginPage',
  components: { LoginForm },
});
</script>

<style lang="scss" scoped>
.background {
  background: linear-gradient(to right bottom, $primary, $secondary);
}
</style>
