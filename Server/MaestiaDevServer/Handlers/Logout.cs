using System;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Core;

namespace MaestiaDevServer.Handlers
{
    public static class Logout
    {
        [Packet(2, 10)]
        public static void CLIENT_GAME_EXIT(Packet packetData, Client packetSender)
        {
            // Called when the client finally disconnects
            var exitFinishPacket = new Packet(2, 11);
            exitFinishPacket.WriteByte(255);
            exitFinishPacket.WriteByte(255);
            packetSender.SendPacket(exitFinishPacket);
        }

        [Packet(2, 41)]
        public static void CLIENT_REQUEST_EXIT(Packet packetData, Client packetSender)
        {
            var exitRequestPacket = new Packet(2, 49);
            packetSender.SendPacket(exitRequestPacket);

            // Do SQL Stuff here I guess
        }
    }
}
