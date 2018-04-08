using System.Net;
using System.Text;

namespace Kitson.SimpleDNS.Packet.Answer.ResourceTypes
{
    public class ARecord : Resource
    {
        public ARecord(IResource temp, IPAddress ip) : base(temp, ip.ToString())
        {
            IP = ip;
        }

        public IPAddress IP { get; }


        /// <summary>
        /// Expects 4 bytes in array to convert to IPAddress
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static IPAddress BytesToIP(byte[] data)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 4; i++)
            {
                sb.Append(((int) data[i]).ToString());
                if (i != 3)
                    sb.Append(".");
            }

            return IPAddress.Parse(sb.ToString());
        }

        internal static IResource Parse(byte[] data, int position, IResource resource)
        {
            IPAddress aIP =
                BytesToIP(new[] {data[position], data[position + 1], data[position + 2], data[position + 3]});
            return new ARecord(resource, aIP);
        }
    }
}
