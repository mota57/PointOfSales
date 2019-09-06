<template>
  <div>

      <div class="modal fade" id="edit" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title" id="exampleModalLabel">New </h5>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">

               <slot name="edit" v-ref="childForm"></slot>

            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
              <button type="button" class="btn btn-primary" @click="callSave">Save</button>
            </div>
          </div>
        </div>
      </div>


    <div class="row">
      <div class="col-12" v-if="isComplete"> 
        <v-server-table :name="name" :url="urls[name].datatable" :columns="columns" :options="options">


          <div slot="Edit" slot-scope="props">
              <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#edit" data-whatever="@mdo">Open modal for @mdo</button>
              <a data-target="#edit" data-toggle="modal"> <icon icon="edit" class="mr-2 menu-icon"    /> </a>
          </div>
          
        </v-server-table>

      </div>

      <!--<div class="card col-6">
        <div class="card-body">
          <h5 class="card-title">Create filter</h5>

          <div class="form-group row">
            <label class="col-sm-2 col-form-label">Field</label>
            <div class="col-sm-10">
              <select class="form-control" >
                <option value="Name">Name</option>
                <option value="Id">Id</option>
              </select>
            </div>
          </div>

          <div class="form-group row">
            <label class="col-sm-2 col-form-label">Operator</label>
            <div class="col-sm-10">
              <select class="form-control">
                <option>Equals</option>
                <option>Not equal to</option>
                <option>less than</option>
                <option>greater or equal</option>
                <option>contains</option>
                <option>does not contain</option>
                <option>start with</option>
              </select>
            </div>
          </div>

          <div class="form-group row">
            <label class="col-sm-2 col-form-label">Value</label>
            <div class="col-sm-10">
              <input type="text"  class="form-control"/>
            </div>
          </div>

          <a href="#" class="btn btn-primary">Go somewhere</a>
        </div>
      </div>-->

         </div>

  </div>
</template>

<script>
  //import _ from 'lodash'

  export default {
    props: ['name'],
    methods: {
      callSave() {
        console.log(this.$refs);
        this.$refs.childComponent.upsert();
      },
      edit(data) {
        //open pop up
        console.log(data);
        this.isEdit = !this.isEdit;

      }
      //emitName: function() {
      //  this.$store.commit('categories/SET_FILTER', { 'Name': this.Name });

      //}
    },
    watch: {

    },
    async created() {

      try {
        let response = await this.$http.get(this.urls.categories.tableMetadata)
        this.isComplete = true;
        console.log(response);
        let data = response.data;

        //todo move this to API CONTROLLER 
        let fieldsToFilter = data.fields.filter(_ => _.filter).map(_ => _.name);
        this.columns = data.fields.map(_ => _.name);
        this.columns.push('Edit');
        this.options.filterable = fieldsToFilter;
        this.options.customFilters = fieldsToFilter; 

      /*
       # create dropdown for pick list
       # create dropdown for date picker
       # create dropdown for operators
        equals,
        not equal to,
        less than,
        greater than,
        less or equal,
        greater or equal,
        contains,
        does not contain,
        starts with
      # configure the pagination
      # configure export to excel
      # configure 
      */

      } catch (err) {
        window.alert(err)
        console.log(err)
      }

    },
    data() {
      return {
        isEdit:false,
        isComplete:false,
        isAjax: false,
        columns: [], // Id, name
        options: {
          customFilters: [],
          filterByColumn: true,
        }
      }
    },
  }
</script>

<style>
</style>
