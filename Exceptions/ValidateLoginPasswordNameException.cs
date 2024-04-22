namespace AtonAssignment.Exceptions
{
    /// <summary>
    /// The exception that is thrown when received information for fields Name, Login, Password do not meet the requirements.
    /// </summary>
    public class ValidateLoginPasswordNameException : Exception
    {
        public ValidateLoginPasswordNameException()
            : base("All characters except Latin letters and numbers in login, password and name fields are prohibited") {  }
    }
}
