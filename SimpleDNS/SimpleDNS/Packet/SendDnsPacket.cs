using Kitson.SimpleDNS.Packet.Header;
using Kitson.SimpleDNS.Packet.Question;

namespace Kitson.SimpleDNS.Packet
{
    public class SendDnsPacket : DnsPacket
    {
        /// <inheritdoc />
        /// <summary>
        /// Construction of a Send DNS Packet
        /// </summary>
        /// <param name="header"></param>
        /// <param name="question"></param>
        public SendDnsPacket(IDnsHeader header, IQuestion question) : base(header, new []{question})
        {}



       
    }
}
