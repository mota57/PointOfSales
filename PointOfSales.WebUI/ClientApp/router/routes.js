import CounterExample from 'components/counter-example'
import FetchData from 'components/fetch-data'
import HomePage from 'components/home-page'
import About from 'components/about'
import productComponent from 'components/product-component' 
import modifierComponent from 'components/modifier-component' 
import categoryComponent from 'components/category-component' 


export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: 'home' },
  { name: 'about', path: '/about', component: About, display: 'About Template', icon: 'info' },
  { name: 'counter', path: '/counter', component: CounterExample, display: 'Counter', icon: 'graduation-cap' },
  { name: 'fetch-data', path: '/fetch-data', component: FetchData, display: 'Data', icon: 'list' },
  { name: 'product-list', path: '/products/list', component: productComponent, display: 'Product', icon: 'list' },
  { name: 'modifier-list', path: '/modifier/list', component: modifierComponent, display: 'Modifier', icon: 'list' },
  { name: 'category-list', path: '/cateogry/list', component: categoryComponent, display: 'Category', icon: 'list' }
]
