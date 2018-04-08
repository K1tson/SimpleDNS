namespace Kitson.SimpleDNS.Packet.Flags
{
    public class ReceiveParameters : Parameters
    {
        public ReceiveParameters(OPCode opCode, AA authoritativeAnswer, bool truncation, bool recursionDesired, bool recursionAvailable, bool answerAuthenticated, bool nonAuthenticatedData, ResponseCode response) : base(opCode, authoritativeAnswer, truncation, recursionDesired, recursionAvailable, answerAuthenticated, nonAuthenticatedData, response)
        {
        }
    }
}
