<template>
  <q-dialog ref="dialog">
    <q-card flat class="card q-pa-sm">
      <q-card-section class="row items-center q-pb-md">
        <div class="text-h6">{{ getTitle() }}</div>
        <q-space />
        <q-btn icon="close" dense round color="primary" v-close-popup />
      </q-card-section>
      <q-card-section class="q-pb-lg">
        <div v-if="!isRegisterCompany && !isActivateEmployee">
          <div class="row text-body1 q-px-sm q-mb-sm">
            Wybierz swój typ konta
          </div>
          <div class="row q-col-gutter-md items-stretch q-px-sm">
            <div
              class="col-6 pointer"
              @mouseover="() => (isLeftHovered = true)"
              @mouseleave="() => (isLeftHovered = false)"
              @click="isRegisterCompany = true"
            >
              <q-img src="~assets/employer.jpeg" height="100%">
                <div
                  class="absolute-bottom text-subtitle1 text-center item-card"
                >
                  <p class="text-subtitle1 q-mt-md">Firma</p>
                  <div class="text-body2" v-if="isLeftHovered">
                    <p class="q-my-none">
                      Chcesz korzystać z aplikacji w swojej firmie?
                    </p>
                    <p class="q-mt-none q-mb-xl">Załóż konto tutaj!</p>
                  </div>
                </div>
              </q-img>
            </div>
            <div
              class="col-6 pointer"
              @mouseover="() => (isRightHovered = true)"
              @mouseleave="() => (isRightHovered = false)"
              @click="isActivateEmployee = true"
            >
              <q-img src="~assets/worker.jpeg" height="100%">
                <div
                  class="absolute-bottom text-subtitle1 text-center item-card"
                >
                  <p class="text-subtitle1 q-mt-md">Pracownik</p>
                  <div class="text-body2" v-if="isRightHovered">
                    <p class="q-my-none">
                      Posiadasz kod rejestracyjny od swojego pracodawcy?
                    </p>
                    <p class="q-mt-none q-mb-xl">Załóż konto tutaj!</p>
                  </div>
                </div>
              </q-img>
            </div>
          </div>
        </div>
        <div v-else-if="isRegisterCompany">
          <register-company-form
            @success="onCompanyRegister"
            v-if="!registerResult"
          />
          <register-result
            :result="registerResult"
            :is-company="isRegisterCompany"
            v-else
          />
        </div>
        <div v-else-if="isActivateEmployee">
          <activate-employee-form @success="onEmployeeActivated" />
        </div>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>
<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { CompanyRegisterResultDTO } from 'components/models';
import RegisterCompanyForm from './RegisterCompanyForm.vue';
import ActivateEmployeeForm from './ActivateEmployeeForm.vue';
import RegisterResult from './RegisterResult.vue';
export default defineComponent({
  name: 'RegisterDialog',
  components: {
    RegisterCompanyForm,
    ActivateEmployeeForm,
    RegisterResult,
  },
  setup() {
    const isRegisterCompany = ref(false);
    const isActivateEmployee = ref(false);
    const registerResult = ref();
    const route = useRoute();
    const dialog = ref(null);
    const getTitle = () => {
      if (registerResult.value) {
        return 'Rejestracja zakończona pomyślnie!';
      } else if (isRegisterCompany.value) {
        return 'Zarejestruj firmę';
      } else if (isActivateEmployee.value) {
        return 'Aktywuj konto';
      }
      return 'Zarejestruj się';
    };
    const onCompanyRegister = (response: CompanyRegisterResultDTO) => {
      registerResult.value = response;
    };
    const onEmployeeActivated = () => {
      dialog.value.hide();
    };
    onMounted(() => {
      if (route.params['regCode']) {
        isActivateEmployee.value = true;
      }
    });
    return {
      isLeftHovered: ref(false),
      isRightHovered: ref(false),
      isMenu: ref(false),
      isRegisterCompany,
      isActivateEmployee,
      registerResult,
      dialog,
      getTitle,
      onCompanyRegister,
      onEmployeeActivated,
    };
  },
});
</script>
<style lang="scss" scoped>
.card {
  width: 900px;
  max-width: 95vw;
}
</style>
