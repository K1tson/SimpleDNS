namespace Kitson.SimpleDNS.Packet.Flags
{
    public class SendParameters : Parameters
    {
        public SendParameters(OPCode opCode, bool recursionDesired, bool answerAuthenticated) : base(opCode, recursionDesired, answerAuthenticated)
        {

        }
    }
}
