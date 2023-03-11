<template>
  <div class="row q-mt-md">
    <q-btn color="primary" icon="arrow_back" @click="onPrev()" />
    <q-btn color="primary" class="q-ml-sm" label="Dzisiaj" @click="onToday()" />
    <q-space />
    <div class="pointer">
      <span class="text-body1"
        >Czas przepracowany w wybranym tygodniu:
        <span class="text-weight-bold">{{
          minutesToWorkedTime(workedMinutes + breakMinutes)
        }}</span
        >.</span
      >
      <q-tooltip class="text-caption">
        <div>
          Praca:
          <span class="text-weight-bold">{{
            minutesToWorkedTime(workedMinutes)
          }}</span>
        </div>
        <div>
          Przerwa:
          <span class="text-weight-bold">{{
            minutesToWorkedTime(breakMinutes)
          }}</span>
        </div>
      </q-tooltip>
    </div>
    <q-space />
    <q-input dense filled v-model="selectedDate" mask="####-##-##">
      <template v-slot:append>
        <q-icon name="event" class="cursor-pointer">
          <q-popup-proxy cover transition-show="scale" transition-hide="scale">
            <q-date v-model="selectedDate" mask="YYYY-MM-DD">
              <div class="row items-center justify-end">
                <q-btn v-close-popup label="Zamknij" color="primary" flat />
              </div>
            </q-date>
          </q-popup-proxy>
        </q-icon>
      </template>
    </q-input>
    <q-btn
      class="q-ml-sm"
      color="primary"
      icon="arrow_forward"
      @click="onNext()"
    />
  </div>
  <div class="row justify-center q-mt-xs">
    <div
      style="
        display: flex;
        max-width: 90vw;
        width: 100%;
        height: 38rem;
        max-height: 70vh;
      "
    >
      <q-calendar-day
        ref="calendar"
        v-model="selectedDate"
        view="week"
        animated
        transition-next="slide-left"
        transition-prev="slide-right"
        no-active-date
        :interval-start="7"
        :interval-count="14"
        :interval-height="50"
        :weekdays="[1, 2, 3, 4, 5, 6, 0]"
        :disabled-weekdays="[0, 6]"
        @change="onChange"
        locale="pl"
        :hour24-format="true"
        class="calendar"
      >
        <template #head-day-event="{ scope: { timestamp } }">
          <div
            style="
              display: flex;
              justify-content: center;
              flex-wrap: wrap;
              padding: 2px;
            "
          >
            <template
              v-for="event in eventsMap[timestamp.date]"
              :key="event.id"
            >
              <q-badge
                v-if="!event.time"
                :class="badgeClasses(event, 'header')"
                :style="badgeStyles(event, 'header')"
                style="
                  width: 100%;
                  cursor: pointer;
                  height: 12px;
                  font-size: 10px;
                  margin: 1px;
                "
              >
                <span class="title q-calendar__ellipsis">
                  {{ event.title }}
                  <q-tooltip>{{ event.details }}</q-tooltip>
                </span>
              </q-badge>
              <q-badge
                v-else
                :class="badgeClasses(event, 'header')"
                :style="badgeStyles(event, 'header')"
                style="
                  margin: 1px;
                  width: 10px;
                  max-width: 10px;
                  height: 10px;
                  max-height: 10px;
                "
                @click="scrollToEvent(event)"
              >
                <q-tooltip>{{ event.time + ' - ' + event.header }}</q-tooltip>
              </q-badge>
            </template>
          </div>
        </template>

        <template
          #day-body="{ scope: { timestamp, timeStartPos, timeDurationHeight } }"
        >
          <template v-for="event in getEvents(timestamp.date)" :key="event.id">
            <div
              v-if="event.time !== undefined"
              class="my-event"
              :class="badgeClasses(event, 'body')"
              :style="
                badgeStyles(event, 'body', timeStartPos, timeDurationHeight)
              "
            >
              <span class="title q-calendar__ellipsis text-weight-bold">
                {{ event.title }}
                <q-tooltip>
                  <div class="text-caption text-weight-bold">
                    {{ event.header }} ({{ event.duration.toFixed(2) }} min)
                  </div>
                  <div class="text-caption">{{ event.details }}</div>
                </q-tooltip>
              </span>
            </div>
          </template>
        </template>
      </q-calendar-day>
    </div>
  </div>
