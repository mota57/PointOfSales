import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)



// TYPES
const MAIN_SET_COUNTER = 'MAIN_SET_COUNTER'
const SET_ORDER_ITEM_LIST = 'setOrderItemList';
const CLEAR_ORDER_ITEM_LIST = 'clearOrderItemList';

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
    let index = state.orderItemList.findIndex(el => el.id === item.id);
    if (index === -1) {
      let orderItem = { quantity: 1, ...item };
      console.log("setOrderItemList::" + JSON.stringify(orderItem));
      state.orderItemList.push(orderItem);
    } else {
      state.orderItemList[index].quantity += 1;
    }
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
