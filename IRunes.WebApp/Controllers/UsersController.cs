namespace IRunes.WebApp.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using IRunes.WebApp.Models;
    using IRunes.WebApp.Services;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class UsersController : BaseController
    {
        private HashService hashService;

        public UsersController()
        {
            this.hashService = new HashService();
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                return this.BadRequestError("You should sign out first.");
            }

            return this.View();
        }

        public IHttpResponse PostLogin(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                return this.BadRequestError("You should sign out first.");
            }

            string username = request.FormData["username"].ToString();
            string password = request.FormData["password"].ToString();

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

            request.Session.AddParameter("username", user.Username);

            return new RedirectResult("/");
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                return this.BadRequestError("You should sign out first.");
            }

            return this.View("Register");
        }

        public IHttpResponse PostRegister(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                return this.BadRequestError("You should sign out first.");
            }

            string username = request.FormData["username"].ToString();
            string password = request.FormData["password"].ToString();
            string confirmPassword = request.FormData["confirmPassword"].ToString();
            string email = request.FormData["email"].ToString();

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
                return new HtmleResult(e.Message, HttpStatusCode.InternalServerError);
            }

            request.Session.AddParameter("username", username);

            return new RedirectResult("/");
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Email is required.</h1>";
                return this.RedirectToAction("/");
            }

            request.Session.ClearParameters();

            return new RedirectResult("/");
        }
    }
}
