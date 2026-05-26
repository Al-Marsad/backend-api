using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestProject.Support;

internal static class ControllerTestHelper
{
    public static void SetUser(this ControllerBase controller, string? userId = "user-1", string? role = null, int? cityId = null)
    {
        var claims = new List<Claim>();

        if (userId is not null)
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));

        if (role is not null)
            claims.Add(new Claim(ClaimTypes.Role, role));

        if (cityId is not null)
            claims.Add(new Claim("CityId", cityId.Value.ToString()));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"))
            }
        };
    }

    public static void SetHttpContext(this ControllerBase controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    public static object? GetProperty(object value, string propertyName)
    {
        return value.GetType().GetProperty(propertyName)?.GetValue(value);
    }
}
