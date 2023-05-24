<template>
  <q-page padding>
    <q-card dark bordered flat>
      <q-card-section>
        <div class="row">
          <span class="text-h4">Historia wpisów</span>
        </div>
      </q-card-section>
      <q-separator dark inset />
      <q-card-section style="min-height: 44rem">
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
