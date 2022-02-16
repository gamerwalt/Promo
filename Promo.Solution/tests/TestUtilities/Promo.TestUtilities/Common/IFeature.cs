using Promo.TestUtilities.Logging;

namespace Promo.TestUtilities.Common
{
    public interface IFeature<TFeature> where TFeature : class
    {
        public TestLogger Logger { get; }
    }
}
