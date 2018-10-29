namespace IRunes.WebApp.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using IRunes.WebApp.Models;
    using IRunes.WebApp.Services;
    using IRunes.WebApp.Services.Contracts;
    using SIS.HTTP.Requests.Contracts;
    using SIS.MvcFramework.ActionResults.Contracts;
    using SIS.MvcFramework.Attributes.Methods;
    using SIS.WebServer.Results;

    public class UsersController : BaseController
    {
        private IHashService hashService;

        public UsersController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (this.IsAuthenticated(this.Request))
            {
                // TODO: return error view.
                //return this.BadRequestError("You should sign out first.");
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Login(string username2, string password2)
        {
            if (this.IsAuthenticated(this.Request))
            {
                // TODO: return error view.
                //return this.BadRequestError("You should sign out first.");
            }

            string username = this.Request.FormData["username"].ToString();
            string password = this.Request.FormData["password"].ToString();

            string hashedPassword = this.hashService.HashPassword(password);

            User user = this.IRunesContext.Users
                .FirstOrDefault(u => (u.Username.ToLower() == username.ToLower() ||
                                u.Email.ToLower() == username.ToLower()) &&
                                u.Password == hashedPassword);

            if (user == null)
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Invalid username or password.</h1>";
                return this.RedirectToAction("/Users/Login");
            }

            this.Request.Session.AddParameter("username", user.Username);

            return this.RedirectToAction("/");
        }

        public IActionResult Register()
        {
            if (this.IsAuthenticated(this.Request))
            {
                // TODO: return error view.
                //return this.BadRequestError("You should sign out first.");
            }

            return this.View("Register");
        }

        public IActionResult PostRegister()
        {
            if (this.IsAuthenticated(this.Request))
            {
                // TODO: return error view.
                //return this.BadRequestError("You should sign out first.");
            }

            string username = this.Request.FormData["username"].ToString();
            string password = this.Request.FormData["password"].ToString();
            string confirmPassword = this.Request.FormData["confirmPassword"].ToString();
            string email = this.Request.FormData["email"].ToString();

            if (string.IsNullOrWhiteSpace(username))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Username should not be null or whitespace.</h1>";
                return this.RedirectToAction("/Users/Register");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Password cannot be empty.</h1>";
                return this.RedirectToAction("/Users/Register");
            }

            if (!password.Equals(confirmPassword))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Passwords doesn't match.</h1>";
                return this.RedirectToAction("/Users/Register");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Email is required.</h1>";
                return this.RedirectToAction("/Users/Register");
            }

            string hashedPassword = this.hashService.HashPassword(password);

            User user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = username.Trim(),
                Email = email.Trim(),
                Password = hashedPassword
            };

            this.IRunesContext.Add(user);

            try
            {
                this.IRunesContext.SaveChanges();
            }
            catch (Exception e)
            {
                // TODO: return error view.
                //return new HtmleResult(e.Message, HttpStatusCode.InternalServerError);
            }

            this.Request.Session.AddParameter("username", username);

            return this.RedirectToAction("/");
        }

        public IActionResult Logout()
        {
            if (this.IsAuthenticated(this.Request))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Email is required.</h1>";
                return this.RedirectToAction("/");
            }

            this.Request.Session.ClearParameters();

            return this.RedirectToAction("/");
        }
    }
}
