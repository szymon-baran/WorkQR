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
        meta: {
          title: 'Strona główna | WorkQR',
        },
      },
      {
        name: 'MyEventsHistory',
        path: 'events-history',
        component: () => import('pages/MyEventsHistoryPage.vue'),
        meta: {
          title: 'Historia | WorkQR',
        },
      },
      {
        name: 'QRScanner',
        path: 'qr-scanner',
        component: () => import('pages/ScannerPage.vue'),
        meta: {
          title: 'Skaner QR | WorkQR',
          requiresQrScanner: true,
        },
      },
      {
        name: 'CompanyManagement',
        path: 'company-management',
        component: () => import('pages/ManageCompanyPage.vue'),
        meta: {
          title: 'Panel moderacyjny | WorkQR',
          requiresModerator: true,
        },
      },
      {
        name: 'Company',
        path: 'company',
        component: () => import('pages/CompanyPage.vue'),
        meta: {
          title: 'Twoja firma | WorkQR',
        },
      },
    ],
  },
  {
    path: '/welcome',
    component: () => import('layouts/LoginLayout.vue'),
    children: [
      {
        name: 'Login',
        path: ':regCode?',
        component: () => import('pages/LoginPage.vue'),
        meta: {
          title: 'Zaloguj się | WorkQR',
          requiresNonAuth: true,
        },
        props: true,
      },
    ],
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
    meta: {
      title: 'Nie znaleziono strony | WorkQR',
    },
  },
];

export default routes;
