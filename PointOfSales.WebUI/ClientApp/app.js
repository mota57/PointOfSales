import Vue from 'vue'
import axios from 'axios'
import router from './router/index'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import { FontAwesomeIcon } from './icons'
import jQuery from 'jquery'

import vSelect from 'vue-ener-select'

// Registration of global window variables 
window.$ = jQuery



// Registration of global components
Vue.component('icon', FontAwesomeIcon)
Vue.component('v-select', vSelect)


Vue.prototype.$http = axios

Vue.mixin({
  data: function () {

   
    class BASE_URL {
      constructor(name) {
        this.name = name;
      }


      getURL(resource) {
        let result = `${window.APP_GLOBALS.URL}${this.name}${resource}`
        return result;
      }

      picklist (partUrl) {
        var result = this.getURL(`/GetPickList/${partUrl}`)
        return result;
      }

      upsert (id = '') {
        let result = this.getURL(`/${id}`);
        return result;
      }

    }

    class categories extends BASE_URL {
      constructor() {
        super('/Categories')
      }

    }

    class products extends BASE_URL {
      constructor() {
        super('/Products')
      }
     }


    return {
      urls: {
        categories: new categories(),
        products: new products()
      }
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
