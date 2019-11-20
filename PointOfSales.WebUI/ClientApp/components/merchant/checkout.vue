<template>
  <div class="container">
    <div>
      <b-card title="Checkout">
        <h2>
          <b-badge>Total: {{totalOrderItemCharge}}</b-badge>
        </h2>
        <br>

        <div class="mb-2">
          <b-button @click="setNewPayment(MethodType.Cash)">Cash</b-button>
          <b-button class="ml-2" @click="setNewPayment(MethodType.Card)">Card</b-button>
          <b-button class="float-right">Pay</b-button>
        </div>

        <table class="table">
          <thead>
            <tr>
              <th scope="col">Due</th>
              <th scope="col">Tendered</th>
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
                             v-model="pts.tendered"
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
  </div>
</template>

<script>
  import { mapGetters } from "vuex";
  import { MethodType, Payment } from "../domain";


  export default {
    data() {
      return {
        MethodType: MethodType,
        payments: []
      };
    },
    computed: {
      ...mapGetters(["totalOrderItemCharge"])
    },
    methods: {
      setCalculations() {
        let total = this.totalOrderItemCharge;
        this.payments.forEach(p => {
          total -= p.tendered;
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
