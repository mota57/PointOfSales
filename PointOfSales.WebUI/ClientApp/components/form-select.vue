<template>
  <div>
    <div v-if="wasFirstAjaxCalled">
    <v-select  :multiple="isMultiple" :label="labelprop" :reduce="buildReduce"  :options="optionlist"  :value="selectedInternal"  v-on:input="emitValue" @search="onSearch" />
    </div>
  </div>
</template>

<script>

import _ from 'lodash'

export default {
  data(){
    return {
      optionlist:[],
      selectedInternal:null,
      wasFirstAjaxCalled: false,
    }
  },
  props : {
    selected:{

      required:false,
      default:0
    },
    isMultiple: {
      type:Boolean,
      default:false
    },
    labelprop: {
      type: String,
      default:'name'
    },
    optionprop: {
      type:Array,
      default: function() {
        return [];
      }
    },
    propkey: {
      type: String,
      default:'id'
    },
    urlapi: {
      type: String,
      required: true
    },
    initsearch: {
      type: Boolean,
      default:true
    }
 
  },
  created() {
    console.log('url api::'+this.urlapi);
    console.log('wasFirstAjaxCalled::'+this.wasFirstAjaxCalled);
    let vm = this;
    vm.makeHttpRequest(vm, '')
    .then(function(){
        vm.wasFirstAjaxCalled = true;
    })
  
  },
  methods: {
    buildReduce(obj){
      //console.log('obje:::'+ JSON.stringify(obj, null, 2));
      return obj[this.propkey];
    },
    emitValue(value) {
     // console.log('item key selected::'+ this.selectedInternal);
      this.$emit('input', value);
    },
    onSearch(search, loading) {
      loading(true)
      this.search(loading, search, this);
    },
    search: _.debounce((loading, search, vm) => {
      vm.makeHttpRequest(vm, search)
      .then(()=> {
        loading(false);
      })
    }, 350),
    makeHttpRequest: function(vm,search)
    {
      return vm.$http.get(vm.urls[vm.urlapi].picklist(search))
      .then(res => {
        vm.optionlist = res.data;
        vm.selectedInternal = _.find(vm.optionlist, (el) => el.id == vm.selected);
      })
    },
  },
}
</script>

<style>

</style>
