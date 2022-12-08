using Advent._2022.Day;
using Advent.Helpers.Extensions;

namespace Advent._2022.Week2;

class Day8 : IDay
{
    public void Execute()
    {
        var input = File.ReadAllLines(@"Week2\input8.txt").ToMatrix();

        Console.WriteLine(TaskA(input));
        Console.WriteLine(TaskB(input));
    }

    private int TaskA(int[,] heightsMap)
    {
        var m = heightsMap.GetLength(0);
        var newMap = new int[m, m];
        int visible = 0;

        VisibleTreeFrom1Dir(0, 1, false);      //left->right
        VisibleTreeFrom1Dir(0, 1, true);       //up->down
        VisibleTreeFrom1Dir(m-1, -1, false);   //right->left
        VisibleTreeFrom1Dir(m-1, -1, true);    //down->up

        return visible;

        void VisibleTreeFrom1Dir(int start, int incr, bool rotate)
        {
            for (var i = start; 0 <= i && i < m; i+=incr)
            {
                int max = -1;
                for (var j = start; 0 <= j && j < m; j+=incr)
                {
                    if (!rotate && heightsMap[i, j] > max)
                    {
                        visible += newMap[i, j] == 0 ? 1 : 0;
                        newMap[i, j]++;
                        max = heightsMap[i, j];
                    }
                    else if (rotate && heightsMap[j, i] > max)
                    {
                        visible += newMap[j, i] == 0 ? 1 : 0;
                        newMap[j, i]++;
                        max = heightsMap[j, i];
                    }
                }
            }
        }
    }
    
    private int TaskB(int[,] input)
    {
        var m = input.GetLength(0);
        var max = 0;
        
        for(var y = 1; y < m-1; y++)
        for (var x = 1; x < m-1; x++)
        {
            max = int.Max(max, ScenicScore(y, x));
        }

        return max;

        int ScenicScore(int y, int x)
        {
            return OneDirScore(x - 1, -1, i => input[y, i] >= input[y, x])    //left
                   * OneDirScore(x + 1, 1, i => input[y, i] >= input[y, x])   //right
                   * OneDirScore(y - 1, -1, i => input[i, x] >= input[y, x])  //down
                   * OneDirScore(y + 1, 1, i => input[i, x] >= input[y, x]);  //up
        }

        int OneDirScore(int start, int incr, Func<int, bool> condition)
        {
            var tmp = 0;
            for (var i = start; 0 <= i && i < m; i+=incr)
            {
                tmp++;
                if (condition(i))
                    break;
            }

            return tmp;
        }
    }
}