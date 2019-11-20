<template>
  <div class="container">
    <div>
      <b-card title="Checkout">
        <div class="mb-2">

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
          <b-button class="float-right">Pay</b-button>


        </div>



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
        dateFormat: 'D MMM',
        dateOne: '',
        dateTwo: ''
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
      }
    },
    created() {

    }
  };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
