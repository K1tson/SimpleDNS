using System;

namespace Kitson.SimpleDNS.Packet.Question
{
    public enum QType : Int16
    {
        A = 1, // a host address
        NS, // an authoritative name server
        //MD - a mail destination **(Obsolete - use MX)**
        //MF - // a mail forwarder **(Obsolete - use MX)**
        CNAME = 5, // the canonical name for an alias
        SOA, // marks the start of a zone of authority
        //MB - a mailbox domain name (EXPERIMENTAL)
        //MG - a mail group member (EXPERIMENTAL)
        //MR - a mail rename domain name (EXPERIMENTAL)
        //NULL - a null RR (EXPERIMENTAL)
        //WKS - a well known service description
        PTR = 12, // a domain name pointer
        //HINFO - host information
        //MINFO - mailbox or mail list information
        MX = 15, // mail exchange
        TXT // text strings
    }
}
