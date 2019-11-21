import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)



// TYPES
const MAIN_SET_COUNTER = 'MAIN_SET_COUNTER'
const SET_ORDER_ITEM_LIST = 'setOrderItemList';
const CLEAR_ORDER_ITEM_LIST = 'clearOrderItemList';
const SET_ORDER_ITEM_CONFIG = 'setOrderItemConfiguration'
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
  [SET_ORDER_ITEM_LIST] (state, item) {
    let orderItem = MerchantHelper.findOrderItemById(state, item.id);
    if (!orderItem) {
      orderItem = { quantity: 1, ...item };
      console.log("setOrderItemList::" + JSON.stringify(orderItem));
      state.orderItemList.push(orderItem);
    } else {
      orderItem.quantity += 1;
    }
  },
  [SET_ORDER_ITEM_CONFIG] (state, payload){
    var orderItem = MerchantHelper.findOrderItemById(state, payload.id);
    Object.assign(orderItem, payload);
  },
  [CLEAR_ORDER_ITEM_LIST] (state) {
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
    return state.orderItemList[state.orderItemList.findIndex(el => el.id === item.id)];
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
