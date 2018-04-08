using System.Collections.Generic;
using System.Linq;
using Kitson.SimpleDNS.Packet.Answer;
using Kitson.SimpleDNS.Packet.Flags;
using Kitson.SimpleDNS.Packet.Header;
using Kitson.SimpleDNS.Packet.Question;

namespace Kitson.SimpleDNS.Packet
{
    public abstract class DnsPacket : IDnsPacket
    {
        /// <summary>
        /// Construction of a Send DNS Packet
        /// </summary>
        /// <param name="header"></param>
        /// <param name="questions"></param>
        protected DnsPacket(IDnsHeader header, IEnumerable<IQuestion> questions) : this(header, questions, null)
        {}

        /// <summary>
        /// Construction of a Receive DNS Packet
        /// </summary>
        /// <param name="header"></param>
        /// <param name="questions"></param>
        /// <param name="resource"></param>
        protected DnsPacket(IDnsHeader header, IEnumerable<IQuestion> questions, IEnumerable<IResource> resource)
        {
            Header = header;
            Questions = questions;
            Resources = resource;
        }

        public IDnsHeader Header { get; }
        public IEnumerable<IQuestion> Questions { get; }
        public IEnumerable<IResource> Resources { get; }

        /// <summary>
        /// Converts Dns Packet to Bytes
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            List<byte> result = new List<byte>();

            result.AddRange(Header.ToBytes());
            Questions.ToList().ForEach(n => result.AddRange(n.ToBytes()));
            Resources?.ToList().ForEach(n => result.AddRange(n.ToBytes()));

            return result.ToArray();
        }


        /// <summary>
        /// Parses Bytes from DNS to IDnsPacket Object
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sendDnsPacket"></param>
        /// <returns></returns>
        public static IDnsPacket Parse(byte[] data, SendDnsPacket sendDnsPacket)
        {
            IDnsHeader header = DnsHeader.Parse(data.Take(12).ToArray()); //Take First 12x Bytes which should always equal to DNS header

            if(header.Parameters.Response != ResponseCode.Ok) //If other response to OK, then return.
                return new ReceiveDnsPacket(header, sendDnsPacket.Questions, null);

            int byteCount = 12; //Amount of bytes before answer (Start with header)
            sendDnsPacket.Questions.ToList().ForEach(n => byteCount += n.ToBytes().Length); //Question Byte Count
            byte[] answerArr = data.TakeLast(data.Length - byteCount).ToArray(); //Extracts answers from data

            IEnumerable<IResource> answers = Resource.Parse(data, new ReceiveDnsPacket(header, sendDnsPacket.Questions, null));
  
            return new ReceiveDnsPacket(header, sendDnsPacket.Questions, answers);
        }
    }
}
