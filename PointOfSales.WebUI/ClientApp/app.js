import Vue from 'vue'
import axios from 'axios'
import router from './router/index'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import AppUrl from './api-url'
import formProduct from 'components/form-product'
import formModifier from 'components/form-modifier'
import formCategory from 'components/form-category'
import formDatatable from 'components/form-datatable'
import formImage from 'components/form-image'
import modifierComponent from 'components/modifier-component'
import { FontAwesomeIcon } from './icons'
import jQuery from 'jquery'
import vSelect from 'vue-select'
import { ServerTable, ClientTable, Event } from 'vue-tables-2';
import Toasted from '@gitlab/vue-toasted';
import BootstrapVue from 'bootstrap-vue'




// Registration of global window variables 
window.$ = jQuery
window.axios = axios;





// Registration of global components

Vue.use(BootstrapVue)
Vue.use(ServerTable, {}, true, 'bootstrap4', 'default');
Vue.use(Toasted)

Vue.component('form-image', formImage)
Vue.component('form-product', formProduct)
Vue.component('form-modifier', formModifier)
Vue.component('form-category', formCategory)
Vue.component('form-datatable', formDatatable)
Vue.component('modifier-component', modifierComponent)
Vue.component('icon', FontAwesomeIcon)
Vue.component('v-select', vSelect);

Vue.prototype.$http = axios

Vue.mixin({
  data: function () {
    return {
      ...AppUrl,
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
