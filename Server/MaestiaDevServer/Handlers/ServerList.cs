using System;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Core;

namespace MaestiaDevServer.Handlers
{
    public static class ServerList
    {
        // Gets called when client received server list, just acknowledge it...
        [Packet(12, 1)]
        public static void CLIENT_RECEIVED_SERVERLIST(Packet packetData, Client packetSender)
        {
            Console.WriteLine("Client received server list!");
        }

        // Gets called when client wants to log into game server
        [Packet(12, 10)]
        public static void DPKUL_SERVER_RS_SELECT(Packet packetData, Client packetSender)
        {
            var serverID = packetData.ReadInt();  //server ID which the clients wants to connect to

            //DPK_HELO_RS_AGREE - Accept connection to zoneserver - Remove this if you are using the forward option!
            var serverSelectPacket = new Packet(12, 11);
            packetSender.SendPacket(serverSelectPacket);

            //DPK_HELO_RS_FORWARD - redirect to different address if necessary
            var serverForwardPacket = new Packet(1, 30);
            serverForwardPacket.WriteByte(127);
            serverForwardPacket.WriteByte(0);
            serverForwardPacket.WriteByte(0);
            serverForwardPacket.WriteByte(1);
            serverForwardPacket.WriteShort(21001);
            //packetSender.SendPacket(serverForwardPacket); //redirect to another instance but the programm closes the connection which resets the game!

            //DSERVER_TYPE_ZONE - Actual zone server | Country needs to be KOREA, Major Version is 2, Minor Version is 0 according to the client
            //[ZONE:server] Nation:KOREA, Major:2, Minor:0, Build:0x07151257 [LOGIN: client] Nation:KOREA, Major:2, Minor:0, Build:0x07151257, ExitCode 0 , 1
            var serverZonePacket = new Packet(1, 20);
            serverZonePacket.WriteByte(2);         // 2 = ZONE:Login
            serverZonePacket.WriteInt(0x07151257); // Engine Version
            serverZonePacket.WriteInt(0x01020002); // Client Version | 01 = Country, 02 = Major Version, 00 = Minor Version, 02 = Zone Login //Country  0 = DEFAULT, 1 = KOREA, 2 = JAPAN, 3 = CHINA, 4 = TIWAN, 5 = INDIA
            serverZonePacket.WriteInt(0x07041700); // Server Version
            packetSender.SendPacket(serverZonePacket);

            //Let the client join the actual zone server
            //DPKUZ_USER_RS_JOIN - Community server (zone)
            var zoneAckPacket = new Packet(51, 2);
            zoneAckPacket.WriteInt(0); //Error Code | 0 = no error
            zoneAckPacket.WriteInt(1000);
            zoneAckPacket.WriteInt(0);
            //zoneAckPacket.WriteInt((byte)serverID);
            //zoneAckPacket.WriteByte(0);
            //zoneAckPacket.WriteByte(0);
            //zoneAckPacket.WriteShort(0);
            packetSender.SendPacket(zoneAckPacket);

            //eLOGIN , eZONE ,ePBRMS eCHAT중의 한개이다.
            //https://github.com/chenbk85/job_mobile/blob/ebdf33d006025a682e9f2dbb670b23d5e3acb285/lib/lib_mech/src/tool/ServerMonitor/common/mech/util/sm/jRoC_SharedMemory.h

        }

