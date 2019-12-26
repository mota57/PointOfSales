```
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TablePlugin.Data;

namespace TablePlugin.Core
{
     public interface IQueryPaginator
     {
         QueryConfig QueryConfig {get; set;} 
         IRequestTableParameter Parameter {get; set;}
         Task<object> GetAsync();
         Task<DataResponse<TData>> GetAsync<TData>();
     }

     public IRequestTableParameterJTable : IRequestTableParameter
     {
         ///implement it differently.
     }

     public abstract class QueryPaginatorAbstract : IQueryPaginator
     {
         public QueryPaginatorAbstract(QueryConfig queryConfig, IRequestTableParameter parameter)
         {
             this.QueryConfig = queryConfig;
             this.Parameter = parameter;
            
         }
         public int PerPage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public QueryConfig QueryConfig { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public IRequestTableParameter Parameter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

         public async Task<object> GetAsync()
         {
             ProcessQuery();
             return await BuildResponseAsync<object>();
         }

         public  async Task<DataResponse<TData>> GetAsync<TData>()
         {
             ProcessQuery();
             return await BuildResponseAsync<TData>();
         }

         private void ProcessQuery()
         {
             Filter();
             OrderBy();
             Paginate();
         }

         protected abstract void Paginate();
         protected abstract void OrderBy();
         protected abstract void Filter();

        
         public abstract Task<DataResponse<TData>> BuildResponseAsync<TData>();

     }

     public class QueryPaginatorJTable : QueryPaginatorAbstract
     {
         public QueryPaginatorJTable(QueryConfig queryConfig, IRequestTableParameter parameter) : base(queryConfig, parameter)
         {
         }

         public override Task<DataResponse<TData>> BuildResponseAsync<TData>()
         {
             throw new NotImplementedException();
         }

         protected override void Filter()
         {
             throw new NotImplementedException();
         }

         protected override void OrderBy()
         {
             throw new NotImplementedException();
         }

         protected override void Paginate()
         {
             throw new NotImplementedException();
         }
     }
}
```