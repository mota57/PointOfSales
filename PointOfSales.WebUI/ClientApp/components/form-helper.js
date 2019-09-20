// register event bus
export const formHelper = {

  buildFormPost(form) {
    let formData = {};

    for (let key in form) {
      let value = form[key];
      if (value) {

        if (Array.isArray(value)) {
          for (let i in value) {
            if (!(formData[key])) {
              formData[key] = []
            } 
            formData[key].push(this.buildFormPost(value[i]));
          }
        } else if (typeof(value) == 'object') {
          formData[key] = this.buildFormPost(value)

        } else {
          formData[key] = value;
        }
      }
    }
    return formData;
  },
  buildForm(form) {
    let formData = new FormData();

    for (let key in form) {
      let value = form[key];
      if (value) {
        if (Array.isArray(value)) {
          for (let i in value) {
            formData.append(key + "[" + i + "]", JSON.stringify(this.buildForm(value[i])));
          }
        } else {
          formData.append(key, value);
        }
      }
    }
    return formData;
  },
  removeWhere(arrayList, callBack, total = 1) {
      let indexEl = arrayList.findIndex(callBack);
      arrayList.splice(indexEl, total);
  }

}; 
