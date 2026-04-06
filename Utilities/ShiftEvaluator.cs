namespace EmployeeTimeTracker.Utilities
{
    /// <summary>
    /// Concrete implementation for computing work hours and generating notes.
    /// Inherits from the abstract ShiftAnalyzer class.
    /// </summary>
    public class ShiftEvaluator : ShiftAnalyzer
    {
        public override double ComputeTotalHours(DateTime timeIn, DateTime timeOut)
        {
            return Math.Round((timeOut - timeIn).TotalHours, 1);
        }

        public override string GenerateNote(double totalHours)
        {
            if (Math.Abs(totalHours - RequiredHours) < 0.01)
                return "";

            if (totalHours < RequiredHours)
                return $"Early Out. Hours left: {RequiredHours - totalHours:F1} hours";

            return $"Overtime. Hours extended: {totalHours - RequiredHours:F1} hours";
        }
    }
}
