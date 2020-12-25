using System;
using System.IO;
using System.Linq;

namespace Advent._2020.Week4
{
    public class Day25
    {
        public static void Execute()
        {
            var data = File.ReadAllLines(@"Week4\input25.txt").Select(long.Parse).ToArray();

            long resultA = TaskA(data[0], data[1]);
            Console.WriteLine(resultA);
        }

        private static long TaskA(long cardPK, long doorPK)
        {
            //int cardLoop = EstablishLoopSize(cardPK);
            int doorLoop = EstablishLoopSize(doorPK);
            long encryptionKey = CountEncryptioKey(cardPK, doorLoop);

            return encryptionKey;
        }

        private static int EstablishLoopSize(long publicKey)
        {
            long value = 1;
            for (int loop = 1; ; loop++)
            {
                value = (value * 7) % 20201227;
                if (value == publicKey)
                    return loop;
            }
        }

        private static long CountEncryptioKey(long publicKey, int loopSize)
        {
            long value = 1;
            for (int loop = 1; loop <= loopSize; loop++)
                value = (value * publicKey) % 20201227;
            return value;
        }

    }
}