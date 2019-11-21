<template>
  <div>

    <h3>Discounts</h3>
   <form-datatable name="discount" eventpostfix="form-discount" title="Discount">
     <template v-slot:edit>
        <form-discount></form-discount>
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
    }
  }
</script>

<style>
</style>
