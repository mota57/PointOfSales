<template>
  <div>

    <div class="modal fade" :id="'form'+name" tabindex="-1" role="dialog" aria-hidden="true">
      <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">{{title}} </h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">

            <slot name="edit" v-on:reload="reloadTable"></slot>

          </div>
          <div class="modal-footer">
            <button type="button" ref="btnCloseEdit" class="btn btn-secondary" data-dismiss="modal">Close</button>
            <button type="button" class="btn btn-primary" @click="callSave">Save</button>
          </div>
        </div>
      </div>
    </div>


    <div class="modal fade" :id="'confirm'+name" tabindex="-1" role="dialog"  aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">{{title}}</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close" @click="rowToDelete = null">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <p>Are you sure you want to delete this record?</p>

          </div>
          <div class="modal-footer">
            <button type="button" ref="btnCloseConfirm" class="btn btn-secondary" @click="rowToDelete = null" data-dismiss="modal">Close</button>
            <button type="button" class="btn btn-primary" @click="callDelete()">Save changes</button>
          </div>
        </div>
      </div>
    </div>


    <div class="row">
      <div class="col-12" v-if="isComplete">
        <v-server-table ref="tableObj" :name="name" :url="urls[name].datatable" :columns="columns" :options="options">
          <div slot="beforeTable" style="border-bottom-width: 2px; border: 1px solid #dee2e6;">
            <button :data-target="'#form' + name" data-toggle="modal" type="button" class="btn "  style="border: 1px solid #dee2e6" > <icon icon="plus" class="mr-2 menu-icon" />Add </button>
          </div>

          <div slot="Edit" slot-scope="props">
            <a style="cursor:pointer" :data-target="'#form' + name" data-toggle="modal" @click="callLoad(props.row)"> <icon icon="edit" class="mr-2 menu-icon" /> </a>
            <a style="cursor:pointer" :data-target="'#confirm' + name" data-toggle="modal" @click="rowToDelete = props.row"> <icon icon="trash" class="mr-2 menu-icon" /> </a>
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
  import { eventBus } from './event-bus'

  export default {
    props: ['name', 'eventpostfix', 'title'],
    methods: {
      callSave() { eventBus.$emit(`saveForm::${this.eventpostfix}`);  },
      callLoad(row) { eventBus.$emit(`loadForm::${this.eventpostfix}`,row);  },
      callDelete() {
        if (this.rowToDelete) { eventBus.$emit(`deleteForm::${this.eventpostfix}`, this.rowToDelete); }
      },
      reloadTable() {
        this.$refs.tableObj.refresh();
        this.$refs.btnCloseEdit.click();
        this.$refs.btnCloseConfirm.click();
      }
      //emitName: function() {
      //  this.$store.commit('categories/SET_FILTER', { 'Name': this.Name });

      //}
    },
    async created() {
      var vm = this;

      try {
        let response = await this.$http.get(this.urls[this.name].tableMetadata)
        this.isComplete = true;
        let data = response.data;

        //todo move .map and. filter  to API CONTROLLER 
        let fieldsToFilter = data.fields.filter(_ => _.filter).map(_ => _.name);
        this.columns = data.fields.map(_ => _.name);
        this.columns.push('Edit');
        this.options.filterable = fieldsToFilter; // set the default input text filter of the api
        this.options.customFilters = fieldsToFilter;  // allow fields to be filter

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
      }

    },
    data() {
      return {
        rowToDelete:null,
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
