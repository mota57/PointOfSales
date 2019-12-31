import CounterExample from 'components/counter-example'
import FetchData from 'components/fetch-data'
import HomePage from 'components/home-page'
import About from 'components/about'

import tableLightProduct from 'components/product/tablelight-product' 
import tableLightModifier from 'components/modifier/tablelight-modifier' 
import tableLightCategory from 'components/category/tablelight-category' 
import tableLightDiscount from 'components/discount/tablelight-discount' 

import posComponent from  'components/merchant/pos'
// import checkoutComponent from  'components/merchant/checkout'


export const routes = [
  { name: 'about', path: '/about', component: About, display: 'About Template', icon: 'info', hidden:true },
  { name: 'counter', path: '/counter', component: CounterExample, display: 'Counter', icon: 'graduation-cap', hidden:true },
  { name: 'fetch-data', path: '/fetch-data', component: FetchData, display: 'Data', icon: 'list', hidden:true },
  
  
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: 'home', hidden:false  },
  { name: 'product-list', path: '/products/list', component: tableLightProduct, display: 'Product', icon: 'boxes' , hidden:false},
  { name: 'modifier-list', path: '/modifier/list', component: tableLightModifier, display: 'Modifier', icon: 'database', hidden:false },
  { name: 'category-list', path: '/cateogry/list', component: tableLightCategory, display: 'Category', icon: 'list', hidden:false },
  { name: 'pos', path: '/merchant/pos', component: posComponent, display: 'POS', icon: 'balance-scale', hidden:false },
  // { name: 'checkout', path: '/merchant/checkout', component: checkoutComponent, display: 'CHECKOUT', icon: 'list', hidden:true },
  { name: 'discount', path: '/discount', component: tableLightDiscount, display: 'Discount', icon: 'database', hidden:false },
]
