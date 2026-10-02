namespace BudgetWPF.Resources.Exceptions
{
    public class UnexpectedNullInMethodException : Exception
    {
        public UnexpectedNullInMethodException(string methodName, string nullFieldName) : 
        base($"Cannot call {methodName} while {nullFieldName} is null") {}
    }
}