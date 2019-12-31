<template>
  <div>
    <div v-if="false">
      <label>pending</label>
      <ul>
        <li> create view/edit form</li>
        <li>datepicker</li>
        <li>multiselect </li>
        <li>checkbox group</li>
        <li>richtexteditor</li>
        <li>multiple files</li>
        <li>create a related tab</li>
        <li>create dropdown select create lookup</li>
        <li>fix on blur reset form</li>
      </ul>
    </div>

    <!-- modal form CREATE /UPDATE -->
    <b-modal static scrollable class="modal-dialog-scrollable" :id="modalFormId" size="xl" :title="title" hide-footer @shown="callLoad">
      <slot name="edit"></slot>

      <div class="modal-footer">
        <button type="button" @click="$bvModal.hide(modalFormId)" class="btn btn-secondary">Close</button>
        <button type="button" class="btn btn-primary" @click="callSave">Save</button>
      </div>
    </b-modal>
    <!--/ modal form CREATE /UPDATE -->


    <!-- MODAL DELETE  -->
    <b-modal :id="modalFormDelete" size="sm" :title="title" hide-footer centered >
      <p>Are you sure you want to delete this record?</p>

      <div class="modal-footer">
        <button type="button" @click="$bvModal.hide(modalFormDelete)" class="btn btn-secondary">Close</button>
        <button type="button" class="btn btn-primary" @click="() => { $bvModal.hide(this.modalFormDelete);  deleteRecord(); }">Save</button>
      </div>

    </b-modal>
    <!-- /MODAL DELETE  -->

    <!-- TABLE -->
    <div class="row">
      <div class="col-12" v-if="isComplete">
        <v-server-table ref="tableObj" :name="name" :url="urls[name].datatable" :columns="columns" :options="options">
          <div slot="beforeTable" style="border-bottom-width: 2px; border: 1px solid #dee2e6;">

            <b-button  @click="() => { currentRow = null; callClearForm();   $bvModal.show(modalFormId)}">
              <icon icon="plus" class="mr-2 menu-icon" /> Add
            </b-button>

            <b-button  @click="$bvModal.show(modalFilterId)"> <icon icon="filter" class="mr-2 menu-icon" /> </b-button>

            <slot name="sectionBeforeTable"></slot>

          </div>
          <!-- column actions -->
          <div slot="Edit" slot-scope="props">
            <a style="cursor:pointer" @click="() => { currentRow = props.row; $bvModal.show(modalFormId);  }"> <icon icon="edit" class="mr-2 menu-icon" /> </a>
            <a style="cursor:pointer" @click="()=> { currentRow = props.row; $bvModal.show(modalFormDelete);  }"> <icon icon="trash" class="mr-2 menu-icon" /> </a>
            <slot name="sectionAction" :row="props.row"></slot>
          </div>
          <!--/ column actions -->
        <template v-if="activeCustomFilters">
          <template v-for="(col, index) in columNames" >
            <div :slot="'filter__'+col" :key="index">
              <div>
                <input type="text " style="width: 7.5rem;height: 1.5rem;"   @input="debounceOnSearch($event, col, queryFields[index])"  >
                <i class="fa fa-filter"></i> 
              </div>
              <div class="">
                <select v-model="queryFields[index].operator">
                  <option >Contains</option>
                  <option value="StartWith">Start With</option>
                  <option value="EndWith">End With</option>
                  <option value="Equals">Equals</option>
                  <option value="NotEquals">Not equal to</option>
                  <option value="LessThan">Less than</option>
                  <option value="LessOrEqual">Less or Equal</option> 
                  <option value="GreaterThan">Greater than</option>
                  <option value="GreaterOrEqual">Greater or Equal</option>
                </select>
              </div>
            </div>
          </template>
        </template>

        </v-server-table>
      </div>
      <!-- /TABLE-->

      <b-modal static scrollable class="modal-dialog-scrollable" :id="modalFilterId" size="lg" title="Filters" hide-footer >
        <div class="card col-12">
          <div class="card-body">
            <h5 class="card-title">Create filter</h5>

            <div class="form-group row">
              <label class="col-sm-2 col-form-label">Search Name</label>
              <div class="col-sm-10">
                <input type="text" placeholder="unique search name" class="form-control"/> 
              </div>
            </div>


            <div class="form-group row">
              <label class="col-sm-2 col-form-label">Field</label>
              <div class="col-sm-10">
                <select class="form-control">
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
                <input type="text" class="form-control" />
              </div>
            </div>
  
            <a href="#" class="btn btn-default" @click="$bvModal.hide(modalFilterId)">Cancel</a>
            <a href="#" class="btn btn-primary">Save</a>
          </div>
        </div>
        </b-modal>

    </div>

  </div>
