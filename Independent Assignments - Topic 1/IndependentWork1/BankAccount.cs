namespace IndependentWork1
{
    /// <summary>
    /// Клас для представлення банківського рахунку.
    /// </summary>
    public class BankAccount
    {
        // Приватні поля
        private string _accountNumber;
        private double _balance;

        // Властивість тільки для читання (read-only)
        public string AccountNumber
        {
            get { return _accountNumber; }
        }

        // Властивість з приватним set
        public double Balance
        {
            get { return _balance; }
            private set { _balance = value; }
        }

        // Конструктор
        public BankAccount(string accountNumber, double initialBalance)
        {
            _accountNumber = accountNumber;
            _balance = initialBalance >= 0 ? initialBalance : 0;
        }

        // Методи з логікою та перевіркою стану
        public void Deposit(double amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                Console.WriteLine($"Рахунок {AccountNumber}: Поповнено на {amount:F2} грн. Новий баланс: {Balance:F2} грн.");
            }
            else
            {
                Console.WriteLine("Сума поповнення повинна бути додатною.");
            }
        }

        public bool Withdraw(double amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Сума зняття повинна бути додатною.");
                return false;
            }

            if (amount <= Balance)
            {
                Balance -= amount;
                Console.WriteLine($"Рахунок {AccountNumber}: Знято {amount:F2} грн. Залишок: {Balance:F2} грн.");
                return true;
            }
            else
            {
                Console.WriteLine($"Рахунок {AccountNumber}: Недостатньо коштів для зняття {amount:F2} грн. Поточний баланс: {Balance:F2} грн.");
                return false;
            }
        }
    }
}
