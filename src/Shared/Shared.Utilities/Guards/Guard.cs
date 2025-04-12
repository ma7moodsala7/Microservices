namespace Shared.Utilities.Guards;

/// <summary>
/// Provides guard clauses for defensive programming
/// </summary>
public static class Guard
{
    public static class Against
    {
        public static T Null<T>(T value, string parameterName) where T : class
        {
            if (value is null)
            {
                throw new ArgumentNullException(parameterName);
            }
            return value;
        }

        public static string NullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Value cannot be null or empty.", parameterName);
            }
            return value;
        }

        public static string NullOrWhiteSpace(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", parameterName);
            }
            return value;
        }

        // TODO: Add more guard clauses as needed
        // - OutOfRange for numbers
        // - InvalidFormat for strings (regex)
        // - NegativeOrZero for numbers
        // - EmptyGuid
        // - EmptyCollection
    }
}
