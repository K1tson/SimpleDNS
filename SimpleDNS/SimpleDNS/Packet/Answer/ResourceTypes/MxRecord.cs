using System;

namespace Kitson.SimpleDNS.Packet.Answer.ResourceTypes
{
    public class MxRecord : Resource
    {
        public MxRecord(IResource temp, ushort pref, string data) : base(temp, data)
        {
            Preference = pref;
        }

        public ushort Preference { get; }

        internal static IResource Parse(byte[] data, int position, IResource resource)
        {
            var pref = BitConverter.ToUInt16(new[] { data[position + 1], data[position] }, 0);
            return new MxRecord(resource, pref, WalkBytesForHostname(ref data, position + 2).ToString());
        }
    }
}
