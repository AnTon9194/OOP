namespace IndependentWork1
{
    /// <summary>
    /// Клас для представлення співробітника компанії.
    /// </summary>
    public class Employee
    {
        // Приватні поля
        private string _fullName;
        private double _monthlySalary;

        // Властивості з get та set
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        public double MonthlySalary
        {
            get { return _monthlySalary; }
            set
            {
                if (value >= 0)
                    _monthlySalary = value;
            }
        }

        // Властивість тільки для читання
        public double AnnualSalary
        {
            get { return _monthlySalary * 12; }
        }

        // Конструктор
        public Employee(string fullName, double monthlySalary)
        {
            _fullName = fullName;
            _monthlySalary = monthlySalary >= 0 ? monthlySalary : 0;
        }

        // Метод з обчисленням премії
        public double CalculateBonus(double percentage)
        {
            if (percentage < 0) return 0;
            return _monthlySalary * (percentage / 100.0);
        }

        public void PrintInfo()
        {
            Console.WriteLine($"[Співробітник] ПІБ: {FullName}, Місячна зарплата: {MonthlySalary:F2} грн, Річний дохід: {AnnualSalary:F2} грн");
        }
    }
}
