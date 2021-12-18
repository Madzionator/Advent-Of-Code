using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2021.Week1
{
    class Day16
    {
        public class Packet
        {
            public int Version;
            public int Type;
            public long Value;
            public List<Packet> SubPackets = new();

            public int VersionSum() => Version + SubPackets.Sum(x => x.VersionSum());

            public long Operate() => Type switch
            {
                0 => SubPackets.Select(x => x.Operate()).Sum(),
                1 => SubPackets.Select(x => x.Operate()).Aggregate(1L, (acc, val) => acc * val),
                2 => SubPackets.Select(x => x.Operate()).Min(),
                3 => SubPackets.Select(x => x.Operate()).Max(),
                4 => Value,
                5 => SubPackets[0].Operate() > SubPackets[1].Operate() ? 1 : 0,
                6 => SubPackets[0].Operate() < SubPackets[1].Operate() ? 1 : 0,
                7 => SubPackets[0].Operate() == SubPackets[1].Operate() ? 1 : 0
            };
        }

        public static void Execute()
        {
            var input = File.ReadAllText(@"Week3\input16.txt");
            var bitsArrays = Convert.FromHexString(input).Select(b => Convert.ToString(b, 2).PadLeft(8, '0')).ToArray();
            var bits = string.Join("", bitsArrays);

            var packet = MakePacket(bits, out _);
            Console.WriteLine(packet.VersionSum()); //TaskA
            Console.WriteLine(packet.Operate());    //TaskB
        }

        public static Packet MakePacket(string bits, out int usedBits)
        {
            var packet = new Packet();
            packet.Version = Convert.ToInt32(bits[..3], 2);
            packet.Type = Convert.ToInt32(bits[3..6], 2);
            usedBits = 6;

            if (packet.Type == 4)
            {
                packet.Value = GetLiteralValue(bits[6..], out var length);
                usedBits += length;
                return packet;
            }

            var lengthTypeId = bits[6] - '0';
            usedBits++;

            if (lengthTypeId == 0) // total length of subpackets
            {
                var length = Convert.ToInt32(bits[7..(7 + 15)], 2);
                usedBits += 15;
                var usedHere = 0;

                while (usedHere < length)
                {
                    packet.SubPackets.Add(MakePacket(bits[(22 + usedHere)..(22 + length)], out var used));
                    usedHere += used;
                }
                usedBits += usedHere;
            }
            else // number of subpackets
            {
                var number = Convert.ToInt32(bits[7..(7 + 11)], 2);
                usedBits += 11;
                var usedHere = 0;

                for (var i = 0; i < number; i++)
                {
                    packet.SubPackets.Add(MakePacket(bits[(18 + usedHere)..], out var used));
                    usedHere += used;
                }
                usedBits += usedHere;
            }

            return packet;
        }

        private static long GetLiteralValue(string bits, out int length)
        {
            var i = 0;
            var value = "";
            for (; i < bits.Length; i += 5)
            {
                value += bits[(i + 1)..(i + 5)];
                if (bits[i] == '0')
                    break;
            }

            length = i + 5;
            return Convert.ToInt64(value, 2);
        }
    }
}
