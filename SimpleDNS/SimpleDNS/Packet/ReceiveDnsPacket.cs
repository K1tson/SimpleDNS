using System.Collections.Generic;
using Kitson.SimpleDNS.Packet.Answer;
using Kitson.SimpleDNS.Packet.Header;
using Kitson.SimpleDNS.Packet.Question;

namespace Kitson.SimpleDNS.Packet
{
    public class ReceiveDnsPacket : DnsPacket
    {
        public ReceiveDnsPacket(IDnsHeader header, IEnumerable<IQuestion> questions, IEnumerable<IResource> resource) : base(header, questions, resource)
        {
        }
    }
}
