using System;
using System.Collections.Generic;
using System.Text;

namespace Kitson.SimpleDNS.Packet.Question
{
    public sealed class Question : IQuestion
    {
        public Question(string qName, QType qType, QClass qClass = QClass.IN)
        {
            if(!IsValidDomainName(qName))
                throw new FormatException($"The domain name {qName} entered for QName is invalid!");

            QName = qName;
            QType = qType;
            QClass = qClass;
        }

        public string QName { get; }
        public QType QType { get; }
        public QClass QClass { get; }

        public byte[] ToBytes()
        {
            List<byte> buffer = new List<byte>();

            //QuestionName
            buffer.AddRange(FormatQName(QName));

            //Adds Question Type to buffer
            buffer.AddRange(new byte[]{ 0x00, Convert.ToByte(QType) });

            //Adds Question Class to buffer
            buffer.AddRange(new byte[] { 0x00, Convert.ToByte(QClass) });

            return buffer.ToArray();
        }

        private static IEnumerable<byte> FormatQName(string qName)
        {
            List<byte> buffer = new List<byte>();
            var domArr = qName.Split('.');

            foreach (var seg in domArr)
            {
                buffer.Add(Convert.ToByte(seg.Length)); 
                buffer.AddRange(Encoding.ASCII.GetBytes(seg));
            }

            buffer.Add(0x00); //Terminator for message
            return buffer.ToArray();
        }

        private static bool IsValidDomainName(string name)
        {
            return Uri.CheckHostName(name) != UriHostNameType.Unknown;
        }
    }
}
