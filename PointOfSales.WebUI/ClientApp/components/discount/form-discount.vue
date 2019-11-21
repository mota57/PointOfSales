<template>
    <div>

        <div v-if="isAjax" class="text-center">
            <p><em>Loading...</em></p>
            <h1><icon icon="spinner" pulse /></h1>
        </div>

        <div v-if="!isAjax">
            <form ref="formElement"  @submit.prevent="upsert">
                <div class="row">


                    
                            <div class="form-group">
                                    <label for="Name">Name</label>
                                        <input id="Name" type="text" v-model="form.name" class="form-control" placeholder="Enter Name">
                                    <template v-if="errList && errList.Name">
                                        <p class="text-danger" :key="$index" v-for="(err,$index) in errList.Name">  </p>
                                    </template>
                            </div>
                        
                        
                     
                            <div class="form-group">
                                    <label for="Amount">Amount</label>
                                        <input id="Amount" type="number" v-model="form.amount" class="form-control" placeholder="Enter Amount">
                                    <template v-if="errList && errList.Amount">
                                        <p class="text-danger" :key="$index" v-for="(err,$index) in errList.Amount">  </p>
                                    </template>
                            </div>
                        
                        
                        
                 
                        
                
                </div>
                <button v-if="displayBtn" type="submit" class="btn btn-primary" :disabled="isAjax">Save</button>
            </form>

          
        </div>
    </div>
</template>

<script>

    import _ from 'lodash'
    import { Helper } from '../../helper'

    const nameComponent = 'form-discount'


    class DiscountDTO {
        constructor() {
            
            this.id = 0; 
            this.amount = 0; 
            this.name = ''; 
        }
    }


    export default {
        props: { displayBtn: Boolean },
        data() {
            return {
                isEdit: true,
                isAjax: false,
                form: new DiscountDTO(),
                errList: { },
                
                apiUrl: ''
            }
        },
        beforeDestroy() {
            this.eventBus.$off(`saveForm::${nameComponent}`)
            this.eventBus.$off(`loadForm::${nameComponent}`)
            this.eventBus.$off(`clearForm::${nameComponent}`)

        },
        created() {
            var vm = this;
            this.apiUrl = this.urls.discount
            console.log(`${nameComponent}::created`)
            this.eventBus.$on(`saveForm::${nameComponent}`, vm.upsert)
            this.eventBus.$on(`loadForm::${nameComponent}`, (row) => vm.loadRecord(row))
            this.eventBus.$on(`clearForm::${nameComponent}`, vm.clearForm)

        },
        methods: {
            clearForm() {
                console.log(`${nameComponent}::clearform`);
                this.form = new DiscountDTO()
            },
            loadRecord(row) {
                this.isAjax = true;
                this.clearForm();
                var vm = this;
                this.$http.get(vm.apiUrl.getById(row.Id))
                    .then((res) => {
                        let data = res.data
                        Object.assign(vm.form, data);
                    })
                    .catch((err) => { if (err) { console.error(err); } })
                    .then(() => { vm.isAjax = false; })

            },
            upsert(e) {
                this.isAjax = true;
                
                var vm = this;
                this.$http({
                    method: vm.form.id ? 'put' : 'post',
                    url: this.apiUrl.upsert(vm.form.id),
                    data:  vm.form ,
                }).then(function (result) {
                    vm.errList = null;
                    vm.clearForm();
                    vm.eventBus.$emit(`reloadTable::${nameComponent}`)
                }).catch(function (error) {
                    if (error) {
                        if (error.response) {
                            vm.errList = error.response.data.errors;
                        }
                        console.error(error);
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
