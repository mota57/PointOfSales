


# data tables example
https://ej2.syncfusion.com/vue/demos/?_ga=2.74712341.662939979.1567944703-949682982.1567944703#/material/grid/grid-overview.html

https://madewithvuejs.com/blog/best-vue-js-datatables

https://www.ag-grid.com/?ref=madewithvuejs.com

# application example for generate forms
https://serenity.is



# calling a child from parent

https://jsfiddle.net/jamesbrndwgn/uat70czp/9/

https://vuejs.org/v2/guide/migration.html#dispatch-and-broadcast-replaced


# ignoring self referencing

https://stackoverflow.com/questions/7397207/json-net-error-self-referencing-loop-detected-for-type

# fix form-image to hold an object
https://simonkollross.de/posts/vuejs-using-v-model-with-objects-for-custom-components



# AUTOMAPPER
* [IGNORE](https://docs.automapper.org/en/stable/8.0-Upgrade-Guide.html?highlight=ignore#forsourcemember-ignore)
* [Map inline or custom converter autommaper for Iformfile and bytes[]] (https://stackoverflow.com/questions/25242783/map-from-httppostedfilebase-to-byte-with-automapper)


# examples of POS application


https://support.revelsystems.com/hc/en-us/articles/203585239-Introduction-to-Modifiers
https://squareup.com/dashboard/
https://ehoppersupport.zendesk.com/hc/en-us

## serial port
* (link1)[https://code.msdn.microsoft.com/SerialPort-brief-Example-ac0d5004]

## example of receipt 
* (link1)[https://codepen.io/Sambra22/pen/JNexJP]


## example of multipayment

* (link2)[https://www.google.com/search?q=split+payment+checkout&safe=active&rlz=1C1GGRV_enDO770DO770&source=lnms&tbm=isch&sa=X&ved=0ahUKEwif4bPb_K3lAhXCnOAKHZRgBTAQ_AUIEigB&biw=1536&bih=772#imgdii=ije9NSY0TSddEM:&imgrc=ije9NSY0TSddEM]


* (link3)[https://www.google.com/search?as_st=y&tbm=isch&hl=en&as_q=split+payment+checkout&as_epq=&as_oq=&as_eq=&cr=&as_sitesearch=&safe=active&tbs=ift:gif#imgdii=EuYPyA1eK5M64M:&imgrc=-NV4yFyW6kU3zM]


# MIGRATIONS links

* (link1)[https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/[]
* (link2)[https://medium.com/@yostane/entity-framework-core-and-sqlite-database-migration-using-vs2017-macos-28812c64e7ef]
* (link3)[https://stackoverflow.com/questions/37780136/asp-core-migrate-ef-core-sql-db-on-startup]
* (link4)[https://github.com/aspnet/Mvc/issues/7772]

# vue third party components
https://vue-multi-select.tuturu.io/#filters
https://vue-select.org/


# found good practice

Always initialize list in dto in order to avoid null parameterers on post object in a action controller

```.cs
    public class ProductFormDTO
    {
        public int Id { get; set; }
        public List<int> ModifierIds { get; set; } = new List<int>();
    }
```

# ISSUES 

* ISSUE BUG when WEB API route not match return 404 instead redirecting to home/index.cshtml
* ISSUE disable save btn on click for modal delete, modal create/edit
* ISSUE can not select more than one option that contains the same name at Product Form
* ISSUE FEATURE replace vue-select for a select checkbox functionality
* ISSUE CLOSED  fix issue with form data sending array && fix  send formData object through ajax request 
https://stackoverflow.com/questions/16104078/appending-array-to-formdata-and-send-via-ajax
https://stackoverflow.com/questions/42883550/ajax-with-formdata-dose-not-bind-childs-array-of-objects-in-asp-net-controller




* implement menu crud.
* implement identity 
* implement inventory


## Entities

supplier
customer
payments
order
oderdetail
product (isProductRent, isProductForRent)
product_rent (start_date,return_date,)
category
discount.
reports
