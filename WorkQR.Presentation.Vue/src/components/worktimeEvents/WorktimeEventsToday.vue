<template>
  <div class="row">
    <div class="col-lg-5 col-xs-12 q-mt-xl text-center text-h5 q-pa-lg">
      <span v-if="worktimeEventStore.getWorktimeEvents.length > 0">
        <div>
          Aktualnie
          <span class="text-weight-bold">{{ getCurrentTaskName() }}</span>
        </div>
        <span
          v-if="
            worktimeEventStore.getWorktimeEvents[0]['eventType'] !==
            eventTypes.END_WORK
          "
        >
          <div>od</div>
          <div class="header-font header-md q-mt-sm">
            {{ hours }} <span class="time-word">godz.</span> {{ minutes }}
            <span class="time-word">min.</span> {{ seconds }}
            <span class="time-word">sek.</span>
          </div>
        </span>
      </span>
    </div>
    <div class="col-lg-7 col-xs-12">
      <worktime-events-list :is-home-page="true" />
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted, onBeforeUnmount } from 'vue';
import { eventTypeEnum } from '../../enums/eventTypeEnum';
import { useWorktimeEventStore } from 'stores/worktime-event-store';
import WorktimeEventsList from './WorktimeEventsList.vue';

export default defineComponent({
  name: 'WorktimeEventsToday',
  components: {
    WorktimeEventsList,
  },
  setup() {
    const worktimeEventStore = useWorktimeEventStore();
    const interval = ref();
    const hours = ref();
    const minutes = ref();
    const seconds = ref();
    const eventTypes = eventTypeEnum;
    const calculateTimeSinceLastEvent = () => {
      if (
        worktimeEventStore.getWorktimeEvents.length > 0 &&
        worktimeEventStore.getWorktimeEvents[0]['eventType'] !==
          eventTypes.END_WORK
      ) {
        let difference =
          Date.now() -
          new Date(
            worktimeEventStore.getWorktimeEvents[
              worktimeEventStore.getWorktimeEvents.length - 1
            ]['eventTime']
          ).getTime();

        seconds.value = Math.floor(difference / 1000);
        minutes.value = Math.floor(seconds.value / 60);
        hours.value = Math.floor(minutes.value / 60);

        hours.value %= 24;
        minutes.value %= 60;
        seconds.value %= 60;
      }
    };
    const getCurrentTaskName = () => {
      switch (worktimeEventStore.getWorktimeEvents[0]['eventType']) {
        case eventTypes.START_WORK:
        case eventTypes.END_BREAK:
          return 'pracujesz';
        case eventTypes.START_BREAK:
          return 'jesteś na przerwie';
        case eventTypes.END_WORK:
          return 'nie pracujesz';
      }
    };
    onMounted(async () => {
      interval.value = setInterval(() => {
        calculateTimeSinceLastEvent();
      }, 1000);
      await worktimeEventStore.setUserWorktimeEventsToday();
    });
    onBeforeUnmount(() => {
      clearInterval(interval.value);
    });
    return {
      worktimeEventStore,
      interval,
      hours,
      minutes,
      seconds,
      eventTypes,
      calculateTimeSinceLastEvent,
      getCurrentTaskName,
    };
  },
});
</script>
<style lang="scss" scoped>
.time-word {
  opacity: 0.8;
  font-size: 70%;
  letter-spacing: 0.02rem;
}
</style>
