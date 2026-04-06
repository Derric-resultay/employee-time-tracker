namespace EmployeeTimeTracker.Utilities
{
    /// <summary>
    /// Abstract base class for mathematical computations related to work shifts.
    /// Demonstrates the use of abstract classes as required by the project.
    /// </summary>
    public abstract class ShiftAnalyzer
    {
        protected const double RequiredHours = 9.0;

        public abstract double ComputeTotalHours(DateTime timeIn, DateTime timeOut);
        public abstract string GenerateNote(double totalHours);
    }
}
