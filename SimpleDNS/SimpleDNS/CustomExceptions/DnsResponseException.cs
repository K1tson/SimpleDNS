using System;

namespace Kitson.SimpleDNS.CustomExceptions
{
    public class DnsResponseException : Exception
    {
        public DnsResponseException(string message) : base(message)
        {}

        public DnsResponseException(string message, Exception innerException) : base(message, innerException)
        {}
    }
}