</template>
<script>
import {
  QCalendarDay,
  addToDate,
  parseTimestamp,
  isBetweenDates,
  today,
  parsed,
  parseTime,
} from '@quasar/quasar-ui-qcalendar/src/QCalendarDay.js';
import '@quasar/quasar-ui-qcalendar/src/QCalendarVariables.sass';
import '@quasar/quasar-ui-qcalendar/src/QCalendarTransitions.sass';
import '@quasar/quasar-ui-qcalendar/src/QCalendarDay.sass';

import { defineComponent, ref } from 'vue';
import { api } from 'boot/axios';
import { colors } from 'quasar';
const { getPaletteColor } = colors;

export default defineComponent({
  name: 'WorktimeEventsCalendar',
  components: {
    QCalendarDay,
  },
  setup() {
    const workedMinutes = ref(-1);
    const breakMinutes = ref(-1);
    const minutesToWorkedTime = (minutes) =>
      minutes
        ? `${Math.floor(minutes / 60)}:${(minutes % 60)
            .toFixed(0)
            .padEnd(2, '0')}`
        : '0:00';
    return {
      events: ref([]),
      selectedDate: ref(today()),
      workedMinutes,
      breakMinutes,
      minutesToWorkedTime,
    };
  },

  computed: {
    // convert the events into a map of lists keyed by date
    eventsMap() {
      const map = {};
      // this.events.forEach(event => (map[ event.date ] = map[ event.date ] || []).push(event))
      this.events.forEach((event) => {
        if (!map[event.date]) {
          map[event.date] = [];
        }
        map[event.date].push(event);
        if (event.days) {
          let timestamp = parseTimestamp(event.date);
          let days = event.days;
          do {
            timestamp = addToDate(timestamp, { day: 1 });
            if (!map[timestamp.date]) {
              map[timestamp.date] = [];
            }
            map[timestamp.date].push(event);
          } while (--days > 0);
        }
      });
      return map;
    },
  },

  methods: {
    badgeClasses(event, type) {
      const isHeader = type === 'header';
      return {
        'full-width': !isHeader && (!event.side || event.side === 'full'),
        'rounded-border': true,
      };
    },

    badgeStyles(
      event,
      type,
      timeStartPos = undefined,
      timeDurationHeight = undefined
    ) {
      const s = {};
      if (timeStartPos && timeDurationHeight) {
        s.top = timeStartPos(event.time) + 'px';
        s.height = timeDurationHeight(event.duration) + 'px';
      }
      s['align-items'] = 'flex-start';
      s['background-color'] = event.bgcolor;
      s['color'] = getPaletteColor('dark');
      return s;
    },

    getEvents(dt) {
      // get all events for the specified date
      const events = this.eventsMap[dt] || [];

      if (events.length === 1) {
        events[0].side = 'full';
      }

      return events;
    },

    scrollToEvent(event) {
      this.$refs.calendar.scrollToTime(event.time, 350);
    },

    onToday() {
      this.$refs.calendar.moveToToday();
    },
    onPrev() {
      this.$refs.calendar.prev();
    },
    onNext() {
      this.$refs.calendar.next();
    },

    async onChange(data) {
      const response = await api.get(
        'worktimeEvent/getUserWorktimeEventsBetweenDates',
        {
          params: {
            DateFrom: new Date(data.start).toISOString(),
            DateTo: new Date(data.end).toISOString(),
          },
        }
      );
      this.events = response.data.timestamps;
      this.workedMinutes = response.data.workedMinutes;
      this.breakMinutes = response.data.breakMinutes;
    },
  },
});
</script>
<style lang="scss" scoped>
.my-event {
  position: absolute;
  font-size: 12px;
  justify-content: center;
  margin: 0 1px;
  text-overflow: ellipsis;
  overflow: hidden;
  cursor: pointer;
}
.title {
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
}
.full-width {
  left: 0;
  width: calc(100% - 2px);
}
.rounded-border {
  border-radius: 5px;
}
--calendar-border-current-dark {
  color: $primary;
}
.calendar {
  --calendar-border-dark: black 1px solid;
  --calendar-background-dark: #2a2a2a;
  --calendar-disabled-date-background: #201f1f;
  --calendar-border-current-dark: #f0ac00 2px solid;
  padding: 5px;
}
.pointer {
  cursor: default;
}
</style>