</template>
<script>

  import { mapGetters } from 'vuex'
  import { QueryFilterStore, QueryFilter } from './QueryFilterStore'

  let queryFilter = new QueryFilterStore();

  

  export default {
    props: ['name', 'eventpostfix', 'title'],
    methods: {
      callClearForm() {
        this.eventBus.$emit(`clearForm::${this.eventpostfix}`);
      },
      callSave() {
        this.eventBus.$emit(`saveForm::${this.eventpostfix}`);
      },
      callLoad() {
        if (this.currentRow) {
          this.eventBus.$emit(`loadForm::${this.eventpostfix}`, this.currentRow);
        }
      },
      beforeDestroy() {
        this.eventBus.$off(`reloadTable::${this.eventpostfix}`)
      },
      debounceOnSearch(event, colName, queryMeta){
        let vm = this;
        clearTimeout(this.debounce);
        this.debounce = setTimeout(()=>{ 
          queryMeta.value = event.target.value;
          vm.$refs.tableObj.setFilter(null);
          // vm.$store.commit(`${this.name}/SET_FILTER`, dataToSend);
          // this.$store.commit(`${this.name}/SET_CUSTOM_FILTER`, {filter:'Query', value:queryFilter.values()});
        },350)
      },
      reloadTable() {
        this.$refs.tableObj.refresh();
      },
      deleteRecord() {
        this.isAjax = true;
        var vm = this;
        this.$http.delete(vm.urls[this.name].delete(this.currentRow.Id))
          .then((res) => {
            this.$toasted.success('save success', {
              position:'top-center',
              duration:3000,
              fullWidth:true
            })
            vm.reloadTable()
          })
          .catch((err) => {
              console.info('err' + JSON.stringify(err.response.data));
              this.$toasted.error(err.response.data.title, {
                position: 'top-center',
                duration: 3000,
                fullWidth: true
              })
          })
          .then(() => { vm.isAjax = false; })
      },

      //to call custom fitler 
      //emitName: function() {
      //  this.$store.commit('${this.name}/SET_FILTER', { '<property_name>': <value> });
      //}
    },
    
    async created() {
      var vm = this;
      console.log(this);

      vm.modalFormId = 'modal-form-' + vm.name;
      vm.modalFormDelete = 'modal-form-delete-' + vm.name;
      vm.modalFilterId = 'modal-filter-' + vm.name;

      vm.eventBus.$on(`reloadTable::${this.eventpostfix}`, function () {
        console.log(`on::reloadTable::${this.eventpostfix}`)
        vm.$bvModal.hide(vm.modalFormId)
        vm.reloadTable()
      })

      try {
        if(this.urls[this.name] == null){
          throw "THE URL DOESN'T EXISTS PLEASE GO TO API-URL.js AND REGISTER IT THERE.";
        }
        let response = await this.$http.get(this.urls[this.name].tableMetadata)
        this.isComplete = true;
        let data = response.data;

        let fieldsToFilter = data.fields.filter(_ => _.filter).map(_ => _.name);
        let dateColumns = data.fields.filter(_ => _.filter && _.type == "DateTime").map(_ => _.name);

        for(let index in fieldsToFilter) {
          this.queryFields.push(new  QueryFilter(fieldsToFilter[index], null, null));
        }
        
        this.columns = data.fields.map(_ => _.name);
        this.columns.push('Edit');

        this.columNames = data.fields.map(_ => _.name);
        console.log('date columns' +  JSON.stringify(dateColumns))
        this.options.dateColumns = dateColumns;
        this.options.filterable = fieldsToFilter; // set the default input text filter of the api
        if(this.activeCustomFilters)
        {
          this.options.filterable = [];
          this.options.requestAdapter = function(data) {
            data.query = JSON.stringify(vm.queryFields);
            console.log('data to send' + JSON.stringify(vm.queryFields, null, 2));
            return data;
          }
       }  
      this.options.resizableColumns = true;

      

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
        console.error("FORM DATATABLE ERROR:: " + err);

          this.$bvToast.toast("FORM-DATATABLE ERROR REPORT:: " + err, {
          title: 'APPLICATION ERROR',
          autoHideDelay: 5000,
         
        })
      }

    },
    data() {
      return {

        modalFormId:'',
        modalFormDelete:'',
        modalFilterId:'',
        currentRow:null, //set this field to the current row on click icon delete
        isEdit:false,
        isComplete:false,
        isAjax: false,
        columns: [], // Id, name
        options: {
          filterByColumn: true,
        },
        activeCustomFilters: false,
        queryFields:[]
      }
    },
  }
</script>

<style>
table {
    font-size: 12px;

}
</style>
