<template>
  <transition>
    <div class="">
      <div class="row">
        <div class="col-3">
          <b-card no-body
                  style="max-width: 20rem;"
                  img-alt="Image"
                  img-top
                  class='mt-2'>
            <template v-slot:header>
              <div class="row">
                <div class="col-12">
                  <b-button class='btn btn-block' @click="moveTo()">Charge ${{totalOrderItemCharge}}</b-button>
                </div>
              </div>
            </template>
            <!-- orderItem -->
            <div style="height: 400px;overflow: auto;">
              <b-list-group flush>
                <b-list-group-item v-for="(orderItem,index) in orderItemList" :key="index">

                  <div class="row">
                    <div class="col-2">
                      <b-img height="30" :src="orderItem.mainImage"></b-img>
                    </div>
                    <div class="col-9">
                      {{orderItem.name}}
                      <b-badge>{{orderItem.quantity}}</b-badge>
                      <span class="float-right">{{orderItem.price * orderItem.quantity}}</span>
                    </div>
                    <div class="col-12" v-if="orderItem.isProductForRent">
                      
                      <b-button class="btn btn-sm float-right" 
                      @click="orderItemToConfig=orderItem;$bvModal.show('item-configure')">
                         config
                        </b-button>
                    </div>
                  </div>

                </b-list-group-item>
              </b-list-group>
              
              <b-button class="btn btn-sm float-right"  @click="$bvModal.show('order-configure')"> set disccount </b-button>
            </div>
            <!--/orderItem -->
            <b-card-footer>

              <b-button @click="clearItem()">clear items</b-button>
            </b-card-footer>
          </b-card>
        </div>

        <div class="col-9">
          <!--categories -->
          <b-nav pills>
            <b-nav-item v-for="(cat,index) in categories" :key="index">
              {{cat.name}}
            </b-nav-item>
          </b-nav>
          <!--/categories -->
          <!-- products -->
          <div class="row">
            <input text="search" class="form-control ml-2 mb-2" placeholder="search..." />
            <template v-for="(item, index) in products">
              <b-card :key="index"
                      :img-src="item.mainImage"
                      img-alt="Image"
                      img-top
                      tag="article"
                      style="width: 120px;"
                      class="mb-2 ml-2" @click="pushOrderItem(item)">
                <b-card-text>
                  {{item.name}}<br />
                  <span class="badge badge-secondary">${{item.price}}</span>
                </b-card-text>
              </b-card>
            </template>
            <b-pagination v-model="currentPage"
                          :total-rows="rows"
                          :per-page="perPage"
                          aria-controls="my-table"></b-pagination>

            <!-- products -->
          </div>
        </div>

      </div>
      <b-modal id="item-configure" size="lg" title="Product configure" ok-only hide-footer>
        <item-configure :orderitem="orderItemToConfig" ></item-configure>
        <!-- <div class="modal-footer">
          <button type="button" @click="$bvModal.hide('item-configure')" class="btn btn-secondary">Close</button>
          <button type="button" class="btn btn-primary" @click="eventBus.$emit('item-configure::handler','upsert')">Save</button>
        </div> -->
      </b-modal>
    </div>

      

  </transition>
  
</template>

<script>
  import faker from "faker";
  import { mapGetters, mapState, mapMutations } from 'vuex'

  export default {
    name: "pos",
    data() {
      return {
        categories: [{ name: 'category1' }, { name: 'category2' }, { name: 'foo' }],
        products: [],
        orderItemToConfig: null,
        imgCategory: faker.image.avatar(),
        perPage: 15,
        currentPage: 1,
      };
    },
    methods: {
      moveTo() {
        this.$router.push({ name: 'checkout' }).catch((err) => { })
      },
      loadData() {
        let vm = this;
        this.$http.post(this.urls.merchant.ProductPosList, {
          OrderBy: [],
          Page: 1,
          Query: '',
          IsFilterByColumn: false
        }).then(function (response) {
          vm.products = response.data
        }).catch(function (err) {
          console.error(err);
          vm.$toasted.error('Sorry an error occured contact the developers', {
            position: 'top-center',
            duration: 3000,
            fullWidth: true
          })
        });
      },
      ...mapMutations({ clearItem: 'clearOrderItemList', pushOrderItem: 'setOrderItemList' })
    },
    computed: {
      ...mapState([
        'orderItemList',
      ]),
      ...mapGetters([
        'totalOrderItemCharge'
      ]),
      rows() {
        return this.products.length
      }
    },
    created() {
      this.loadData();
      //this.$http.get(this.urls..getById(row.Id))
      //    .then((res) => {
      //      let data = res.data
      //      Object.assign(vm.form, res.data)
      //    })
      //    .then((err) => { if (err) { console.log(err); } })
      //    .then(() => { vm.isAjax = false; })

      //},
       //for (let i = 0; i < 10; i++) {
       //  let imgSrc = faker.image.avatar();

       //  this.products.push({
       //    id: i + 1,
       //    title: faker.name.firstName(),
       //    mainImage: imgSrc,
       //    body: faker.lorem.sentence(),
       //    price: faker.random.number({ min: 5, max: 20000 }),
       //    isProductForRent: true,
       //    startDate: '',
       //    endDate: '',
       //    disscountType: 'none',
       //    discountId: -1,
       //  })
       //}
    }
  };
</script>

<style>
</style>
