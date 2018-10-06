namespace IRunes.WebApp.Services
{
    using System.Security.Cryptography;
    using System.Text;

    public class HashService
    {
        public string HashPassword(string password)
        {
            var crypt = new SHA256Managed();
            string hash = string.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(password));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }
    }
}
