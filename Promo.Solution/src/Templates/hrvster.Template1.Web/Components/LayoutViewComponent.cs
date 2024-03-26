using hrvster.Core.Authentication;
using hrvster.Templates.Services;
using Microsoft.AspNetCore.Mvc;

namespace hrvster.Template1.Web.Components
{
	[ViewComponent(Name = "LayoutView")]
	public class LayoutViewComponent : ViewComponent
	{
		private readonly ILogger<LayoutViewComponent> _logger;
		private readonly ILayoutService _layoutService;
		private readonly ISessionManager _sessionManager;

		public LayoutViewComponent(
			ILoggerFactory loggerFactory,
			ILayoutService layoutService, 
			ISessionManager sessionManager)
		{
			_logger = loggerFactory.CreateLogger<LayoutViewComponent>();
			_layoutService = layoutService;
			_sessionManager = sessionManager;
		}

		public async Task<IViewComponentResult> InvokeAsync(string view = "Body")
		{
			var accountUidString = _sessionManager.GetValue(SessionConstants.AccountUid);
			if (string.IsNullOrWhiteSpace(accountUidString))
			{
				_logger.LogWarning(view + " view component: Unable to get account uid from session");

				throw new Exception("Unable to get account uid from session");
			}

			var accountUid = Guid.Parse(accountUidString);

			var layoutModel = await _layoutService.GetLayoutPropertyAsync(accountUid, default);

			switch (view)
			{
				case "Header":
					return View("HeaderView", layoutModel);
				case "Footer":
					return View("FooterView", layoutModel);
				default:
					return View("BodyView", layoutModel);
			}
		}
	}
}