        //After Server was selected, send the characters to the client!
        [Packet(51, 1)]
        public static void SPKUZ_USER_RQ_CHARLIST(Packet packetData, Client packetSender)
        {
            //DPKUZ_USER_RS_CHARLIST_BEGIN - Begin to transfer charlist data
            var characterListBeginPacket = new Packet(51, 12);
            packetSender.SendPacket(characterListBeginPacket);

            //###########################################################################################################################################################################

            //DPKUZ_USER_RS_CHARLIST_DATA - Actual Charlist data for 1 up to 4 chars sent in a row closed by the end packet
            var charNameX = "Plasma";
            var guildName = "Test Guild";

            var characterListDataPacket = new Packet(51, 13);

            characterListDataPacket.WriteInt(1);   // Char ID

            characterListDataPacket.WriteByte(8);  // class and gender | 1 warrior male | 2 warrior female | 3 mage male | 4 mage female | 5 archer male | 6 archer female | 7 priest male | 8 priest female
            characterListDataPacket.WriteByte(0);  // Must be 0
            characterListDataPacket.WriteByte(1);  // Must be 1
            characterListDataPacket.WriteByte(5);  // Must be 5

            characterListDataPacket.WriteShort(1); // guild rank 1 - 5 | 1 = guildmanager | 5 = guildnewcomer
            characterListDataPacket.WriteByte(0);  // Must be 0
            characterListDataPacket.WriteByte(10); // Must be 10

            // Char name
            characterListDataPacket.WriteBytes(Encoding.Unicode.GetBytes(charNameX));  //character name (40 chars)
            characterListDataPacket.WriteBytes(new byte[80 - charNameX.Length * 2]);

            // Guild name
            characterListDataPacket.WriteBytes(Encoding.Unicode.GetBytes(guildName));  //guild name (40 chars)
            characterListDataPacket.WriteBytes(new byte[80 - guildName.Length * 2]);

            // Hair/Tatoo
            characterListDataPacket.WriteShort(241); // Face and Tatoo ID: (Find out all values later) 150 - 249 warrior male | 162 - 253 warrior female | 174 - 257 mage male | 186 - 1281? mage female | 198 - 145 archer male | 210 - 209 archer female | 222 - 273 priest male | 234 - 337 priest female
            characterListDataPacket.WriteByte(119);  // Hair ID: warrior male: 1 - 8 | warrior female: 17 - 24 | mage male: 33 - 40 | mage female: 49 - 56 | archer male: 65 - 72 | archer female: 81 - 88 | priest male: 97 - 104 | priest female: 113 - 120
            characterListDataPacket.WriteByte(0);    // 00 (placeholder)

            // Hair color
            characterListDataPacket.WriteByte(255); // Hair Color B (reversed!)
            characterListDataPacket.WriteByte(255); // Hair Color G (reversed!)
            characterListDataPacket.WriteByte(255); // Hair Color R (reversed!)
            characterListDataPacket.WriteByte(0);   // 00 (placeholder)

            // Skin color
            characterListDataPacket.WriteByte(148); // Skin Color B (reversed!)
            characterListDataPacket.WriteByte(205); // Skin Color G (reversed!)
            characterListDataPacket.WriteByte(255); // Skin Color R (reversed!)
            characterListDataPacket.WriteByte(0);   // 00 (placeholder)

            // Char size
            characterListDataPacket.WriteByte(5);   // Char Size from 0 to 10
            characterListDataPacket.WriteByte(0);   // 00 (placeholder)
            characterListDataPacket.WriteByte(0);   // 00 (placeholder)
            characterListDataPacket.WriteByte(0);   // 00 (placeholder)

            // Char weigth
            characterListDataPacket.WriteByte(5);   // Char Weigth from 0 to 10
            characterListDataPacket.WriteByte(0);   // 00 (placeholder)
            characterListDataPacket.WriteByte(0);   // 00 (placeholder)
            characterListDataPacket.WriteByte(0);   // 00 (placeholder)

            // 3D Model Torso
            characterListDataPacket.WriteByte(97); // warrior male = 4 | warrior female = 40 | mage male = 15 | mage female = 189 | archer male = 220 | archer female = 0 | priest male = 168 | priest female = 97
            characterListDataPacket.WriteByte(2);  // warrior both & archer male = 0 | mage female & archer female = 1 | mage male & priest both = 2
            characterListDataPacket.WriteByte(0);  // 00 (placeholder)
            characterListDataPacket.WriteByte(0);  // 00 placeholder

            // 3D Model hands/wrists
            characterListDataPacket.WriteByte(95); // warrior male = 2 | warrior female = 38 | mage male = 13 | mage female = 187 | archer male = 218 | archer female = 254 | priest male = 166 | priest female = 95
            characterListDataPacket.WriteByte(2);  // warrior both & archer male = 0 | mage female & archer female = 1 | mage male & priest both = 2
            characterListDataPacket.WriteByte(0);  // 00 (placeholder)
            characterListDataPacket.WriteByte(0);  // 00 placeholder

            // 3D Model Shoes
            characterListDataPacket.WriteByte(94); // warrior male = 1 | warrior female = 37 | mage male = 12 | mage female = 186 | archer male = 217 | archer female = 253 | priest male = 165 | priest female = 94
            characterListDataPacket.WriteByte(2);  // warrior both & archer male = 0 | mage female & archer female = 1 | mage male & priest both = 2
            characterListDataPacket.WriteByte(0);  // 00 (placeholder)
            characterListDataPacket.WriteByte(0);  // 00 placeholder

            // 3D Model Legs
            characterListDataPacket.WriteByte(96); // warrior male = 3 | warrior female = 39 | mage male = 14 | mage female = 188 | archer male = 219 | archer female = 255 | priest male = 167 | priest female = 96
            characterListDataPacket.WriteByte(2);  // warrior both & archer male = 0 | mage female & archer female = 1 | mage male & priest both = 2
            characterListDataPacket.WriteByte(0);  // 00 (placeholder)
            characterListDataPacket.WriteByte(0);  // 00 placeholder

            characterListDataPacket.WriteByte(69); // Char Level
            characterListDataPacket.WriteByte(2);   // Slot from 0 to 3
            characterListDataPacket.WriteByte(0);
            characterListDataPacket.WriteByte(0);

            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??

            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??

            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??

            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??

            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??

            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??

            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??

            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??

            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // byte GAP[1];
            characterListDataPacket.WriteByte(0);   // Char locked? 0 = off / 1 = on

            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??

            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??

            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??

            characterListDataPacket.WriteBytes(new byte[2]); //byte GAP_2[2];
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??

            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteShort(-1);  // -1 = no char deletion in progress | delete time in minutes - 72 hours = 4320

            characterListDataPacket.WriteByte(2);   // faction | 1 = SG | 2 = TK
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??

            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??

            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??
            characterListDataPacket.WriteByte(0);   // ??

            characterListDataPacket.WriteByte(0);   // ??
            packetSender.SendPacket(characterListDataPacket);

            //###########################################################################################################################################################################

            //Received DPKUZ_USER_RS_CHARLIST_END - End to transfer charlist data
            var characterListEndPacket = new Packet(51, 14);
            packetSender.SendPacket(characterListEndPacket);
        }
    }
}
