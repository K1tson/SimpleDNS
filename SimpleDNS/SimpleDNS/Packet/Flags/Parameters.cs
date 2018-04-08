using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Kitson.SimpleDNS.Packet.Flags
{
    /// <summary>
    /// Base Class for Parameters/Flags. Send and Receive to inherit.
    /// </summary>
    public abstract class Parameters : IParameters
    {
        /// <summary>
        /// Designed for modelling a DNS packet request.
        /// </summary>
        /// <param name="queryResponse"></param>
        /// <param name="opCode"></param>
        /// <param name="recursionDesired"></param>
        /// <param name="answerAuthenticated"></param>
        protected Parameters(OPCode opCode, bool recursionDesired, bool answerAuthenticated) : this(opCode: opCode,
            authoritativeAnswer: AA.NonAuthoritive, truncation: false, recursionDesired: recursionDesired,
            recursionAvailable: false, answerAuthenticated: answerAuthenticated, nonAuthenticatedData: false,
            response: ResponseCode.Ok)
        {
            QueryResponse = QR.Query;
        }

        /// <summary>
        /// Designed for modelling a DNS packet response.
        /// </summary>
        /// <param name="queryResponse"></param>
        /// <param name="opCode"></param>
        /// <param name="authoritativeAnswer"></param>
        /// <param name="truncation"></param>
        /// <param name="recursionDesired"></param>
        /// <param name="recursionAvailable"></param>
        /// <param name="nonAuthenticatedData"></param>
        /// <param name="response"></param>
        /// <param name="answerAuthenticated"></param>
        protected Parameters(OPCode opCode, AA authoritativeAnswer, bool truncation,
            bool recursionDesired, bool recursionAvailable, bool answerAuthenticated, bool nonAuthenticatedData, ResponseCode response)
        {
            QueryResponse = QR.Response;
            OpCode = opCode;
            AuthoritativeAnswer = authoritativeAnswer;
            Truncation = truncation;
            RecursionDesired = recursionDesired;
            RecursionAvailable = recursionAvailable;
            Reserved = 00; //Zero'd bits as not used...yet.
            AnswerAuthenticated = answerAuthenticated;
            NonAuthenticatedData = nonAuthenticatedData;
            Response = response;
        }

        /// <summary>
        /// Type of Query e.g. Query or Response
        /// </summary>
        public QR QueryResponse { get; }

        /// <summary>
        /// specifies the kind of query in the message
        /// </summary>
        public OPCode OpCode { get; }

        /// <summary>
        ///  Authoritative Answer. This flag is only valid for responses. 
        /// </summary>
        public AA AuthoritativeAnswer { get; }

        /// <summary>
        /// specifies that this message was truncated due to length greater than that permitted on the transmission channel
        /// </summary>
        public bool Truncation { get; }

        /// <summary>
        /// this bit may be set in a query and is copied into the response.If RD is set, it directs the name server to pursue the query recursively. Recursive query support is optional.
        /// </summary>
        public bool RecursionDesired { get; }

        /// <summary>
        ///  this be is set or cleared in a response, and denotes whether recursive query support is available in the name server.
        /// </summary>
        public bool RecursionAvailable { get; }

        /// <summary>
        ///  Reserved for future use.  Must be zero in all queries and responses.
        /// </summary>
        private int Reserved { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool AnswerAuthenticated { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool NonAuthenticatedData { get; }

        /// <summary>
        /// this 4 bit field is set as part of responses.
        /// </summary>
        public ResponseCode Response { get; }

      
        /// <summary>
        /// Returns all parameters as a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            byte[] results = new byte[2];
            var byteOne = new bool[8];
            var byteTwo = new bool[8];
            
            byteOne[0] = Convert.ToBoolean((int)QueryResponse); //0 Position = QueryResponse (Send & Receive)
            SplitBitArray((int)OpCode).CopyTo(byteOne, 1); //1-4 Position = OpCode (Send)
            byteOne[5] = Convert.ToBoolean((int)AuthoritativeAnswer); //5 = Authoritative Answer (Receive)
            byteOne[6] = Truncation; //6 Position = Trucation
            byteOne[7] = RecursionDesired; //7 Position = Recursion Desired (Send)

            byteTwo[0] = RecursionAvailable; //8 Position = Resursion Available (Receive)
            SplitBitArray(Reserved).CopyTo(byteTwo, 1); //9-10 Position = Reserved (Not Used)
            byteTwo[3] = AnswerAuthenticated; //11 = 
            byteTwo[4] = NonAuthenticatedData; //12 = 
            SplitBitArray((int)Response).CopyTo(byteTwo, 5); //13-16 Position = Response (Receive)

            BitArray byte1 = new BitArray(byteOne.Reverse().ToArray()); //Reverse == Bigendian
            BitArray byte2 = new BitArray(byteTwo.Reverse().ToArray()); //Reverse == Bigendian

            byte1.CopyTo(results,0);
            byte2.CopyTo(results,1);

            return results;
        }


        public static IParameters Parse(byte[] data)
        {
            if(data.Length != 2)
                throw new ArgumentException("Error: There should only be two bytes when parsing parameters");

            BitArray bits = new BitArray(data.Reverse().ToArray());
   
            var opCode = ConvertTo<OPCode>(new[] {bits[14], bits[13], bits[12], bits[11]});
            var authServer = (AA)Enum.Parse(typeof(AA), Convert.ToInt32(bits[10]).ToString());
            var truncation = bits[9];
            var recursionDesired = bits[8];
            var recursionAvailable = bits[7];
            //var reserved = byteTwo[1]; Reserved is already added
            var aa = bits[5];
            var nonAuthData = bits[4];
            var replyCode = ConvertTo<ResponseCode>(new[] {bits[3], bits[2], bits[1], bits[0]});

            return new ReceiveParameters(opCode,authServer,truncation,recursionDesired,recursionAvailable,aa,nonAuthData,replyCode);
        }


        private bool[] SplitBitArray(int bits)
        {
            return bits.ToString().Select(n => n == '1').ToArray();
        }



        private static T ConvertTo<T>(bool[] data) where T : struct, IConvertible
        {
     
            StringBuilder buffer = new StringBuilder(4);

            for (int i = 0; i < 4; i++)
            {
                buffer.Append(data[i] ? "1" : "0");
            }

            return (T)Enum.Parse(typeof(T), buffer.ToString());
        }


    }
}
