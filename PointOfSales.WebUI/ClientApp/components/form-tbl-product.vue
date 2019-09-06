<template>
  <div>
    <h1>Product</h1>

    <div v-if="isAjax" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>

    <div>
      <input type="text" class="form-control" v-model="Name">
      <button @click="emitName"> trigger name</button>

      <v-server-table  name="categories" :url="urls.categories.datatable" :columns="columns" :options="options">
      </v-server-table>
    </div>

  </div>
</template>

<script>
  import _ from 'lodash'

  export default {
    methods: {
      emitName: function() {
        this.$store.commit('categories/SET_FILTER', { 'Name': this.Name });

      }
    },
    watch: {

    },
    created() {

    },
    data() {
      return {
        Name:'',
        isAjax: false,
        columns: ['Id', 'Name'],
        options: {
          // see the options API
          initFilters: { Name: 'Cat' },
          customFilters: ['Name'],
          filterByColumn: true,
          filterable: []
          //responseAdapter: function (resp) {

          //  var d = resp.data;

          //  return {
          //    data: d.data,
          //    count: d.count
          //  }
          //}
        }
      }
    },
  }
</script>

<style>
</style>
