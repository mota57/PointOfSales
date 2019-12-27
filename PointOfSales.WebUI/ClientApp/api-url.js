

class BASE_URL {
  constructor(name) {
    this.name = name;
  }


  getURL(resource) {
    let result = `${window.APP_GLOBALS.URL}${this.name}${!resource ? '' : resource}`
    return result;
  }

  delete(id = '') {
    var result = this.getURL(`/${id}`)
    return result;
  }

  picklist(partUrl = '') {
    var result = this.getURL(`/GetPickList/${partUrl}`)
    return result;
  }

  list() {
    let result = this.getURL();
    return result;
  }

  getById(id) {
    let result = this.getURL(`/${id}`);
    return result;
  }

  upsert(id = '') {
    let result = this.getURL(`/${(id ? id : '')}`);
    return result;
  }

  get datatable() {
    let result = this.getURL(`/GetDataTable`)
    return result;
  }

  get tableMetadata() {
    let result = this.getURL(`/GetTableMetadata`)
    return result;
  }


}



class category extends BASE_URL {
  constructor() {
    super('/Category')
  }

}

class product extends BASE_URL {
  constructor() {
    super('/Product')
  }
}



class discount extends BASE_URL {
  constructor() {
    super('/Discount')
  }
}


class modifier extends BASE_URL {
  constructor() {
    super('/Modifier')
  }

  upsertProductModifier(productId) {
    let result = this.getURL(`UpsertProductModifiers/${productId}`);
    return result;
  }

}

const merchant = function(){
  let _merchantUrl = new BASE_URL('/Merchant');

  return  {
    pay: _merchantUrl.getURL('/pay'),
    ProductPosList: _merchantUrl.getURL(`/ProductPosList/`)
  }
}


export default {
  urls: {
    category: new category(),
    product: new product(),
    modifier: new modifier(),
    merchant : merchant(),
    discount: new discount()
   
  }
}
