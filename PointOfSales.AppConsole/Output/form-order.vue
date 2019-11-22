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
                                <label for="DiscountId">DiscountId</label>
                                <form-select :options="optionsDiscountId"  v-model="form.discountId"  ></form-select>
                                
                                <b-button class="btn btn-light" v-b-modal.form-discount>
                                    <icon icon="plus" class="mr-2 menu-icon" />
                                    Category
                                </b-button>

                                <template v-if="errList && errList.DiscountId">
                                    <p class="text-danger" v-for="err in errList.DiscountId">  </p>
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
    import { Helper } from '../helper'

    const nameComponent = 'form-order'


    class OrderDTO {
        constructor() {
            
            this.orderId = 0; 
            this.discountId = []; 
        }
    }


    export default {
        props: { displayBtn: Boolean },
        data() {
            return {
                isEdit: true,
                isAjax: false,
                form: new OrderDTO(),
                errList: { },
                 
                optionsDiscountId: [], 
                
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
            this.apiUrl = this.urls.order
            console.log(`${nameComponent}::created`)
            this.eventBus.$on(`saveForm::${nameComponent}`, vm.upsert)
            this.eventBus.$on(`loadForm::${nameComponent}`, (row) => vm.loadRecord(row))
            this.eventBus.$on(`clearForm::${nameComponent}`, vm.clearForm)

        },
        methods: {
            clearForm() {
                console.log(`${nameComponent}::clearform`);
                this.form = new OrderDTO()
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
