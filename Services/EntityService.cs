using AtonAssignment.Exceptions;
using AtonAssignment.Models;
using System.Text.RegularExpressions;

namespace AtonAssignment.Services
{
    /// <summary>
    /// Service that store database operations for both controllers.
    /// </summary>
    public class EntityService
    {
        /// <summary>
        /// Database context.
        /// </summary>
        private readonly AtondbContext _context;

        /// <summary>
        /// Regex to check Name, Password and Login fields.
        /// </summary>
        private readonly Regex latinLettersAndNumbers = new Regex(@"^[a-zA-Z0-9]+$");

        public EntityService(AtondbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create user with passed parameters.
        /// </summary>
        /// <param name="newUserLogin">New user login.</param>
        /// <param name="newUserPassword">New user password.</param>
        /// <param name="newUserName">New user name.</param>
        /// <param name="newUserGender">New user gender.</param>
        /// <param name="newUserBirthday">New user birtday.</param>
        /// <param name="createdBy">New user was created by.</param>
        /// <param name="isAdmin">Is the new user an administrator?</param>
        /// <exception cref="ValidateLoginPasswordNameException">Name field must contain only Latin letters and numbers.</exception>
        public void CreateUser(string newUserLogin, string newUserPassword, string newUserName, int newUserGender, DateTime newUserBirthday, string createdBy, bool isAdmin = false)
        {
            IsUserLoginUnique(newUserLogin);

            if (!latinLettersAndNumbers.Match(newUserLogin).Success || !latinLettersAndNumbers.Match(newUserPassword).Success || !latinLettersAndNumbers.Match(newUserName).Success)
                throw new ValidateLoginPasswordNameException();

            User userToCreate = new User()
            {
                Login = newUserLogin,
                Password = newUserPassword,
                Name = newUserName,
                Gender = newUserGender,
                Birthday = newUserBirthday,
                CreatedBy = createdBy,
                Admin = isAdmin
            };

            if (userToCreate.Birthday == DateTime.MinValue)
            {
                userToCreate.Birthday = DateTime.MaxValue;
            }

            try
            {
                _context.Users.Add(userToCreate);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Change users name.
        /// </summary>
        /// <param name="userLogin">User to change name.</param>
        /// <param name="nameTochange">New users name.</param>
        /// <param name="modifiedBy">User was modified by.</param>
        /// <exception cref="ValidateLoginPasswordNameException">Login field must contain only Latin letters and numbers.</exception>
        public void ChangeUserName(string userLogin, string nameTochange, string modifiedBy)
        {
            var user = _context.Users.First(u => u.Login == userLogin);

            if (!latinLettersAndNumbers.Match(nameTochange).Success)
                throw new ValidateLoginPasswordNameException();

            user.Name = nameTochange;
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.UtcNow;

            _context.SaveChanges();
        }

        /// <summary>
        /// Change users gender.
        /// </summary>
        /// <param name="userLogin">User to change gender.</param>
        /// <param name="genderToChange">New users gender.</param>
        /// <param name="modifiedBy">User was modified by.</param>
        public void ChangeUserGender(string userLogin, int genderToChange, string modifiedBy)
        {
            var user = _context.Users.First(u => u.Login == userLogin);

            user.Gender = genderToChange;
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.UtcNow;

            _context.SaveChanges();
        }

        /// <summary>
        /// Change users birthday.
        /// </summary>
        /// <param name="userLogin">User to change birthday.</param>
        /// <param name="birthdayToChange">New users birthday.</param>
        /// <param name="modifiedBy">User was modified by.</param>
        public void ChangeUserBirthday(string userLogin, DateTime birthdayToChange, string modifiedBy)
        {
            var user = _context.Users.First(u => u.Login == userLogin);

            user.Birthday = birthdayToChange;
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.UtcNow;

            _context.SaveChanges();
        }

        /// <summary>
        /// Change users password.
        /// </summary>
        /// <param name="userLogin">User to change password.</param>
        /// <param name="password">New users password.</param>
        /// <param name="modifiedBy">User was modified by.</param>
        /// <exception cref="ValidateLoginPasswordNameException">Password field must contain only Latin letters and numbers.</exception>
        public void ChangeUserPassword(string userLogin, string password, string modifiedBy)
        {
            if (!latinLettersAndNumbers.Match(password).Success)
                throw new ValidateLoginPasswordNameException();

            var user = _context.Users.First(u => u.Login == userLogin);

            user.Password = password;
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.UtcNow;

            _context.SaveChanges();
        }

        /// <summary>
        /// Change users login.
        /// </summary>
        /// <param name="userLogin">User to change login.</param>
        /// <param name="newLogin">New users login.</param>
        /// <param name="modifiedBy">User was modified by.</param>
        /// <exception cref="ValidateLoginPasswordNameException">Login field must contain only Latin letters and numbers.</exception>
        public void ChangeUserLogin(string userLogin, string newLogin, string modifiedBy)
        {
            IsUserLoginUnique(newLogin);

            if (!latinLettersAndNumbers.Match(newLogin).Success)
                throw new ValidateLoginPasswordNameException();

            var user = _context.Users.First(u => u.Login == userLogin);

            user.Login = newLogin;
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.UtcNow;

            _context.SaveChanges();
        }

        /// <summary>
        /// Check is users login unique.
        /// </summary>
        /// <param name="login">Users login.</param>
        /// <exception cref="UserAlreadyExistsException">Users login must be unique.</exception>
        private void IsUserLoginUnique(string login)
        {
            if (_context.Users.Any(u => u.Login == login))
                throw new UserAlreadyExistsException();
        }
    }
}
