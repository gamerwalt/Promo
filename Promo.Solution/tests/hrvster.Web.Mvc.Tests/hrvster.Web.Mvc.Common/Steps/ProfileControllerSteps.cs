using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Web.Mvc.Common.Steps
{
    public static class ProfileControllerSteps
    {
        public static Feature WhenILoadMyProfilePage(this Feature feature)
        {
            Task.Run(async () => feature.Response = await feature.Client.GetAsync($"/Profile")).Wait();

            return feature;
        }
    }
}
