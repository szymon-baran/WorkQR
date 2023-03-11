import { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      {
        name: 'Home',
        path: 'home',
        component: () => import('pages/IndexPage.vue'),
      },
      {
        name: 'QRScanner',
        path: 'qr-scanner',
        component: () => import('pages/ScannerPage.vue'),
        meta: {
          requiresQrScanner: true,
        },
      },
      {
        name: 'MyEventsHistory',
        path: 'events-history',
        component: () => import('pages/MyEventsHistoryPage.vue'),
      },
    ],
  },
  {
    path: '/login',
    component: () => import('layouts/LoginLayout.vue'),
    children: [
      {
        name: 'Login',
        path: '',
        component: () => import('pages/LoginPage.vue'),
        meta: {
          requiresNonAuth: true,
        },
      },
    ],
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
  },
];

export default routes;
