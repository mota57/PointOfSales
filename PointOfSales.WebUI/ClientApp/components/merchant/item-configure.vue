<template>
  <div class="container">
    <div>
      <b-card title="Checkout">
        <div class="row">
          
          <div class="form-group">
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


           <div class="form-group">
              <label for="Discount">Discount </label>
              <form-select></form-select>

              <template v-if="errList && errList.Category">
                <p class="text-danger" v-bind:key="$index" v-for="(err, $index) in errList.Category"> {{err}} </p>
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
        orderItem: {}
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
     
      saveConfiguration()
      { 
        var orderItemPropsToUpdate = {
           productId : this.productId,
           discount : this.discount
        }

        if(this.orderItem.isProductForRent){
          if(dateAreInvalid) return;
          
        }
        this.$store.commit('setOrderItemConfiguration', {
            productId: this.$route.params.productId,
            starDate: dateOne,
            endDate: dateTwo
        })
      },
      setDataValues(){
        this.orderItem = this.$route.params.orderItem;
        this.dateOne = orderItem.startDate;
        this.dateTwo = orderItem.endDate;
        this.discountId = orderItem.discountId;
        this.customDiscountAmount = orderItem.customDiscountAmount;
      }
  
    },
    computed: {
      dateAreInvalid(){
         return (!dateOne || !dateTwo)
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
