namespace IndependentWork19;

public class InsufficientFundsException : Exception
{
    public decimal RequestedAmount { get; }
    public decimal CurrentBalance { get; }

    // Три стандартні конструктори
    public InsufficientFundsException() { }

    public InsufficientFundsException(string message) : base(message) { }

    public InsufficientFundsException(string message, Exception innerException)
        : base(message, innerException) { }

    // Конструктор з контекстом помилки
    public InsufficientFundsException(string message, decimal requestedAmount, decimal currentBalance)
        : base(message)
    {
        RequestedAmount = requestedAmount;
        CurrentBalance = currentBalance;
    }
}

public class BankAccount
{
    public decimal Balance { get; private set; }

    public BankAccount(decimal balance) => Balance = balance;

    public void Withdraw(decimal amount)
    {
        if (amount > Balance)
            throw new InsufficientFundsException(
                $"Недостатньо коштів: запит {amount:F2} грн, на рахунку {Balance:F2} грн.", amount, Balance);

        Balance -= amount;
    }
}
