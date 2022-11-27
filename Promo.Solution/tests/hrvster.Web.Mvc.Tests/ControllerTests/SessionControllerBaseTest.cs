using Firebase.Auth;
using hrvster.Web.Controllers;
using hrvster.Web.Mvc.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NToastNotify;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ControllerTests
{
    public abstract class SessionControllerBaseTest
    {
        public SessionController Controller { get; private set; }
        public Mock<ILoggerFactory> _loggerFactory { get; private set; }
        public Mock<IToastNotification> _notificationService { get; private set; }
        public Mock<IFirebaseAuthProvider> _firebaseAuthProvider { get; private set; }

        public SessionControllerBaseTest()
        {
            _loggerFactory = new Mock<ILoggerFactory>();
            _notificationService = new Mock<IToastNotification>();
            _firebaseAuthProvider = new Mock<IFirebaseAuthProvider>();

            Controller = new SessionController(_loggerFactory.Object, _notificationService.Object, _firebaseAuthProvider.Object);
            Controller.ControllerContext = new ControllerContext();
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            Controller.HttpContext.Session = new MockHttpSession();
        }
    }
}
