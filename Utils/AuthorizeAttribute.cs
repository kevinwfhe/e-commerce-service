namespace csi5112group1project_service.Utils;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using csi5112group1project_service.Models;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
  public void OnAuthorization(AuthorizationFilterContext context)
  {
    var user = (User)context.HttpContext.Items["User"];
    if (user == null)
    {
      // not logged in
      context.Result = new UnauthorizedResult();
    }
  }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AdminAuthorizeAttribute : Attribute, IAuthorizationFilter
{
  public void OnAuthorization(AuthorizationFilterContext context)
  {
    var user = (User)context.HttpContext.Items["User"];
    if (user == null || user.role != "admin")
    {
      // not logged in as admin
      context.Result = new UnauthorizedResult();
    }
  }
}
