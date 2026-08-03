namespace AlHudhud.Services.TimezoneService;

public interface ITimezoneService
{
    double GetTimezoneOffsetHours();
    DateTime ConvertToLocalTime(DateTime utcDateTime);
}
