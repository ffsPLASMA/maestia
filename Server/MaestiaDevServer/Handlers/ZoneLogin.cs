using System;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Core;

namespace MaestiaDevServer.Handlers
{
    public static class ZoneLogin
    {
        [Packet(51, 31)]
        public static void CLIENT_ZONE_LOGIN_WITH_CHAR(Packet packetData, Client packetSender)
        {
            //DPKUZ_USER_RS_CHARSEL
            var characterSelectEndPacket = new Packet(51, 32);
            characterSelectEndPacket.WriteInt(1); //char ID
            packetSender.SendPacket(characterSelectEndPacket);

            var iLength = packetData.Length;
            var loginPacket = BitConverter.ToString(packetData.ReadBytes(iLength));

            //Console.WriteLine(loginPacket); // raw data output
            //Console.WriteLine("Total length: " + iLength);
            //System.Diagnostics.Debug.WriteLine(loginPacket);

            // Structure: Length 657
            // Selected Char Slot (0 - 3)
            // ??

            // DPKUZ_USER_RS_BEGIN_PCINFO
            var beginPCInfoPacket = new Packet(51, 22);
            packetSender.SendPacket(beginPCInfoPacket);

            // DPKUZ_USER_RS_CHAR_BASE_DATA
            var charBaseDataPacket = new Packet(51, 24);

            charBaseDataPacket.WriteInt(1);
            charBaseDataPacket.WriteInt(2);
            charBaseDataPacket.WriteInt(3);
            charBaseDataPacket.WriteInt(4);
            charBaseDataPacket.WriteInt(5);
            charBaseDataPacket.WriteInt(6);
            charBaseDataPacket.WriteInt(7);
            charBaseDataPacket.WriteInt(8);
            charBaseDataPacket.WriteInt(9);
            charBaseDataPacket.WriteInt(10);
            charBaseDataPacket.WriteInt(11);
            charBaseDataPacket.WriteInt(12);
            charBaseDataPacket.WriteInt(13);
            charBaseDataPacket.WriteInt(14);
            charBaseDataPacket.WriteInt(15);
            charBaseDataPacket.WriteInt(16);
            charBaseDataPacket.WriteInt(17);
            charBaseDataPacket.WriteInt(18);
            charBaseDataPacket.WriteInt(19);
            charBaseDataPacket.WriteInt(20);
            charBaseDataPacket.WriteInt(21);
            charBaseDataPacket.WriteInt(22);
            charBaseDataPacket.WriteInt(23);
            charBaseDataPacket.WriteInt(24);
            charBaseDataPacket.WriteInt(25);
            charBaseDataPacket.WriteInt(26);
            charBaseDataPacket.WriteInt(27);
            charBaseDataPacket.WriteInt(28);
            charBaseDataPacket.WriteInt(29);
            charBaseDataPacket.WriteInt(30);
            charBaseDataPacket.WriteInt(31);
            charBaseDataPacket.WriteInt(32);

            charBaseDataPacket.WriteInt(1);
            charBaseDataPacket.WriteInt(2);
            charBaseDataPacket.WriteInt(3);
            charBaseDataPacket.WriteInt(4);
            charBaseDataPacket.WriteInt(5);
            charBaseDataPacket.WriteInt(6);
            charBaseDataPacket.WriteInt(7);
            charBaseDataPacket.WriteInt(8);
            charBaseDataPacket.WriteInt(9);
            charBaseDataPacket.WriteInt(10);
            charBaseDataPacket.WriteInt(11);
            charBaseDataPacket.WriteInt(12);
            charBaseDataPacket.WriteInt(13);
            charBaseDataPacket.WriteInt(14);
            charBaseDataPacket.WriteInt(15);
            charBaseDataPacket.WriteInt(16);
            charBaseDataPacket.WriteInt(17);
            charBaseDataPacket.WriteInt(18);
            charBaseDataPacket.WriteInt(19);
            charBaseDataPacket.WriteInt(20);
            charBaseDataPacket.WriteInt(21);
            charBaseDataPacket.WriteInt(22);
            charBaseDataPacket.WriteInt(23);
            charBaseDataPacket.WriteInt(24);
            charBaseDataPacket.WriteInt(25);
            charBaseDataPacket.WriteInt(26);
            charBaseDataPacket.WriteInt(27);
            charBaseDataPacket.WriteInt(28);
            charBaseDataPacket.WriteInt(29);
            charBaseDataPacket.WriteInt(30);
            charBaseDataPacket.WriteInt(31);
            charBaseDataPacket.WriteInt(32);

            charBaseDataPacket.WriteInt(1);
            charBaseDataPacket.WriteInt(2);
            charBaseDataPacket.WriteInt(3);
            charBaseDataPacket.WriteInt(4);
            charBaseDataPacket.WriteInt(5);
            charBaseDataPacket.WriteInt(6);
            charBaseDataPacket.WriteInt(7);
            charBaseDataPacket.WriteInt(8);
            charBaseDataPacket.WriteInt(9);
            charBaseDataPacket.WriteInt(10);
            charBaseDataPacket.WriteInt(11);
            charBaseDataPacket.WriteInt(12);
            charBaseDataPacket.WriteInt(13);
            charBaseDataPacket.WriteInt(14);
            charBaseDataPacket.WriteInt(15);
            charBaseDataPacket.WriteInt(16);
            charBaseDataPacket.WriteInt(17);
            charBaseDataPacket.WriteInt(18);
            charBaseDataPacket.WriteInt(19);
            charBaseDataPacket.WriteInt(20);
            charBaseDataPacket.WriteInt(21);
            charBaseDataPacket.WriteInt(22);
            charBaseDataPacket.WriteInt(23);
            charBaseDataPacket.WriteInt(24);
            charBaseDataPacket.WriteInt(25);
            charBaseDataPacket.WriteInt(26);
            charBaseDataPacket.WriteInt(27);
            charBaseDataPacket.WriteInt(28);
            charBaseDataPacket.WriteInt(29);
            charBaseDataPacket.WriteInt(30);
            charBaseDataPacket.WriteInt(31);
            charBaseDataPacket.WriteInt(32);

            charBaseDataPacket.WriteInt(1);
            charBaseDataPacket.WriteInt(2);
            charBaseDataPacket.WriteInt(3);
            charBaseDataPacket.WriteInt(4);
            charBaseDataPacket.WriteInt(5);
            charBaseDataPacket.WriteInt(6);
            charBaseDataPacket.WriteInt(7);
            charBaseDataPacket.WriteInt(8);
            charBaseDataPacket.WriteInt(9);
            charBaseDataPacket.WriteInt(10);
            charBaseDataPacket.WriteInt(11);
            charBaseDataPacket.WriteInt(12);
            charBaseDataPacket.WriteInt(13);
            charBaseDataPacket.WriteInt(14);
            charBaseDataPacket.WriteInt(15);
            charBaseDataPacket.WriteInt(16);
            charBaseDataPacket.WriteInt(17);
            charBaseDataPacket.WriteInt(18);
            charBaseDataPacket.WriteInt(19);
            charBaseDataPacket.WriteInt(20);
            charBaseDataPacket.WriteInt(21);
            charBaseDataPacket.WriteInt(22);
            charBaseDataPacket.WriteInt(23);
            charBaseDataPacket.WriteInt(24);
            charBaseDataPacket.WriteInt(25);
            charBaseDataPacket.WriteInt(26);
            charBaseDataPacket.WriteInt(27);
            charBaseDataPacket.WriteInt(28);
            charBaseDataPacket.WriteInt(29);
            charBaseDataPacket.WriteInt(30);
            charBaseDataPacket.WriteInt(31);
            charBaseDataPacket.WriteInt(32);

            charBaseDataPacket.WriteInt(1);
            charBaseDataPacket.WriteInt(2);
            charBaseDataPacket.WriteInt(3);
            charBaseDataPacket.WriteInt(4);
            charBaseDataPacket.WriteInt(5);
            charBaseDataPacket.WriteInt(6);
            charBaseDataPacket.WriteInt(7);
            charBaseDataPacket.WriteInt(8);
            charBaseDataPacket.WriteInt(9);
            charBaseDataPacket.WriteInt(10);
            charBaseDataPacket.WriteInt(11);
            charBaseDataPacket.WriteInt(12);
            charBaseDataPacket.WriteInt(13);
            charBaseDataPacket.WriteInt(14);
            charBaseDataPacket.WriteInt(15);
            charBaseDataPacket.WriteInt(16);
            charBaseDataPacket.WriteInt(17);
            charBaseDataPacket.WriteInt(18);
            charBaseDataPacket.WriteInt(19);
            charBaseDataPacket.WriteInt(20);
            charBaseDataPacket.WriteInt(21);
            charBaseDataPacket.WriteInt(22);
            charBaseDataPacket.WriteInt(23);
            charBaseDataPacket.WriteInt(24);
            charBaseDataPacket.WriteInt(25);
            charBaseDataPacket.WriteInt(26);
            charBaseDataPacket.WriteInt(27);
            charBaseDataPacket.WriteInt(28);
            charBaseDataPacket.WriteInt(29);
            charBaseDataPacket.WriteInt(30);
            charBaseDataPacket.WriteInt(31);
            charBaseDataPacket.WriteInt(32);

            charBaseDataPacket.WriteInt(1);
            charBaseDataPacket.WriteInt(2);
            charBaseDataPacket.WriteInt(3);
            charBaseDataPacket.WriteInt(4);

            //packetSender.SendPacket(charBaseDataPacket);

            //Set Curent Step EXT_DTA : 19 
            var setCurStepEXT_DTAPacket = new Packet(51, 27);
            setCurStepEXT_DTAPacket.WriteByte(19);
            packetSender.SendPacket(setCurStepEXT_DTAPacket);

            //Begin receive skill
            var BeginReceiveSkillPacket = new Packet(58, 1);
            packetSender.SendPacket(BeginReceiveSkillPacket);

            //Actual Skill data -  do that later
            var SkillDataPacket = new Packet(58, 2);
            //packetSender.SendPacket(SkillDataPacket);

            //End receive skill
            var EndReceiveSkillPacket = new Packet(58, 3);
            packetSender.SendPacket(EndReceiveSkillPacket);

            //DPKUZ_USER_RS_END_PCINFO
            var endPCInfoPacket = new Packet(51, 23);
            packetSender.SendPacket(endPCInfoPacket);
















            //DPKUZ_USER_RS_START_PERMISSION
            var permissionMoveZonePacket = new Packet(51, 35);
            //packetSender.SendPacket(permissionMoveZonePacket);

            //DPKUZ_USER_RS_START_GAME
            var enterWorldPacket2 = new Packet(51, 26);
            //packetSender.SendPacket(enterWorldPacket2);
        }
    }
}
