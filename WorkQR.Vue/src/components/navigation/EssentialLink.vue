<template>
  <q-item
    clickable
    tag="a"
    :target="isBlank ? '_blank' : ''"
    :to="{ name: routerTo }"
    :href="isExternalLink ? link : undefined"
    class="q-my-sm text-center"
    style="width: 10rem"
  >
    <q-item-section>
      <div class="row self-center">
        <q-icon :name="icon" size="180%" class="" />
      </div>
      <div class="row self-center">
        <q-item-label class="q-mt-xs">{{ title.toUpperCase() }}</q-item-label>
      </div>
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

    isExternalLink: {
      type: Boolean,
      default: false,
    },
  },
});
</script>
