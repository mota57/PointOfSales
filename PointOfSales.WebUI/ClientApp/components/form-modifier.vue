<template>
  <div>
    <h1>Product Information</h1>

    <div v-if="isAjax" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>

    <div v-if="!isAjax">
      <form ref="formElement" v-if="isEdit" @submit.prevent="upsert">
        <div class="row">
          <input type="hidden" v-model="form.Id" />

          <div class="col-6">
            <div class="form-group">
              <label for="Name">Modifier Name</label>
              <input type="text" v-model="form.Name" class="form-control" placeholder="Enter name">

              <template v-if="errList && errList.Name">
                <p class="text-danger" v-for="err in errList.Name"> {{err}} </p>
              </template>
            </div>

            <div class="form-group">
              <label for="Price">Price</label>
              <input type="number" v-model="form.Price" class="form-control" placeholder="Enter Price">
              <template v-if="errList && errList.Price">
                <p class="text-danger" v-for="err in errList.Price"> {{err}} </p>
              </template>
            </div>

          </div>
          <hr/>
          <div>
            <table class="table table-bordered">
              <thead>
                <tr>
                  <th scope="col">#</th>
                  <th scope="col">Item Modifier</th>
                  <th scope="col">Price</th>
                </tr>
              </thead>
              <tbody>
                  <tr v-for="item in form.itemModifier" key="item.id">
                    <th>{{item.id}}</th>
                    <td>>item.name</td>
                    <td>item.price</td>
                  </tr>
              </tbody>
            </table>
          </div>

        </div>
        <button v-if="display" type="submit" class="btn btn-primary" :disabled="isAjax">Save</button>
      </form>
    </div>
    <pre>
{{form}}
</pre>

  </div>
</template>

<script>

  import _ from 'lodash'

  import { eventBus } from './event-bus'

  class Modifier {
    constructor() {
      this.Id = '';
      this.Name = '';
      this.itemModifier = []; 
    }
  }

  //class ItemModifier {
  //  constructor() {
  //    this.Id = '';
  //    this.Name = '';
  //    this.ModifierId = '';
  //    this.Price = '';
  //  }
  //}

  export default {
    props: { display: Boolean },
    data() {
      return {
        isEdit: true,
        isAjax: false,
        form: new Modifier(),
        errList: { MainImage: ['test error'] },
        options: [],
      }
    },
    created() {
      var vm = this;
      console.log(this.$refs);
      eventBus.$on('saveForm::form-modifier', () => vm.upsert({}))
      eventBus.$on('loadForm::form-modifier', (row) => vm.loadRecord(row))
      eventBus.$on('deleteForm::form-modifier', (row) => vm.deleteRecord(row))

      eventBus.$on('clearForm::form-modifier', (row) => {
        //clear image this.$refs.formImage1.removeImage();
        vm.clearForm()
      })

      //vm.onSearch('', () => { })

    },
    methods: {
      clearForm() {
        this.form = new Modifier()
        console.log('clearForm');
      },
      deleteRecord(row) {
        this.isAjax = true;
        var vm = this;
        this.$http.delete(vm.urls.products.delete(row.Id))
          .then((res) => {
            vm.$parent.reloadTable();
          })
          .then((err) => { if (err) { window.alert(err) } })
          .then(() => { vm.isAjax = false; })

      },
      loadRecord(row) {
        this.isAjax = true;
        this.clearForm();
        var vm = this;
        this.$http.get(vm.urls.modifier.getById(row.Id))
          .then((res) => {
            let data = res.data
            vm.form.Id = data.id
            vm.form.Name = data.name
            vm.form.ItemModifier = data.itemModifier;
          })
          .then((err) => { if (err) { console.log(err); } })
          .then(() => { vm.isAjax = false; })

      },
      //onSearch(search, loading) {
      //  loading(true)
      //  this.search(loading, search, this);
      //},
      //search: _.debounce((loading, search, vm) => {
      //  vm.$http.get(vm.urls.categories.picklist(search))
      //    .then(res => {
      //      vm.options = res.data;
      //      loading(false)
      //    })

      //}, 350),
      upsert(e) {

        this.isAjax = true;
        let formData = new FormData();

        for (let key in this.form) {
          let value = this.form[key];
          if (value) {
            if (Array.isArray(value)) {
              for (let i in value) {
                formData.append(key + "[" + i + "]", JSON.stringify(value[i]));
              }
            } else {
              formData.append(key, value);
            }
          }
        }

        //avoid sending bytes
        formData.delete('MainImage');

        if (this.$refs.formImage1.isImageDeleted()) {
          formData.append('imageDeleted', true)
        }

        var vm = this;
        this.$http({
          method: vm.form.Id ? 'put' : 'post',
          url: this.urls.products.upsert(vm.form.Id),
          data: formData,
        }).then(function (result) {
          vm.errList = null;
          vm.clearForm();
          vm.$parent.reloadTable();
        }).catch(function (error) {
          if (error) {
            vm.errList = error.response.data.errors;
          }
        }).then(function () {
          vm.isAjax = false;

        });

      },
    }
  }
</script>

<style>

 </style>
