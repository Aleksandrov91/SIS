namespace IRunes.WebApp.Controllers
{
    using System;
    using System.Net;
    using System.Web;
    using IRunes.WebApp.Models;
    using SIS.HTTP.Requests.Contracts;
    using SIS.MvcFramework.ActionResults.Contracts;
    using SIS.WebServer.Results;

    public class TracksController : BaseController
    {
        public IActionResult Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Login first.</h1>";
                return this.RedirectToAction("/Users/Login");
            }

            this.ViewBag["albumId"] = request.QueryData["id"].ToString();

            return this.View();
        }

        public IActionResult PostCreate(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Login first.</h1>";
                return this.RedirectToAction("/Users/Login");
            }

            string trackName = request.FormData["name"].ToString().Trim();
            string trackLink = request.FormData["link"].ToString().Trim();
            decimal trackPrice = decimal.Parse(request.FormData["price"].ToString());

            if (string.IsNullOrWhiteSpace(trackName))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Name is required.</h1>";
                return this.RedirectToAction("/Tracks/Create");
            }

            if (string.IsNullOrWhiteSpace(trackLink))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Url is required.</h1>";
                return this.RedirectToAction("/Tracks/Create");
            }

            if (trackPrice <= 0)
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Price cannot be negative.</h1>";
                return this.RedirectToAction("/Tracks/Create");
            }

            Track track = new Track
            {
                Id = Guid.NewGuid().ToString(),
                Name = trackName,
                Link = trackLink,
                Price = trackPrice,
                AlbumId = request.QueryData["albumId"].ToString()
            };

            this.IRunesContext.Tracks.Add(track);

            try
            {
                this.IRunesContext.SaveChanges();
            }
            catch (Exception e)
            {
                // TODO: return error view.
                //return new HtmleResult(e.Message, HttpStatusCode.InternalServerError);
            }

            return this.RedirectToAction($"/Albums/Details?{track.AlbumId}");
        }

        public IActionResult Details(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Login first.</h1>";
                return this.RedirectToAction("/Users/Login");
            }

            string albumId = request.QueryData["albumId"].ToString();
            string trackId = request.QueryData["trackId"].ToString();

            Track track = this.IRunesContext.Tracks.Find(trackId);

            if (track == null)
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Invalid track.</h1>";
                return this.RedirectToAction($"/Albums/Details?{albumId}");
            }

            this.ViewBag["trackLink"] = HttpUtility.UrlDecode(track.Link);
            this.ViewBag["trackName"] = track.Name;
            this.ViewBag["trackPrice"] = track.Price.ToString();
            this.ViewBag["albumId"] = track.AlbumId;

            return this.View();
        }
    }
}
