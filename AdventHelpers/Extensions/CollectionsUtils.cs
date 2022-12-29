using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.Helpers.Extensions
{
    public static class CollectionsUtils
    {
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int index)
        {
            return index == 0 ?
                new[] { Array.Empty<T>() } :
                elements.SelectMany((element, indexer) =>
                    elements.Skip(indexer + 1).Combinations(index - 1)
                        .Select(combo => (new[] { element })
                            .Concat(combo)));
        }
    }
}