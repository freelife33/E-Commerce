using ECommerce.Business.Services;
using System.Net;

namespace ECommerce.Web.Middleware
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;

        public MaintenanceMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, ISystemSettingsService settingsService)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant() ?? "/";

            // 1) Sert bypass: bu yollar her koşulda geçsin (döngüyü keser)
            if (path.StartsWith("/home/maintenance") ||
                path.StartsWith("/account/login") ||
                path.StartsWith("/health") ||
                path.Contains("/css/") || path.Contains("/js/") ||
                path.Contains("/images/") || path.Contains("/lib/") ||
                PathHasExtension(path))
            {
                await _next(context);
                return;
            }

            var settings = await settingsService.GetAsync();

            // 2) İzinli path'ler: DB boş/null ise varsayılanları kullan
            var allowedPaths = (settings.MaintenanceAllowedPaths ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(p => p.ToLowerInvariant())
                .ToList();

            if (allowedPaths.Count == 0)
            {
                allowedPaths = new List<string> { "/account/login", "/home/maintenance", "/health" };
            }

            var pathAllowed = allowedPaths.Any(p => path.StartsWith(p));

            // İzinli roller
            var allowedRoles = (settings.MaintenanceAllowedRoles ?? "Admin")
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var userAllowed = allowedRoles.Any(r => context.User.IsInRole(r));

            if (settings.MaintenanceEnabled && !pathAllowed && !userAllowed)
            {
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                if (settings.MaintenancePlannedEnd.HasValue)
                {
                    var seconds = Math.Max(60, (int)(settings.MaintenancePlannedEnd.Value - DateTime.UtcNow).TotalSeconds);
                    context.Response.Headers["Retry-After"] = seconds.ToString();
                }

                // İstersen redirect yerine rewrite da yapabilirsin:
                // context.Request.Path = "/home/maintenance";
                // await _next(context);
                // return;

                context.Response.Redirect("/Home/Maintenance");
                return;
            }

            await _next(context);
        }
        private static bool PathHasExtension(string path)
        {
            try { return !string.IsNullOrEmpty(Path.GetExtension(path)); }
            catch { return false; }
        }
    }
}
