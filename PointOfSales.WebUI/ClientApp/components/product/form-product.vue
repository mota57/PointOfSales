<template>
  <div>
    <!--<pre>
{{form}}
</pre>-->

    <div v-if="isAjax" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>

    <div v-if="!isAjax">
      <form ref="formElement" :class="{ 'sr-only' : isEdit == false}" @submit.prevent="upsert">
        <div class="row">
          <input type="hidden" v-model="form.Id" />

          <div class="col-6">
            <div class="form-group">
              <label for="Name">Name</label>
              <input id="Name" type="text" v-model="form.name" class="form-control" placeholder="Enter name">

              <template v-if="errList && errList.Name">
                <p class="text-danger" v-for="err in errList.Name"> {{err}} </p>
              </template>
            </div>

            <div class="form-group">
              <label for="Price">Price</label>
              <input id="Price" type="number" v-model="form.price" class="form-control" placeholder="Enter Price">
              <template v-if="errList && errList.Price">
                <p class="text-danger" v-for="err in errList.Price"> {{err}} </p>
              </template>
            </div>

          </div>

          <div class="col-6">
            <div class="form-group">
              <label for="ProductCode">Product Code</label>
              <input id="ProductCode" type="text" v-model="form.productCode" class="form-control" placeholder="Enter product code">
              <template v-if="errList && errList.ProductCode">
                <p class="text-danger" v-for="err in errList.ProductCode"> {{err}} </p>
              </template>
            </div>

            <div class="form-group">
              <label for="Category">Category </label>
              <v-select  label="name" :options="options" :reduce="m => m.id" v-model="form.categoryId" @search="onSearch" />

              <b-button class="btn btn-light" v-b-modal.form-category>
                <icon icon="plus" class="mr-2 menu-icon" />
                Category
              </b-button>

              <template v-if="errList && errList.Category">
                <p class="text-danger" v-for="err in errList.Category"> {{err}} </p>
              </template>
            </div>
          </div>
          <div class="col-6">

            <div class="form-group">
              <label for="ProductModifier">Product Modifier </label>
              <v-select id="ProductModifier" multiple label="name" :options="optionModifier" :reduce="m => m.id" v-model="form.modifierIds" @search="onSearchModifier" />

              <b-button class="btn btn-light" v-b-modal.form-modifier>
                <icon icon="plus" class="mr-2 menu-icon" />
               Modifier 
              </b-button>
              <template v-if="errList && errList.ProductModifier">
                <p class="text-danger" v-for="err in errList.ProductModifier"> {{err}} </p>
              </template>
            </div>

            <form-image  ref="formImage1" :imagebytes="form.mainImage" v-model="form.mainImageForm">
              <template v-if="errList && errList.MainImage">
                <p class="text-danger" v-for="err in errList.MainImage"> {{err}} </p>
              </template>
            </form-image>


          </div>

        </div>
        <button v-if="display" type="submit" class="btn btn-primary" :disabled="isAjax">Save</button>
      </form>

      <!-- MODAL CATEGORY-->
      <b-modal id="form-category" title="Category" ok-only hide-footer>
        <form-category v-on:save-success="$bvModal.hide('form-category')"></form-category>
        <div class="modal-footer">
          <button type="button" @click="$bvModal.hide('form-category')" class="btn btn-secondary">Close</button>
          <button type="button" class="btn btn-primary" @click="eventBus.$emit('form-category::handler','upsert')">Save</button>
        </div>
      </b-modal>


      <!-- MODAL MODIFIER-->
      <b-modal id="form-modifier" size="lg" title="Modifier" ok-only hide-footer>
        <form-modifier v-on:save-success="$bvModal.hide('form-modifier')"></form-modifier>
        <div class="modal-footer">
          <button type="button" @click="$bvModal.hide('form-modifier')" class="btn btn-secondary">Close</button>
          <button type="button" class="btn btn-primary" @click="eventBus.$emit('form-modifier::handler','upsert')">Save</button>
        </div>
      </b-modal>

      <!--<div :class="{'row':true,  'sr-only': isEdit == true}">

    <div class="col-6">
      <div class="form-group">
        <label >Name</label>
        <span class="form-detail-value">
          {{form.name}}
          <icon icon="pen" class="mr-2 menu-icon" style="float:right;cursor:pointer" @click="isEdit=!isEdit"/>
        </span>
      </div>

      <div class="form-group">
        <label >Price</label>
        <span class="form-detail-value">
          {{form.price}}
          <icon icon="pen" class="mr-2 menu-icon" style="float:right;cursor:pointer;" @click="isEdit=!isEdit"/>
        </span>

      </div>

    </div>

    <div class="col-6">
      <div class="form-group">
        <label >Product Code</label>
        <span class="form-detail-value">
          {{form.productCode}}
          <icon icon="pen" class="mr-2 menu-icon" style="float:right;cursor:pointer" @click="isEdit=!isEdit"/>
        </span>
      </div>

      <div class="form-group">
        <label >Category </label>
        <div>
          <v-select :disabled="isEdit == false" label="name" :options="options" :reduce="m => m.id" v-model="form.categoryId" @search="onSearch" />
        </div>
      </div>
    </div>
    <div class="col-6">


      <div class="form-group">
        <label for="Product Modifier">Product Modifier </label>
        <v-select  :disabled="isEdit == false"  label="name" :options="optionModifier" :reduce="m => m.id" v-model="form.modifierIds" @search="onSearchModifier" />
      </div>

      <div class="form-group" @click="isEdit=!isEdit">
        <label>Image </label>
        <span class="form-detail-image">
          <img :src="'data:image/png;base64, '+ form.mainImage" /><br />
        </span>
      </div>
    </div>
  </div>-->

    </div>

  </div>
