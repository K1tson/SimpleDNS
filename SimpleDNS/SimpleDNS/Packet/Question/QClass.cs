using System;

namespace Kitson.SimpleDNS.Packet.Question
{
    public enum QClass : Int16
    {
        IN = 1, //The Internet
        CS = 2, //the CSNET class (Obsolete - used only for examples in some obsolete RFCs)
        CH = 3, //the CHAOS class
        HS = 4, //Hesiod [Dyer 87]
    }
}
