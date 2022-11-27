using hrvster.Infrastructure.Accounts;
using hrvster.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System;

namespace TestUtilities.Mocks.Repositories
{
    public static class MockProfileRepositoryExtensions
    {
        public static IServiceCollection MockProfileRepository(
            this IServiceCollection services,
            Action<Mock<IProfilesRepository>> mockSetup = null)
        {
            services.RemoveAll<IProfilesRepository>();

            var mock = new Mock<IProfilesRepository>();
            mockSetup?.Invoke(mock);

            services.AddSingleton(mock);
            services.AddSingleton(mock.Object);

            return services;
        }

        public static Mock<IProfilesRepository> GetRecordReturnsException(
            this Mock<IProfilesRepository> mock,
            Guid userUid,
            ProfileType profileType)
        {
            mock.Setup(r => r.GetProfileAsync(userUid, profileType)).Throws<ArgumentException>();

            return mock;
        }
    }
}
