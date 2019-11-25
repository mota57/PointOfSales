<template>
  <div class="container">
    <div>
      <div class="row">
        
        <div class="col-6">
         
          <b-form-group label="Discount Type">
            <b-form-radio v-model="discountOfOrder.disscountType" value="none">No discount</b-form-radio>
            <b-form-radio v-model="discountOfOrder.disscountType" value="custom">Custom discount</b-form-radio>
            <b-form-radio v-model="discountOfOrder.disscountType" value="system">System discount</b-form-radio>
          </b-form-group>

          <div class="form-group" v-if="discountOfOrder.disscountType == 'system'">
            <label for="Discount">System discount</label>
            <form-select urlapi="discount" v-model="discountId"></form-select>
          </div>

          <div class="form-group" v-if="discountOfOrder.disscountType == 'custom'">
            <label for="CustomDiscount">Custom Discount</label>
            <vue-numeric
              class="form-control"
              currency="%"
              separator=","
              v-bind:min="0"
              v-bind:max="70"
              v-model="customDiscountAmount"
            ></vue-numeric>

          </div>
        </div>

        
        <div class="col-12">
          <div class="form-group float-right">
            <a href="#" @click="close" class="btn btn-secondary">Cancel</a>
            <b-button class v-on:click="saveConfiguration">Save</b-button>
          </div>
        </div>
      </div>
    </div>

   <pre>
    {{ discountOfOrder }}
   </pre>
  </div>
</template>

<script>


export default {
  
  data() {
    return {
      errList: {},
      discountOfOrder: {
        disscountType:'none',
        discountId: -1,
        customDiscountAmount: ''
      },
    };
  },
  computed: {


  },
  methods: {
    close() {
      this.$bvModal.hide('order-configure')
    },
    validateForm() {
      this.errList = {};
      if (this.disscountType == "custom" && (this.customDiscountAmount == null || this.customDiscountAmount < 0)) {
        this.errList.customDiscountAmount = "Custom amount must be greater than 0";
      }
      return Object.keys(this.errList).length == 0;
    },
    saveConfiguration() {
   
      if (this.validateForm()) {
         this.$store.commit('setOrderConfiguration', this.discountOfOrder);
        this.close();
      }
    },
  },
  created() {
    this.discountOfOrder = { ...this.$store.state.discountOfOrder};
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
