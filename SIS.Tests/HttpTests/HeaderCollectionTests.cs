namespace SIS.Tests.HttpTests
{
    using SIS.HTTP.Exceptions;
    using SIS.HTTP.Headers;
    using Xunit;

    public class HeaderCollectionTests
    {
        [Fact]
        public void AddHeaderWithEmptyKeyShouldThrowException()
        {
            HttpHeader httpHeader = new HttpHeader(string.Empty, "pesho");

            HttpHeaderCollection headerCollection = new HttpHeaderCollection();

            Assert.Throws<BadRequestException>(() => headerCollection.Add(httpHeader));
        }

        [Fact]
        public void AddHeaderWithEmptyValueShouldThrowException()
        {
            HttpHeader httpHeader = new HttpHeader("name", string.Empty);

            HttpHeaderCollection headerCollection = new HttpHeaderCollection();

            Assert.Throws<BadRequestException>(() => headerCollection.Add(httpHeader));
        }

        [Fact]
        public void GetContainedHeaderShouldReturnTrue()
        {
            HttpHeader httpHeader = new HttpHeader("name", "pesho");

            HttpHeaderCollection headerCollection = new HttpHeaderCollection();
            headerCollection.Add(httpHeader);

            Assert.True(headerCollection.ContainsHeader(httpHeader.Key));
        }

        [Fact]
        public void GetNotContainedHeaderShouldReturnFalse()
        {
            HttpHeaderCollection headerCollection = new HttpHeaderCollection();

            Assert.False(headerCollection.ContainsHeader("name"));
        }

        [Fact]
        public void ContainsHeaderWithoutKeyShouldThrowException()
        {
            HttpHeader httpHeader = new HttpHeader("name", "pesho");

            HttpHeaderCollection headerCollection = new HttpHeaderCollection();
            headerCollection.Add(httpHeader);

            Assert.Throws<BadRequestException>(() => headerCollection.ContainsHeader(string.Empty));
        }

        [Fact]
        public void TryGetHeaderWithoutKeyShouldThrowException()
        {
            HttpHeader httpHeader = new HttpHeader("name", "pesho");

            HttpHeaderCollection headerCollection = new HttpHeaderCollection();
            headerCollection.Add(httpHeader);

            Assert.Throws<BadRequestException>(() => headerCollection.GetHeader(string.Empty));
        }

        [Fact]
        public void TryGetValidHeaderShouldReturnIt()
        {
            HttpHeader httpHeader = new HttpHeader("name", "pesho");

            HttpHeaderCollection headerCollection = new HttpHeaderCollection();
            headerCollection.Add(httpHeader);

            Assert.Equal<HttpHeader>(httpHeader, headerCollection.GetHeader(httpHeader.Key));
        }

        [Fact]
        public void TryGetInvalidHeaderShouldReturnNull()
        {
            HttpHeader httpHeader = new HttpHeader("name", "pesho");

            HttpHeaderCollection headerCollection = new HttpHeaderCollection();
            headerCollection.Add(httpHeader);

            Assert.Null(headerCollection.GetHeader("age"));
        }
    }
}
