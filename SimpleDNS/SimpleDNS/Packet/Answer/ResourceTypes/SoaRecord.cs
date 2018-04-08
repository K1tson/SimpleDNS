using System;
using System.Linq;

namespace Kitson.SimpleDNS.Packet.Answer.ResourceTypes
{
    public class SoaRecord : Resource
    {
        public SoaRecord(IResource temp, string priSrv, string ram, uint serial, uint refresh, uint retry, uint expire, uint minTtl) : base(temp, priSrv)
        {
            PrimaryNameServer = priSrv;
            ResponsibleAuthoritysMailbox = ram;
            SerialNumber = serial;
            RefreshInterval = refresh;
            RetryInterval = retry;
            ExpireLimit = expire;
            MinTtl = minTtl;
        }

        public string PrimaryNameServer { get; }
        public string ResponsibleAuthoritysMailbox { get; }
        public uint SerialNumber { get; }
        public uint RefreshInterval { get; }
        public uint RetryInterval { get; }
        public uint ExpireLimit { get; }
        public uint MinTtl { get; }

        internal static IResource Parse(byte[] data, int position, IResource resource)
        {
            var priSrvWalk = WalkBytesForHostname(ref data, position);
            var ramWalk = WalkBytesForHostname(ref data, priSrvWalk.pos + 1);
            uint serial = ConvertToUint(data, ramWalk.pos + 1);
            uint refresh = ConvertToUint(data, ramWalk.pos + 5);
            uint retry = ConvertToUint(data, ramWalk.pos + 9);
            uint expire = ConvertToUint(data, ramWalk.pos + 13);
            uint ttl = ConvertToUint(data, ramWalk.pos + 17);
            return new SoaRecord(resource, priSrvWalk.sb.ToString(), ramWalk.sb.ToString(), serial, refresh, retry, expire, ttl);
        }

        private static uint ConvertToUint(byte[] data, int position)
        {
            var temp = data.ToList().GetRange(position, 4);
            temp.Reverse();
      
            return BitConverter.ToUInt32(temp.ToArray(), 0);
        }

    }
}
