using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ApiKeyAttribute : Attribute, IAsyncActionFilter {
    private const string HeaderName = "X-API-KEY";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
        // 1. Check if the header exists
        if (!context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var extractedApiKey)) {
            context.Result = new ContentResult() { StatusCode = 401, Content = "API Key was not provided." };
            return;
        }

        // 2. Fetch the target key from your configuration
        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var targetKey = configuration["ApiKeySettings:SecretKey"];

        // 3. Simple character string validation
        if (string.IsNullOrEmpty(targetKey) || !targetKey.Equals(extractedApiKey)) {
            context.Result = new ContentResult() { StatusCode = 403, Content = "Unauthorized Client." };
            return;
        }

        await next();
    }
}
