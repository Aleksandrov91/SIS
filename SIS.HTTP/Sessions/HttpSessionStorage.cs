namespace SIS.HTTP.Sessions
{
    using System.Collections.Concurrent;
    using SIS.HTTP.Sessions.Contracts;

    public class HttpSessionStorage
    {
        public const string SessionCookieKey = "SIS_ID";

        private static readonly ConcurrentDictionary<string, IHttpSession> Sessions = new ConcurrentDictionary<string, IHttpSession>();

        public static IHttpSession GetSession(string id) => Sessions.GetOrAdd(id, _ => new HttpSession(id));
    }
}
