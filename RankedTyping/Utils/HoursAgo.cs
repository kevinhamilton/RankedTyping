using System;
namespace RankedTyping.Utils
{
    public class HoursAgo
    {
        public static string AsString(DateTime dt)
        {
            var tspan = DateTime.Now.Subtract(dt);
            return PeriodOfTimeOutput(tspan);
        }
        
        private static string PeriodOfTimeOutput(TimeSpan tspan, int level = 0)
        {
            string how_long_ago = "ago";
            if (level >= 2) return how_long_ago;
            if (tspan.Days > 1)
                how_long_ago = string.Format("{0} days ago", tspan.Days);
            else if (tspan.Days == 1)
                how_long_ago = "1 day ago";
            else if (tspan.Hours >= 1)
                how_long_ago = string.Format("{0} {1} ago", tspan.Hours, (tspan.Hours > 1) ? "hours" : "hour");
            else if (tspan.Minutes >= 1)
                how_long_ago = string.Format("{0} {1} ago", tspan.Minutes, (tspan.Minutes > 1) ? "minutes" : "minute");
            else if (tspan.Seconds >= 1)
                how_long_ago = string.Format("{0} {1} ago", tspan.Seconds, (tspan.Seconds > 1) ? "seconds" : "second");        
            return how_long_ago;
        }
    }
}