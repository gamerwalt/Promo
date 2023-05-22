using hrvster.Templates.Services;
using Microsoft.AspNetCore.Mvc;

namespace hrvster.Template1.Web.Components
{
	public class LayoutViewComponent : ViewComponent
	{
		private readonly ILayoutService _layoutService;

		public LayoutViewComponent(ILayoutService layoutService)
		{
			_layoutService = layoutService;
		}

		public async Task<IViewComponentResult> InvokeAsync(string view = "Body")
		{
			var layoutModel = await _layoutService.GetLayoutPropertyAsync(default);

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
