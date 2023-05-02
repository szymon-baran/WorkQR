<template>
  <div>
    <q-form class="q-mt-sm" @submit="onSubmit">
      <div class="row q-col-gutter-md">
        <div class="col-md-6 col-sm-12">
          <q-date
            range
            filled
            v-model="datesRange"
            label="Przedział czasu"
            mask="YYYY-MM-DD"
            style="width: 100%"
          />
        </div>
        <div class="col-md-6 col-sm-12">
          <p class="text-body1">
            Wybrano przedział od {{ datesRange.from }} do {{ datesRange.to }}
          </p>
          <q-btn label="Pobierz raport" type="submit" color="primary" />
        </div>
      </div>
    </q-form>
  </div>
</template>
<script setup lang="ts">
import { ref } from 'vue';
import { api } from 'boot/axios';

const now = new Date();
const datesRange = ref({
  from: new Date(
    now.getFullYear() - (now.getMonth() > 0 ? 0 : 1),
    (now.getMonth() - 1 + 12) % 12,
    1
  ).toLocaleDateString('en-CA'),
  to: new Date(now.getFullYear(), now.getMonth(), 0).toLocaleDateString(
    'en-CA'
  ),
});

const onSubmit = async () => {
  const response = await api.get('companyModeration/getCompanyRaportForDate', {
    params: {
      DateFrom: datesRange.value.from,
      DateTo: datesRange.value.to,
    },
    responseType: 'blob',
  });

  var fileURL = window.URL.createObjectURL(new Blob([response.data]));
  var fileLink = document.createElement('a');
  fileLink.href = fileURL;
  fileLink.setAttribute('download', 'raport.pdf');
  document.body.appendChild(fileLink);
  fileLink.click();
};
</script>
