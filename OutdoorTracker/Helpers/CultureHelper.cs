using System.Globalization;

using Windows.Globalization.DateTimeFormatting;

namespace OutdoorTracker.Helpers
{
    public static class CultureHelper
    {
        public static CultureInfo CurrentCulture { get; private set; }

        static CultureHelper()
        {
            Reload();
        }

        public static void Reload()
        {
            // This is an ugly hack for the missing regionl information...
            DateTimeFormatter dtf = new DateTimeFormatter("longdate", new[] { "US" });
            string regionInfoName = dtf.ResolvedLanguage;
            CurrentCulture = new CultureInfo(regionInfoName);
        }
    }
}