using FluentAssertions;
using hrvster.Models.Extensions;
using System;
using Xunit;

namespace hrvster.Models.Tests
{
    public class UriExtensionTests
    {
        [Fact]
        public void Should_Return_A_Dashboard_Link()
        {
            var uri = new Uri("https://someuri/");
            var result = uri.GenerateUrl(UrlTypes.DashboardLink);

            result.ToString().Should().Be("https://someuri/Dashboard");
        }
    }
}