using System;
using Kitson.SimpleDNS.Packet.Question;

namespace Kitson.SimpleDNS.Packet.Answer
{
    public interface IResource
    {
        string Name { get; }
        QType Type { get; }
        QClass Class { get; }
        UInt32 Ttl { get; }
        UInt16 Length { get; }
        string Data { get; }
        byte[] ToBytes();
    }
}
