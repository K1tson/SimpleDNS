using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kitson.SimpleDNS.Packet.Answer.ResourceTypes;
using Kitson.SimpleDNS.Packet.Question;

namespace Kitson.SimpleDNS.Packet.Answer
{
    public class Resource : IResource
    {
        protected Resource(IResource temp, string data) : this(temp.Name, temp.Type, temp.Class, temp.Ttl, temp.Length, data)
        {}

        protected Resource(string name, QType type, QClass qClass, uint ttl, ushort length, string data)
        {
            Name = name;
            Type = type;
            Class = qClass;
            Ttl = ttl;
            Length = length;
            Data = data;
        }

        public string Name { get; }
        public QType Type { get; }
        public QClass Class { get; }
        public uint Ttl { get; }
        public ushort Length { get; }
        public string Data { get; }

        /// <summary>
        /// Returns the Resource object as Bytes
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            List<byte> result = new List<byte>();

            result.AddRange(Encoding.UTF8.GetBytes(Name));
            result.AddRange(BitConverter.GetBytes((ushort)Type).Reverse());
            result.AddRange(BitConverter.GetBytes((ushort)Class).Reverse());
            result.AddRange(BitConverter.GetBytes(Ttl).Reverse());
            result.AddRange(BitConverter.GetBytes(Length).Reverse());
            result.AddRange(Encoding.UTF8.GetBytes(Data));

            return result.ToArray();
        }

        /// <summary>
        /// Will parse resource data (DNS Answers) into IResource. The concrete type will be the answer type eg. MX = MxRecord Type
        /// </summary>
        /// <param name="data"></param>
        /// <param name="receiveDnsPacket"></param>
        /// <returns></returns>
        public static IEnumerable<IResource> Parse(byte[] data, ReceiveDnsPacket receiveDnsPacket)
        {
            IResource[] resources = new IResource[receiveDnsPacket.Header.AnswersCounts];
            int position = receiveDnsPacket.ToBytes().Length;

            for (var i = 0; i < receiveDnsPacket.Header.AnswersCounts; i++)
            {
                var walkedHostname = GetRecordName(ref data, position);

                string name = walkedHostname.hostname.ToString();
                QType type = (QType)Enum.Parse(typeof(QType), ((int)data[walkedHostname.pos + 1]).ToString());
                QClass qClass = (QClass)Enum.Parse(typeof(QClass),  ((int)data[walkedHostname.pos + 3]).ToString());
                uint ttl = BitConverter.ToUInt16(new[]{ data[walkedHostname.pos + 5], data[walkedHostname.pos + 6], data[walkedHostname.pos + 7], data[walkedHostname.pos + 8]}, 0);
                ushort length = data[walkedHostname.pos + 9];
                var resourceData = ReadAnswer(ref data, walkedHostname.pos + 10, new Resource(name, type, qClass, ttl, length, null));
 
                position = walkedHostname.pos + 10 + length;
                resources[i] = resourceData;
            } 

            return resources;
        }

        private static (StringBuilder hostname, int pos) GetRecordName(ref byte[] data, int position)
        {
            return WalkBytesForHostname(ref data, position);
        }

        protected static (StringBuilder sb, int pos) WalkBytesForHostname(ref byte[] data, int startPostion)
        {
            StringBuilder sb = new StringBuilder();
            bool hostnameComplete = false;
            bool wasRef = false;
            int position = startPostion;

            while (!hostnameComplete)
            {
                //Check if its a Reference or Label
                if (IsPointer(data[position]))
                {
                    int refPosition = data[position + 1];

                    //Pass reference pointer to read method
                    ReadReference(ref data, refPosition, sb);
                    wasRef = true;

                    //Incase its the last record
                    if(data.Length > position + 2)
                        position += 2;
                }
                else //Is Label
                {
                    sb.Append($"{Encoding.UTF8.GetString(data, position + 1, data[position])}.");

                    //Incase its the last record
                    if (data.Length > position + data[position] + 1)
                        position += data[position] + 1;
                }

                if (data[position] == 0x00 || wasRef)
                {
                    hostnameComplete = true;

                     if(sb[sb.Length - 1] == '.')
                        sb.Replace('.', ' ', sb.Length - 1, 1);
                }
                    
               
            }

            return (sb, position);
        }


        private static StringBuilder ReadReference(ref byte[] data, int start, StringBuilder sb)
        {
            int position = start;
            bool walking = true;

            while (walking)
            {
                sb.Append($"{Encoding.UTF8.GetString(data, position + 1, data[position])}");
                var temp = position + data[position] + 1;

                switch (data[temp])
                {
                    case 0xc0:
                        sb.Append(".");
                        position += data[position] + 1;
                        return ReadReference(ref data, data[position + 1], sb);
                    case 0x00:
                        walking = false;
                        break;
                    default:
                        sb.Append(".");
                        position += data[position] + 1;
                        break;
                }
            }

            return sb;
        }

        /// <summary>
        /// Acts as a Factory which reads QType from Immutable IResource param to return concrete type which implements IResource.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="position"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        private static IResource ReadAnswer(ref byte[] data, int position, IResource resource)
        {
            switch (resource.Type)
            {
                case QType.TXT:
                    return TxtRecord.Parse(data, position, resource);
                case QType.MX:
                    return MxRecord.Parse(data, position, resource);
                case QType.A:
                    return ARecord.Parse(data, position, resource);
                case QType.SOA:
                    return SoaRecord.Parse(data, position, resource);
                case QType.NS:
                    return NsRecord.Parse(data, position, resource);
                case QType.CNAME:
                    return CNameRecord.Parse(data, position, resource);
            }

            return null; //TODO: Thrown Exception Here
        }

        /// <summary>
        /// Checks if the given Byte is a Pointer or Label
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsPointer(byte data)
        {
            BitArray bits = new BitArray(new []{data});
            return bits[7] && bits[6];
        }

      
    }
}
