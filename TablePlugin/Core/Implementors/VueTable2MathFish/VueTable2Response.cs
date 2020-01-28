namespace TablePlugin.Core
{
    public class VueTable2Response<TResult> : DataResponseAbstract<TResult>
    {
        public VueTable2Response(DataResponse<TResult> dataResponse) : base(dataResponse)
        {
        }
    }
}