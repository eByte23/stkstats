using System;
namespace STKBC.Stats.Services;

public interface IClock
{
    DateTime GetUtcNow();

    DateTime GetLocalNow();
    DateTime GetLocalNow(string timeZone);
}

public class Clock: IClock
{
    public DateTime GetUtcNow() => DateTimeOffset.UtcNow.DateTime;

    public DateTime GetLocalNow() => DateTimeOffset.Now.ToLocalTime().DateTime;
    public DateTime GetLocalNow(string timeZone) => DateTimeOffset.Now.ToLocalTime().DateTime;
}