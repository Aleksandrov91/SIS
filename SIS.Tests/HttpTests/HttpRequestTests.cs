using SIS.HTTP.Exceptions;
using SIS.HTTP.Requests;
using System.Text;
using Xunit;

namespace SIS.Tests.HttpTests
{
    public class HttpRequestTests
    {
        [Fact]
        public void RequestWithoudRequestStringShouldThrowException()
        {
            Assert.Throws<BadRequestException>(() => new HttpRequest(string.Empty));
        }

        [Fact]
        public void RequestWithInvalidRequestMethodShouldThrowException()
        {
            StringBuilder requestBuilder = new StringBuilder();

            requestBuilder.AppendLine("GETA / HTTP/1.1");
            requestBuilder.AppendLine("Host: localhost");
            requestBuilder.AppendLine("Connection: keep-alive");
            requestBuilder.AppendLine("Purpose: prefetch");
            requestBuilder.AppendLine("Upgrade-Insecure-Requests: 1");
            requestBuilder.AppendLine("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");
            requestBuilder.AppendLine("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");                         
            requestBuilder.AppendLine("Accept-Encoding: gzip, deflate, br");
            requestBuilder.AppendLine("Accept-Language: bg-BG,bg;q=0.9,en;q=0.8");
            requestBuilder.AppendLine();

            Assert.Throws<BadRequestException>(() => new HttpRequest(requestBuilder.ToString()));
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("POST")]
        [InlineData("PUT")]
        [InlineData("DELETE")]
        public void RequestWithValidRequestMethodMethod(string requestMethod)
        {
            StringBuilder requestString = new StringBuilder();
            requestString.AppendLine($"{requestMethod} /users?key=value&pesho=25 HTTP/1.1");
            requestString.AppendLine("Host: localhost");
            requestString.AppendLine("Connection: keep-alive");
            requestString.AppendLine();

            HttpRequest request = new HttpRequest(requestString.ToString());

            Assert.Equal(requestMethod.ToLower(), request.RequestMethod.ToString().ToLower());
        }

