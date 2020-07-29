using System;
using System.Collections.Generic;

namespace Core
{
    public class PacketDefinitions : Dictionary<Tuple<byte, byte>, string>
    {
        public PacketDefinitions()
        {
            // Connection to server
            Add(1, 10, "SPK_HELO_RQ_ASK");
            Add(1, 20, "DPK_HELO_RS_AGREE");
            Add(1, 30, "DPK_HELO_RS_FORWARD");

            // Logout from server
            Add(2, 10, "CLIENT_REQUEST_LOGOUT"); // needs aproval

            // Encryption 1
            Add(3, 10, "SPK_CRYPT_RQ_CHANGE");
            Add(3, 20, "DPK_CRYPT_RS_UNK");

            // Errors (4)
            // Client doesn't send errors to server

            // Encryption 2 - Account
            Add(11, 1, "SPKUL_ACCOUNT_RQ_IDPASS");
            Add(11, 2, "DPKUL_ACCOUNT_RS_SUCCEED");
            Add(11, 3, "DPKUL_ACCOUNT_RS_FAILED");
            Add(11, 4, "SPKUL_ACCOUNT_RQ_PCAUTH");
            Add(11, 5, "DPKUL_ACCOUNT_RS_PENALTY");
            Add(11, 11, "SPKUL_ACCOUNT_RQ_MACADDRESS");
            Add(11, 12, "DPKUL_ACCOUNT_RS_MACADDRESS");
            Add(11, 13, "DPKUL_ACCOUNT_RS_UNK"); // Used to send the public key to the client for RsaCrypter.
            Add(11, 20, "SPKUL_ACCOUNT_RQ_UNK");
            Add(11, 255, "DPKUL_ACCOUNT_RS_ERROR");

            // Gameserver list
            Add(12, 1, "SPKUL_SERVER_RQ_GETLIST");
            Add(12, 2, "DPKUL_SERVER_RS_LISTBEGIN");
            Add(12, 3, "DPKUL_SERVER_RS_CHILD");
            Add(12, 4, "DPKUL_SERVER_RS_ACTIVE");
            Add(12, 5, "DPKUL_SERVER_RS_LISTEND");
            Add(12, 6, "DPKUL_SERVER_RS_UNK");
            Add(12, 7, "DPKUL_SERVER_RS_LISTEND");
            Add(12, 11, "DPKUL_SERVER_RS_SELECT");
            Add(12, 12, "DPKUL_SERVER_RS_UNK");
            Add(12, 14, "DPKUL_SERVER_RS_UNK");
            Add(12, 255, "DPKUL_SERVER_RS_ERROR");

            // Network update/handle
            Add(51, 1, "SPKUZ_USER_RQ_JOIN");
            Add(51, 31, "CLIENT_REQUEST_CHAR_LOGIN");
            Add(51, 41, "CLIENT_SUBMIT_NEW_CHAR_CREATION");

            // Char update/handle
            Add(52, 37, "SPKUZ_CHARACTER_RQ_RESERVE_AUTO_ATTACK");
            Add(52, 38, "SPKUZ_CHARACTER_RQ_RESERVE_AUTO_ATTACK_CANCEL");

            // ?? 53

            // Item update/handle (54)
            Add(54, 61, "");

            // Shop update/handle (55)

            // Chat update/handle (56)

            // Party update/handle (57)

            // Skill update/handle
            Add(58, 11, "SPKUZ_SKILL_RQ_RESERVE_CAST");
            Add(58, 12, "SPKUZ_SKILL_RQ_RESERVE_CAST_CANCEL");
            Add(58, 21, "");

            // Item 2 update/handle (59)

            // Trade update/handle (61)

            // ?? (62)
            Add(62, 4, "");
            Add(62, 8, "");
            Add(62, 9, "");
            Add(62, 25, "");

            // Zone movement/update/handle (63)

            // Motion update/handle (64)

            // Item notice/handle (65)

            // Item storage (66)

            // Admin commands/actions (67)

            // Mail update/handle (68)

            // Guild update/handle (70)

            // Whisper handle (94)

            // Friendlist (95)

            // Chatrooms (96)

            // Auction system (97)

            // Private shop (98)

            // ?? (99)

            // ?? (100)

            // Beauty shop (137)

            // Battlezone update/handle (143)

            // Pet system (144)

            // ?? (145)

            // Guildwarface handle (149)

            // ?? (150)

            // ?? (200)

            // Event handle (202)

            // Pet 2 system (203)

            // Char title system (204)

            // Penalty system (205)

        }

        public void Add(byte packetMainId, byte packetSubId, string packetName)
        {
            Add(Tuple.Create(packetMainId, packetSubId), packetName);
        }
    }
}