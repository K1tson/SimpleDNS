namespace Kitson.SimpleDNS.Packet.Flags
{

        // 0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
        //+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        //|QR|   Opcode  |AA|TC|RD|RA|   Z    |   RCODE   |
        //+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        // Taken from https://www.ietf.org/rfc/rfc1035.txt 

        /// <summary>
        /// Type of Query e.g. Query or Response
        /// </summary>
        public enum QR //(Query or Response) Position 0 = 1x Bit
        {
            Query = 0,
            Response = 1
        }

        /// <summary>
        /// OPCode specifies the kind of query in the message
        /// </summary>
        public enum OPCode //Postion 1-4 = 4x Bits
        {
            Standard = 0000, //Standard Query
            Inverse = 0001, //Inverse Query
            Status = 0010 //Server status request
        }

        /// <summary>
        ///  Authoritative Answer. This flag is only valid for responses. 
        /// </summary>
        public enum AA //Position 5 = 1x bit
        {
            NonAuthoritive = 0,
            Authoritive = 1
        }
       
        /// <summary>
        ///  Response code or RCODE is set as part of responses
        /// </summary>
        public enum ResponseCode //Postion 2 = 4x bits
        {
            Ok = 0000, //No error condition
            FormatError = 0001, //The name server was unable to interpret the query
            ServerFailure = 0010, //The name server was unable to process this query due to a problem with the name server.
            NameError = 0011, //Meaningful only for responses from an authoritative name server, this code signifies that the domain name referenced in the query does not exist
            NotSupported = 0100, //The name server does not support the requested kind of query
            Refused = 0101 //Well....you fucked up with your sending IP at somepoint!
        }

}
