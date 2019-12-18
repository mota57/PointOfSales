import Vue from 'vue'
import axios from 'axios'
import router from './router/index'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import AppUrl from './api-url'



/** third party */

import { eventBus } from './event-bus'
import  ApplicationStartup  from './application-startup'


ApplicationStartup
  .UseCustomFilers(Vue)
  .UseAxiousConfiguration(axios)
  .UseCustomComponents(Vue)
  .UseThirdPartyComponents(Vue)
  .RegisterGlobals(axios);




Vue.prototype.$http = axios

Vue.mixin({
  data: function () {
    return {
      ...AppUrl,
      eventBus: eventBus,
      URLIMAGEFOR(folder, imageId) {
        //validate if image exists
        return `/images/${folder}/${imageId}`;

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
