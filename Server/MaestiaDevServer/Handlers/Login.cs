using System;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Core;

namespace MaestiaDevServer.Handlers
{
    public static class Login
    {
        private static byte[] _publicKey;
        private static byte[] _privateKey;

        [Packet(1, 10)]
        public static void SPK_HELO_RQ_ASK(Packet packetData, Client packetSender)
        {
            var unknownInt = packetData.ReadInt();
            var unknownInt2 = packetData.ReadInt();
            var unknownInt3 = packetData.ReadInt();

            Log.WriteInfo($"{nameof(SPK_HELO_RQ_ASK)} ( UnknownInt: {unknownInt}, UnknownInt2: {unknownInt2}, UnknownInt3: {unknownInt3} )");

            // DPK_HELO_RS_AGREE
            var agreePacket = new Packet(1, 20);
            agreePacket.WriteByte(0); // Login - 0 | Zone - 2
            agreePacket.WriteInt(0x07151257); // Engine Version
            agreePacket.WriteInt(0x01020000); // Client Version
            agreePacket.WriteInt(0x07041700); // Server Version
            packetSender.SendPacket(agreePacket);

            // DPKUL_ACCOUNT_RS_UNK
            var unkPacket = new Packet(11, 13);

            using (var rsa = new RSACryptoServiceProvider())
            {
                _publicKey = rsa.ExportCspBlob(false);
                _privateKey = rsa.ExportCspBlob(true);
            }

            unkPacket.WriteBytes(_publicKey);

            packetSender.SendPacket(unkPacket);
        }

        [Packet(3, 10)]
        public static void SPK_CRYPT_RQ_CHANGE(Packet packetData, Client packetSender)
        {
            var unknownInt = packetData.ReadInt();

            Log.WriteInfo($"{nameof(SPK_CRYPT_RQ_CHANGE)} ( UnknownInt: {unknownInt} )");

            // DPK_CRYPT_RS_UNK
            var unkPacket = new Packet(3, 20);

            var cryptArray = Encoding.Default.GetBytes("");

            unkPacket.WriteByte(Convert.ToByte(cryptArray.Length));
            unkPacket.WriteBytes(cryptArray);

            packetSender.SendPacket(unkPacket);
        }

        [Packet(11, 1)]
        public static void SPKUL_ACCOUNT_RQ_IDPASS(Packet packetData, Client packetSender)
        {
            var totalLength = packetData.ReadByte();
            var idLength = packetData.ReadByte();
            var passLength = packetData.ReadByte();
            var charLength = packetData.ReadByte();
            var rawIdPass = packetData.ReadBytes(totalLength);

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(_privateKey);

                Array.Reverse(rawIdPass, 0, rawIdPass.Length);

                rawIdPass = rsa.Decrypt(rawIdPass, false);
            }

            Log.WriteInfo($"{nameof(SPKUL_ACCOUNT_RQ_IDPASS)} ( TotalLength: {totalLength}, IdLength: {idLength}, PassLength: {passLength}, CharLength: {charLength} )");

            // Enable Multilanguage login support
            var name = Encoding.Unicode.GetString(rawIdPass, 0, idLength * 2);
            var pass = Encoding.Unicode.GetString(rawIdPass, idLength * 2, passLength * 2);

            //MySqlCommand sqlCMD         = new MySqlCommand("SELECT password FROM accounts WHERE name='"+name+"'", Program.SQLconnection);
            //MySqlDataReader sqlReader   = sqlCMD.ExecuteReader();

            /*
            while (sqlReader.Read())
            {
                Log.WriteInfo("User Login - account name: " + name + " tries to login.");
            }

            if(!sqlReader.HasRows)
            {
                var failedNamePacket = new Packet(11, 3);
                failedNamePacket.WriteInt(0x104);
                packetSender.SendPacket(failedNamePacket);

                Log.WriteInfo("User Login - Cannot find any data related to account name: " + name);
                sqlReader.Close();
                return;
            }
            */
            if (name != pass)
            {
                var failedPasswordPacket = new Packet(11, 3);
                failedPasswordPacket.WriteInt(0x104);
                packetSender.SendPacket(failedPasswordPacket);

                //sqlReader.Close();
                Log.WriteInfo("User Login Failed - ID: " + name + " | Password: " + pass + " - Incorrect password - Disconnecting!");

                return;
            }
            else
            {
                //DPKUL_ACCOUNT_RS_SUCCEED
                Log.WriteInfo("User Login Success - ID: " + name + " | Password: " + pass + " - Login successfuly.");
                packetSender.SendPacket(new Packet(11, 2));
            }

