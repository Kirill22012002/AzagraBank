namespace AzagraBank.CommandProcessor.Exceptions;

public class TransactionValidationException : Exception
{
    public TransactionValidationException() : base() { }

    public TransactionValidationException(string message): base(message) { }
}
