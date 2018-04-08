using System.Collections.Generic;
using Kitson.SimpleDNS.Packet.Answer;
using Kitson.SimpleDNS.Packet.Header;
using Kitson.SimpleDNS.Packet.Question;

namespace Kitson.SimpleDNS.Packet
{
    public interface IDnsPacket
    {
         IDnsHeader Header { get; }
         IEnumerable<IQuestion> Questions { get; }
         IEnumerable<IResource> Resources { get; }
    }
}
