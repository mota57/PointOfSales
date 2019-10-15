

class BASE_URL {
  constructor(name) {
    this.name = name;
  }


  getURL(resource) {
    let result = `${window.APP_GLOBALS.URL}${this.name}${resource}`
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

class categories extends BASE_URL {
  constructor() {
    super('/Category')
  }

}

class products extends BASE_URL {
  constructor() {
    super('/Product')
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


export default {
  urls: {
    categories: new categories(),
    products: new products(),
    modifier: new modifier()
  }
}
