using System;
using System.IO;
using System.Text;

namespace Core
{
    public class Packet
    {
        public short Length;

        public byte MainId;
        public byte SubId;

        private readonly PacketWriter _writer;
        private readonly PacketReader _reader;

        public Packet(byte packetMainId, byte packetSubId)
        {
            MainId = packetMainId;
            SubId = packetSubId;

            _writer = new PacketWriter();

            _writer.Write(MainId);
            _writer.Write(SubId);
        }

        public Packet(short packetLength, byte packetMainId, byte packetSubId, byte[] packetBuffer)
        {
            Length = packetLength;

            MainId = packetMainId;
            SubId = packetSubId;

            _reader = new PacketReader(packetBuffer);
        }

        public void WriteByte(byte value) => _writer.Write(value);
        public void WriteBytes(byte[] buffer) => _writer.Write(buffer);
        public void WriteShort(short value) => _writer.Write(value);
        public void WriteInt(int value) => _writer.Write(value);
        public void WriteFloat(float value) => _writer.Write(value);
        public void WriteLong(long value) => _writer.Write(value);

        public byte ReadByte() => _reader.ReadByte();
        public byte[] ReadBytes(int count) => _reader.ReadBytes(count);
        public short ReadShort() => _reader.ReadInt16();
        public int ReadInt() => _reader.ReadInt32();
        public float ReadFloat() => _reader.ReadSingle();
        public long ReadLong() => _reader.ReadInt64();

        public byte[] GetBuffer()
        {
            return (_writer.BaseStream as MemoryStream)?.ToArray();
        }
    }

    public class PacketWriter : BinaryWriter
    {
        public PacketWriter() : base(new MemoryStream())
        {
        }
    }

    public class PacketReader : BinaryReader
    {
        public PacketReader(byte[] packetBuffer) : base(new MemoryStream(packetBuffer))
        {
        }
    }
}