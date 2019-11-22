<template>
  <div class="container">
    <div>
      <div class="row">
        
        <div class="col-6">
          <div class="form-group" v-if="orderitem && orderitem.isProductForRent">
            <label for="datepicker-trigger">Dates</label>
       
            <date-range-picker
              ref="picker"
              opens="center"
              :locale-data="{ firstDay: 1, format: 'DD-MM-YYYY HH:mm:ss' }"
             
              :singleDatePicker="false"
              :timePicker="false"
              :showWeekNumbers="true"
              :showDropdowns="true"
              :autoApply="false"
              v-model="dateRange"
              @update="updateValues"

              :linkedCalendars="true"
            
            >
             <div slot="input" slot-scope="picker" style="min-width: 350px;">
            {{ picker.startDate | formatDate }} - {{ picker.endDate | formatDate }}
            </div>
            </date-range-picker>

            <div  class="text-danger"  v-if="errList && errList.dateErrMessage">{{errList.dateErrMessage}}</div>
          </div>

          <b-form-group label="Discount Type">
            <b-form-radio v-model="disscountType" value="none">No discount</b-form-radio>
            <b-form-radio v-model="disscountType" value="custom">Custom discount</b-form-radio>
            <b-form-radio v-model="disscountType" value="system">System discount</b-form-radio>
          </b-form-group>

          <div class="form-group" v-if="disscountType == 'system'">
            <label for="Discount">System discount</label>
            <form-select urlapi="discount" v-model="discountId"></form-select>

            <template v-if="errList && errList.Disscount">
              <p
                class="text-danger"
                v-bind:key="$index"
                v-for="(err, $index) in errList.Category"
              >{{err}}</p>
            </template>
          </div>

          <div class="form-group" v-if="disscountType == 'custom'">
            <label for="CustomDiscount">Custom Discount</label>
            <vue-numeric
              class="form-control"
              currency="%"
              separator=","
              v-bind:min="0"
              v-bind:max="70"
              v-model="customDiscountAmount"
            ></vue-numeric>

            <template v-if="errList && errList.customDiscountAmount">
              <p class="text-danger">{{errList.customDiscountAmoun}}</p>
            </template>
          </div>
        </div>
        <div class="col-6">
          <label for="quantity">Quantity</label>
          <input id="quantity" type="number" v-model="quantity" min="0" class="form-control" />
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
            {{orderitem}}
        </pre>
  </div>
</template>

<script>
export default {
  props: {
    orderitem: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      dateRange:{},
      dateFormat: "D MMM",
      starDate: "",
      endDate: "",
      discountId: "",
      disscountType: "none",
      quantity: 0,
      customDiscountAmount: 0,
      errList: {}
    };
  },
  computed: {},
  methods: {
    close() {
      this.$bvModal.hide('item-configure')
    },
  
    updateValues(dataDates){
      console.log('dates');
      console.log(dataDates);
      this.dateRange.startDate = dataDates.startDate
      this.dateRange.endDate = dataDates.endDate
    },
    validateForm() {
      this.errList = {};
      if (
        this.disscountType == "custom" &&
        (this.customDiscountAmount == null || this.customDiscountAmount < 0)
      ) {
        this.errList.customDiscountAmount =
          "Custom amount must be greater than 0";
      }

      if(this.dateAreInvalid){
         this.errList.dateErrMessage =
          "Dates are required";
      }
     
      return Object.keys(this.errList).length == 0;
    },
    saveConfiguration() {
   
      if (this.validateForm()) {
        let propsToUpdate = {
          quantity: Number.parseInt(this.quantity),
          id: this.orderitem.id,
          discount: this.disscountType == "system" ? this.discountId : null,
          customDiscountAmount: this.disscountType == "custom" ? this.customDiscountAmount : null,
          startDate: this.orderitem.isProductForRent ? this.dateRange.startDate : null,
          endDate: this.orderitem.isProductForRent ? this.dateRange.endDate : null,
          disscountType : this.disscountType
        };

        this.$store.commit("setOrderItemConfiguration", propsToUpdate);
        this.close();
      }
    },
    setDataValues() {
      this.disscountType = this.orderitem.disscountType;
      this.quantity = this.orderitem.quantity;
      this.dateRange.startDate = this.orderitem.startDate;
      this.dateRange.endDate = this.orderitem.endDate;
      this.discountId = this.orderitem.discountId;
      this.customDiscountAmount = this.orderitem.customDiscountAmount;
    }
  },
  computed: {
   
    dateAreInvalid() {
      return !this.dateRange.startDate || !this.dateRange.endDate;
    }
  },
  created() {
    console.log(this.orderitem);

    this.setDataValues();
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
