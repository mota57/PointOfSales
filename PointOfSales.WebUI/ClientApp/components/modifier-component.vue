<template>
  <div>

    <h3>Modifier</h3>
    <form-datatable name="modifier" eventpostfix="form-modifier" title="Modifier">
      <template v-slot:edit>
        <form-modifier></form-modifier>
      </template>
      <template v-slot:sectionAction="row">
        <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#modalApplyToItem" @click="currentRow = row">Apply to an item</button>
      </template>
    </form-datatable>

    <div class="modal fade" id="modalApplyToItem" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">Apply to Product</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <div class="col-12">
              <div class="form-group">
                <label>Product </label>
                <div>
                  <v-select multiple label="name" :options="optionProduct" :reduce="m => m.id" v-model="form.productId" @search="onSearch" />
                </div>
              </div>
            </div>
          </div>

          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            <button type="button" class="btn btn-primary">Save changes</button>
          </div>
        </div>
      </div>
    </div>

  </div>
</template>

<script>


  export default {
    data() {
      return {
        currentRow: null,
        optionProduct: [],
        form: {
          productId: 0
        }
      }
    },
    methods: {
      onSearch(search, loading) {
        loading(true)
        this.search(loading, search, this);
      },
      search: _.debounce((loading, search, vm) => {
        vm.$http.get(vm.urls.products.picklist(search))
          .then(res => {
            vm.optionProduct = res.data;
            loading(false)
          })

      }, 350),
    },

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
    upsertProductModifiers(e) {

      this.isAjax = true;


      var vm = this;
      this.$http({
        method: 'post',
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
</script>

<style>
</style>
