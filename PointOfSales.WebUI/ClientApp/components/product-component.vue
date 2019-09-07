<template>
  <div>

    <h3>Products</h3>
   <form-datatable name="products">
     <template v-slot:edit>
        <form-product></form-product>
     </template>
   </form-datatable> 


   </div>
</template>

<script>

  export default {
    data() {
      return {

      }
    }

  }
</script>

<style>
</style>
