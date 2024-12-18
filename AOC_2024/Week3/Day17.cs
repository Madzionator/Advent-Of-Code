using System.ComponentModel;

namespace AdventOfCode2024.Week3;

internal class Day17 : Day
{
    private int[] _program;
    private List<int> _output = new();

    public override (object resultA, object resultB) Execute()
    {
        // [A, B, C]
        var register = InputLines.Take(3)
            .Select(line => line.Split(": ")[1])
            .Select(long.Parse)
            .ToArray();

        _program = InputLines[4]
            .Split(": ")[1]
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        var resA = string.Join(',', TaskA(register.ToArray()));
        var resB = TaskB(register.ToArray())!;

        return (resA, resB);
    }

    List<int> TaskA(long[] register)
    {
        _output.Clear();
        var ip = 0;

        while (ip < _program.Length)
        {
            ip = DoInstruction(ip, register);
        }

        return _output;
    }

    long? TaskB(long[] register)
    {
        var programIdx = _program.Length - 1;
        long[] newRegister = [0, register[1], register[2]];

        return FindA(newRegister, programIdx);
    }

    long? FindA(long[] register, int programIdx)
    {
        if (programIdx < 0)
        {
            return register[0];
        }

        for (var i = 0; i < 8; i++)
        {
            long[] newReg = [(register[0] * 8 + i), .. register[1..]];
            if (TaskA(newReg)[0] == _program[programIdx])
            {
                var a = FindA([register[0] * 8 + i, .. register[1..]], programIdx - 1);
                if (a != null)
                    return a;
            }
        }

        return null;
    }

    private int DoInstruction(int instructionPointer, long[] register)
    {
        var instruction = _program[instructionPointer];
        var operand = _program[instructionPointer + 1];

        switch (instruction)
        {
            case 0: //adv
                register[0] = register[0] / (1L << (int)OperandToCombo(operand, register));
                break;
            case 1: //bxl
                register[1] = register[1] ^ OperandToLiteral(operand);
                break;
            case 2: //bst
                register[1] = OperandToCombo(operand, register) & 7;
                break;
            case 3: //jnz
                if (register[0] != 0)
                {
                    instructionPointer = OperandToLiteral(operand) - 2;
                }
                break;
            case 4: //bxc
                register[1] = register[1] ^ register[2];
                break;
            case 5: //out
                _output.Add((int)(OperandToCombo(operand, register) & 7));
                break;
            case 6: //bdv
                register[1] = register[0] / (1L << (int)OperandToCombo(operand, register));
                break;
            case 7: //cdv
                register[2] = register[0] / (1L << (int)OperandToCombo(operand, register));
                break;
        }

        return instructionPointer + 2;
    }

    private long OperandToCombo(int operand, long[] register) =>
        operand switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => register[0], // A
            5 => register[1], // B
            6 => register[2], // C
            _ => throw new InvalidEnumArgumentException()
        };

    private int OperandToLiteral(int operand)
    {
        if (operand < 8)
            return operand;

        throw new ArgumentException();
    }
}