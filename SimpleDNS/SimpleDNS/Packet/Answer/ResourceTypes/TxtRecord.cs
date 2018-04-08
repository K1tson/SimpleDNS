using System.Text;

namespace Kitson.SimpleDNS.Packet.Answer.ResourceTypes
{
    public class TxtRecord : Resource
    {
        public TxtRecord(IResource temp, ushort textLength, string data) : base(temp, data)
        {
            TextLength = textLength;
        }

        public ushort TextLength { get; }

        internal static IResource Parse(byte[] data, int position, IResource resource)
        {
            return new TxtRecord(resource, data[position], BuildTxtAnswer(ref data, position).ToString());
        }

        private static StringBuilder BuildTxtAnswer(ref byte[] data, int position)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Encoding.UTF8.GetString(data, position + 1, data[position])}");
            return sb;
        }
    }
}
