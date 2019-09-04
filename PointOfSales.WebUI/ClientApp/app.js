import Vue from 'vue'
import axios from 'axios'
import router from './router/index'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import { FontAwesomeIcon } from './icons'

// Registration of global components
Vue.component('icon', FontAwesomeIcon)

Vue.prototype.$http = axios

Vue.mixin({
  data: function() {

    function getURL (resource) {
      let result = `${window.APP_GLOBALS.URL}${resource}`
      return result;
    }

    function getProduct (resource) {
      console.log(resource)
      return getURL(`/Products${resource}`)
    }

    return {
       urls: {
        product: {
          upsert: function(id = '') {
            let result = getProduct(`/${id}`);
            return result;
          },
        }
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
