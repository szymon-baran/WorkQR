import { Notify } from 'quasar';

Notify.setDefaults({
  position: 'top',
  timeout: 2500,
  textColor: '#f5f5f5',
  progress: true,
  actions: [{ icon: 'close', color: 'white' }],
});
