<template>
  <div>
    <v-select  :multiple="isMultiple" :label="labelprop" :options="optionlist" :reduce="m => m[propkey]" v-model="selected" v-on:input="emitValue" @search="onSearch" />
  </div>
</template>

<script>

import _ from 'lodash'

export default {
  data(){
    return {
      optionlist:[],
      selected: ''

    }
  },
  props : {
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
    this.optionlist = this.$props.optionprop;
    if (this.$props.initsearch) {
      this.onSearch('', () => { })
    }
   
  },
  methods: {
    emitValue() {
      console.log('item key selected::'+ this.selected);
      this.$emit('input', this.selected);
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
  }
}
</script>

<style>

</style>
