using EmployeeTimeTracker.Core;
using EmployeeTimeTracker.Utilities;

namespace EmployeeTimeTracker
{
    class Program
    {
        // Dictionary: "Employee Timein-TimeOut Record"
        static Dictionary<string, Employee> EmployeeTimeinTimeOutRecord = new Dictionary<string, Employee>();

        static void Main()
        {
            Console.Title = "Employee Time Tracker";
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("+--------------------------------------+");
                Console.WriteLine("|   Employee Time Tracker               |");
                Console.WriteLine("+--------------------------------------+");
                Console.WriteLine();
                Console.WriteLine("  1 - Clock In");
                Console.WriteLine("  2 - Clock Out");
                Console.WriteLine("  3 - Show All Records");
                Console.WriteLine("  4 - Find Record");
                Console.WriteLine("  5 - Exit");
                Console.WriteLine();
                Console.Write("Choose an option: ");

                string? choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": ClockIn(); break;
                    case "2": ClockOut(); break;
                    case "3": ShowAllRecords(); break;
                    case "4": FindRecord(); break;
                    case "5":
                        running = false;
                        Console.Clear();
                        Console.WriteLine("\nGoodbye!\n");
                        break;
                    default:
                        Console.WriteLine("\nInvalid option. Try again.");
                        Pause();
                        break;
                }
            }
        }

        // ── Clock In ──
        static void ClockIn()
        {
            Console.Clear();

            Console.Write("Enter Employee Number: ");
            string? empNum = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(empNum))
            {
                Console.WriteLine("Employee number is required.");
                Pause();
                return;
            }

            if (EmployeeTimeinTimeOutRecord.ContainsKey(empNum))
            {
                Console.WriteLine($"\nEmployee {empNum} already has an active record.");
                Pause();
                return;
            }

            Console.Write("Enter Employee Name: ");
            string? empName = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(empName))
            {
                Console.WriteLine("Employee name is required.");
                Pause();
                return;
            }

            Console.Write("Office Location (Philippines, United States, India): ");
            string? location = Console.ReadLine()?.Trim();

            // Get the matching office using abstract class + inheritance
            OfficeLocation? office = GetOffice(location);
            if (office == null)
            {
                Console.WriteLine("Invalid location! Accepted: Philippines, United States, India.");
                Pause();
                return;
            }

            // Get current time based on office time zone
            DateTime localTime = office.GetCurrentTime();

            // Create employee record and store in dictionary
            Employee emp = new Employee(empNum, empName, office.Name, localTime);
            EmployeeTimeinTimeOutRecord[empNum] = emp;

            Console.WriteLine($"\nClock-in recorded!");
            Console.WriteLine($"  Date: {localTime:MM/dd/yy}");
            Console.WriteLine($"  Time: {localTime:hh:mm:ss tt}");
            Console.WriteLine();
            Console.WriteLine("-- Employee Log --");
            Console.WriteLine($"  Name:     {emp.EmployeeName}");
            Console.WriteLine($"  Location: {emp.OfficeLocation}");
            Console.WriteLine($"  Time-In:  {emp.TimeIn:MM/dd/yy hh:mm:ss tt}");

            Pause();
        }

        // ── Clock Out ──
        static void ClockOut()
        {
            Console.Clear();

            Console.Write("Enter Employee Number: ");
            string? empNum = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(empNum))
            {
                Console.WriteLine("Employee number is required.");
                Pause();
                return;
            }

            if (!EmployeeTimeinTimeOutRecord.ContainsKey(empNum))
            {
                Console.WriteLine($"\nNo record found for employee {empNum}.");
                Pause();
                return;
            }

            Employee emp = EmployeeTimeinTimeOutRecord[empNum];

            if (emp.TimeOut.HasValue)
            {
                Console.WriteLine($"\nEmployee {empNum} has already clocked out.");
                Pause();
                return;
            }

            // Get office to use the correct time zone for clock-out too
            OfficeLocation? office = GetOffice(emp.OfficeLocation);
            DateTime localNow = office!.GetCurrentTime();
            emp.TimeOut = localNow;

            // Compute hours and note using abstract class (ShiftAnalyzer -> ShiftEvaluator)
            ShiftEvaluator evaluator = new ShiftEvaluator();
            double totalHours = evaluator.ComputeTotalHours(emp.TimeIn, emp.TimeOut.Value);
            string note = evaluator.GenerateNote(totalHours);

            Console.WriteLine();
            Console.WriteLine("--- TIMESHEET SUMMARY ---");
            Console.WriteLine($"  Employee Number: {emp.EmployeeNumber}");
            Console.WriteLine($"  Name:            {emp.EmployeeName}");
            Console.WriteLine($"  Office:          {emp.OfficeLocation}");
            Console.WriteLine($"  Time In:         {emp.TimeIn:MM/dd/yy hh:mm:ss tt}");
            Console.WriteLine($"  Time Out:        {emp.TimeOut.Value:MM/dd/yy hh:mm:ss tt}");
            Console.WriteLine($"  Total Hours:     {totalHours} hours");
            Console.WriteLine($"  Note:            {(string.IsNullOrEmpty(note) ? "" : note)}");

            Pause();
        }

        // ── Show All Records ──
        static void ShowAllRecords()
        {
            Console.Clear();
            Console.WriteLine("+--------------------------------------+");
            Console.WriteLine("|  Employee Timein-TimeOut Record       |");
            Console.WriteLine("+--------------------------------------+");
            Console.WriteLine();

            if (EmployeeTimeinTimeOutRecord.Count == 0)
            {
                Console.WriteLine("  (no records yet)");
                Pause();
                return;
            }

            ShiftEvaluator evaluator = new ShiftEvaluator();
            int count = 1;

            foreach (var pair in EmployeeTimeinTimeOutRecord)
            {
                Console.WriteLine($"  ── Record {count} ──");
                PrintRecord(pair.Value, evaluator);
                Console.WriteLine();
                count++;
            }

            Pause();
        }

        // ── Find Record ──
        static void FindRecord()
        {
            Console.Clear();
            Console.Write("Enter Employee Number to search: ");
            string? empNum = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(empNum))
            {
                Console.WriteLine("Employee number is required.");
                Pause();
                return;
            }

            if (!EmployeeTimeinTimeOutRecord.ContainsKey(empNum))
            {
                Console.WriteLine($"\nNo record found for employee {empNum}.");
                Pause();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("+--------------------------------------+");
            Console.WriteLine("|  Employee Timein-TimeOut Record       |");
            Console.WriteLine("+--------------------------------------+");
            Console.WriteLine();

            ShiftEvaluator evaluator = new ShiftEvaluator();
            PrintRecord(EmployeeTimeinTimeOutRecord[empNum], evaluator);

            Pause();
        }

        // ── Helpers ──

        /// <summary>
        /// Returns the matching OfficeLocation subclass, or null if invalid.
        /// </summary>
        static OfficeLocation? GetOffice(string? location)
        {
            if (string.IsNullOrEmpty(location)) return null;

            switch (location.ToLower())
            {
                case "philippines": return new PhilippinesOffice();
                case "united states": return new USOffice();
                case "india": return new IndiaOffice();
                default: return null;
            }
        }

        /// <summary>
        /// Prints a single employee record to the console.
        /// </summary>
        static void PrintRecord(Employee emp, ShiftEvaluator evaluator)
        {
            Console.WriteLine($"  Employee Number: {emp.EmployeeNumber}");
            Console.WriteLine($"  Name:            {emp.EmployeeName}");
            Console.WriteLine($"  Location:        {emp.OfficeLocation}");
            Console.WriteLine($"  Time In:         {emp.TimeIn:MM/dd/yy hh:mm:ss tt}");

            if (emp.TimeOut.HasValue)
            {
                double totalHours = evaluator.ComputeTotalHours(emp.TimeIn, emp.TimeOut.Value);
                string note = evaluator.GenerateNote(totalHours);

                Console.WriteLine($"  Time Out:        {emp.TimeOut.Value:MM/dd/yy hh:mm:ss tt}");
                Console.WriteLine($"  Total Hours:     {totalHours} hours");
                Console.WriteLine($"  Note:            {(string.IsNullOrEmpty(note) ? "" : note)}");
            }
            else
            {
                Console.WriteLine("  Time Out:        (pending)");
            }
        }

        static void Pause()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
