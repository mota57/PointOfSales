import Vue from 'vue'
import axios from 'axios'
import router from './router/index'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import AppUrl from './api-url'

import formProduct from 'components/product/form-product'
import formModifier from 'components/modifier/form-modifier'
import formCategory from 'components/category/form-category'


/** common **/
import formDatatable from 'components/form-datatable'
import formImage from 'components/form-image'

import { FontAwesomeIcon } from './icons'
import jQuery from 'jquery'
import vSelect from 'vue-select'
import { ServerTable, ClientTable, Event } from 'vue-tables-2';
import Toasted from '@gitlab/vue-toasted';
import BootstrapVue from 'bootstrap-vue'
import VueNumeric from 'vue-numeric'
import AirbnbStyleDatepicker from 'vue-airbnb-style-datepicker'

import { eventBus } from './event-bus'



// Registration of global window variables 
window.$ = jQuery
window.axios = axios;


// Add a request interceptor
axios.interceptors.request.use(function (config) {
  // Do something before request is sent
  let headers = {};
  //https://github.com/aspnet/AspNetCore/issues/9039
  headers['X-Requested-With'] = 'XMLHttpRequest';
  if (config.headers) {
    Object.assign(config.headers, headers);
  } else {
    config.headers = headers;
  }

  return config;
}, function (error) {
  // Do something with request error
  return Promise.reject(error);
});

// Add a response interceptor
axios.interceptors.response.use(function (response) {
  // Any status code that lie within the range of 2xx cause this function to trigger
  // Do something with response data
  return response;
}, function (error) {
  // Any status codes that falls outside the range of 2xx cause this function to trigger
  // Do something with response error
  console.debug('INTERCEPTOR AXIOS ERROR NEXT LINE')
  console.debug(error)
  return Promise.reject(error);
});




// Registration of global components

Vue.use(BootstrapVue)
Vue.use(ServerTable, {}, true, 'bootstrap4', 'default');
Vue.use(Toasted)
Vue.use(VueNumeric)

// see docs for available options
const datepickerOptions = {}

// make sure we can use it in our components
Vue.use(AirbnbStyleDatepicker, datepickerOptions)

Vue.component('form-image', formImage)
Vue.component('form-product', formProduct)
Vue.component('form-modifier', formModifier)
Vue.component('form-category', formCategory)
Vue.component('form-datatable', formDatatable)

Vue.component('icon', FontAwesomeIcon)
Vue.component('v-select', vSelect);

Vue.prototype.$http = axios

Vue.mixin({
  data: function () {
    return {
      ...AppUrl,
      eventBus: eventBus
    }
  }
})

sync(store, router)

const app = new Vue({
  store,
  router,
  ...App
})

export {
  app,
  router,
  store
}
