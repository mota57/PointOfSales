namespace TablePlugin.Core
{
    public abstract  class DataResponseAbstract<T>
    {
        public DataResponse<T> DataResponse { get; set; }
        public DataResponseAbstract(DataResponse<T> dataResponse)
        {
            DataResponse = dataResponse;
        }

    }
   
  
}
