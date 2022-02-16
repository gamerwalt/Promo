namespace Promo.TestUtilities.Database
{
    public interface IStorageTestFixtureBase<TFeature> where TFeature : class
    {
        public void DeleteTestTableData();
    }
}
