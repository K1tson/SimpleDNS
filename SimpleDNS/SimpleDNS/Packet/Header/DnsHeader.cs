using System;
using System.Collections.Generic;
using System.Linq;
using Kitson.SimpleDNS.Packet.Flags;

namespace Kitson.SimpleDNS.Packet.Header
{
    /// <summary>
    ///  The header includes fields that specify which of the remaining sections are present, and also specify whether the message is a query or a response, a standard query or some other opcode, etc.
    /// </summary>
    public sealed class DnsHeader : IDnsHeader
    {
        public DnsHeader(ushort id, IParameters parameters, ushort questionsCount, ushort answersCount, ushort authority, ushort additional)
        {
            TransactionId = id;
            Parameters = parameters;
            QuestionsCount = questionsCount;
            AnswersCounts = answersCount;
            Authority = authority;
            Additional = additional;
        }
       
        public ushort TransactionId { get; }
        public IParameters Parameters { get; }
        public ushort QuestionsCount { get; }
        public ushort AnswersCounts { get; }
        public ushort Authority { get; }
        public ushort Additional { get; }

        public byte[] ToBytes()
        {
            List<byte> result = new List<byte>();

            result.AddRange(BitConverter.GetBytes(TransactionId).Reverse());
            result.AddRange(Parameters.ToBytes());
            result.AddRange(BitConverter.GetBytes(QuestionsCount).Reverse());
            result.AddRange(BitConverter.GetBytes(AnswersCounts).Reverse());
            result.AddRange(BitConverter.GetBytes(Authority).Reverse());
            result.AddRange(BitConverter.GetBytes(Additional).Reverse());

            return result.ToArray();
        }

        public static IDnsHeader Parse(byte[] header)
        {
            var transId = (ushort)BitConverter.ToInt16(new[] { header[1], header[0] }, 0);
            var parameters = Flags.Parameters.Parse(new[] {header[2], header[3]});
            var queCount = (ushort)BitConverter.ToInt16(new []{ header[5], header[4] }, 0);
            var ansCount = (ushort)BitConverter.ToInt16(new[] { header[7], header[6] }, 0);
            var authCount = (ushort)BitConverter.ToInt16(new[] { header[9], header[8] }, 0);
            var addCount = (ushort) BitConverter.ToInt16(new[] { header[11], header[10] }, 0);

            return new DnsHeader(transId, parameters, queCount, ansCount, authCount, addCount);
        }

    }
}
