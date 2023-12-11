using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.Helpers.Extensions
{
    public static class CollectionsUtils
    {
        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> elements, int index)
        {
            return index == 0 ?
                new[] { Array.Empty<T>() } :
                elements.SelectMany((element, indexer) =>
                    elements.Skip(indexer + 1).Permutations(index - 1)
                        .Select(combo => (new[] { element })
                            .Concat(combo)));
        }

        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            if (k == 0)
            {
                yield return Enumerable.Empty<T>();
                yield break;
            }

            var i = 0;
            foreach (var element in elements)
            {
                var remainingElements = elements.Skip(i + 1);
                foreach (var combination in Combinations(remainingElements, k - 1))
                {
                    yield return (new[] { element }).Concat(combination);
                }
                i++;
            }
        }
    }
}