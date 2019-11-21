/** third party */

import AxiosStartup from 'axios-startup'
import { FontAwesomeIcon } from './icons'
import jQuery from 'jquery'
import vSelect from 'vue-select'
import { ServerTable, ClientTable, Event } from 'vue-tables-2';
import Toasted from '@gitlab/vue-toasted';
import BootstrapVue from 'bootstrap-vue'
import VueNumeric from 'vue-numeric'
import AirbnbStyleDatepicker from 'vue-airbnb-style-datepicker'


/** custom */
import formProduct from 'components/product/form-product'
import formModifier from 'components/modifier/form-modifier'
import formCategory from 'components/category/form-category'
import formDatatable from 'components/form-datatable'
import formImage from 'components/form-image'
import formSelect from 'components/form-select'


export default {
    UseAxiousConfiguration(axios){
        AxiosStartup.Config(axios);
        return this;
    },
    UseCustomComponents(Vue){
        Vue.component('form-image', formImage)
        Vue.component('form-product', formProduct)
        Vue.component('form-modifier', formModifier)
        Vue.component('form-category', formCategory)
        Vue.component('form-datatable', formDatatable)
        Vue.component('form-select', formSelect)
        return this;
    },
    UseCustomComponents(Vue){
        // Registration of global components
        Vue.use(BootstrapVue)
        Vue.use(ServerTable, {}, true, 'bootstrap4', 'default');
        Vue.use(Toasted)
        Vue.use(VueNumeric)
        Vue.component('icon', FontAwesomeIcon)
        Vue.component('v-select', vSelect);

        // see docs for available options
        const datepickerOptions = {}

        // make sure we can use it in our components
        Vue.use(AirbnbStyleDatepicker, datepickerOptions)
     

        return this;
    },
    RegisterGlobals(){
        // Registration of global window variables 
        window.$ = jQuery
        window.axios = axios;
        return this;
    }
}