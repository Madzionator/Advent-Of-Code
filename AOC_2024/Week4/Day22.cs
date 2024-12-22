namespace AdventOfCode2024.Week4;

internal class Day22 : Day
{
    private record Sequence(int a, int b, int c, int d);

    private Dictionary<Sequence, int> _sequences = new();

    public override (object resultA, object resultB) Execute()
    {
        return (TaskA(), TaskB());
    }

    long TaskA() => InputLines
        .Select(long.Parse)
        .Sum(Evolve2000Times);

    long TaskB() => _sequences.Max(x => x.Value);

    long Evolve2000Times(long secret)
    {
        var changes = new int[4];
        var buyerSequences = new Dictionary<Sequence, int>();

        // First result for comparison
        secret = EvolveSecretNumber(secret);
        var last = secret % 10;

        // Prepare the buyer's first sequence
        for (var i = 1; i < 4; i++)
        {
            secret = EvolveSecretNumber(secret);
            changes[i] = ((int)(secret % 10 - last));
            last = secret % 10;
        }

        // Check the buyer's other sequences
        for (var i = 4; i < 2000; i++)
        {
            secret = EvolveSecretNumber(secret);

            changes[0] = changes[1];
            changes[1] = changes[2];
            changes[2] = changes[3];

            changes[3] = (int)(secret % 10 - last);
            last = secret % 10;

            var sequence = new Sequence(changes[0], changes[1], changes[2], changes[3]);
            if (last > 0 && !buyerSequences.ContainsKey(sequence))
            {
                buyerSequences.Add(sequence, (int)last);
            }
        }

        // Update all buyers' sequences
        foreach (var sequence in buyerSequences)
        {
            _sequences.AddOrSet(sequence.Key, sequence.Value);
        }

        return secret;
    }

    long EvolveSecretNumber(long secret)
    {
        secret = (secret << 6) ^ secret; // (s * 2^6) xor s
        secret &= ((1 << 24) - 1); // s % 2^24

        secret = (secret >> 5) ^ secret;
        secret &= ((1 << 24) - 1);

        secret = (secret << 11) ^ secret;
        secret &= ((1 << 24) - 1);

        return secret;
    }
}