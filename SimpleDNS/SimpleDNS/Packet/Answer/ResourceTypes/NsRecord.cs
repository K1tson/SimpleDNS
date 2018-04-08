namespace Kitson.SimpleDNS.Packet.Answer.ResourceTypes
{
    public class NsRecord : Resource
    {
        protected NsRecord(IResource temp, string ns) : base(temp, ns)
        {
            NameServer = ns;
        }

        public string NameServer { get; }

        internal static IResource Parse(byte[] data, int position, IResource resource)
        {
            var ns = WalkBytesForHostname(ref data, position).sb.ToString();
            return new NsRecord(resource, ns);
        }
    }
}
