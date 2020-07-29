using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Core
{
    public class Listener
    {
        private readonly TcpListener _tcpListener;
        private readonly Dictionary<Tuple<byte, byte>, Action<Packet, Client>> _handlers;

        public Listener(int serverPort)
        {
            _tcpListener = new TcpListener(IPAddress.Any, serverPort);
            _handlers = new Dictionary<Tuple<byte, byte>, Action<Packet, Client>>();
        }

        public void Start()
        {
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(OnAccept, _tcpListener);
        }

        private void OnAccept(IAsyncResult asyncResult)
        {
            var tcpClient = _tcpListener.EndAcceptTcpClient(asyncResult);

            Log.WriteInfo("New connection found at ( {0} )", tcpClient.Client.RemoteEndPoint);

            var unused = new Client(tcpClient, this);

            _tcpListener.BeginAcceptTcpClient(OnAccept, _tcpListener);
        }

        public void SetHandler(Action<Packet, Client> packetHandler, byte packetMainId, byte packetSubId)
        {
            _handlers.Add(Tuple.Create(packetMainId, packetSubId), packetHandler);
        }

        public void Handle(Client packetSender, short packetLength, byte packetMainId, byte packetSubId, byte[] packetBuffer)
        {
            File.WriteAllBytes($".CS {packetMainId:D3}-{packetSubId:D3}_{packetLength:D5}.dat", packetBuffer);

            if (_handlers.TryGetValue(Tuple.Create(packetMainId, packetSubId), out var packetHandler))
            {
                Log.WriteSuccess("Handle {0}. ( MainId: {1}, SubId: {2} )", packetHandler.Method.Name, packetMainId, packetSubId);

                packetHandler(new Packet(packetLength, packetMainId, packetSubId, packetBuffer), packetSender);
            }
            else
                Log.WriteWarning("Received unknown packet. ( Length: {0}, MainId: {1}, SubId: {2} )", packetLength, packetMainId, packetSubId);
        }
    }
}