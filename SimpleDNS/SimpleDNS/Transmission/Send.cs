using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Kitson.SimpleDNS.CustomExceptions;
using Kitson.SimpleDNS.Packet;

namespace Kitson.SimpleDNS.Transmission
{
    internal sealed class Send
    {
        private readonly AddressFamily _af = AddressFamily.InterNetwork;
        private readonly SocketType _st;
        private readonly ProtocolType _pt;
        private readonly int _sockSendTimeOut = 3000;
        private readonly int _sockReceiveTimeOut = 3000;

        public Send(SendDnsPacket sendPacket, IPAddress serverIpAddress, TransmissionType type) : this(sendPacket, serverIpAddress, 53, type)
        {}

        public Send(SendDnsPacket sendPacket, IPAddress serverIpAddress, int port, TransmissionType type)
        {
            SendingDnsPacket = sendPacket;
            ServerAddress = serverIpAddress;
            Type = type;
            Port = port;

            var socketOptions = SocketTypes(type);
            _st = socketOptions.Item1;
            _pt = socketOptions.Item2;
        }

        internal SendDnsPacket SendingDnsPacket { get; }
        internal IPAddress ServerAddress { get; }
        internal TransmissionType Type { get; }
        internal int Port { get; }

        internal async Task<byte[]> Async()
        {
            var sock = CreateSocket();

            try
            {
                return await ConnectToSocketAsync(sock);
            }
            catch (AggregateException e)
            {
                throw e.Flatten();
            }
            finally
            {
                sock.Dispose();
            }

        }

        internal byte[] Synchronous()
        {
            var sock = CreateSocket();

            try
            {
                return ConnectToSocket(sock);
            }
            catch (AggregateException e)
            {
                throw e.Flatten();
            }
            finally
            {
                sock.Dispose();
            }
        }

        private Tuple<SocketType, ProtocolType> SocketTypes(TransmissionType type)
        {
            return type == TransmissionType.TCP ?
                new Tuple<SocketType, ProtocolType>(SocketType.Stream, ProtocolType.Tcp) :
                new Tuple<SocketType, ProtocolType>(SocketType.Dgram, ProtocolType.Udp);
        }

        private Socket CreateSocket()
        {
            return new Socket(_af, _st, _pt)
            {
                ReceiveTimeout = _sockReceiveTimeOut,
                SendTimeout = _sockSendTimeOut
            };
        }

        private async Task<byte[]> ConnectToSocketAsync(Socket socket)
        {
            return await Task.Factory.StartNew(() => ConnectToSocket(socket));
        }

        private byte[] ConnectToSocket(Socket socket)
        {
            socket.Connect(ServerAddress, Port);

            if (!socket.Connected)
                throw new DnsConnectException("Error: Socket was unable to connect to DNS server. Check the DNS server details.");

            var packet = SendingDnsPacket.ToBytes();
            socket.Send(packet);
            byte[] buffer = new byte[512];

            try
            {
                return buffer.Take(socket.Receive(buffer)).ToArray();
            }
            catch (SocketException ex)
            {
                throw new DnsConnectException("Error: Did not receive any data from server. Check the DNS server details.", ex);
            }
        }


    }
}