        [Fact]
        public void RequestWithInvalidHttpVersionShouldThrowException()
        {
            StringBuilder requestBuilder = new StringBuilder();

            requestBuilder.AppendLine("GETA / HTTP/1.4");
            requestBuilder.AppendLine("Host: localhost");
            requestBuilder.AppendLine("Connection: keep-alive");
            requestBuilder.AppendLine("Purpose: prefetch");
            requestBuilder.AppendLine("Upgrade-Insecure-Requests: 1");
            requestBuilder.AppendLine("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");
            requestBuilder.AppendLine("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            requestBuilder.AppendLine("Accept-Encoding: gzip, deflate, br");
            requestBuilder.AppendLine("Accept-Language: bg-BG,bg;q=0.9,en;q=0.8");
            requestBuilder.AppendLine();

            Assert.Throws<BadRequestException>(() => new HttpRequest(requestBuilder.ToString()));
        }

        [Fact]
        public void RequestWithoutHttpVersionShouldThrowException()
        {
            StringBuilder requestBuilder = new StringBuilder();

            requestBuilder.AppendLine("GETA /");
            requestBuilder.AppendLine("Host: localhost");
            requestBuilder.AppendLine("Connection: keep-alive");
            requestBuilder.AppendLine("Purpose: prefetch");
            requestBuilder.AppendLine("Upgrade-Insecure-Requests: 1");
            requestBuilder.AppendLine("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");
            requestBuilder.AppendLine("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            requestBuilder.AppendLine("Accept-Encoding: gzip, deflate, br");
            requestBuilder.AppendLine("Accept-Language: bg-BG,bg;q=0.9,en;q=0.8");
            requestBuilder.AppendLine();

            Assert.Throws<BadRequestException>(() => new HttpRequest(requestBuilder.ToString()));
        }

        [Fact]
        public void RequestWithInvalidParametersShouldThrowException()
        {
            StringBuilder requestBuilder = new StringBuilder();

            requestBuilder.AppendLine("GET HTTP/1.1");
            requestBuilder.AppendLine("Host: localhost");
            requestBuilder.AppendLine("Connection: keep-alive");
            requestBuilder.AppendLine("Purpose: prefetch");
            requestBuilder.AppendLine("Upgrade-Insecure-Requests: 1");
            requestBuilder.AppendLine("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");
            requestBuilder.AppendLine("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            requestBuilder.AppendLine("Accept-Encoding: gzip, deflate, br");
            requestBuilder.AppendLine("Accept-Language: bg-BG,bg;q=0.9,en;q=0.8");
            requestBuilder.AppendLine();

            Assert.Throws<BadRequestException>(() => new HttpRequest(requestBuilder.ToString()));
        }

        [Fact]
        public void RequestWithoutRequestMethodShouldThrowException()
        {
            StringBuilder requestBuilder = new StringBuilder();

            requestBuilder.AppendLine("/ HTTP/1.1");
            requestBuilder.AppendLine("Host: localhost");
            requestBuilder.AppendLine("Connection: keep-alive");
            requestBuilder.AppendLine("Purpose: prefetch");
            requestBuilder.AppendLine("Upgrade-Insecure-Requests: 1");
            requestBuilder.AppendLine("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");
            requestBuilder.AppendLine("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            requestBuilder.AppendLine("Accept-Encoding: gzip, deflate, br");
            requestBuilder.AppendLine("Accept-Language: bg-BG,bg;q=0.9,en;q=0.8");
            requestBuilder.AppendLine();

            Assert.Throws<BadRequestException>(() => new HttpRequest(requestBuilder.ToString()));
        }

        [Fact]
        public void RequestWithoutRequestLineShouldThrowException()
        {
            StringBuilder requestBuilder = new StringBuilder();

            requestBuilder.AppendLine("Host: localhost");
            requestBuilder.AppendLine("Connection: keep-alive");
            requestBuilder.AppendLine("Purpose: prefetch");
            requestBuilder.AppendLine("Upgrade-Insecure-Requests: 1");
            requestBuilder.AppendLine("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");
            requestBuilder.AppendLine("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            requestBuilder.AppendLine("Accept-Encoding: gzip, deflate, br");
            requestBuilder.AppendLine("Accept-Language: bg-BG,bg;q=0.9,en;q=0.8");
            requestBuilder.AppendLine();

            Assert.Throws<BadRequestException>(() => new HttpRequest(requestBuilder.ToString()));
        }

        [Fact]
        public void RequestWithoutUrlShouldThrowException()
        {
            StringBuilder requestBuilder = new StringBuilder();

            requestBuilder.AppendLine("GET  HTTP/1.1");
            requestBuilder.AppendLine("Host: localhost");
            requestBuilder.AppendLine("Connection: keep-alive");
            requestBuilder.AppendLine("Purpose: prefetch");
            requestBuilder.AppendLine("Upgrade-Insecure-Requests: 1");
            requestBuilder.AppendLine("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");
            requestBuilder.AppendLine("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            requestBuilder.AppendLine("Accept-Encoding: gzip, deflate, br");
            requestBuilder.AppendLine("Accept-Language: bg-BG,bg;q=0.9,en;q=0.8");
            requestBuilder.AppendLine();

            Assert.Throws<BadRequestException>(() => new HttpRequest(requestBuilder.ToString()));
        }

        [Fact]
        public void RequestWithUrlShouldParse()
        {
            StringBuilder requestString = new StringBuilder();
            requestString.AppendLine("GET /users?key=value&pesho=25 HTTP/1.1");
            requestString.AppendLine("Host: localhost");
            requestString.AppendLine("Connection: keep-alive");
            requestString.AppendLine();

            HttpRequest request = new HttpRequest(requestString.ToString());

            Assert.Equal("/users?key=value&pesho=25", request.Url);
        }

        [Fact]
        public void RequestWithoutHeadersShouldThrowException()
        {
            string requestString = "GET / HTTP/1.1";

            Assert.Throws<BadRequestException>(() => new HttpRequest(requestString));
        }

        [Fact]
        public void RequestWithoutHostHeaderShouldThrowException()
        {
            StringBuilder requestBuilder = new StringBuilder();

            requestBuilder.AppendLine("GET / HTTP/1.1");
            requestBuilder.AppendLine("Connection: keep-alive");
            requestBuilder.AppendLine("Purpose: prefetch");
            requestBuilder.AppendLine("Accept-Encoding: gzip, deflate, br");
            requestBuilder.AppendLine();

            Assert.Throws<BadRequestException>(() => new HttpRequest(requestBuilder.ToString()));
        }

        [Fact]
        public void RequestWithoutQueryParametersShouldReturnEmptyCollection()
        {
            StringBuilder requestBuilder = new StringBuilder();

            requestBuilder.AppendLine("GET /users? HTTP/1.1");
            requestBuilder.AppendLine("Host: localhost");
            requestBuilder.AppendLine("Connection: keep-alive");
            requestBuilder.AppendLine();

            HttpRequest request = new HttpRequest(requestBuilder.ToString());

            Assert.Empty(request.QueryData);
        }

        [Fact]
        public void RequestWithOneParameterShouldNotReturnEmptyCollection()
        {
            StringBuilder requestBuilder = new StringBuilder();

            requestBuilder.AppendLine("GET /users?key=value HTTP/1.1");
            requestBuilder.AppendLine("Host: localhost");
            requestBuilder.AppendLine("Connection: keep-alive");
            requestBuilder.AppendLine();

            HttpRequest request = new HttpRequest(requestBuilder.ToString());

            Assert.NotEmpty(request.QueryData);
        }

        [Theory]
        [InlineData("key=value&key2=value2")]
        [InlineData("name=pesho&role=user&age=26")]
        public void RequestWithFewParametersShouldNotReturnEmptyCollection(string kvp)
        {
            int expectedLength = kvp.Split('&').Length;

            StringBuilder requestBuilder = new StringBuilder();

            requestBuilder.AppendLine($"GET /users?{kvp} HTTP/1.1");
            requestBuilder.AppendLine("Host: localhost");
            requestBuilder.AppendLine("Connection: keep-alive");
            requestBuilder.AppendLine();

            HttpRequest request = new HttpRequest(requestBuilder.ToString());

            Assert.Equal(expectedLength, request.QueryData.Count);
        }

        [Fact]
        public void RequestWithDuplicateParameterShouldAddItTwice()
        {
            StringBuilder requestBuilder = new StringBuilder();

            requestBuilder.AppendLine("GET /users?name=pesho&name=pesho HTTP/1.1");
            requestBuilder.AppendLine("Host: localhost");
            requestBuilder.AppendLine("Connection: keep-alive");
            requestBuilder.AppendLine();

            HttpRequest request = new HttpRequest(requestBuilder.ToString());

            Assert.Equal(2, request.QueryData.Count);
        }

        [Fact]
        public void RequestWithInvalidQueryParameterShouldThrowException()
        {
            StringBuilder requestString = new StringBuilder();

            requestString.AppendLine("GET /users?= HTTP/1.1");
            requestString.AppendLine("Host: localhost");
            requestString.AppendLine("Connection: keep-alive");
            requestString.AppendLine();

            Assert.Throws<BadRequestException>(() => new HttpRequest(requestString.ToString()));
        }

        [Fact]
        public void RequestWithQueryParameterSeparatorOnlyShouldNoAction()
        {
            StringBuilder requestBuilder = new StringBuilder();

            requestBuilder.AppendLine("GET /users?& HTTP/1.1");
            requestBuilder.AppendLine("Host: localhost");
            requestBuilder.AppendLine("Connection: keep-alive");
            requestBuilder.AppendLine();

            HttpRequest request = new HttpRequest(requestBuilder.ToString());

            Assert.Empty(request.QueryData);
        }

        [Fact]
        public void RequestWithInvalidQueryValueParameter()
        {
            StringBuilder requestString = new StringBuilder();
            requestString.AppendLine("GET /users?key= HTTP/1.1");
            requestString.AppendLine("Host: localhost");
            requestString.AppendLine("Connection: keep-alive");
            requestString.AppendLine();

            Assert.Throws<BadRequestException>(() => new HttpRequest(requestString.ToString()));
        }

        [Fact]
        public void RequestWithInvalidQueryValueParameters()
        {
            StringBuilder requestString = new StringBuilder();
            requestString.AppendLine("GET /users?key= HTTP/1.1");
            requestString.AppendLine("Host: localhost");
            requestString.AppendLine("Connection: keep-alive");
            requestString.AppendLine();

            Assert.Throws<BadRequestException>(() => new HttpRequest(requestString.ToString()));
        }

        [Fact]
        public void RequestWithInvalidQueryKeyAndValue()
        {
            StringBuilder requestString = new StringBuilder();
            requestString.AppendLine("GET /users?key=&=value HTTP/1.1");
            requestString.AppendLine("Host: localhost");
            requestString.AppendLine("Connection: keep-alive");
            requestString.AppendLine();

            Assert.Throws<BadRequestException>(() => new HttpRequest(requestString.ToString()));
        }

        [Fact]
        public void RequestWithInvalidQueryKey()
        {
            StringBuilder requestString = new StringBuilder();
            requestString.AppendLine("GET /users?key=val&=value HTTP/1.1");
            requestString.AppendLine("Host: localhost");
            requestString.AppendLine("Connection: keep-alive");
            requestString.AppendLine();

            Assert.Throws<BadRequestException>(() => new HttpRequest(requestString.ToString()));
        }

        [Fact]
        public void RequestWithEmptyQueryParametersShouldReturnNull()
        {
            StringBuilder requestBuilder = new StringBuilder();

            requestBuilder.AppendLine("GET /users? HTTP/1.1");
            requestBuilder.AppendLine("Host: localhost");
            requestBuilder.AppendLine("Connection: keep-alive");
            requestBuilder.AppendLine("Purpose: prefetch");
            requestBuilder.AppendLine("Upgrade-Insecure-Requests: 1");
            requestBuilder.AppendLine("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");
            requestBuilder.AppendLine("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            requestBuilder.AppendLine("Accept-Encoding: gzip, deflate, br");
            requestBuilder.AppendLine("Accept-Language: bg-BG,bg;q=0.9,en;q=0.8");
            requestBuilder.AppendLine();

            HttpRequest request = new HttpRequest(requestBuilder.ToString());

            Assert.Empty(request.QueryData);
        }



        [Fact]
        public void RequestWithValidPath()
        {
            StringBuilder requestString = new StringBuilder();
            requestString.AppendLine("GET /users?key=value&pesho=25 HTTP/1.1");
            requestString.AppendLine("Host: localhost");
            requestString.AppendLine("Connection: keep-alive");
            requestString.AppendLine();

            HttpRequest request = new HttpRequest(requestString.ToString());

            Assert.Equal("/users", request.Path);
        }
    }
}
