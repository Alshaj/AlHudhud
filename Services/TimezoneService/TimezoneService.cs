using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace AlHudhud.Services.TimezoneService;

public class TimezoneService : ITimezoneService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const double DefaultYemenOffsetHours = 3.0; // Default: GMT+3 (Yemen / Arab Standard Time)

    public TimezoneService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public double GetTimezoneOffsetHours()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null && httpContext.Request.Headers.TryGetValue("X-Timezone-Offset", out var headerValues))
        {
            var headerValue = headerValues.ToString();
            if (!string.IsNullOrWhiteSpace(headerValue) &&
                double.TryParse(headerValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedOffset))
            {
                return parsedOffset;
            }
        }

        return DefaultYemenOffsetHours;
    }

    public DateTime ConvertToLocalTime(DateTime utcDateTime)
    {
        var offsetHours = GetTimezoneOffsetHours();
        var utc = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        return utc.AddHours(offsetHours);
    }
}
