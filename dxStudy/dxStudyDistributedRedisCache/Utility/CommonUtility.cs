namespace dxStudyDistributedRedisCache.Utility;

public class CommonUtility
{
    public static TimeSpan CalculateTimeSpan(double timeSpan, TimeSpanType spanType)
    {
        switch (spanType)
        {
            case TimeSpanType.Millisecond:
                return TimeSpan.FromMilliseconds(timeSpan);
            case TimeSpanType.Second:
                return TimeSpan.FromSeconds(timeSpan);
            case TimeSpanType.Minute:
                return TimeSpan.FromMinutes(timeSpan);
            case TimeSpanType.Hour:
                return TimeSpan.FromHours(timeSpan);
        }

        return TimeSpan.FromSeconds(timeSpan);
    }

    public enum TimeSpanType
    {
        Millisecond,
        Second,
        Minute,
        Hour
    }

}