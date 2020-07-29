using System;

namespace Core
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class PacketAttribute : Attribute
    {
        public byte MainId { get; set; }
        public byte SubId { get; set; }

        public PacketAttribute(byte packetMainId, byte packetSubId)
        {
            MainId = packetMainId;
            SubId = packetSubId;
        }
    }
}