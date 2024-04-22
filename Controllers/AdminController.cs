using AtonAssignment.Constants;
using AtonAssignment.Exceptions;
using AtonAssignment.Models;
using AtonAssignment.Services;
using Microsoft.AspNetCore.Mvc;

namespace AtonAssignment.Controllers
{
    /// <summary>
    /// Contain administrators functionality.
    /// </summary>
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        /// <summary>
        /// Database context.
        /// </summary>
        private readonly AtondbContext _context;

        /// <summary>
        /// Service that store database operations for both controllers.
        /// </summary>
        private readonly EntityService _service;

        public AdminController(AtondbContext context, EntityService service)
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
        /// <param name="isAdmin">Is the new user an administrator?</param>
        /// <returns>Status code and text that describes it.</returns>
        [HttpPost]
        public IActionResult CreateUserAdmin(string login, string password, string newUserLogin, string newUserPassword, string newUserName, int newUserGender, DateTime newUserBirthday, bool isAdmin)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            try
            {
                _service.CreateUser(newUserLogin, newUserPassword, newUserName, newUserGender, newUserBirthday, login, isAdmin);
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ValidateLoginPasswordNameException ex)
            {
                return Conflict(ex.Message);
            }

            return Ok("User has been successfully created.");
        }

        /// <summary>
        /// Change users name by his login.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="userLogin">Login of the user whose name you want to change.</param>
        /// <param name="nameToChange">New users name.</param>
        /// <returns>Status code and text that describes it.</returns>
        [HttpPatch]
        public IActionResult ChangeUserNameAdmin(string login, string password, string userLogin, string nameToChange)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            try
            {
                _service.ChangeUserName(userLogin, nameToChange, login);
            }
            catch (ValidateLoginPasswordNameException)
            {
                return Conflict("All characters except Latin letters and numbers in name field are prohibited");
            }

            return Ok("Name has been successfully changed.");
        }

        /// <summary>
        /// Change users gender by his login.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="userLogin">Login of the user whose gender you want to change.</param>
        /// <param name="gender">New users gender.</param>
        /// <returns>Status code and text that describes it.</returns>
        [HttpPatch]
        public IActionResult ChangeUserGenderAdmin(string login, string password, string userLogin, int gender)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            _service.ChangeUserGender(userLogin, gender, login);

            return Ok("Gender has been successfully changed.");
        }

        /// <summary>
        /// Change users birthday by his login.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="userLogin">Login of the user whose birthday you want to change.</param>
        /// <param name="birthday">New users birthday.</param>
        /// <returns>Status code and text that describes it.</returns>
        [HttpPatch]
        public IActionResult ChangeUserBirthdayAdmin(string login, string password, string userLogin, DateTime birthday)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            _service.ChangeUserBirthday(userLogin, birthday, login);

            return Ok("Birthday has been successfully changed.");
        }

        /// <summary>
        /// Change users password by his login.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="userLogin">Login of the user whose password you want to change.</param>
        /// <param name="newPassword">New users password.</param>
        /// <returns>Status code and text that describes it.</returns>
        [HttpPatch]
        public IActionResult ChangeUserPasswordAdmin(string login, string password, string userLogin, string newPassword)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            try
            {
                _service.ChangeUserPassword(userLogin, newPassword, login);
            }
            catch (ValidateLoginPasswordNameException)
            {
                return Conflict("All characters except Latin letters and numbers in password field are prohibited");
            }

            return Ok("Password has been successfully changed.");
        }

        /// <summary>
        /// Change users login by his login.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="userLogin">Login of the user whose login you want to change.</param>
        /// <param name="newLogin">New users login.</param>
        /// <returns>Status code and text that describes it.</returns>
        [HttpPatch]
        public IActionResult ChangeUserLoginAdmin(string login, string password, string userLogin, string newLogin)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            try
            {
                _service.ChangeUserLogin(userLogin, newLogin, login);
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
        /// Gives all active users.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <returns>All active users.</returns>
        [HttpGet]
        public IActionResult GetAllActiveUsers(string login, string password)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            return Json(_context.Users.Where(u => string.IsNullOrEmpty(u.RevokedBy)).OrderBy(u => u.CreatedOn).ToList());
        }

        /// <summary>
        /// Gives user by login.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="loginToSearch">Users login to search.</param>
        /// <returns>Users name, gender, birthday and active status.</returns>
        [HttpGet]
        public IActionResult GetUserByLogin(string login, string password, string loginToSearch)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            var searchedUser = _context.Users.FirstOrDefault(u => u.Login == loginToSearch);

            if (searchedUser is null)
            {
                return NotFound("User with this login is not founded.");
            }

            var activeStatus = string.IsNullOrEmpty(searchedUser.RevokedBy) ? "Active" : "Revoked";

            return Json(new { searchedUser.Name, searchedUser.Gender, searchedUser.Birthday, activeStatus });
        }

        /// <summary>
        /// Gives users older than the years parameter passed.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="years">Users older than this amount of years.</param>
        /// <returns>Users list older than the years parameter passed</returns>
        [HttpGet]
        public IActionResult GetUserOlderThan(string login, string password, int years)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            var yearFilter = DateTime.UtcNow.Year - years - 1;
            var currentDate = DateTime.UtcNow;

            var filteredUsers = _context.Users.Where(u => yearFilter > u.Birthday.Value.Year
                || (yearFilter == u.Birthday.Value.Year && u.Birthday.Value.Month < currentDate.Month)
                || (yearFilter == u.Birthday.Value.Year && u.Birthday.Value.Month == currentDate.Month && currentDate.Day > u.Birthday.Value.Day));

            if (filteredUsers is null)
            {
                return NotFound($"Users older than {years} is not found.");
            }

            return Json(filteredUsers);
        }

        /// <summary>
        /// Hard delete user.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="loginToDelete">Users login to delete.</param>
        /// <returns>Status code and text that describes it.</returns>
        [HttpDelete]
        public IActionResult DeleteUserByLoginHard(string login, string password, string loginToDelete)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            var user = _context.Users.FirstOrDefault(u => u.Login == loginToDelete);

            if (user is null)
            {
                return NotFound($"User with login:{loginToDelete} is not found.");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok("User has been successfully deleted.");
        }

        /// <summary>
        /// Soft delete user.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="loginToDelete">Users login to delete.</param>
        /// <returns>Status code and text that describes it.</returns>
        [HttpDelete]
        public IActionResult DeleteUserByLoginSoft(string login, string password, string loginToDelete)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            var user = _context.Users.FirstOrDefault(u => u.Login == loginToDelete);

            if (user is null)
            {
                return NotFound($"Users with login:{loginToDelete} is not found.");
            }

            user.RevokedOn = DateTime.Now;
            user.RevokedBy = login;

            _context.SaveChanges();

            return Ok("User has been successfully deleted.");
        }

        /// <summary>
        /// Resore user.
        /// </summary>
        /// <param name="login">Your login.</param>
        /// <param name="password">Your password.</param>
        /// <param name="loginToRestore">Users login to restore.</param>
        /// <returns>Status code and text that describes it.</returns>
        [HttpPost]
        public IActionResult RestoreUser(string login, string password, string loginToRestore)
        {
            if (IsAdminExist(login, password) is null)
            {
                return Unauthorized(ResponseTextConstants.INCORRECT_LOGIN_OR_PASSWORD);
            }

            var user = _context.Users.FirstOrDefault(u => u.Login == loginToRestore);

            if (user is null)
            {
                return NotFound($"Users with login:{loginToRestore} is not found.");
            }

            user.RevokedOn = DateTime.MaxValue;
            user.RevokedBy = string.Empty;

            _context.SaveChanges();

            return Ok("User has been successfully restored.");
        }

        /// <summary>
        /// Check is admin with this login and password exists.
        /// </summary>
        /// <param name="login">Admins login.</param>
        /// <param name="password">Admins password.</param>
        /// <returns>If an administrator exists, returns its object, otherwise null.</returns>
        private User IsAdminExist(string login, string password) => _context.Users.FirstOrDefault(u => u.Login == login && u.Password == password && u.Admin == true);

    }
}
