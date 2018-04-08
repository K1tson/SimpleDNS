namespace Kitson.SimpleDNS.Packet.Answer.ResourceTypes
{
    public class CNameRecord : Resource
    {
        protected CNameRecord(IResource temp, string data) : base(temp, data)
        {
        }

        internal static IResource Parse(byte[] data, int position, IResource resource)
        {
            var cname = WalkBytesForHostname(ref data, position).sb.ToString();
            return new CNameRecord(resource, cname);
        }
    }
}
