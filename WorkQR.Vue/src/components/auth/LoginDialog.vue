<template>
  <q-dialog position="top">
    <LoginForm />
  </q-dialog>
</template>
<script lang="ts">
import { defineComponent } from 'vue';
import LoginForm from './LoginForm.vue';

export default defineComponent({
  name: 'LoginDialog',
  components: { LoginForm },
});
</script>
