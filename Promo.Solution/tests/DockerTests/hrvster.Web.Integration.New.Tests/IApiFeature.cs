namespace hrvster.Web.TestUtilities
{
    public interface IApiFeature<TFeature> where TFeature : class
    {
        public HttpResponseMessage Response { get; set; }
        public HttpClient Client { get; set; }
        void DeleteAllTableData();
    }
}
