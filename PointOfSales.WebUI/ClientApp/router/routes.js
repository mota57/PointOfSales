import CounterExample from 'components/counter-example'
import FetchData from 'components/fetch-data'
import HomePage from 'components/home-page'
import About from 'components/about'
import FormProduct from 'components/form-product'
import productComponent from 'components/product-component' 


export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: 'home' },
  { name: 'about', path: '/about', component: About, display: 'About Template', icon: 'info' },
  { name: 'counter', path: '/counter', component: CounterExample, display: 'Counter', icon: 'graduation-cap' },
  { name: 'fetch-data', path: '/fetch-data', component: FetchData, display: 'Data', icon: 'list' },
  { name: 'product-list', path: '/products/list', component: productComponent, display: 'Product', icon: 'list' }
]
