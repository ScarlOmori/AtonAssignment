using AtonAssignment.Constants;
using AtonAssignment.Exceptions;
using AtonAssignment.Models;
using AtonAssignment.Services;
using Microsoft.AspNetCore.Mvc;

namespace AtonAssignment.Controllers
{
    /// <summary>
    /// Contain users functionality.
    /// </summary>
    [Route("[controller]/[action]")]
    public class UsersController : Controller
    {
        /// <summary>
        /// Database context.
        /// </summary>
        private readonly AtondbContext _context;

        /// <summary>
        /// Service that store database operations for both controllers.
        /// </summary>
        private readonly EntityService _service;

        public UsersController(AtondbContext context, EntityService service)
        {
            _context = context;
            _service = service;
        }

        /// <summary>
        /// Creates user by passed fields.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="newUserLogin">New user login.</param>
        /// <param name="newUserPassword">New user password.</param>
        /// <param name="newUserName">New user name.</param>
        /// <param name="newUserGender">New user gender.</param>
        /// <param name="newUserBirthday">New user birtday.</param>
        /// <returns>Status code and text that describes it.</returns>
        [HttpPost]
        public IActionResult CreateUser(string login, string password, string newUserLogin, string newUserPassword, string newUserName, int newUserGender, DateTime newUserBirthday)
        {
            if (IsUserExist(login, password) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            try
            {
                _service.CreateUser(newUserLogin, newUserPassword, newUserName, newUserGender, newUserBirthday, login);
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ValidateLoginPasswordNameException ex)
            {
                return Conflict(ex.Message);
            }

            return Ok("New user has been successfully created.");
        }

        /// <summary>
        /// Change users name.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="nameToChange">New users name.</param>
        /// <returns>Status code and text that describes it.</returns>
        [HttpPatch]
        public IActionResult ChangeUserName(string login, string password, string nameToChange)
        {
            User user;

            if ((user = IsUserExist(login, password)) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            if (!string.IsNullOrEmpty(user.RevokedBy))
            {
                return Conflict(ResponseTextConstants.USER_IS_ALREADY_REVOKED);
            }

            try
            {
                _service.ChangeUserName(login, nameToChange, login);
            }
            catch (ValidateLoginPasswordNameException)
            {
                return Conflict("All characters except Latin letters and numbers in name field are prohibited");
            }

            return Ok("Name has been successfully changed.");
        }

        /// <summary>
        /// Change users gender.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="gender">New users gender.</param>
        /// <returns>Status code and text that describes it.</returns>
        [HttpPatch]
        public IActionResult ChangeUserGender(string login, string password, int gender)
        {
            User user;

            if ((user = IsUserExist(login, password)) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            if (!string.IsNullOrEmpty(user.RevokedBy))
            {
                return Conflict(ResponseTextConstants.USER_IS_ALREADY_REVOKED);
            }

            _service.ChangeUserGender(login, gender, login);

            return Ok("Gender has been successfully changed.");
        }

        /// <summary>
        /// Change users birthday.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="birthday">New users birthday.</param>
        /// <returns>Status code and text that describes it.</returns>
        [HttpPatch]
        public IActionResult ChangeUserBirthday(string login, string password, DateTime birthday)
        {
            User user;

            if ((user = IsUserExist(login, password)) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            if (!string.IsNullOrEmpty(user.RevokedBy))
            {
                return Conflict(ResponseTextConstants.USER_IS_ALREADY_REVOKED);
            }

            _service.ChangeUserBirthday(login, birthday, login);

            return Ok("Birthday has been successfully changed.");
        }

        /// <summary>
        /// Change users password.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="newPassword">New users password.</param>
        /// <returns>Status code and text that describes it.</returns>
        [HttpPatch]
        public IActionResult ChangeUserPassword(string login, string password, string newPassword)
        {
            User user;

            if ((user = IsUserExist(login, password)) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            if (!string.IsNullOrEmpty(user.RevokedBy))
            {
                return Conflict(ResponseTextConstants.USER_IS_ALREADY_REVOKED);
            }

            try
            {
                _service.ChangeUserPassword(login, newPassword, login);
            }
            catch (ValidateLoginPasswordNameException)
            {
                return Conflict("All characters except Latin letters and numbers in password field are prohibited");
            }

            return Ok("Password has been successfully changed.");
        }

        /// <summary>
        /// Change users login.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="newLogin">New users login.</param>
        /// <returns>Status code and text that describes it.</returns>
        [HttpPatch]
        public IActionResult ChangeUserLogin(string login, string password, string newLogin)
        {
            User user;

            if ((user = IsUserExist(login, password)) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            if (!string.IsNullOrEmpty(user.RevokedBy))
            {
                return Conflict(ResponseTextConstants.USER_IS_ALREADY_REVOKED);
            }

            try
            {
                _service.ChangeUserLogin(login, newLogin, newLogin);
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ValidateLoginPasswordNameException)
            {
                return Conflict("All characters except Latin letters and numbers in login field are prohibited");
            }

            return Ok("Login has been successfully changed.");
        }

        /// <summary>
        /// Give all users info by login and password.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <returns>If user exists and active returns it object, otherwise returns status code and text that rescribes it.</returns>
        [HttpGet]
        public IActionResult GetUserByLoginAndPassword(string login, string password)
        {
            User user;

            if ((user = IsUserExist(login, password)) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            if (!string.IsNullOrEmpty(user.RevokedBy))
            {
                return Conflict(ResponseTextConstants.USER_IS_ALREADY_REVOKED);
            }

            return Json(user);
        }

        /// <summary>
        /// Check is user with this login and password exists.
        /// </summary>
        /// <param name="login">Users login.</param>
        /// <param name="password">Users password.</param>
        /// <returns>If an user exists, returns its object, otherwise null.</returns>
        private User IsUserExist(string login, string password) => _context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

    }
}
