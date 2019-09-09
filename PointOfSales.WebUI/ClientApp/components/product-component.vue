<template>
  <div>

    <h3>Products</h3>
   <form-datatable name="products" eventpostfix="form-product" title="Product">
     <template v-slot:edit>
        <form-product ></form-product>
     </template>
   </form-datatable> 

   </div>
</template>

<script>

  import { eventBus } from './event-bus'

  export default {
    data() {
      return {

      }
    },
  }
</script>

<style>
</style>
