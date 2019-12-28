<template>
  <transition>
    <div>
      <div class="row">
        <div class="col-3">
          <b-card no-body style="" img-alt="Image" img-top class="mt-2">
            <template v-slot:header>
              <div class="row">
                <div class="col-12">
                  <b-button class="btn btn-block" @click="moveTo()">Charge ${{totalOrderItemCharge}}</b-button>
                </div>
              </div>
            </template>
            <!-- orderItem -->
            <div style="height: 500px;overflow: auto;">
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
                    <div class="col-12">
                      <b-button class="btn btn-sm float-right" 
                       @click="orderItemToConfig=orderItem;$bvModal.show('item-configure')">config</b-button>
                    </div>
                  </div>
                </b-list-group-item>
              </b-list-group>
            </div>
            <!--/orderItem -->
            <b-card-footer>

              <b-button class="btn-block" @click="$bvModal.show('order-configure')">set disccount</b-button>
              <b-button class="btn-block" @click="clearItem()">clear items</b-button>

            </b-card-footer>
          </b-card>
        </div>

        <div class="col-9">
          <!--categories -->
          <b-nav pills>
            <template v-for="(cat,index) in categories">
              <b-nav-item :key="index" v-on:click="filterByCategory(cat)">{{cat.name}}</b-nav-item>
            </template>
          </b-nav>
          <!--/categories -->

          <input type="text" @input="debounceOnSearch"  class="form-control ml-2 mb-2" placeholder="search..." />
          

           <select v-model="perPage">
            <option>5</option>
            <option>10</option>
            <option>15</option>
          </select>

          <b-pagination
            v-model="currentPage"
            :total-rows="rows"
            :per-page="perPage"
            aria-controls="my-table"
          ></b-pagination>
          <!-- products -->
          <div class="row">
            <template v-for="(item, index) in products">
              <b-card
                :key="index"
                :img-src="item.mainImage"
                img-alt="Image"
                img-top
                tag="article"
                footer="Card Footer"
                style="width: 220px;"
                class="mb-2 ml-2"
                @click="pushOrderItem(item)"
              >
                <b-card-body>
                  <b-card-text>{{item.name}}</b-card-text>
                </b-card-body>

                <template v-slot:footer>
                  <span class="badge badge-secondary">${{item.price}}</span>
                </template>
              </b-card>
            </template>

            <!-- products -->
          </div>

          <b-pagination
            v-model="currentPage"
            :total-rows="rows"
            :per-page="perPage"
            aria-controls="my-table"
          ></b-pagination>
        </div>
      </div>
      <!-- modals --> 
      <b-modal id="item-configure" size="lg" title="Product configure" ok-only hide-footer>
        <item-configure :orderitem="orderItemToConfig"></item-configure>
        <!-- <div class="modal-footer">
          <button type="button" @click="$bvModal.hide('item-configure')" class="btn btn-secondary">Close</button>
          <button type="button" class="btn btn-primary" @click="eventBus.$emit('item-configure::handler','upsert')">Save</button>
        </div>-->
      </b-modal>
      <!-- modals -->
    </div>

  </transition>
</template>

<script>
import _ from 'lodash'
import faker from "faker";
import { mapGetters, mapState, mapMutations } from "vuex";

function buildRequest(vm) {
  if (vm.query != null && typeof vm.query === "object") {
    vm.query = JSON.stringify(vm.query);
  }

  return {
    PerPage: vm.perPage,
    OrderBy: vm.orderBy,
    Page: vm.currentPage,
    Query: vm.query,
    ByColumn: 1
  };
}

export default {
  name: "pos",
  data() {
    return {
      categories: [],
      products: [],
      orderItemToConfig: null,
      imgCategory: faker.image.avatar(),
      searchTextbox:'',
      debounce: null,

      perPage: 15,
      orderBy: [],
      currentPage: 1,
      query: '',
      totalElements: 0,
      ByColumn: 1
    };
  },
  methods: {
    filterByCategory(categoryObject) {
      this.query = [
        {
          Name: "categoryName",
          Value: categoryObject.name,
          Operator: "Contains",
          DateLogicalOperator: null
        }
      ];
      this.loadData(buildRequest(this));
    },
    debounceOnSearch(event){
      let vm = this;
 
      clearTimeout(this.debounce);
      this.debounce = setTimeout(()=>{
        let searchValue = event.target.value;
        this.query = [{
          Name: "name",
          Value:  searchValue,
          Operator: "Contains",
          DateLogicalOperator: null
        }];
        vm.loadData(buildRequest(vm));
      },350)
    },
    moveTo() {
      this.$router.push({ name: "checkout" }).catch(err => {});
    },
    loadData(data) {
      let vm = this;
      this.$http
        .post(this.urls.merchant.ProductPosList, data)
        .then(function(response) {
          vm.products = response.data.data;
          vm.totalElements = response.data.count;
        })
        .catch(function(err) {
          console.error(err);
          vm.$toasted.error("Sorry an error occured contact the developers", {
            position: "top-center",
            duration: 3000,
            fullWidth: true
          });
        });
    },
    loadCategories(){
      let vm = this;  
      this.$http.get(this.urls.category.picklist()).then(function(response) {
        vm.categories = response.data;
      });
    },
    ...mapMutations({
      clearItem: "clearOrderItemList",
      pushOrderItem: "setOrderItemList"
    })
  },
  computed: {
    ...mapState(["orderItemList"]),
    ...mapGetters(["totalOrderItemCharge"]),
    
    rows() {
      return this.totalElements;
    }
  },
  watch: {
    perPage: function(newVal, oldVal) {
      this.loadData(buildRequest(this));
    },
    currentPage: function(newVal, oldVal) {
      this.loadData(buildRequest(this));
    }
  },
  created() {
    var vm = this;
    this.loadCategories();

    this.loadData({
      PerPage: this.perPage,
      OrderBy: [],
      Page: 1,
      Query: "",
      ByColumn: 1
    });
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
