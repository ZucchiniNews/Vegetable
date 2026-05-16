using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Zucchinimvc.Controllers.ApiInternal.Filters

{
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string HeaderName = "x-api-key";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var expctedApiKey = configuration["ZucchiniInternal:ApiKey"];
            if (!context.HttpContext.Request.Headers.TryGetValue(
                HeaderName, out var providedApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            if (providedApiKey != expctedApiKey)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();
        }
    }
}
