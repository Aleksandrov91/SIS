namespace IRunes.WebApp.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using IRunes.WebApp.Models;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class AlbumsController : BaseController
    {
        public IHttpResponse All(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Login first.</h1>";
                return this.RedirectToAction("/Users/Login");
            }

            Album[] albums = this.IRunesContext.Albums.ToArray();

            StringBuilder albumList = new StringBuilder();

            if (albums.Length == 0)
            {
                albumList.AppendLine("<h1>There are currently no albums.</h1>");
            }

            foreach (var album in albums)
            {
                albumList.AppendLine($"<h1><a href=\"/Albums/Details?id={album.Id}\">{album.Name}</a></h1>");
            }

            this.ViewBag["albums"] = albumList.ToString();

            return this.View();
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Login first.</h1>";
                return this.RedirectToAction("/Users/Login");
            }

            return this.View();
        }

        public IHttpResponse PostCreate(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Login first.</h1>";
                return this.RedirectToAction("/Users/Login");
            }

            string albumName = request.FormData["name"].ToString().Trim();
            string albumCoverUrl = request.FormData["cover"].ToString().Trim();

            if (string.IsNullOrWhiteSpace(albumName))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Album name is required.</h1>";
                return this.RedirectToAction("/Albums/All");
            }

            if (string.IsNullOrWhiteSpace(albumCoverUrl))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Album cover url is required.</h1>";
                return this.RedirectToAction("/Albums/All");
            }

            Album album = new Album
            {
                Id = Guid.NewGuid().ToString(),
                Name = albumName,
                CoverUrl = albumCoverUrl
            };

            this.IRunesContext.Albums.Add(album);

            try
            {
                this.IRunesContext.SaveChanges();
            }
            catch (Exception e)
            {
                return new HtmleResult(e.Message, HttpStatusCode.InternalServerError);
            }

            return this.RedirectToAction("/Albums/All");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Login first.</h1>";
                return this.RedirectToAction("/Users/Login");
            }

            string albumId = request.QueryData["id"].ToString();

            Album album = this.IRunesContext.Albums.Find(albumId);

            if (album == null)
            {
                TempData["errorMessage"] = $"<h1 style=\"color: red\">Invalid album.</h1>";
                return this.RedirectToAction("/Albums/All");
            }

            StringBuilder albumTrackList = new StringBuilder();
            albumTrackList.AppendLine("<ul>");

            int count = 1;
            foreach (var albumTrack in album.Tracks)
            {
                albumTrackList.AppendLine($"<li>{count++}.<a href=\"/Tracks/Details?albumId={album.Id}&trackId={albumTrack.Id}\">{albumTrack.Name}</a></li>");
            }

            albumTrackList.AppendLine("</ul>");

            this.ViewBag["albumCover"] = HttpUtility.UrlDecode(album.CoverUrl);
            this.ViewBag["albumName"] = album.Name;
            this.ViewBag["albumPrice"] = album.Price.ToString();
            this.ViewBag["albumTracks"] = albumTrackList.ToString();
            this.ViewBag["albumId"] = album.Id;

            return this.View();
        }
    }
}
