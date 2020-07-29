using System;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Core;

namespace MaestiaDevServer.Handlers
{
    public static class CharCreation
    {
        /*
        [Packet(51, 11)]
        public static void CLIENT_CREATE_NEW_CHARACTER(Packet packetData, Client packetSender)
        {
            var characterCreatePacket = new Packet(51, 12);
            packetSender.SendPacket(characterCreatePacket);
        }
        */

        [Packet(51, 41)]
        public static void CLIENT_SUBMIT_NEW_CHARACTER(Packet packetData, Client packetSender)
        {
            var iLength = packetData.Length;
            var charPacket = BitConverter.ToString(packetData.ReadBytes(iLength));

            //Console.WriteLine(charPacket); // raw data output
            //System.Diagnostics.Debug.WriteLine(charPacket);

            var charClass = charPacket[0].ToString() + charPacket[1].ToString(); //character class and gender | 01 = warrior male - 02 = warrior female - 03 = mage male - 04 = mage female - 05 = archer male - 06 = archer female - 07 = priest male - 08 = priest female
            var charUnknown1 = charPacket[6].ToString() + charPacket[7].ToString(); // ?? always 1
            var charUnknown2 = charPacket[9].ToString() + charPacket[10].ToString(); // ?? always 5
            var charFaction = charPacket[12].ToString() + charPacket[13].ToString(); //character faction | 01 = SG faction - 02 = TK faction
            var charUnknown3 = charPacket[21].ToString() + charPacket[22].ToString(); // ?? always 10
            var charName = Util.ConvertHex(charPacket[24].ToString() + charPacket[25].ToString() + charPacket[30].ToString() + charPacket[31].ToString() + charPacket[36].ToString() + charPacket[37].ToString() + charPacket[42].ToString() + charPacket[43].ToString() + charPacket[48].ToString() + charPacket[49].ToString() + charPacket[54].ToString() + charPacket[55].ToString() + charPacket[60].ToString() + charPacket[61].ToString() + charPacket[66].ToString() + charPacket[67].ToString() + charPacket[72].ToString() + charPacket[73].ToString() + charPacket[78].ToString() + charPacket[79].ToString() + charPacket[84].ToString() + charPacket[85].ToString() + charPacket[90].ToString() + charPacket[91].ToString() + charPacket[96].ToString() + charPacket[97].ToString() + charPacket[102].ToString() + charPacket[103].ToString() + charPacket[108].ToString() + charPacket[109].ToString()); // character name
            var charFaceTatoo = charPacket[264].ToString() + charPacket[265].ToString(); // character face and tatoo | 96 = face 0 & tatoo 0 - 97 = face 0 & tatoo 1 - 98 = face 0 & tatoo 2 - 99 = face 0 & tatoo 3 - 9A = face 1 & tatoo 0 - 9B = face 1 & tatoo 1 - 9C = face 1 & tatoo 2 - 9D = face 1 & tatoo 3 - 9E = face 2 & tatoo 0 - 9F = face 2 & tatoo 1 - A0 = face 2 & tatoo 2 - A1 = face 2 & tatoo 3 - F6 = face 3 & tatoo 0 - F7 = face 3 & tatoo 1 - F8 = face 3 & tatoo 2 - F9 = face 3 & tatoo 3
            var charHair = charPacket[270].ToString() + charPacket[271].ToString(); // character hair | 01 = hair 0 - 02 = hair 1 - 03 = hair 2 - 04 = hair 3 - 05 = hair 4 - 06 = hair 5 - 07 = hair 6
            var charHairColor_R = charPacket[276].ToString() + charPacket[277].ToString(); // character hair color R | 00 to FF
            var charHairColor_G = charPacket[279].ToString() + charPacket[280].ToString(); // character hair color G | 00 to FF
            var charHairColor_B = charPacket[282].ToString() + charPacket[283].ToString(); // character hair color B | 00 to FF
            var charSkinColor_R = charPacket[288].ToString() + charPacket[289].ToString(); // character skin color R | 00 to FF
            var charSkinColor_G = charPacket[291].ToString() + charPacket[292].ToString(); // character skin color G | 00 to FF
            var charSkinColor_B = charPacket[294].ToString() + charPacket[295].ToString(); // character skin color B | 00 to FF
            var charHeigth = charPacket[300].ToString() + charPacket[301].ToString(); // character heigth | 00 = tiny - 05 = medium - 0A = tall
            var charSize = charPacket[312].ToString() + charPacket[313].ToString(); // character size | 00 = thin - 05 = medium - 0A = fat
            var charUnknown4 = charPacket[324].ToString() + charPacket[325].ToString(); // ?? | 04 = warrior male - 28 = warrior female - 0F = mage male - BD = mage female - DC = archer male - 00 = archer female - A8 = priest male - 61 = priest female
            var charUnknown5 = charPacket[327].ToString() + charPacket[328].ToString(); // ?? | 00 = warrior both // archer male - 01 = mage female // archer female - 02 = mage male // priest both
            var charUnknown6 = charPacket[333].ToString() + charPacket[334].ToString(); // ?? always 71
            var charUnknown7 = charPacket[336].ToString() + charPacket[337].ToString(); // ?? | 02 = warrior male - 26 = warrior female - 5F = priest female - A6 = priest male - BB = mage female - 0D = mage male - DA = archer male - FE = archer female
            var charUnknown8 = charPacket[345].ToString() + charPacket[346].ToString(); // ?? always 71
            var charUnknown9 = charPacket[348].ToString() + charPacket[349].ToString(); // ?? | 01 = warrior male - 25 = warrior female - 0C = mage male - BA = mage female - D9 = archer male - FD = archer female - A5 = priest male - 5E = priest female
            var charUnknown10 = charPacket[351].ToString() + charPacket[352].ToString(); // ?? | 00 = warrior both // archer both - 01 = mage female - 02 = mage male // priest both
            var charUnknown11 = charPacket[357].ToString() + charPacket[358].ToString(); // ?? always 71
            var charUnknown12 = charPacket[360].ToString() + charPacket[361].ToString(); // ?? | 03 = warrior male - 27 = warrior female - 0E = mage male - BC = mage female - DB = archer male - FF = archer female - A7 = priest male - 60 = priest female
            var charUnknown13 = charPacket[363].ToString() + charPacket[364].ToString(); // ?? | 00 = warrior both // archer both - 01 = mage female - 02 = mage male // priest both
            var charUnknown14 = charPacket[369].ToString() + charPacket[370].ToString(); // ?? always 71
            /*
            Console.WriteLine("Class / Gender   | " + charClass);
            Console.WriteLine("Unknown value 1  | " + charUnknown1);
            Console.WriteLine("Unknown value 2  | " + charUnknown2);
            Console.WriteLine("Faction          | " + charFaction);
            Console.WriteLine("Unknown value 3  | " + charUnknown3);
            Console.WriteLine("Player name      | " + charName);
            Console.WriteLine("Face / Tatoo ID  | " + charFaceTatoo);
            Console.WriteLine("Hair ID          | " + charHair);
            Console.WriteLine("Hair Color RGB   | " + charHairColor_R + " - " + charHairColor_G + " - " + charHairColor_B);
            Console.WriteLine("Skin Color RGB   | " + charSkinColor_R + " - " + charSkinColor_G + " - " + charSkinColor_B);
            Console.WriteLine("Heigth           | " + charHeigth);
            Console.WriteLine("Size             | " + charSize);
            Console.WriteLine("Unknown value 4  | " + charUnknown4);
            Console.WriteLine("Unknown value 5  | " + charUnknown5);
            Console.WriteLine("Unknown value 6  | " + charUnknown6);
            Console.WriteLine("Unknown value 7  | " + charUnknown7);
            Console.WriteLine("Unknown value 8  | " + charUnknown8);
            Console.WriteLine("Unknown value 9  | " + charUnknown9);
            Console.WriteLine("Unknown value 10 | " + charUnknown10);
            Console.WriteLine("Unknown value 11 | " + charUnknown11);
            Console.WriteLine("Unknown value 12 | " + charUnknown12);
            Console.WriteLine("Unknown value 13 | " + charUnknown13);
            Console.WriteLine("Unknown value 14 | " + charUnknown14);
            */
            /*
            CHAR LIST                    CREATE CHAR
            int m_nUnknown;                ID?
            int m_nUnknown2;            characterGender
            int m_nUnknown3;            characterClass
            wchar_t m_strUnknown[40];    One of these is the Character Name.
            wchar_t m_strUnknown2[40];    One of these is the Character Name.
            short m_nUnknown4;            characterFace
            short m_nUnknown5;            characterHair
            int m_nUnknown6;            unknownInt3
            int m_nUnknown7;            unknownInt4
            int m_nUnknown8;            unknownInt5
            int m_nUnknown9;            unknownInt6
            int m_nUnknown10;            unknownInt7
            int m_nUnknown11;            unknownInt8
            int m_nUnknown12;            unknownInt9
            int m_nUnknown13;            unknownInt10
            byte m_byUnknown;            unknownByte
             */
        }
    }
}
