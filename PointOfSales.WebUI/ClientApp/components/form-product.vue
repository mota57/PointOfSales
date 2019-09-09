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
            <input type="text" v-model="form.Name"  class="form-control" aria-describedby="Name" placeholder="Enter name">
            <template v-if="errList && errList.Name">
              <p class="text-danger" v-for="err in errList.Name"> {{err}} </p>
            </template>
          </div>

          <div class="form-group">
            <label for="Price">Price</label>
            <input type="number"  v-model="form.Price" class="form-control" aria-describedby="price" placeholder="Enter Price">
            <template v-if="errList && errList.Price">
              <p class="text-danger" v-for="err in errList.Price"> {{err}} </p>
            </template>
          </div>

        </div>

        <div class="col-6">
          <div class="form-group">
            <label for="ProductCode">Product Code</label>
            <input type="text"  v-model="form.ProductCode" class="form-control" aria-describedby="ProductCode" placeholder="Enter product code">
            <template v-if="errList && errList.ProductCode">
              <p class="text-danger" v-for="err in errList.ProductCode"> {{err}} </p>
            </template>
          </div>

          <div class="form-group">
            <label for="Category">Category </label>
            <v-select label="name" :options="options" :reduce="model => model.id" v-model="form.CategoryId" @search="onSearch" />
            <template v-if="errList && errList.Category">
              <p class="text-danger" v-for="err in errList.Category"> {{err}} </p>
            </template>
          </div>
        </div>
        <div class="col-6">

          <form-image v-model="form.Image">
            <template v-if="errList && errList.Image">
              <p class="text-danger" v-for="err in errList.Image"> {{err}} </p>
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
          CategoryId: '',
          Image: '',
        },
        errList: { Image: ['test error'] },
        ImagePicture: '',
        options: [],
      }
    },
    created() {
      var vm = this;
      eventBus.$on('saveForm::form-product', () => vm.upsert({}))
      eventBus.$on('loadForm::form-product', (row) => vm.loadRecord(row))
      eventBus.$on('deleteForm::form-product', (row) => vm.deleteRecord(row))

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
        var vm = this;
        this.$http.get(vm.urls.products.getById(row.Id))
          .then((res) => {
            console.info(res);

            let data = res.data



            vm.form.Id = data.id
            //TODO show image picture
            vm.form.Name = data.name
            vm.form.Price = data.price
            vm.form.ProductCode = data.productCode
            vm.form.CategoryId = data.categories[0].id

            this.$http.get(vm.urls.products.get() +'/GetImage/'+data.id)
              .then((res) => {
                vm.form.Image = res.data.mainImage
              })


          }, {timeout: 10*1000})
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
        var vm = this;
        console.log(vm);
        var formData = new FormData();
        //if (this.Id) {
        //  formData.set("Id", this.form.Id);
        //}
        //formData.set("Name", this.form.Name);
        //formData.set("Price", this.form.Price);
        //formData.set("ProductCode", this.form.ProductCode);
        //if (this.Category) {
          //can't send null if not it will send it as a string
          //formData.set("CategoryId", this.Category);
        //}
        //formData.append("Image", this.Image);
        for (let key in this.form) {
          let value = this.form[key]
          if (value) {
            formData.append(key, value);
          }
        }

        this.isAjax = true;

        this.$http({
          method: vm.form.Id ? 'put' : 'post',
          url: this.urls.products.upsert(vm.form.Id),
          data: formData,
          config: {
            headers: {
              "mimeType": "multipart/form-data"
            }
          }
        }).then(function (result) {
          vm.errList = null;
          vm.$parent.reloadTable();
          vm.form = { Id: '', Name: '', Price: 0, ProductCode: '', CategoryId: '', Image: '' }

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
