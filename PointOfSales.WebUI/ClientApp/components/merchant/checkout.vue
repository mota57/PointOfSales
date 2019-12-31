<template>
  <div class="container">

    <div v-if="!isAjax">
      <b-card title="Checkout">
        <h2>
          <b-badge>Total: {{totalOrderItemCharge}}</b-badge>
        </h2>
        <br>

        <div class="mb-2">
          <b-button @click="setNewPayment(MethodType.Cash)">Cash</b-button>
          <b-button class="ml-2" @click="setNewPayment(MethodType.Card)">Card</b-button>
          <b-button class="float-right" v-on:click="callPay()">Pay</b-button>

        </div>

        <table class="table">
          <thead>
            <tr>
              <th scope="col">Due</th>
              <th scope="col">Amount</th> <!-- amount -->
              <th scope="col">Change</th>
              <th scope="col">Method</th>
              <th scope="col">Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(pts, index) in payments" :key="index">
              <th>{{pts.due}}</th>
              <td>
                <vue-numeric currency="$"
                             separator=","
                             v-model="pts.amount"
                             v-on:input="setCalculations()"></vue-numeric>
              </td>
              <td>
                <vue-numeric currency="$" separator="," v-model="pts.change" read-only></vue-numeric>
              </td>
              <td>{{pts.paymentType}}</td>
              <td>
                <b-button @click="removePayment(index)" variant="danger">Remove</b-button>
              </td>
            </tr>
          </tbody>
        </table>

       <a href="#" @click="goBack()" class="btn btn-secondary">Back</a>
      </b-card>
    </div>
    
    <div class="text-center" v-if="isAjax">
      <b-spinner style="width: 10rem; height: 10rem;" variant="primary" label="Text Centered"></b-spinner>
      <br/><span style="position: relative;top: -5.48rem;">processing payment</span>
    </div>
    
  </div>
</template>

<script>
  import { mapGetters, mapMutations } from "vuex";
  import { MethodType, Payment } from "../domain";


  export default {
    data() {
      return {
        MethodType: MethodType,
        payments: [],
        isAjax: false,
      };
    },
    computed: {
      ...mapGetters(["totalOrderItemCharge"])
    },
    methods: {
       ...mapMutations({
        clearOrder: "clearOrderItemList",
      }),
      callPay(){
         let discountOfOrder = this.$store.state.discountOfOrder;

         var vm  = this;
         vm.isAjax = true;
         let formData = {
           PaymentOrders : this.payments,
           OrderDetails: this.$store.state.orderItemList,
           discountId: discountOfOrder.discountId,
           customDiscountAmount: discountOfOrder.customDiscountAmount,
           disscountType: discountOfOrder.disscountType,
           //TODO set discount
           //TODO set orderId
         }

        this.$http({
          method:  'post',
          url: this.urls.merchant.pay,
          data: formData,
        }).then(function(result){
          console.log(JSON.stringify(result, null, 1));
          vm.clearOrder();
          this.$toasted.success('save success', {
           position:'top-center',
           duration:3000,
           fullWidth:true
          })
          
        }).catch(function (errorResult) {
          
          console.log(JSON.stringify(errorResult, null, 1));

          vm.$toasted.error('Error on trying to save the order', {
           position:'top-center',
           duration:3000,
           fullWidth:true
          })
          
        }).then(function () {
          vm.isAjax = false;
        });


      },
      setCalculations() {
        let total = this.totalOrderItemCharge;
        this.payments.forEach(p => {
          total -= p.amount;
          p.due = total > 0 ? total : 0;
          p.change =
            p.paymentType === MethodType.Cash && total < 0 ? -1 * total : 0;
        });
      },
      setNewPayment(paymentType) {
        let payment = new Payment(this.totalOrderItemCharge, paymentType);
        this.payments.push(payment);
      },
      removePayment(index) {
        this.payments.splice(index, 1);
        this.setCalculations();
      },
      goBack() {
        this.$router.push({ name: "pos" })
      }
    },
    created() { }
  };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
