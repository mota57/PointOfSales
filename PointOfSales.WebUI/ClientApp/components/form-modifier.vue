<template>
  <div style="padding:1rem">
    <h1>Modifier Information</h1>

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
              <label for="Name">Modifier Name</label>
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
                    <td> <input v-model="item.name"/> </td>
                    <td> <input type="number" v-model="item.price"/> </td>
                    <td> <button type="button" @click="removeItemModifier(index)">Remove</button> </td>
                  </tr>
                  <tr>
                    <td> <input placeholder="enter name" v-model="formItemModifier.name"/> </td>
                    <td> <input type="number" placeholder="enter the price" v-model="formItemModifier.price"/> </td>
                    <td> <button type="button" @click="addItemModifier()">Add</button> </td>
                  </tr>
              </tbody>
            </table>
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
        formItemModifier: new ItemModifier()
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
        this.$http.get(vm.urls.modifier.getById(row.id))
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
          url: this.urls.modifier.upsert(vm.form.id),
          data: this.form,
        }).then(function (result) {
          vm.errList = null;
          vm.clearForm();
          eventBus.$emit(`reloadTable::${nameComponent}`)
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
