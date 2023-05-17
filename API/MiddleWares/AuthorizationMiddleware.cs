using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.MiddleWares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            PathString path = context.Request.Path;
            string? requiredRole = null;
            if (path == "/api/Login")
            {
                await _next(context);
                return;
            }
            else if (path.StartsWithSegments("/api/Bank"))
            {
                requiredRole = "ReserveBankManager";
                await AuthorizeUser(context, requiredRole);
            }
            else if (path.StartsWithSegments("/api/Branch"))
            {
                requiredRole = "HeadManager";
                await AuthorizeUser(context, requiredRole);
            }
            else if (path.StartsWithSegments("/api/Currency"))
            {
                var pathString = path.ToString();
                if (pathString.Contains("/bankId/"))
                {
                    requiredRole = "Manager,Staff,Customer";
                }
                else if (pathString.Contains("/currencyCode/") && context.Request.Method == "GET")
                {
                    requiredRole = "Manager,Staff,Customer";
                }
                else
                {
                    requiredRole = "HeadManager";
                }
                await AuthorizeUser(context, requiredRole);
            }
            else if (path.StartsWithSegments("/api/Transaction"))
            {
                var pathString = path.ToString();
                if (pathString.EndsWith("/revert"))
                {
                    requiredRole = "Manager,Staff";
                }
                else if (pathString.Contains("/accountId"))
                {
                    requiredRole = "Manager,Staff,Customer";
                }
                else if (pathString.Contains("/transactionId/"))
                {
                    requiredRole = "Manager,Staff,Customer";
                }
                else
                {
                    requiredRole = "Customer";
                }
                await AuthorizeUser(context, requiredRole);
            }
            else if (path.StartsWithSegments("/api/TransactionCharge"))
            {
                var pathString = path.ToString();
                if (pathString.Contains("/branchId/") && context.Request.Method == "GET")
                {
                    requiredRole = "Manager,Staff,Customer";
                }
                else
                {
                    requiredRole = "Manager";
                }
                await AuthorizeUser(context, requiredRole);
            }
            else if (path.StartsWithSegments("/api/ReserveBankManager"))
            {
                requiredRole = "ReserveBankManager";
                await AuthorizeUser(context, requiredRole);
            }
            else if (path.StartsWithSegments("/api/HeadManager"))
            {
                var pathString = path.ToString();
                if (pathString.Contains("/bankId/"))
                {
                    requiredRole = "HeadManager,ReserveBankManager";
                }
                else if (pathString.Contains("/accountId/") && context.Request.Method == "GET")
                {
                    requiredRole = "HeadManager,ReserveBankManager";
                }
                else if (pathString.Contains("/name/"))
                {
                    requiredRole = "HeadManager,ReserveBankManager";
                }
                else
                {
                    requiredRole = "ReserveBankManager";
                }
                await AuthorizeUser(context, requiredRole);
            }
            else if (path.StartsWithSegments("/api/Manager"))
            {
                var pathString = path.ToString();
                if (pathString.Contains("/branchId/"))
                {
                    requiredRole = "Manager,HeadManager";
                }
                else if (pathString.Contains("/accountId/") && context.Request.Method == "GET")
                {
                    requiredRole = "Manager,HeadManager";
                }
                else if (pathString.Contains("/name/"))
                {
                    requiredRole = "Manager,HeadManager";
                }
                else
                {
                    requiredRole = "HeadManager";
                }
                await AuthorizeUser(context, requiredRole);
            }
            else if (path.StartsWithSegments("/api/Staff"))
            {
                var pathString = path.ToString();
                if (pathString.Contains("/branchId/"))
                {
                    requiredRole = "Staff,Manager";
                }
                else if (pathString.Contains("/accountId/") && context.Request.Method == "GET")
                {
                    requiredRole = "Staff,Manager";
                }
                else if (pathString.Contains("/name/"))
                {
                    requiredRole = "Staff,Manager";
                }
                else
                {
                    requiredRole = "Manager";
                }

                await AuthorizeUser(context, requiredRole);
            }
            else if (path.StartsWithSegments("/api/Customer"))
            {
                var pathString = path.ToString();
                if (pathString.Contains("/branchId/"))
                {
                    requiredRole = "Staff,Manager,Customer";
                }
                else if (pathString.Contains("/accountId/") && context.Request.Method == "GET")
                {
                    requiredRole = "Staff,Manager,Customer";
                }
                else if (pathString.Contains("/accountId/") && context.Request.Method == "DELETE")
                {
                    requiredRole = "Staff,Manager";
                }
                else if (pathString.EndsWith("/Customer") && context.Request.Method == "POST")
                {
                    requiredRole = "Staff,Manager";
                }
                else if (pathString.EndsWith("/Customer") && context.Request.Method == "PUT")
                {
                    requiredRole = "Staff,Manager";
                }
                else if (pathString.Contains("/name/"))
                {
                    requiredRole = "Staff,Manager,Customer";
                }
                else
                {
                    requiredRole = "Customer";
                }
                await AuthorizeUser(context, requiredRole);
            }
        }
        private async Task AuthorizeUser(HttpContext context, string requiredRole)
        {
            string[] roles = requiredRole.Split(",");
            StringValues authorizationHeader = context.Request.Headers["Authorization"];
            string? token = authorizationHeader == StringValues.Empty ? null : authorizationHeader.FirstOrDefault()?.Split(' ').Last();

            if (token is not null)
            {
                JwtSecurityTokenHandler tokenHandler = new();
                IEnumerable<Claim> claims = tokenHandler.ReadJwtToken(token).Claims;
                string roleValue = claims.FirstOrDefault(c => c.Type == "Role")?.Value!;
                int indexValue = Array.FindIndex(roles, x => x == roleValue);
                if (roleValue is not null && !indexValue.Equals(-1) && roleValue.Contains(roles[indexValue]))
                {
                    await _next(context);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Access to this resource is forbidden. Please ensure that you have the necessary" +
                        " permissions to access this resource.");
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Authorization Token not given in the headers");
            }
        }
    }
}
