using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace routing_netcore
{
    public class OnlyAlbertsConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var name = values[routeKey]?.ToString();
            return (name != null && name.StartsWith("albert", StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