            // 0x1		IDS_NETWORK_ERROR
            // 0x101	IDS_ACCOUNT_PROCEDURE_FAILED
            // 0x102	IDS_LOBBY_ERR_USING_ACCOUNT
            // 0x103	IDS_LOBBY_ERR_ACCOUNT_REQUIRE_CONFIRM
            // 0x104	IDS_ACCOUNT_MISMATCH_IDPASS
            // 0x105	IDS_ACCOUNT_BLOCKED
            // 0x106	IDS_ACCOUNT_DISCONNECT_PREVCONNECTION
            // 0x107	IDS_ACCOUNT_HAVE_NOT_AUTHORITY
            // 0x108	IDS_ACCOUNT_NOT_CLOSEBETA_MEMBER
            // 0x109	IDS_ACCOUNT_RETRY_LATER
            // 0x832	IDS_DONOT_CREATE_POCKETVIEW_YET
            // 0x9901	IDS_LOBBY_ERR_DENIED_ADDRESS
            // 0x9902	IDS_LOBBY_ERR_NOT_OPEN_SERVER
            // 0x9902	IDS_LOBBY_ERR_UNKNOWN
            // 0x10201	IDS_LOBBY_ERR_NO_WORLD_CONNECTED
            // 0x10202	IDS_LOBBY_ERR_INACTIVE_WORLD
            // 0x10203	IDS_LOBBY_ERR_FULL_WORLD
            // 0x10204	IDS_LOBBY_ERR_NOT_OPEN_SERVER
            // 0x220001	IDS_ALREADY_CHARACTER_BEING
            // 0x220003	IDS_CREATE_FAILED_DISABLEACCEPT
            // 0x310003	IDS_LOBBY_ERR_USING_ACCOUNT
            // 0x310010	IDS_LOBBY_ERR_FAILED_CREATE_CHAR
            // 0x310011	IDS_LOBBY_ERR_INVALID_CHAR_PROTO
            // 0x310012	IDS_LOBBY_ERR_INVALID_FACE_TYPE
            // 0x310013	IDS_LOBBY_ERR_INVALID_CHAR_HAIR
            // 0x310015	IDS_LOBBY_ERR_EXIST_CHAR_NAME
            // 0x310016	IDS_LOBBY_ERR_DATABASE_ERROR
            // 0x310017	IDS_LOBBY_ERR_INVALID_SLOT
            // 0x310018	IDS_LOBBY_ERR_DUPLICATE_SLOT
            // 0x310019	IDS_LOBBY_WRONG_NAME_KOREA
            // 0x310020	IDS_LOBBY_ERR_NOT_CREATE_ANYMORE
            // 0x310021	IDS_LOBBY_ERR_INVALID_CHAR
            // 0x310022	IDS_LOBBY_ERR_INVALID_ZONE
            // 0x310023	IDS_LOBBY_ERR_NO_ACTIVE_ZONESERVER
            // 0x310025	IDS_LOBBY_ERR_NO_ZONE_CONNECTED
            // 0x310026	IDS_LOBBY_ERR_INACTIVE_ZONE
            // 0x310028	IDS_LOBBY_ERR_FAILED_DELCHAR
            // 0x310029	IDS_CLTNET_ERR_GUILD_MASTER
            // 0x310030	IDS_LOBBY_ERR_DENY_ADDRESS
            // 0x310031	IDS_LOBBY_ERR_NOT_READY
            // 0x310032	IDS_USER_CREATE_INVALID_ALLIANCE_PROTO
            // 0x310033	IDS_ERRORCODE_USER_CHARACTER_INVALID_PASSWORD
            // 0x310034	IDS_ERRORCODE_USER_CHARACTER_ALREADY_SETTED_PASSWORD
            // 0x310035	IDS_ERRORCODE_USER_CHARACTER_DIDNT_SET_PASSWORD
            // 0x310044	IDS_USER_ALLIANCE_CREATE_CHAR_BLOCK
            // 0x310045	IDS_USER_NOT_DELETE_CHAR_CHARPET_USE
            // 0x310046	IDS_USER_NOT_DELETE_PET_REGISTERED
        }

