<template>
  <div style="padding:1rem">
    <div v-if="isAjax" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>

    <div v-if="!isAjax">
      <form ref="formElement" v-if="isEdit" @submit.prevent="upsert">
        <div class="row">

          <div class="col-6">
            <div class="form-group">
              <label for="Name">Category Name</label>
              <input type="text" v-model="form.name" class="form-control" placeholder="Enter name">

              <template v-if="errList && errList.Name">
                <p class="text-danger" v-for="err in errList.Name"> {{err}} </p>
              </template>
            </div>
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

  import { eventBus } from './event-bus'

 const nameComponent = 'form-category'

  class Category {
    constructor() {
      this.id = '';
      this.name = '';
    }
  }

 export default {
    props: { display: Boolean },
    data() {
      return {
        isEdit: true,
        isAjax: false,
        form: new Category(),
        errList: {  },
        options: [],
      }
    },
    created() {
      var vm = this;
      console.log(this.$refs);
      eventBus.$on(`saveForm::${nameComponent}`, () => vm.upsert({}))
      eventBus.$on(`loadForm::${nameComponent}`, (row) => vm.loadRecord(row))
      eventBus.$on(`clearForm::${nameComponent}`, (row) => {
        vm.clearForm()
      })

    },
    methods: {
      clearForm() {
        this.form = new Category()
      },
      loadRecord(row) {
        this.isAjax = true;
        this.clearForm();
        var vm = this;
        this.$http.get(vm.urls.categories.getById(row.Id))
          .then((res) => {
            let data = res.data
            Object.assign(vm.form, res.data)
          })
          .then((err) => { if (err) { console.log(err); } })
          .then(() => { vm.isAjax = false; })

      },
      upsert(e) {

        this.isAjax = true;
        var vm = this;
        this.$http({
          method: 'post',
          url: this.urls.categories.upsert(vm.form.id),
          data: JSON.stringify(this.form),
        }).then(function (result) {
          vm.errList = null;
          vm.clearForm();
          vm.$parent.reloadTable(); //warning remove this
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