</template>

<script>
 
import _ from 'lodash'
import { Helper } from '../../helper'

const nameComponent = 'form-product'


  class ProductFormDTO {
    constructor() {
      this.id = ''; 
      this.name = '';
      this.price = 0;
      this.productCode = '';
      this.categoryId = 0;
      this.mainImage = '';
      this.mainImageForm = null;
      this.modifierIds = [];
    }
  }


  export default {
    props: { display: Boolean },
    data() {
      return {
        isEdit: true,
        isAjax: false,
        form: new ProductFormDTO(),
        errList: { MainImage: ['test error'] },
        options: [],
        optionModifier:[],
        isImageDeleted: false,
        apiUrl:''
      }
    },
    beforeDestroy() {
      this.eventBus.$off(`saveForm::${nameComponent}`)
      this.eventBus.$off(`loadForm::${nameComponent}`)
      this.eventBus.$off(`clearForm::${nameComponent}`)

    },
    created() {
      var vm = this;
      this.apiUrl = this.urls.product
      console.log(`${nameComponent}::created`)
      this.eventBus.$on(`saveForm::${nameComponent}`, vm.upsert)
      this.eventBus.$on(`loadForm::${nameComponent}`, (row) => vm.loadRecord(row))
      this.eventBus.$on(`clearForm::${nameComponent}`, vm.clearForm)
      //init searches 
      this.onSearch('', () => { })
      this.onSearchModifier('', () => { })

    },
    methods: {
      clearForm() {
        console.log(`${nameComponent}::clearform`);
        this.form = new ProductFormDTO()
      },
      loadRecord(row) {
        this.isAjax = true;
        this.clearForm();
        var vm = this;
        this.$http.get(vm.apiUrl.getById(row.Id))
          .then((res) => {
            let data = res.data
            Object.assign(vm.form, data);
          })
          .catch((err) => { if (err) { console.error(err); } })
          .then(() => { vm.isAjax = false; })

      },
      onSearchModifier(search, loading) {
        loading(true)
        this.searchModifier(loading, search, this);
      },
      searchModifier: _.debounce((loading, search, vm) => {
        vm.$http.get(vm.urls.modifier.picklist(search))
          .then(res => {
            vm.optionModifier = res.data;
            loading(false)
          })

      }, 350),
      onSearch(search, loading) {
        loading(true)
        this.search(loading, search, this);
      },
      search: _.debounce((loading, search, vm) => {
        vm.$http.get(vm.urls.category.picklist(search))
          .then(res => {
            vm.options = res.data;
            loading(false)
          })

      }, 350),
      upsert(e) {
        this.isAjax = true;
        let formData = Helper.createFormData(this.form);
     
        //avoid sending bytes
        formData.delete('mainImage');

        if (this.$refs.formImage1.isImageDeleted()) {
          formData.append('imageDeleted', true)
        }

        var vm = this;
        this.$http({
          method: vm.form.id ? 'put' : 'post',
          url: this.apiUrl.upsert(vm.form.id),
          data: formData,
        }).then(function (result) {
          vm.errList = null;
          vm.clearForm();
          vm.eventBus.$emit(`reloadTable::${nameComponent}`)
        }).catch(function (error) {
          if (error) {
            if (error.response) {
              vm.errList = error.response.data.errors;
            }
            console.error(error);
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
