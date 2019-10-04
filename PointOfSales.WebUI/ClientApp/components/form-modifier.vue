<template>
  <div style="padding:1rem">

    <div v-if="isAjax" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>

    <div v-if="!isAjax">
      <form ref="formElement" v-if="isEdit" @submit.prevent="upsert">
        <div class="row">
          <input type="hidden" v-model="form.id" />

          <div class="col-6">
            <div class="form-group">
              <label for="Name">Name</label>
              <input type="text" v-model="form.name" class="form-control" placeholder="Enter name">

              <template v-if="errList && errList.Name">
                <p class="text-danger" v-for="err in errList.Name"> {{err}} </p>
              </template>
            </div>
            <table class="table table-bordered">
              <thead>
                <tr>
                  <th scope="col">Item Modifier</th>
                  <th scope="col">Price</th>
                  <th scope="col">Action</th>
                </tr>
              </thead>
              <tbody>
                  <tr v-for="(item, index) in form.itemModifier" :key="index">
                    <td> <input class="form-control form-control-sm" v-model="item.name"/> </td>
                    <td> <input class="form-control form-control-sm" type="number" v-model="item.price"/> </td>
                    <td> <button type="button" class="btn-sm btn-block btn-secondary" @click="removeItemModifier(index)">Remove</button> </td>

                  </tr>
                  <tr>
                    <td> <input class="form-control form-control-sm" placeholder="enter name" v-model="formItemModifier.name"/> </td>
                    <td> <input class="form-control form-control-sm" type="number" placeholder="enter the price" v-model="formItemModifier.price"/> </td>
                    <td> <button type="button" class="btn-sm btn-block btn-primary" @click="addItemModifier()">Add</button> </td>
                  </tr>
              </tbody>
            </table>
          </div>

        </div>
        <button v-if="display" type="submit" class="btn btn-primary" :disabled="isAjax">Save</button>
      </form>
    </div>

  </div>
</template>

<script>

 const nameComponent = 'form-modifier'

  class Modifier {
    constructor() {
      this.id = 0;
      this.name = '';
      this.itemModifier = []; 
    }
  }

  class ItemModifier {
    constructor() {
      this.id = 0;
      this.name = '';
      this.modifierId = 0;
      this.price = 0;
    }
  }

  export default {
    props: { display: Boolean },
    data() {
      return {
        isEdit: true,
        isAjax: false,
        form: new Modifier(),
        errList: {  },
        options: [],
        formItemModifier: new ItemModifier(),
        apiUrl:''
      }
    },
    beforeDestroy() {
      this.eventBus.$on(`${nameComponent}::handler`)
      this.eventBus.$off(`saveForm::${nameComponent}`)
      this.eventBus.$off(`loadForm::${nameComponent}`)
      this.eventBus.$off(`clearForm::${nameComponent}`)
    },
    created() {
      var vm = this;
      this.apiUrl = this.urls.modifier
      this.eventBus.$on(`${nameComponent}::handler`, (handlerName) => vm[handlerName]())
      this.eventBus.$on(`saveForm::${nameComponent}`, () => vm.upsert({}))
      this.eventBus.$on(`loadForm::${nameComponent}`, (row) => vm.loadRecord(row))
      this.eventBus.$on(`clearForm::${nameComponent}`, (row) => {
        vm.clearForm()
      })

    },
    methods: {
      addItemModifier() {
        this.form.itemModifier.push(Object.assign({}, this.formItemModifier))
        this.formItemModifier = new ItemModifier()
      },
      removeItemModifier(index) {
        this.form.itemModifier.splice(index, 1)
      },
      clearForm() {
        this.form = new Modifier()
        this.formItemModifier = new ItemModifier()
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
      upsert(e) {

        this.isAjax = true;
        var vm = this;
        this.$http({
          method:  'post',
          url: this.apiUrl.upsert(),
          data: this.form,
        }).then(function (result) {
          vm.errList = null;
          vm.clearForm();
          vm.eventBus.$emit(`reloadTable::${nameComponent}`)
          vm.$emit(`save-success`)
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
