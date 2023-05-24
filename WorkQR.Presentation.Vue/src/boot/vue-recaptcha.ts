import { boot } from 'quasar/wrappers';
import { VueRecaptchaPlugin } from 'vue-recaptcha/head';

export default boot(({ app }) => {
  app.use(VueRecaptchaPlugin, {
    v2SiteKey: '6Lfj-TcmAAAAAEQKKfOScq1ALmknmz1vaB-JyTdz',
  });
});
