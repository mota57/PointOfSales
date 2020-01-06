using System.Threading.Tasks;

namespace TablePlugin.Core
{
    
    /// <summary>
    ///  Responsable of build queries from
    /// </summary>
    public class QueryPaginatorDbService
    {
        public async static Task<object> Build(string configName, IRequestParameter parameter)
        {
            var config = TablePluginOptions.QueryRepositoryInstance.GetByConfig(configName);
            config.ConnectionString = TablePluginOptions.SQLConnectionName;
            config.Provider = TablePluginOptions.DatabaseProvider;

            var paginator = new QueryPaginatorBasic();
            var result = await paginator.GetAsync(config, parameter);
            return result;
        }

        public async static Task<DataResponse<T>> Build<T>(string configName, IRequestParameter parameter)
        {
            var config = TablePluginOptions.QueryRepositoryInstance.GetByConfig(configName);
            config.ConnectionString = TablePluginOptions.SQLConnectionName;
            config.Provider = TablePluginOptions.DatabaseProvider;

            var paginator = new QueryPaginatorBasic();
            var result = await paginator.GetAsync<T>(config, parameter);
            return result;
        }
    }

}