        [Packet(11, 11)]
        public static void SPKUL_ACCOUNT_RQ_MACADDRESS(Packet packetData, Client packetSender)
        {
            // The MAC Address is sent using 2 integers, for now I'm reading it as 8 bytes.
            var macAddress = BitConverter.ToString(packetData.ReadBytes(8)).Replace('-', ':');

            Log.WriteInfo($"{nameof(SPKUL_ACCOUNT_RQ_MACADDRESS)} ( MacAddress: {macAddress} )");

            // DPKUL_ACCOUNT_RS_MACADDRESS
            packetSender.SendPacket(new Packet(11, 12));

            //send the server list here to follow the actual game logic from the 2015 log file!!!

            // === Concept of Serverlist ===
            // First packet server sends to client is DPKUL_SERVER_RS_LISTBEGIN to start serverlist transfer
            // Second packet is DPKUL_SERVER_RS_CHILD which tells the servername. This can be repeated multiple times for more servers
            // Third packet is DPKUL_SERVER_RS_LISTEND to indicate end of serverlist transfer
            // Fourth packet is DPKUL_SERVER_RS_ACTIVE to activate server and set playercount

            // DPKUL_SERVER_RS_LISTBEGIN
            var listBeginPacket = new Packet(12, 2);
            packetSender.SendPacket(listBeginPacket);

            // DPKUL_SERVER_RS_CHILD
            var childPacket = new Packet(12, 3);
            var serverName = "<GE> Delphi @ maestia.net";
            childPacket.WriteInt(0);//server ID starting from 0
            childPacket.WriteBytes(Encoding.Unicode.GetBytes(serverName));// ServerName
            childPacket.WriteBytes(new byte[160 - serverName.Length * 2]);// ServerName (remaining bytes)   server name must be 160 bytes long (80 unicode chars)
            childPacket.WriteInt(0);//??
            childPacket.WriteByte(0);//??
            childPacket.WriteByte(4);//num characters on this server -> max 4 // Replace later with SQL check
            packetSender.SendPacket(childPacket);

            var childPacket2 = new Packet(12, 3);
            var serverName2 = "<FR> Pectus @ maestia.net";
            childPacket2.WriteInt(1);
            childPacket2.WriteBytes(Encoding.Unicode.GetBytes(serverName2));
            childPacket2.WriteBytes(new byte[160 - serverName2.Length * 2]);
            childPacket2.WriteInt(0);
            childPacket2.WriteByte(0);
            childPacket2.WriteByte(0);
            packetSender.SendPacket(childPacket2);

            var childPacket3 = new Packet(12, 3);
            var serverName3 = "<EU> Papyrus @ maestia.net";
            childPacket3.WriteInt(2);
            childPacket3.WriteBytes(Encoding.Unicode.GetBytes(serverName3));
            childPacket3.WriteBytes(new byte[160 - serverName3.Length * 2]);
            childPacket3.WriteInt(0);
            childPacket3.WriteByte(0);
            childPacket3.WriteByte(0);
            packetSender.SendPacket(childPacket3);

            // DPKUL_SERVER_RS_LISTEND
            packetSender.SendPacket(new Packet(12, 5));

            // DPKUL_SERVER_RS_ACTIVE
            var activePacket = new Packet(12, 4);
            activePacket.WriteInt(0); //server id -> 0 = first one in list
            activePacket.WriteInt(0); //server condition: 0 to 25 - Normal, 26 to 80 - Busy, 81 or over - Full
            activePacket.WriteInt(1);  //boolean -> server active?
            packetSender.SendPacket(activePacket);

            var activePacket2 = new Packet(12, 4);
            activePacket2.WriteInt(1); //server id -> 0 = first one in list
            activePacket2.WriteInt(26); //server condition: 0 to 25 - Normal, 26 to 80 - Busy, 81 or over - Full
            activePacket2.WriteInt(1);  //boolean -> server active?
            packetSender.SendPacket(activePacket2);

            var activePacket3 = new Packet(12, 4);
            activePacket3.WriteInt(2); //server id -> 0 = first one in list
            activePacket3.WriteInt(81); //server condition: 0 to 25 - Normal, 26 to 80 - Busy, 81 or over - Full
            activePacket3.WriteInt(1);  //boolean -> server active?
            packetSender.SendPacket(activePacket3);
        }
        
        [Packet(11, 20)]
        public static void SPKUL_ACCOUNT_RQ_UNK(Packet packetData, Client packetSender)
        {
            // Do we need to send something in here?

            // The MAC Address is sent using 2 integers, for now I'm reading it as 8 bytes.
            var macAddress = BitConverter.ToString(packetData.ReadBytes(8)).Replace('-', ':');

            Log.WriteInfo($"{nameof(SPKUL_ACCOUNT_RQ_MACADDRESS)} ( MacAddress: {macAddress} )");

            // DPKUL_ACCOUNT_RS_MACADDRESS
            //packetSender.SendPacket(new Packet(11, 12));
        }
    }
}
