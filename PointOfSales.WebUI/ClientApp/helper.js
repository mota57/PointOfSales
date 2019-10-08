// register event bus
export const Helper = {

  createFormData(form) {
    let formData = new FormData();
    for (let key in form) {
      let value = form[key];
      if (value) {
        if (Array.isArray(value)) {
          for (let i in value) {
            if (value[i] instanceof Object) {
              Object.keys(value[i]).forEach((prop) => {
                formData.append(`${key}[${i}].${prop}`, value[i][prop]);
              });
            } else {
              formData.append(`${key}[${i}]`, value[i]);
            }
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
