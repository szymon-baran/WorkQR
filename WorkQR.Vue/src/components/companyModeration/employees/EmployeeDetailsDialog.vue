<template>
  <q-dialog>
    <q-card flat class="card q-pa-sm">
      <q-card-section class="row items-center q-pb-md">
        <div class="text-h6">Szczegóły użytkownika {{ username }}</div>
      </q-card-section>
      <q-card-section class="row items-center q-pb-md q-col-gutter-sm">
        <q-input
          dense
          filled
          v-model="worktimeEventStore.settings.DateFrom"
          mask="####-##-##"
          class="col"
          label="Data od"
          clearable
          @blur="worktimeEventStore.setCompanyEmployeeWorktimeEvents()"
        >
          <template v-slot:append>
            <q-icon name="event" class="pointer">
              <q-popup-proxy
                cover
                transition-show="scale"
                transition-hide="scale"
              >
                <q-date
                  v-model="worktimeEventStore.settings.DateFrom"
                  mask="YYYY-MM-DD"
                >
                  <div class="row items-center justify-end">
                    <q-btn v-close-popup label="Zamknij" color="primary" flat />
                  </div>
                </q-date>
              </q-popup-proxy>
            </q-icon>
          </template>
        </q-input>
        <q-input
          dense
          filled
          v-model="worktimeEventStore.settings.DateTo"
          mask="####-##-##"
          class="col"
          label="Data do"
          clearable
          @blur="worktimeEventStore.setCompanyEmployeeWorktimeEvents()"
        >
          <template v-slot:append>
            <q-icon name="event" class="pointer">
              <q-popup-proxy
                cover
                transition-show="scale"
                transition-hide="scale"
              >
                <q-date
                  v-model="worktimeEventStore.settings.DateTo"
                  mask="YYYY-MM-DD"
                >
                  <div class="row items-center justify-end">
                    <q-btn v-close-popup label="Zamknij" color="primary" flat />
                  </div>
                </q-date>
              </q-popup-proxy>
            </q-icon>
          </template>
        </q-input>
        <q-input
          dense
          filled
          v-model="worktimeEventStore.settings.Description"
          label="Opis"
          class="col"
          clearable
          @blur="worktimeEventStore.setCompanyEmployeeWorktimeEvents()"
        />
      </q-card-section>
      <q-card-section class="q-pb-md">
        <worktime-events-list
          :title="`Wydarzenia spełniające wybrane filtry`"
        />
      </q-card-section>
    </q-card>
  </q-dialog>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { useWorktimeEventStore } from 'stores/worktime-event-store';
import WorktimeEventsList from '../../worktimeEvents/WorktimeEventsList.vue';

export default defineComponent({
  name: 'EmployeeDetailsDialog',
  props: {
    userId: {
      type: String,
      required: true,
    },
    username: {
      type: String,
      required: true,
    },
  },
  components: {
    WorktimeEventsList,
  },
  setup(props) {
    const worktimeEventStore = useWorktimeEventStore();

    onMounted(async () => {
      worktimeEventStore.settings.UserId = props.userId;
      let date = new Date();
      date.setMonth(date.getMonth() - 1);
      worktimeEventStore.settings.DateFrom = date.toISOString().slice(0, 10);
      worktimeEventStore.settings.DateTo = new Date()
        .toISOString()
        .slice(0, 10);
    });
    return {
      worktimeEventStore,
    };
  },
});
</script>
<style lang="scss" scoped>
.card {
  width: 120em;
  height: 60em;
  max-width: 95vw;
  max-height: 80vh;
}
</style>
