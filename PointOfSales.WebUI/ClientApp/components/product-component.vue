<template>
  <div>

   <form-datatable name="products">
     <template v-slot:edit>
        <form-product ></form-product>
     </template>
   </form-datatable> 


   </div>
</template>

<script>

  export default {
    data() {
      return {

      }
    },
    methods: {
    },
  }
</script>

<style>
</style>
