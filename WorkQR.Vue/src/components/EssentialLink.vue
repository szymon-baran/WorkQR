<template>
  <q-separator inset v-if="separatorBefore" class="q-my-md" />
  <q-item
    clickable
    tag="a"
    :target="isBlank ? '_blank' : ''"
    :to="{ name: routerTo }"
    :href="isExternalLink ? link : undefined"
    class="q-my-sm"
  >
    <q-item-section v-if="icon" avatar>
      <q-icon :name="icon" size="180%" />
    </q-item-section>

    <q-item-section>
      <q-item-label>{{ title }}</q-item-label>
      <q-item-label caption>{{ caption }}</q-item-label>
    </q-item-section>
  </q-item>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'EssentialLink',
  props: {
    title: {
      type: String,
      required: true,
    },

    caption: {
      type: String,
      default: '',
    },

    link: {
      type: String,
      default: '',
    },

    routerTo: {
      type: String,
      default: '',
    },

    icon: {
      type: String,
      default: '',
    },

    isBlank: {
      type: Boolean,
      default: true,
    },

    separatorBefore: {
      type: Boolean,
      default: false,
    },

    isExternalLink: {
      type: Boolean,
      default: false,
    },
  },
});
</script>
