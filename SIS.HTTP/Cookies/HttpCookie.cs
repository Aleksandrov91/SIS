namespace SIS.HTTP.Cookies
{
    using System;
    using SIS.HTTP.Common;

    public class HttpCookie
    {
        private const int HttpCookieDefaultExpirationInDays = 3;

        public HttpCookie(string key, string value, int expires = HttpCookieDefaultExpirationInDays)
        {
            this.Key = key;
            this.Value = value;
            this.Expires = DateTime.UtcNow.AddDays(expires);
            this.IsNew = true;
        }

        public HttpCookie(string key, string value, bool isNew, int expires = HttpCookieDefaultExpirationInDays)
            : this(key, value, expires)
        {
            this.IsNew = isNew;
        }

        public string Key { get; }

        public string Value { get; }

        public DateTime Expires { get; }

        public bool IsNew { get; }

        public override string ToString() => $"{this.Key}{GlobalConstants.KeyValuePairDelimiter}{this.Value}{GlobalConstants.CookieDelimiter}{nameof(this.Expires)}{GlobalConstants.KeyValuePairDelimiter}{this.Expires.ToString("r")}";
    }
}
