using System.Collections.Generic;
using System.Linq;

namespace KnizhnyVoz.Utils
{
    public static class Extentions
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }
    }
}
