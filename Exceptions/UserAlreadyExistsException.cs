namespace AtonAssignment.Exceptions
{
    /// <summary>
    /// The exception that is thrown when an attempt was made to create a user with an existing login. 
    /// </summary>
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException()
            : base("User with this login is already exists") { }
    }
}
