using Moq;
using NToastNotify;
using System;

namespace TestUtilities.Common.Steps
{
    public static class ToastNotificationStep
    {
        public static TFeature ThenMockedToastNotificationShouldAssert<TFeature>(
            this IWebFeature<TFeature> feature,
            Mock<IToastNotification> mockedToastNotification,
            Action<Mock<IToastNotification>> assert) where TFeature : class
        {
            assert(mockedToastNotification);

            return feature as TFeature;
        }
    }
}
