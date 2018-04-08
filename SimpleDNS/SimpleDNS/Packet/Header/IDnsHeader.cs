using System;
using Kitson.SimpleDNS.Packet.Flags;

namespace Kitson.SimpleDNS.Packet.Header
{
    public interface IDnsHeader
    {
        /// <summary>
        /// A 16 bit identifier assigned by the program that generates any kind of query. This identifier is copied the corresponding reply and can be used by the requester to match up replies to outstanding queries.
        /// </summary>
        UInt16 TransactionId { get; }

        /// <summary>
        /// 16 bit parameters for Query.
        /// </summary>
        IParameters Parameters { get; }

        /// <summary>
        /// An unsigned 16 bit integer specifying the number of entries in the question section.
        /// </summary>
        UInt16 QuestionsCount { get; }

        /// <summary>
        /// An unsigned 16 bit integer specifying the number of resource records in the answer section.
        /// </summary>
        UInt16 AnswersCounts { get; }

        /// <summary>
        /// An unsigned 16 bit integer specifying the number of name server resource records in the authority records section.
        /// </summary>
        UInt16 Authority { get; }

        /// <summary>
        /// an unsigned 16 bit integer specifying the number of resource records in the additional records section.
        /// </summary>
        UInt16 Additional { get; }
       
        /// <summary>
        /// Converts IDnsHeader to byte[]
        /// </summary>
        /// <returns></returns>
        byte[] ToBytes();

    }
}
