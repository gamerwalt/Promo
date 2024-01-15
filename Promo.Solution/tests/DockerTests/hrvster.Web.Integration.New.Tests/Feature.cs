using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Web.TestUtilities
{
    public class Feature : StorageFeatureBase<Feature>, IApiFeature<Feature>
    {
        public HttpResponseMessage Response { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public HttpClient Client { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
