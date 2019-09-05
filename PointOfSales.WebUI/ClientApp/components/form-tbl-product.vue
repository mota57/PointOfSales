<template>
  <div>
    <h1>Product</h1>

    <div v-if="isAjax" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>

    <div>
      {{urls.categories.datatable}}
      <v-server-table :url="urls.categories.datatable" :columns="columns" :options="options"></v-server-table>
    </div>

  </div>
</template>

<script>
  export default {
  
  data () {
    return {
      isAjax:false,
      columns: ['Id', 'Name'],
      options: {
        // see the options API
        initFilters: {Name: 'Cat'},
        customFilters: [ 'Name'],
        filterByColumn: true,
        responseAdapter: function (resp) {

          var d = resp.data;

          return {
            data: d.data,
            count: d.count
          }
        }
      }
    }
  },
  methods: {
  },
  async created () {
    //fetch data
  }
}
</script>

<style>
</style>
