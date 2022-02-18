using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace DockerizeTaskManagementApi.AppCode.Extensions
{
    static public partial class Extension
    {
        static public string GetHeaderValue(this HttpContext ctx, string key, IDictionary<string, object> items = null)
        {
            if (ctx.Request.Headers.TryGetValue(key, out StringValues incomingValue))
            {
                string value = incomingValue.FirstOrDefault();
                items?.Add(key, value);
                return value;
            }

            return null;
        }

        static public string GetHeaderValue(this IActionContextAccessor ctx, string key, IDictionary<string, object> items = null)
        {
            return GetHeaderValue(ctx.ActionContext.HttpContext, key, items);
        }

        static public string GetHeaderValue(this HttpRequest request, string key, IDictionary<string, object> items = null)
        {
            return GetHeaderValue(request.HttpContext, key, items);
        }
    }
}
