namespace EmployeeTimeTracker.Core
{
    /// <summary>
    /// Abstract base class for office locations.
    /// Each subclass defines the office name and its time zone.
    /// Demonstrates inheritance and abstraction.
    /// </summary>
    public abstract class OfficeLocation
    {
        public abstract string Name { get; }
        public abstract TimeZoneInfo TimeZone { get; }

        /// <summary>
        /// Gets the current local date and time for this office.
        /// </summary>
        public DateTime GetCurrentTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZone);
        }
    }

    public class PhilippinesOffice : OfficeLocation
    {
        public override string Name => "Philippines";
        public override TimeZoneInfo TimeZone => TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
    }

    public class USOffice : OfficeLocation
    {
        public override string Name => "United States";
        public override TimeZoneInfo TimeZone => TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
    }

    public class IndiaOffice : OfficeLocation
    {
        public override string Name => "India";
        public override TimeZoneInfo TimeZone => TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
    }
}
