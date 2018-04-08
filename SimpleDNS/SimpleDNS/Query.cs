using System;
using System.Net;
using System.Threading.Tasks;
using Kitson.SimpleDNS.Models;
using Kitson.SimpleDNS.Packet;
using Kitson.SimpleDNS.Packet.Flags;
using Kitson.SimpleDNS.Packet.Header;
using Kitson.SimpleDNS.Transmission;

namespace Kitson.SimpleDNS
{
    /// <summary>
    /// The utility class for utilising Kitson.DNS.
    /// </summary>
    public static class Query
    {

        /// <summary>
        /// Queries DNS using the SimpleDnsPacket packet model synchronously.
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        /// <exception cref="T:Kitson.Dns.CustomExceptions.DnsConnectException"></exception>
        public static IDnsPacket Simple(SimpleDnsPacket packet)
        {
            return ConstructPacketFromSimple(packet);
        }

        /// <summary>
        /// Queries DNS using the SimpleDnsPacket packet model asynchronously.
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        /// <exception cref="T:Kitson.Dns.CustomExceptions.DnsConnectException"></exception>
        public static async Task<IDnsPacket> SimpleAsync(SimpleDnsPacket packet)
        {
            return await ConstructPacketFromSimpleAsync(packet);
        }

        /// <summary>
        /// Queries DNS using the SendDnsPacket object synchronously.
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="serverIpAddress"></param>
        /// <returns></returns>
        /// <exception cref="T:Kitson.Dns.CustomExceptions.DnsConnectException"></exception>
        public static IDnsPacket Dns(SendDnsPacket packet, IPAddress serverIpAddress)
        {
            return TransmitSynchronous(new Send(packet, serverIpAddress, TransmissionType.UDP));
        }

        /// <summary>
        /// Queries DNS using the SendDnsPacket object asynchronously.
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="serverIpAddress"></param>
        /// <returns></returns>
        /// <exception cref="T:Kitson.Dns.CustomExceptions.DnsConnectException"></exception>
        public static async Task<IDnsPacket> DnsAsync(SendDnsPacket packet, IPAddress serverIpAddress)
        {
            return await TransmitAsync(new Send(packet, serverIpAddress, TransmissionType.UDP));
        }

        private static IDnsPacket ConstructPacketFromSimple(SimpleDnsPacket packet)
        {
            var header = new DnsHeader(GenerateId(), new SendParameters(OPCode.Standard, true, false), 1, 0, 0, 0);
            var dnsPacket = new SendDnsPacket(header, packet.Query);

            return TransmitSynchronous(new Send(dnsPacket, packet.ServerIPAddress, packet.Type));
        }

        private static async Task<IDnsPacket> ConstructPacketFromSimpleAsync(SimpleDnsPacket packet)
        {
            var header = new DnsHeader(GenerateId(), new SendParameters(OPCode.Standard, true, false), 1, 0, 0, 0);
            var dnsPacket = new SendDnsPacket(header, packet.Query);

            return await TransmitAsync(new Send(dnsPacket, packet.ServerIPAddress, packet.Type));
        }

        private static ushort GenerateId()
        {
            return (ushort)new Random().Next(ushort.MinValue, ushort.MaxValue);
        }

        private static IDnsPacket TransmitSynchronous(Send sendObj)
        {
            var receivedBytes = sendObj.Synchronous();
            IDnsPacket parsedBytes = DnsPacket.Parse(receivedBytes, sendObj.SendingDnsPacket);
            return parsedBytes;
        }

        private static async Task<IDnsPacket> TransmitAsync(Send sendObj)
        {
            var receivedBytes = await sendObj.Async();
            IDnsPacket parsedBytes = DnsPacket.Parse(receivedBytes, sendObj.SendingDnsPacket);
            return parsedBytes;
        }
    }
}
