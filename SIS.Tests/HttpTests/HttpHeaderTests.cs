namespace SIS.Tests.HttpTests
{
    using SIS.HTTP.Headers;
    using Xunit;

    public class HttpHeaderTests
    {
        [Fact]
        public void CreateValidHeaderShouldCreateit()
        {
            HttpHeader httpHeader = new HttpHeader("name", "pesho");

            Assert.Equal("name: pesho", httpHeader.ToString());
        }
    }
}
