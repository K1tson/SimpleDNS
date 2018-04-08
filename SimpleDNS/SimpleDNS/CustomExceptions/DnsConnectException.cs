using System;

namespace Kitson.SimpleDNS.CustomExceptions
{
    public class DnsConnectException : Exception
    {
        public DnsConnectException(string message) : base(message)
        {}

        public DnsConnectException(string message, Exception inner) : base(message, inner)
        {}
    }
}
