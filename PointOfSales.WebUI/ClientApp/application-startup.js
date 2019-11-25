

/** third party */


import { FontAwesomeIcon } from './icons'
import jQuery from 'jquery'
import vSelect from 'vue-select'
import { ServerTable, ClientTable, Event } from 'vue-tables-2';
import Toasted from '@gitlab/vue-toasted';
import BootstrapVue from 'bootstrap-vue'
import VueNumeric from 'vue-numeric'
import DateRangePicker from 'vue2-daterange-picker'
import moment from 'moment'


/** custom */
import AxiosStartup from './axios-startup'

import formProduct from 'components/product/form-product'
import formModifier from 'components/modifier/form-modifier'
import formCategory from 'components/category/form-category'
import formDatatable from 'components/form-datatable'
import formImage from 'components/form-image'
import formSelect from 'components/form-select'
import formDiscount from 'components/discount/form-discount'
import itemConfigure from 'components/merchant/item-configure'
import orderConfigure from 'components/merchant/order-configure'


export default {
    UseAxiousConfiguration(axios){
        AxiosStartup.Config(axios);
        return this;
    },
    UseCustomComponents(Vue){
        Vue.component('form-image', formImage);
        Vue.component('form-product', formProduct);
        Vue.component('form-modifier', formModifier);
        Vue.component('form-category', formCategory);
        Vue.component('form-datatable', formDatatable);
        Vue.component('form-select', formSelect);
        Vue.component('form-discount', formDiscount);
        Vue.component('item-configure', itemConfigure);
        Vue.component('order-configure', orderConfigure);
        return this;
    },
    UseThirdPartyComponents (Vue){
        // Registration of global components
        Vue.use(BootstrapVue)
        Vue.use(ServerTable, {}, true, 'bootstrap4', 'default');
        Vue.use(Toasted)
        Vue.use(VueNumeric)
        Vue.component('date-range-picker',DateRangePicker)
        Vue.component('icon', FontAwesomeIcon)
        Vue.component('v-select', vSelect);

 
    
     

        return this;
    },
    RegisterGlobals(axios){
        // Registration of global window variables 
        window.$ = jQuery
        window.axios = axios;
        return this;
    },
    UseCustomFilers(Vue){
        Vue.filter('formatDate', function(value) {
            if (value) {
              return moment(String(value)).format('MM/DD/YYYY')
            }
        });
        return this;
    }
}