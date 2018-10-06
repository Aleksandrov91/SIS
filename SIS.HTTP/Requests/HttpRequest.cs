namespace SIS.HTTP.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SIS.HTTP.Common;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Cookies.Contracts;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Exceptions;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Headers.Contracts;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Sessions;
    using SIS.HTTP.Sessions.Contracts;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        // TODO: Try to remove this.
        public IHttpSession Session { get; set; }

        private void ParseRequest(string requestString)
        {
            if (string.IsNullOrEmpty(requestString))
            {
                throw new BadRequestException();
            }

            string[] splitRequestContent = requestString.Split(Environment.NewLine);

            string[] requestLine = splitRequestContent[0].Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            string[] headers = splitRequestContent.Skip(1).ToArray();
            string requestParameters = splitRequestContent.LastOrDefault();

            this.ParseHeaders(headers);
            this.ParseCookies();
            this.ParseRequestParameters(requestParameters);
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            string requestMethod = requestLine.FirstOrDefault();

            bool isParsed = Enum.TryParse<HttpRequestMethod>(requestMethod, true, out HttpRequestMethod parsedMethod);

            if (!isParsed)
            {
                throw new BadRequestException();
            }

            this.RequestMethod = parsedMethod;
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            string requestLineUrl = requestLine
                .Skip(1)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(requestLineUrl))
            {
                throw new BadRequestException();
            }

            this.Url = requestLineUrl;
        }

        private void ParseRequestPath()
        {
            string requestPath = this.Url
                .Split(GlobalConstants.QueryParamsSeparator)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(requestPath))
            {
                throw new BadRequestException();
            }

            this.Path = requestPath;
        }

        private void ParseHeaders(string[] headers)
        {
            if (!headers.Any())
            {
                throw new BadRequestException();
            }

            bool containsHostHeader = false;

            foreach (string kvp in headers)
            {
                if (string.IsNullOrEmpty(kvp))
                {
                    break;
                }

                string[] headerParams = kvp
                    .Split(": ", StringSplitOptions.RemoveEmptyEntries);

                string headerKey = headerParams[0];
                string headerValue = headerParams[1];

                if (headerKey == GlobalConstants.HostHeaderKey)
                {
                    containsHostHeader = true;
                }

                this.Headers.Add(new HttpHeader(headerKey, headerValue));
            }

            if (!containsHostHeader)
            {
                throw new BadRequestException();
            }
        }

        private void ParseCookies()
        {
            if (!this.Headers.ContainsHeader(GlobalConstants.CookieHeaderKey))
            {
                return;
            }

            string[] cookieRow = this.Headers.GetHeader(GlobalConstants.CookieHeaderKey).Value
                .Split(GlobalConstants.CookieDelimiter, StringSplitOptions.RemoveEmptyEntries);

            foreach (string cookie in cookieRow)
            {
                string[] cookieKvps = cookie.Split(GlobalConstants.KeyValuePairDelimiter, 2);

                string cookieKey = cookieKvps.FirstOrDefault();
                string cookieValue = cookieKvps.LastOrDefault();

                this.Cookies.Add(new HttpCookie(cookieKey, cookieValue));
            }
        }

        private void ParseRequestParameters(string bodyParameters)
        {
            this.ParseQueryParameters();
            this.ParseFormDataParameters(bodyParameters);
        }

        private void ParseQueryParameters()
        {
            string queryParams = this.Url.Split(GlobalConstants.QueryParamsSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .FirstOrDefault();

            this.ExtractRequestParameters(queryParams, this.QueryData);
        }

        private void ParseFormDataParameters(string bodyParameters)
        {
            this.ExtractRequestParameters(bodyParameters, this.FormData);
        }

        private void ExtractRequestParameters(string paramsString, Dictionary<string, object> data)
        {
            // this.IsValidrequestQueryString(paramsString);
            if (string.IsNullOrEmpty(paramsString))
            {
                return;
            }

            paramsString = HttpUtility.UrlDecode(paramsString);

            string[] parameters = paramsString.Split(GlobalConstants.QueryParamsDelimiter, StringSplitOptions.RemoveEmptyEntries);

            foreach (string param in parameters)
            {
                string[] queryParam = param.Split(GlobalConstants.KeyValuePairDelimiter, StringSplitOptions.RemoveEmptyEntries);

                if (queryParam.Length != 2)
                {
                    throw new BadRequestException();
                }

                string queryKey = queryParam.FirstOrDefault();
                string queryValue = queryParam.LastOrDefault();

                // Should we ovveride values?
                data[queryKey] = queryValue;
            }
        }

        private bool IsValidRequestLine(string[] requestLine)
        {
            return requestLine.Length == 3 && requestLine[2].Contains(GlobalConstants.HttpOneProtocolFragment);
        }

        // TODO: Remvoe
        private bool IsValidrequestQueryString(string queryString) => !string.IsNullOrEmpty(queryString) && queryString.Split(GlobalConstants.QueryParamsDelimiter).Length > 0;
    }
}
