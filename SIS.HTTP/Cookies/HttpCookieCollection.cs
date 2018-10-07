namespace SIS.HTTP.Cookies
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SIS.HTTP.Common;
    using SIS.HTTP.Cookies.Contracts;

    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly IDictionary<string, HttpCookie> cookies;

        public HttpCookieCollection()
        {
            this.cookies = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie cookie)
        {
            this.cookies[cookie.Key] = cookie;
        }

        public HttpCookie GetCookie(string key)
        {
            if (!this.ContainsCookie(key))
            {
                return null;
            }

            return this.cookies[key];
        }

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            return this.cookies.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool ContainsCookie(string key) => this.cookies.ContainsKey(key);

        public bool HasCookies() => this.cookies.Any();

        public override string ToString() => string.Join(GlobalConstants.CookieDelimiter, this.cookies.Values);
    }
}
