using ECommerce.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;

namespace ECommerce.API.Attributes
{
    public class RedisCacheAttribute: ActionFilterAttribute
    {
        private readonly int durationInSec;

        public RedisCacheAttribute(int durationInSec)
        {
            this.durationInSec = durationInSec;
        }
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            //Get Cache Service From DI Contanier
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheServices>();

            //Check If Cached Data Exsist
            var cacheKey = CreateCacheKey(context.HttpContext.Request);
            var cached = await cacheService.GetAsync(cacheKey);

            //If Exsist ,Return Cached Data ,and Skip Excution of EndPoint
            if (!string.IsNullOrEmpty(cached))
            {
                context.Result = new ContentResult
                {
                    Content = cached,
                    StatusCode = StatusCodes.Status200OK,
                    ContentType = "application/json"
                };
                return;
            }
            //If Not Exsist ,Excute EndPoint ,and Store Result in Cache if 200 Ok()
            var Executed = await next.Invoke();
            if (Executed.Result is OkObjectResult { Value: not null } ok)
                await cacheService.SetAsync(cacheKey, ok.Value, TimeSpan.FromSeconds(durationInSec));
            return;
        }
        private static string CreateCacheKey(HttpRequest request)
        {
            var key = new StringBuilder();
            key.Append(request.Path).Append("?");

            foreach (var (k, v) in request.Query.OrderBy(q => q.Key))
            {
                key.Append(k).Append("=").Append(v).Append("&");
            }
            return key.ToString();
        }
    }
}
