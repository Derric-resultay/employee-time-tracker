namespace EmployeeTimeTracker.Core
{
    /// <summary>
    /// Represents an employee with their attendance data.
    /// Demonstrates encapsulation through private backing fields.
    /// </summary>
    public class Employee
    {
        // ── Private backing fields (encapsulation) ──
        private string _employeeNumber;
        private string _employeeName;
        private string _officeLocation;
        private DateTime _timeIn;
        private DateTime? _timeOut;

        public string EmployeeNumber
        {
            get { return _employeeNumber; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Employee number cannot be empty.");
                _employeeNumber = value;
            }
        }

        public string EmployeeName
        {
            get { return _employeeName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Employee name cannot be empty.");
                _employeeName = value;
            }
        }

        public string OfficeLocation
        {
            get { return _officeLocation; }
            set { _officeLocation = value; }
        }

        public DateTime TimeIn
        {
            get { return _timeIn; }
            set { _timeIn = value; }
        }

        public DateTime? TimeOut
        {
            get { return _timeOut; }
            set { _timeOut = value; }
        }

        public Employee(string empNumber, string empName, string location, DateTime timeIn)
        {
            if (string.IsNullOrWhiteSpace(empNumber))
                throw new ArgumentException("Employee number cannot be empty.");
            if (string.IsNullOrWhiteSpace(empName))
                throw new ArgumentException("Employee name cannot be empty.");

            _employeeNumber = empNumber;
            _employeeName = empName;
            _officeLocation = location;
            _timeIn = timeIn;
            _timeOut = null;
        }
    }
}
