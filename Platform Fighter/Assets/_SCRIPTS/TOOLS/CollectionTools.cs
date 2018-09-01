using System.Collections.Generic;
using System.Linq;

namespace TOOLS
{
    public static class CollectionTools
    {
        public static IEnumerable<T> Concat<T>(params IEnumerable<T>[] sequences)
        {
            return sequences.SelectMany(x => x);
        }
    }
}