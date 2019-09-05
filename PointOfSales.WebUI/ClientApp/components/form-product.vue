<template>
  <div>
    <h1>Product Information</h1>
    <div v-if="isAjax" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>
    <form @submit="upsert" method="post" enctype="multipart/form-data" action="">

      <div class="form-group">
        <label for="Name">Name</label>
        <input type="text" v-model="Name" id="Name" class="form-control" aria-describedby="Name" placeholder="Enter name">
        <template v-if="errList && errList.Name">
          <p class="text-danger" v-for="err in errList.Name"> {{err}} </p>
        </template>
      </div>

      <div class="form-group">
        <label for="Price">Price</label>
        <input type="number" id="Price" v-model="Price" class="form-control" aria-describedby="price" placeholder="Enter Price">
        <template v-if="errList && errList.Price">
          <p class="text-danger" v-for="err in errList.Price"> {{err}} </p>
        </template>
      </div>

      <div class="form-group">
        <label for="ProductCode">Product Code</label>
        <input type="text" id="ProductCode" v-model="ProductCode" class="form-control" aria-describedby="ProductCode" placeholder="Enter product code">
        <template v-if="errList && errList.ProductCode">
          <p class="text-danger" v-for="err in errList.ProductCode"> {{err}} </p>
        </template>
      </div>

      <div class="form-group">
        <img :src="ImagePicture" width="200" height="200" /><br />
        <input type="button" @click="removeImage" value="Remove" :disabled="!Image">
        <input type="file" @change="onFileChange" placeholder="Enter image">
        <template v-if="errList && errList.Image">
          <p class="text-danger" v-for="err in errList.Image"> {{err}} </p>
        </template>
      </div>

      <div class="form-group">
        <label for="Category">Category </label>
        <v-select label="name" :options="options" v-model="Category" @search="onSearch" />
        <template v-if="errList && errList.Category">
          <p class="text-danger" v-for="err in errList.Category"> {{err}} </p>
        </template>
      </div>
      <button type="submit" class="btn btn-primary">Save</button>
    </form>

  </div>
</template>

<script>

  import _ from 'lodash'

  export default {
    data() {
      return {
        isAjax: false,
        Name: '',
        Price: 0,
        ProductCode: '',
        Category: '',
        Image: '',
        ImagePicture: '',
        errList: null,
        options: [],
      }
    },
    methods: {
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
        e.preventDefault();


        var formData = new FormData();
        formData.set("Name", this.Name);
        formData.set("Price", this.Price);
        formData.set("ProductCode", this.ProductCode);
        if (this.Category) {
          //can't send null if not it will send it as a string
          formData.set("CategoryId", this.Category.id);
        }
        console.log(formData);
        formData.append("Image", this.Image);

        this.$http({
          method:'post',
          url: this.urls.products.upsert(),
          data: formData,
          config: {
            headers: {
              "mimeType": "multipart/form-data"
            }
          }
        }).then(function (result) {
          vm.errList = null;

        }).catch(function (error) {
          vm.errList = error.response.data.errors;
        });

      },
      onFileChange(e) {
        var files = e.target.files || e.dataTransfer.files;
        if (!files.length)
          return;

        this.Image = files[0];
        this.createImageBase64(files[0]);
      },
      createImageBase64(file) {
        var reader = new FileReader();
        var vm = this;

        reader.onload = (e) => {
          console.log('loaded');
          vm.ImagePicture = e.target.result;
        };

        reader.readAsDataURL(file);
      },
      removeImage: function (e) {
        this.ImagePicture = '';
        this.Image = null;
      },
    }
  }
</script>

<style>
</style>
