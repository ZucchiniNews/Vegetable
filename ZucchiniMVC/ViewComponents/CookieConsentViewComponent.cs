using Microsoft.AspNetCore.Mvc;

namespace Zucchinimvc.ViewComponents
{
    public class CookieConsentViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var conseted = Request.Cookies.ContainsKey("CookieConsent");
            if (conseted) {
                return Content(string.Empty);
            }
            return View("Default", new object());
        }
    }
}
