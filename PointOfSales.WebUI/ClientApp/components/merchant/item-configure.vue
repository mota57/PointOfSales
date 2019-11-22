<template>
  <div class="container">
    <div>
      <b-card title="Checkout">
        <div class="row">
          <div class="form-group" v-if="orderItem && orderItem.isProductForRent">
            <input type="text"
                  id="datepicker-trigger"
                  placeholder="Select dates"
                  :value="formatDates(dateOne, dateTwo)">

            <AirbnbStyleDatepicker :trigger-element-id="'datepicker-trigger'"
                                  :mode="'range'"
                                  :fullscreen-mobile="true"
                                  :date-one="dateOne"
                                  :date-two="dateTwo"
                                  @date-one-selected="val => { dateOne = val }"
                                  @date-two-selected="val => { dateTwo = val }" />

            <div v-if="dateAreInvalid" class="text-danger">
                Both dates are required  
            </div>
          </div>


          <b-form-group label="Discount Type">
            <b-form-radio v-model="disscountType" value="custom">Custom discount</b-form-radio>
            <b-form-radio v-model="disscountType" value="system">System discount</b-form-radio>
          </b-form-group>


           <div class="form-group" v-if="disscountType == 'system'">
              <label for="Discount">Discount </label>
              <form-select urlapi='discount'></form-select>

               <template v-if="errList && errList.Disscount">
                <p class="text-danger" v-bind:key="$index" v-for="(err, $index) in errList.Category"> {{err}} </p>
              </template>
          </div>

          <div class="form-group" v-if="disscountType == 'custom'">
              <label for="CustomDiscount">Custom  Discount </label>
              <vue-numeric currency="$" separator="," v-model="customDiscountAmount" ></vue-numeric>

              <template v-if="errList && errList.customDiscountAmount">
                <p class="text-danger">  {{errList.customDiscountAmoun}}  </p>
              </template>
          </div>

          <br/>
          <b-button class="float-right" v-on:click="saveConfiguration">Save</b-button>
          <a href="#" @click="goBack()" class="btn btn-secondary">Back</a>
          
        </div>
      </b-card>
    </div>
  </div>
</template>

<script>

  import format from 'date-fns/format'


  export default {
    data() {
      return {
        dateFormat: 'D MMM',
        dateOne: '',
        dateTwo: '',
        discountId:'',
        disscountType:'',
        orderItem: {},
        customDiscountAmount:0,
        errList: {}
      };
    },
    computed: {
    },
    methods: {
      goBack() {
        this.$router.push({ name: "pos" })
      },
      formatDates(dateOne, dateTwo) {
        let formattedDates = ''
        if (dateOne) {
          formattedDates = format(dateOne, this.dateFormat)
        }
        if (dateTwo) {
          formattedDates += ' - ' + format(dateTwo, this.dateFormat)
        }
        return formattedDates
      },
      validateForm(){
        this.errList = {};
        if(disscountType == 'custom' && (customDiscountAmount == null || customDiscountAmount < 0)){
          this.errList.customDiscountAmount = 'Custom amount must be greater than 0';
        }
        //TODO validate dates
        
      },
      saveConfiguration(){ 
        if(validateForm()){
          let propsToUpdate = {
            productId : this.orderItem.id,
            discount : this.disscountType == 'system' ? this.discountId : null,
            customDiscountAmount: this.disscountType == 'custom' ?  this.customDiscountAmount : null,
            startDate : (this.orderItem.isProductForRent) ? this.dateOne : null,
            endDate : (this.orderItem.isProductForRent) ? this.dateTwo : null
          }
        
          this.$store.commit('setOrderItemConfiguration', propsToUpdate);
        }
      },
      setDataValues(){
        this.orderItem = this.$route.params.orderItem;
        this.dateOne = this.orderItem.startDate;
        this.dateTwo = this.orderItem.endDate;
        this.discountId = this.orderItem.discountId;
        this.customDiscountAmount = this.orderItem.customDiscountAmount;
      }
  
    },
    computed: {
      dateAreInvalid(){
         return (!this.dateOne || !this.dateTwo)
      }
    },
    created() {
      console.info('route paramss!! ');
      console.info(this.$route.params.orderItem); //productId
      this.setDataValues();    
    }
  };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
