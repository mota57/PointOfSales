<template>
  <div>
    <h1>Create Product</h1>
    <p>This component demonstrates fetching data from the server.</p>
    <div v-if="display" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>
    <form @submit="upsert" method="post" enctype="multipart/form-data">

      <div class="form-group">
        <label for="Name">Name</label>
        <input type="text" id="Name" class="form-control" aria-describedby="Name" placeholder="Enter name">
      </div>

      <div class="form-group">
        <label for="Price">Price</label>
        <input type="number" id="Price" class="form-control" aria-describedby="price" placeholder="Enter Price">
      </div>

      <div class="form-group">
        <label for="ProductCode">Product Code</label>
        <input type="text" id="ProductCode" class="form-control" aria-describedby="ProductCode" placeholder="Enter product code">
      </div>

      <div class="form-group">
        <label for="Image">Image </label> <br />
        <input type="file" id="Image" aria-describedby="Image" placeholder="Enter image">
      </div>
      <div class="form-group">
        <label for="Category">Category </label>
      </div>
      <button type="submit" class="btn btn-primary">Save</button>
    </form>

  </div>
</template>

<script>
export default {
  computed: {
    
  },
  data () {
    return {
      display:true,
      Name:'',
      Price:0,
      ProductCode:'',
      Category:''
    }
  },
  methods: {
      upsert(e) {
      e.preventDefault();
      //alert('hello');
      var result = this.$http.post(this.urls.product.upsert(), {
        Name: this.Name,
        Price:this.Price,
        ProductCode: this.ProductCode,
        Category: this.Category
      }).then(function (result) {
        console.log(result);
      }).catch(function (error) {
        console.log(error);
      });

    },
    loadPage () {
     
    }
  },

  async created () {
  }
}
</script>

<style>
</style>
