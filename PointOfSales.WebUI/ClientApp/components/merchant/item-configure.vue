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
          

          <div v-if="dateAreInvalid" class="text-danger">
              Both dates are required  
          </div>
          <br/>>
          <b-button class="float-right" v-on:click="setDateToOrderItem">Save</b-button>
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
      },
     
      setDateToOrderItem(){ 
        if(dateAreInvalid) return;
        this.$store.commit('setDateToOrderItem', {
          productId: this.$route.params.productId,
          starDate: dateOne,
          endDate: dateTwo
        })
      }
  
    },
    computed: {
      dateAreInvalid(){
         return (!dateOne || !dateTwo)
      }
    },
    created() {
      console.info('route paramss!! ');
      console.info(this.$route.params); //productId
    }
  };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
