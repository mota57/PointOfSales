<template>
  <div>

    <h3>Categories</h3>
   <form-datatable name="categories" eventpostfix="form-category" title="Category">
     <template v-slot:edit>
        <form-category></form-category>
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
