using hrvster.Web.Mvc.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ControllerTests
{
    [Collection("Sequential")]
    public class DashboardControllerTests : IClassFixture<Feature>
    {
        private readonly Feature _feature;

        public DashboardControllerTests(Feature feature)
        {
            _feature = feature;
        }

        
    }
}
