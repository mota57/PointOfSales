<template>
  <div>
    <h1>Product Information</h1>

    <div v-if="isAjax" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>
    <form ref="form" v-if="!isAjax" @submit.prevent="upsert" >
      <div class="row">

        <div class="col-6">
          <div class="form-group">
            <label for="Name">Name</label>
            <input type="text" v-model="form.Name"  class="form-control"  placeholder="Enter name">
            <template v-if="errList && errList.Name">
              <p class="text-danger" v-for="err in errList.Name"> {{err}} </p>
            </template>
          </div>

          <div class="form-group">
            <label for="Price">Price</label>
            <input type="number"  v-model="form.Price" class="form-control"  placeholder="Enter Price">
            <template v-if="errList && errList.Price">
              <p class="text-danger" v-for="err in errList.Price"> {{err}} </p>
            </template>
          </div>

        </div>

        <div class="col-6">
          <div class="form-group">
            <label for="ProductCode">Product Code</label>
            <input type="text" v-model="form.ProductCode" class="form-control"  placeholder="Enter product code">
            <template v-if="errList && errList.ProductCode">
              <p class="text-danger" v-for="err in errList.ProductCode"> {{err}} </p>
            </template>
          </div>

          <div class="form-group">
            {{form.CategoryId}}
            <label for="Category">Category </label>
            <v-select label="name" :options="options" :reduce="m => m.id" v-model="form.CategoryId" @search="onSearch" />
            <template v-if="errList && errList.Category">
              <p class="text-danger" v-for="err in errList.Category"> {{err}} </p>
            </template>
          </div>
        </div>
        <div class="col-6">

          <form-image v-model="form.MainImage">
            <template v-if="errList && errList.MainImage">
              <p class="text-danger" v-for="err in errList.MainImage"> {{err}} </p>
            </template>
          </form-image>

        </div>
      </div>

      <button v-if="display" type="submit" class="btn btn-primary" :disabled="isAjax">Save</button>
    </form>

  </div>
</template>

<script>

  import _ from 'lodash'

  import { eventBus } from './event-bus'

  export default {
    props: { display: Boolean },
    data() {
      return {
        isAjax: false,
        form : {
          Id: '',
          Name: '',
          Price: 0,
          ProductCode: '',
          CategoryId: [],
          MainImage: '',
        },
        errList: { MainImage: ['test error'] },
        ImagePicture: '',
        options: [],
      }
    },
    created() {
      var vm = this;
      console.log(this.$refs);
      eventBus.$on('saveForm::form-product', () => vm.upsert({}))
      eventBus.$on('loadForm::form-product', (row) => vm.loadRecord(row))
      eventBus.$on('deleteForm::form-product', (row) => vm.deleteRecord(row))
      eventBus.$on('clearForm::form-product', (row) => vm.clearForm())
      vm.onSearch('', () => {})

    },
    mounted() {
      this.formElement = this.$refs.form

    },
    methods: {
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
        this.$http.get(vm.urls.products.getById(row.Id))
          .then((res) => {

            let data = res.data
            vm.form.Id = data.id
            vm.form.Name = data.name
            vm.form.Price = data.price
            vm.form.ProductCode = data.productCode
            vm.form.CategoryId = data.categoryId
            vm.form.MainImage = data.imageByte
          })
          .then((err) => { if (err) { console.log(err); } })
          .then(() => { vm.isAjax = false; })

      },
      onSearch(search, loading) {
        loading(true)
        this.search(loading, search, this);
      },
      search: _.debounce((loading, search, vm) => {
        vm.$http.get(vm.urls.categories.picklist(search))
          .then(res => {
            vm.options = res.data;
            loading(false)
          })

      }, 350),
      clearForm() {
        this.form = { Id: '', Name: '', Price: 0, ProductCode: '', CategoryIds:'', MainImage: '' }
        console.log('clearForm');
      },
      upsert(e) {
        var vm = this;
        console.log(vm);
        var formData = new FormData();

        for (let key in this.form) {
          let value = this.form[key];
          if (value) {
            if (Array.isArray(value)) {
              for (let i in value) {
                formData.append(key +"[" + i + "]",  JSON.stringify(value[i]));
              }
            } else {
              formData.append(key, value);
            }
          }
        }

        this.isAjax = true;

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
