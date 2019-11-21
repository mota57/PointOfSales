import CounterExample from 'components/counter-example'
import FetchData from 'components/fetch-data'
import HomePage from 'components/home-page'
import About from 'components/about'
import tableLightProduct from 'components/product/tablelight-product' 
import tableLightModifier from 'components/modifier/tablelight-modifier' 
import tableLightCategory from 'components/category/tablelight-category' 

import posComponent from  'components/merchant/pos'
import checkoutComponent from  'components/merchant/checkout'
import itemDateComponent from 'components/merchant/item-date'

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: 'home',  },
  { name: 'about', path: '/about', component: About, display: 'About Template', icon: 'info', hidden:true },
  { name: 'counter', path: '/counter', component: CounterExample, display: 'Counter', icon: 'graduation-cap', hidden:true },
  { name: 'fetch-data', path: '/fetch-data', component: FetchData, display: 'Data', icon: 'list', hidden:false },
  { name: 'product-list', path: '/products/list', component: tableLightProduct, display: 'Product', icon: 'list' },
  { name: 'modifier-list', path: '/modifier/list', component: tableLightModifier, display: 'Modifier', icon: 'list' },
  { name: 'category-list', path: '/cateogry/list', component: tableLightCategory, display: 'Category', icon: 'list' },
  { name: 'pos', path: '/merchant/pos', component: posComponent, display: 'POS', icon: 'list' },
  { name: 'checkout', path: '/merchant/checkout', component: checkoutComponent, display: 'CHECKOUT', icon: 'list', hidden:true },
  { name: 'item-date', path: '/merchant/product-set-dates', component: itemDateComponent, display: 'Configure product dates', icon: 'list', hidden:false },
]
