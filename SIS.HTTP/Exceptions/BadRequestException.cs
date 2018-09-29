namespace SIS.HTTP.Exceptions
{
    using System;

    public class BadRequestException : Exception
    {
        private const string ErrorMessage = "The Request was malformed or contains unsupported elements.";

        public BadRequestException()
            : base(ErrorMessage)
        {
        }
    }
}
