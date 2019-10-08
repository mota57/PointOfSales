<template>
  <div>
    <v-select  multiple="isMultiple" label="label" :options="options" :reduce="reduce" @input="this.$emit('input',  $event.target.value)" @search="onSearch" />
  </div>
</template>

<script>
export default {
  data () {
    return {
      isMultiple: {
        type:Boolean,
        default:false
      },
      label: {
        type: String,
        default:'name'
      },
      options: {
        type:Array,
        default: []
      },
      reduce: {
        type: Function,
        default: (m) => m.id
      },
      urlapi: {
        type: string,
        default: ''
      },
      initsearch: {
        type: Boolean,
        default:true
      }
    }
  },
  created() {
    if (this.initsearch) {
      this.onSearch('', () => { })
    }
  },
  methods: {
    onSearch(search, loading) {
      loading(true)
      this.search(loading, search, this);
    },
    search: _.debounce((loading, search, vm) => {
      vm.$http.get(vm.urls[vm.urlapi].picklist(search))
        .then(res => {
          vm.options = res.data;
          loading(false)
        })

    }, 350),
  }
}
</script>

<style>

</style>
