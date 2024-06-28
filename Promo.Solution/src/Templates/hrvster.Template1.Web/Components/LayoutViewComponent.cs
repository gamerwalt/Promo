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

		private const string HrvsterAccountUid = "eae9d83d-a73e-4e28-9ee4-631e66ec4b9f";

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
				//Set the account uid to hrvster's own
				accountUidString = HrvsterAccountUid;
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
