import Vue from 'vue'
import Vuex from 'vuex'
import _ from 'lodash'
Vue.use(Vuex)



// TYPES
const MAIN_SET_COUNTER = 'MAIN_SET_COUNTER'

// STATE
const state = {
  counter: 1,
  orderItemList: [],
  discountOfOrder:
  {
    discountId: '',
    customDiscountAmount: '',
    disscountType: 'none',
  }
}

// MUTATIONS
const mutations = {
  [MAIN_SET_COUNTER] (state, obj) {
    state.counter = obj.counter
  },
  setOrderItemList (state, item) {
    let orderItem = MerchantHelper.findOrderItemById(state, item.id);
    if (!orderItem) {
      orderItem = { discountId:-1, quantity: 1, customDiscountAmount:0, disscountType: null, ...item };
      //console.log("setOrderItemList::" + JSON.stringify(orderItem));
      state.orderItemList.push(orderItem);
    } else {
      orderItem.quantity += 1;
    }
  },
  setOrderConfiguration(state, payload){
    state.discountOfOrder.discountId           =  payload.disscountType == "system" ? payload.discountId : null;
    state.discountOfOrder.customDiscountAmount =  payload.disscountType == "custom" ? payload.customDiscountAmount : null;
    state.discountOfOrder.disscountType        =  payload.disscountType;
  },
  setOrderItemConfiguration (state, payload){
      var orderItem = MerchantHelper.findOrderItemById(state, payload.id);
      Object.assign(orderItem, payload);
  },
  clearOrderItemList (state) {
    state.orderItemList = [];
  },
}
const getters = {
    totalOrderItemCharge(state, getters) {
      return MerchantHelper.getTotalOrderItemCharge(state);
    }
}

// ACTIONS
const actions = ({
  setCounter ({ commit }, obj) {
    commit(MAIN_SET_COUNTER, obj)
  }
})


const MerchantHelper = {
  findOrderItemById(state, id){
    return _.find(state.orderItemList, l => l.id == id);
  },
  getTotalOrderItemCharge(state) {
    let total = 0;
    if (state.orderItemList.length > 0) {
      state.orderItemList.forEach(el => {
        total = total + el.price * el.quantity;
      });
    }
    return total;
  }
};


export default new Vuex.Store({
  state,
  mutations,
  actions,
  getters
})
