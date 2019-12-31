using System.Threading.Tasks;
using TablePlugin.Core;
using TablePlugin.Data;

namespace TablePlugin
{

    //rename to this to QueryPaginator
    public class TablePluginQueryPaginator 
    {
        public async static Task<object> Build(string configName, IRequestTableParameter parameter)
        {
            var config = TablePluginOptions.QueryRepositoryInstance.GetByConfig(configName);
            config.ConnectionString = TablePluginOptions.SQLConnectionName;
            config.Provider = TablePluginOptions.DatabaseProvider;

            var paginator = new QueryPaginatorBasic(new BasicFilterByColumnStrategy());
            var result = await paginator.GetAsync(config, parameter);
            return result;
        }

        public async static Task<DataResponse<T>> Build<T>(string configName, IRequestTableParameter parameter)
        {
            var config = TablePluginOptions.QueryRepositoryInstance.GetByConfig(configName);
            config.ConnectionString = TablePluginOptions.SQLConnectionName;
            config.Provider = TablePluginOptions.DatabaseProvider;

            var paginator = new QueryPaginatorBasic(new BasicFilterByColumnStrategy());
            var result = await paginator.GetAsync<T>(config, parameter);
            return result;
        }
    }
}
