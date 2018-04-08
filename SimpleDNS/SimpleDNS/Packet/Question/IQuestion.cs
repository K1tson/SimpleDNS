namespace Kitson.SimpleDNS.Packet.Question
{
    public interface IQuestion
    {
        string QName { get; }
        QType QType { get; }
        QClass QClass { get; }
        byte[] ToBytes();
    }
}
