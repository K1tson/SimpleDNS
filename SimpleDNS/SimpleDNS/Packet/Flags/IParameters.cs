namespace Kitson.SimpleDNS.Packet.Flags
{
    public interface IParameters
    {
        QR QueryResponse { get; }
        OPCode OpCode { get; }
        AA AuthoritativeAnswer { get; }
        bool Truncation { get; }
        bool RecursionDesired { get; }
        bool RecursionAvailable { get; }
        bool AnswerAuthenticated { get; }
        bool NonAuthenticatedData { get; }
        ResponseCode Response { get; }
        byte[] ToBytes();
    }
}
