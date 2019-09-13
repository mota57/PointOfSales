<template>
  <div>
    <h1>Product Information</h1>

    <div v-if="isAjax" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>

    <div v-if="!isAjax">

    <form ref="formElement"  v-if="isEdit"  @submit.prevent="upsert">
      <div class="row">
        <input type="hidden" v-model="form.Id" />

        <div class="col-6">
          <div class="form-group">
            <label for="Name">Name</label>
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

        <div class="col-6">
          <div class="form-group">
            <label for="ProductCode">Product Code</label>
            <input type="text" v-model="form.ProductCode" class="form-control" placeholder="Enter product code">
            <template v-if="errList && errList.ProductCode">
              <p class="text-danger" v-for="err in errList.ProductCode"> {{err}} </p>
            </template>
          </div>

          <div class="form-group">
            <label for="Category">Category </label>
            <v-select label="name" :options="options" :reduce="m => m.id" v-model="form.CategoryId" @search="onSearch" />
            <template v-if="errList && errList.Category">
              <p class="text-danger" v-for="err in errList.Category"> {{err}} </p>
            </template>
          </div>
        </div>
        <div class="col-6">

          <form-image ref="formImage1" :imagebytes="form.MainImage" v-model="form.MainImageForm">
            <template v-if="errList && errList.MainImage">
              <p class="text-danger" v-for="err in errList.MainImage"> {{err}} </p>
            </template>
          </form-image>

        </div>

      </div>
      <button v-if="display" type="submit" class="btn btn-primary" :disabled="isAjax">Save</button>
    </form>


      <div class="row" v-if="!isEdit">

        <div class="col-6">
          <div class="form-group">
            <label >Name</label>
            <span class="form-detail-value">
              {{form.Name}}
              <icon icon="pen" class="mr-2 menu-icon" style="float:right;cursor:pointer" @click="isEdit=!isEdit"/>
            </span>
          </div>

          <div class="form-group">
            <label >Price</label>
            <span class="form-detail-value">
              {{form.Price}}
              <icon icon="pen" class="mr-2 menu-icon" style="float:right;cursor:pointer;" @click="isEdit=!isEdit"/>
            </span>

          </div>

        </div>

        <div class="col-6">
          <div class="form-group">
            <label >Product Code</label>
            <span class="form-detail-value">
              {{form.ProductCode}}
              <icon icon="pen" class="mr-2 menu-icon" style="float:right;cursor:pointer" @click="isEdit=!isEdit"/>
            </span>
          </div>

          <div class="form-group">
            <label >Category </label>
            <div>
              <v-select disabled="true" label="name" :options="options" :reduce="m => m.id" v-model="form.CategoryId" @search="onSearch" />
            </div>
          </div>
        </div>
        <div class="col-6">
          <div class="form-group">
            <label>Image </label>
            <span class="form-detail-image">
              <img :src="'data:image/png;base64, '+ form.MainImage"  /><br />
              <icon icon="pen" class="mr-2 menu-icon" style="float:right;cursor:pointer" @click="isEdit=!isEdit"/>
            </span>
          </div>
        </div>
      </div>

    </div>
       <!--<pre>
{{form}}
</pre>-->

  </div>
</template>

<script>

  import _ from 'lodash'

  import { eventBus } from './event-bus'

  class ProductFormDTO {
    constructor() {
      this.Id = '';
      this.Name = '';
      this.Price = 0;
      this.ProductCode = '';
      this.CategoryId = 0;
      this.MainImage = '';
      this.MainImageForm = null;
      this.AttributeIds = []
    }
  }

  export default {
    props: { display: Boolean },
    data() {
      return {
        isEdit: false,
        isAjax: false,
        form: new ProductFormDTO(),
        errList: { MainImage: ['test error'] },
        options: [],
      }
    },
    created() {
      var vm = this;
      console.log(this.$refs);
      eventBus.$on('saveForm::form-product', () => vm.upsert({}))
      eventBus.$on('loadForm::form-product', (row) => vm.loadRecord(row))
      eventBus.$on('deleteForm::form-product', (row) => vm.deleteRecord(row))
      eventBus.$on('clearForm::form-product', (row) => {
        this.$refs.formImage1.removeImage();
        vm.clearForm()
      })
      vm.onSearch('', () => { })

    },
    methods: {
      clearForm() {
        this.form = new ProductFormDTO()
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
        this.$http.get(vm.urls.products.getById(row.Id))
          .then((res) => {
            let data = res.data
            vm.form.Id = data.id
            vm.form.Name = data.name
            vm.form.Price = data.price
            vm.form.ProductCode = data.productCode
            vm.form.CategoryId = data.categoryId
            vm.form.MainImage = data.mainImage
            vm.form.AttributeIds = data.attributeIds
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

  .form-detail-image {
    display: block;
    margin-top: .25rem;
    border-bottom: 0.5px solid #e4d5d5;
    height: 100%;
    padding-bottom:20px;
  }
  .form-detail-image img {
    height:200px;
    width:200px;
  }

  .form-detail-value {
    display: block;
    margin-top: .25rem;
    border-bottom: 0.5px solid #e4d5d5;
    height: 1.5rem;
  }
</style>
