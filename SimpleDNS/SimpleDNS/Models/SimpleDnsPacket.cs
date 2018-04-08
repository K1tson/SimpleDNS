using System.Net;
using Kitson.SimpleDNS.Packet.Question;
using Kitson.SimpleDNS.Transmission;

namespace Kitson.SimpleDNS.Models
{
    public class SimpleDnsPacket
    {
        public SimpleDnsPacket(Question question, IPAddress serverIpAddress, TransmissionType type = TransmissionType.UDP)
        {
            Query = question;
            ServerIPAddress = serverIpAddress;
            Type = type;
        }

        public Question Query { get; }
        public IPAddress ServerIPAddress { get; set; }
        public TransmissionType Type { get; set; }
        
    }
}
