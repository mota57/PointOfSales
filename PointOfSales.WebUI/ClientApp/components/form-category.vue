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
        <button v-if="displayBtn" type="submit" class="btn btn-primary" :disabled="isAjax">Save</button>
      </form>
    </div>

 <!--<pre>
{{form}}
</pre>-->

  </div>
</template>

<script>


 const nameComponent = 'form-category'

  class Category {
    constructor() {
      this.id = 0;
      this.name = '';
    }
  }

 export default {
    props: { displayBtn: Boolean, default:false },
    data() {
      return {
        isEdit: true,
        isAjax: false,
        form: new Category(),
        errList: {  },
        options: [],
        apiUrl:'',
      }
    },
    beforeDestroy() {
      this.eventBus.$off(`${nameComponent}::handler`)
      this.eventBus.$off(`saveForm::${nameComponent}`)
      this.eventBus.$off(`loadForm::${nameComponent}`)
      this.eventBus.$off(`clearForm::${nameComponent}`)

    },
    created() {
      var vm = this;
      vm.apiUrl = vm.urls.categories;
      this.eventBus.$on(`${nameComponent}::handler`, (handlerName) => vm[handlerName]())
      this.eventBus.$on(`saveForm::${nameComponent}`,  vm.upsert)
      this.eventBus.$on(`loadForm::${nameComponent}`,  vm.loadRecord)
      this.eventBus.$on(`clearForm::${nameComponent}`, (row) => { vm.clearForm() })

    },
    methods: {
      clearForm() {
        this.form = new Category()
      },
      loadRecord(row) {
        this.isAjax = true;
        this.clearForm();
        var vm = this;
        this.$http.get(vm.apiUrl.getById(row.Id))
          .then((res) => {
            let data = res.data
            Object.assign(vm.form, res.data)
          })
          .then((err) => { if (err) { console.log(err); } })
          .then(() => { vm.isAjax = false; })

      },
      upsert() {

        this.isAjax = true;
        var vm = this;
        this.$http({
          method:  vm.form.id ? 'put' : 'post',
          url: this.apiUrl.upsert(vm.form.id),
          data: this.form,
        }).then(function (result) {
          vm.errList = null;
          vm.clearForm();
          vm.eventBus.$emit(`reloadTable::${nameComponent}`)
          vm.$emit(`save-success`)
        }).catch(function (error) {
          if (error) {
            if (error.response) {
              vm.errList = error.response.data.errors;
            }
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
