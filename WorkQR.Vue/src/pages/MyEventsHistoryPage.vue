<template>
  <q-page padding>
    <q-card dark bordered>
      <q-card-section>
        <div class="row">
          <span class="text-h4">Dane historyczne</span>
        </div>
      </q-card-section>
      <q-separator dark inset />
      <q-card-section>
        <worktime-events-calendar></worktime-events-calendar>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import WorktimeEventsCalendar from 'components/worktimeEvents/WorktimeEventsCalendar.vue';

export default defineComponent({
  name: 'MyEventsHistoryPage',
  components: { WorktimeEventsCalendar },
});
</script>
