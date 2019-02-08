namespace SIS.WebServer
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Requests;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.HTTP.Sessions;
    using SIS.WebServer.Api;

    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly IHttpHandler routeHandler;

        private readonly IHttpHandler resourceHandler;

        public ConnectionHandler(Socket client, IHttpHandler handler, IHttpHandler resourceHandler)
        {
            this.client = client;
            this.routeHandler = handler;
            this.resourceHandler = resourceHandler;
        }

        public async Task ProcessRequestAsync()
        {
            IHttpRequest httpRequest = await this.ReadRequest();

            if (httpRequest != null)
            {
                string sessionId = this.SetRequestSession(httpRequest);

                IHttpResponse httpResponse = null;

                if (this.IsResourceRequest(httpRequest))
                {
                    httpResponse = this.resourceHandler.Handle(httpRequest);
                }
                else
                {
                    httpResponse = this.routeHandler.Handle(httpRequest);
                }

                // TODO: Use this method to set cookie only ones.
                if (!httpRequest.Cookies.ContainsCookie(HttpSessionStorage.SessionCookieKey))
                {
                    this.SetResponseSession(httpResponse, sessionId);
                }

                await this.PrepareResponse(httpResponse);
            }

            this.client.Shutdown(SocketShutdown.Both);
        }

        private async Task<IHttpRequest> ReadRequest()
        {
            StringBuilder result = new StringBuilder();
            ArraySegment<byte> data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int numberOfBytesRead = await this.client.ReceiveAsync(data.Array, SocketFlags.None);

                if (numberOfBytesRead == 0)
                {
                    break;
                }

                string bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);
                result.Append(bytesAsString);

                if (numberOfBytesRead < 1023)
                {
                    break;
                }
            }

            if (result.Length == 0)
            {
                return null;
            }

            return new HttpRequest(result.ToString());
        }

        private async Task PrepareResponse(IHttpResponse httpResponse)
        {
            byte[] byteSegments = httpResponse.GetBytes();

            await this.client.SendAsync(byteSegments, SocketFlags.None);
        }

        private string SetRequestSession(IHttpRequest httpRequest)
        {
            string sessionId = null;

            if (httpRequest.Cookies.ContainsCookie(HttpSessionStorage.SessionCookieKey))
            {
                HttpCookie cookie = httpRequest.Cookies.GetCookie(HttpSessionStorage.SessionCookieKey);
                sessionId = cookie.Value;
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }

            return sessionId;
        }

        private void SetResponseSession(IHttpResponse httpResponse, string sessionId)
        {
            if (sessionId != null)
            {
                httpResponse.AddCookie(new HttpCookie(HttpSessionStorage.SessionCookieKey, $"{sessionId}; HttpOnly"));
            }
        }

        private bool IsResourceRequest(IHttpRequest httpRequest)
        {
            return httpRequest.Path.Contains('.');
        }
    }
}
