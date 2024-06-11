using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocalDateTimeManager : ManagerBase
{
    public enum ETIME_TYPE
    {
        Local,
        UTC,
    }

    public ETIME_TYPE eTimeType = ETIME_TYPE.UTC;

    private DateTime checkTimer;

    public override async UniTask ManagerInitProcessing()
    {
        await InitManager();

        await base.ManagerInitProcessing();
    }

    public override async UniTask InitManager()
    {
        await base.InitManager();
    }

    public override void AfterInitProcessing()
    {
        //checkTimer = Core.DM.GetLastEnergyCheckTime();
    }

    public DateTime GetDateTime()
    {
        switch (eTimeType)
        {
            case ETIME_TYPE.Local:
                return DateTime.Now;
            case ETIME_TYPE.UTC:
                return DateTime.UtcNow;
        }

        return DateTime.UtcNow;
    }

    public bool IsValidDateTime(DateTime dateTime)
    {
        switch (eTimeType)
        {
            case ETIME_TYPE.Local:
                if (DateTime.Compare(dateTime, DateTime.Now) < 0)
                    return true;
                else
                    return false;
            case ETIME_TYPE.UTC:
                if (DateTime.Compare(dateTime, DateTime.UtcNow) < 0)
                    return true;
                else
                    return false;
        }

        return true;
    }

    public bool IsNextDay(DateTime dateTime)
    {
        switch (eTimeType)
        {
            case ETIME_TYPE.Local:
                {
                    int value = DateTime.Now.DayOfYear - dateTime.DayOfYear;
                    return value == 1 ? true : false;
                }
            case ETIME_TYPE.UTC:
                {
                    int value = DateTime.UtcNow.DayOfYear - dateTime.DayOfYear;
                    return value == 1 ? true : false;
                }
        }

        return true;
    }

    public bool IsOClock()
    {
        switch (eTimeType)
        {
            case ETIME_TYPE.Local:
                {
                    return DateTime.Now.Minute == 0 ? true : false;
                }
            case ETIME_TYPE.UTC:
                {
                    return DateTime.UtcNow.Minute == 0 ? true : false;
                }
        }

        return true;
    }

    public bool IsAfterDay(DateTime dateTime)
    {
        switch (eTimeType)
        {
            case ETIME_TYPE.Local:
                {
                    int value = DateTime.Now.DayOfYear - dateTime.DayOfYear;
                    return value >= 1 ? true : false;
                }
            case ETIME_TYPE.UTC:
                {
                    int value = DateTime.UtcNow.DayOfYear - dateTime.DayOfYear;
                    return value >= 1 ? true : false;
                }
        }

        return true;
    }

    public bool IsNewWeek(DateTime dateTime)
    {
        DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(dateTime);

        if (DayOfWeek.Monday >= day)
        {
            return true;
        }

        return false;
    }

    public TimeSpan GetElapsedTime(DateTime dateTime)
    {
        switch(eTimeType)
        {
            case ETIME_TYPE.Local:
                {
                    TimeSpan value = DateTime.Now.Subtract(dateTime);
                    return value;
                }
            case ETIME_TYPE.UTC:
                {
                    TimeSpan value = DateTime.UtcNow.Subtract(dateTime);
                    return value;
                }
        }
        return default;
    }
    public double GetElapsedSeconds(DateTime dateTime)
    {
        switch(eTimeType)
        {
            case ETIME_TYPE.Local:
                {
                    TimeSpan value = DateTime.Now.Subtract(dateTime);
                    return value.TotalSeconds;
                }
            case ETIME_TYPE.UTC:
                {
                    TimeSpan value = DateTime.UtcNow.Subtract(dateTime);
                    return value.TotalSeconds;
                }
        }
        return 0;
    }

    public double GetElapsedMinutes(DateTime dateTime)
    {
        switch(eTimeType)
        {
            case ETIME_TYPE.Local:
                {
                    TimeSpan value = DateTime.Now.Subtract(dateTime);
                    return value.TotalMinutes;
                }
            case ETIME_TYPE.UTC:
                {
                    TimeSpan value = DateTime.UtcNow.Subtract(dateTime);
                    return value.TotalMinutes;
                }
        }
        return 0;
    }

    public double GetElapsedHours(DateTime dateTime)
    {
        switch (eTimeType)
        {
            case ETIME_TYPE.Local:
                {
                    TimeSpan value = DateTime.Now.Subtract(dateTime);
                    return value.TotalHours;
                }
            case ETIME_TYPE.UTC:
                {
                    TimeSpan value = DateTime.UtcNow.Subtract(dateTime);
                    return value.TotalHours;
                }
        }
        return 0;
    }

    public double GetElapsedDays(DateTime dateTime)
    {
        switch (eTimeType)
        {
            case ETIME_TYPE.Local:
                {
                    TimeSpan value = DateTime.Now.Subtract(dateTime);
                    return value.TotalDays;
                }
            case ETIME_TYPE.UTC:
                {
                    TimeSpan value = DateTime.UtcNow.Subtract(dateTime);
                    return value.TotalDays;
                }
        }
        return 0;
    }

    public TimeSpan GetCheckTimer()
    {
        switch (eTimeType)
        {
            case ETIME_TYPE.Local:
                {
                    TimeSpan value = DateTime.Now.Subtract(checkTimer);
                    return value;
                }
            case ETIME_TYPE.UTC:
                {
                    TimeSpan value = DateTime.UtcNow.Subtract(checkTimer);
                    return value;
                }
        }
        return TimeSpan.Zero;
    }

    public bool IsOverCheckTime(int interval)
    {
        switch (eTimeType)
        {
            case ETIME_TYPE.Local:
                {
                    TimeSpan value = DateTime.Now.Subtract(checkTimer);
                    return value.TotalMinutes > interval ? true : false;
                }
            case ETIME_TYPE.UTC:
                {
                    TimeSpan value = DateTime.Now.Subtract(checkTimer);
                    return value.TotalMinutes > interval ? true : false;
                }
        }
        return true;
    }

    public TimeSpan LeftTimeForDay()
    {
        switch (eTimeType)
        {
            case ETIME_TYPE.Local:
                {
                    return TimeSpan.FromHours(24) - GetDateTime().TimeOfDay;
                }
            case ETIME_TYPE.UTC:
                {
                    return TimeSpan.FromHours(24) - GetDateTime().TimeOfDay;
                }
        }
        return TimeSpan.Zero;
    }

    public TimeSpan LeftTimeForWeek()
    {
        switch (eTimeType)
        {
            case ETIME_TYPE.Local:
                {
                    int daysToAdd = ((int)DayOfWeek.Monday - (int)GetDateTime().DayOfWeek + 7) % 7;
                    if (daysToAdd == 0)
                        daysToAdd = 7;
                    DateTime target = GetDateTime().AddDays(daysToAdd);
                    target = target.AddHours(-GetDateTime().Hour);
                    target = target.AddMinutes(-GetDateTime().Minute);
                    target = target.AddSeconds(-GetDateTime().Second);
                    return target.Subtract(GetDateTime());
                }
            case ETIME_TYPE.UTC:
                {
                    int daysToAdd = ((int)DayOfWeek.Monday - (int)GetDateTime().DayOfWeek + 7) % 7;
                    if (daysToAdd == 0)
                        daysToAdd = 7;
                    DateTime target = GetDateTime().AddDays(daysToAdd);
                    target = target.AddHours(-GetDateTime().Hour);
                    target = target.AddMinutes(-GetDateTime().Minute);
                    target = target.AddSeconds(-GetDateTime().Second);
                    return target.Subtract(GetDateTime());
                }
        }
        return TimeSpan.Zero;
    }

    public TimeSpan LeftTimeForDays(DateTime begin, int addDays)
    {
        switch (eTimeType)
        {
            case ETIME_TYPE.Local:
                {
                    DateTime target = begin.AddDays(addDays);
                    return target.Subtract(GetDateTime());
                }
            case ETIME_TYPE.UTC:
                {
                    DateTime target = begin.AddDays(addDays);
                    return target.Subtract(GetDateTime());
                }
        }
        return TimeSpan.Zero;
    }
}