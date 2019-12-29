<template>
  <div>
    <v-select  :multiple="isMultiple" :label="labelprop" :options="optionlist" :reduce="buildReduce" v-model="selectedInternal" v-on:input="emitValue" @search="onSearch" />
      prop {{selected}}<br/>
      internal {{selectedInternal}}
  </div>
</template>

<script>

import _ from 'lodash'

export default {
  data(){
    return {
      optionlist:[],
      selectedInternal:''
      
    }
  },
  props : {
    selected:{
      type:Number,
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
    console.log(this);
    this.optionlist = this.$props.optionprop;
    if (this.$props.initsearch) {
      this.onSearch('', () => { })
    }
   
  },
  methods: {
    buildReduce(obj){
      console.log('obje:::'+ JSON.stringify(obj, null, 2));
      return obj[this.propkey];
    },
    emitValue() {
      console.log('item key selected::'+ this.selectedInternal);
      this.$emit('input', this.selectedInternal);
    },
    onSearch(search, loading) {
      loading(true)
      this.search(loading, search, this);
    },
    search: _.debounce((loading, search, vm) => {
      vm.$http.get(vm.urls[vm.urlapi].picklist(search))
        .then(res => {
          vm.optionlist = res.data;
          loading(false)
        })

    }, 350),
  },
  
}
</script>

<style>

</style>
