using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PointOfSales.Core.Entities;
using TablePlugin.Core;
using TablePlugin.Data;

namespace PointOfSales.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Product2Controller : ControllerBase
    {

        private QueryConfig productConfig = new QueryConfig("v_product_controller",
        new QueryField(name: "Id"),
         new QueryField(name: "Name"),
         new QueryField(name: "Price"),
         new QueryField(name: "ProductCode"),
         new QueryField(name: "CategoryName"),
         new QueryField(name: "CreateDate", type:typeof(DateTime), filter:false)
        );

        /// <summary>
        /// This function handle the data for the datatable.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("GetDatatable")]
        public async Task<object> GetDataTable([FromQuery] VueTable2ParameterMathFish parameters)
        {
            productConfig.ConnectionString = GlobalVariables.Connection;
            productConfig.Provider = TablePlugin.Core.DatabaseProvider.SQLite;

            IRequestParameter requestTableParameter = new VueTable2RequestParameterAdapter(parameters);
            var reader = new QueryPaginatorBasic(new VueFilterByColumnStrategy());
            var result = await reader.GetAsync(productConfig, requestTableParameter);
            return result;
        }

        /// <summary>
        /// This function return the vueTableConfig to create the table columns
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTableMetadata")]
        public object GetTableMetadata()
        {
            return new QueryConfigDTO(productConfig);
        }

    }
}
