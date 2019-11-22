import Vue from 'vue'
import Vuex from 'vuex'
import _ from 'lodash'
Vue.use(Vuex)



// TYPES
const MAIN_SET_COUNTER = 'MAIN_SET_COUNTER'

// STATE
const state = {
  counter: 1,
  orderItemList: []
}

// MUTATIONS
const mutations = {
  [MAIN_SET_COUNTER] (state, obj) {
    state.counter = obj.counter
  },
  setOrderItemList (state, item) {
    let orderItem = MerchantHelper.findOrderItemById(state, item.id);
    if (!orderItem) {
      orderItem = { quantity: 1, ...item };
      console.log("setOrderItemList::" + JSON.stringify(orderItem));
      state.orderItemList.push(orderItem);
    } else {
      orderItem.quantity += 1;
    }
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
    return _.find(state.orderItemList, function(el) { el.id === id.id});
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
