using System;

namespace MISC
{
    public static class MapExtensions
    {
        public static MapResult<TValue, TResult> Map<TValue, TResult>(this TValue originalValue, TValue ifValue,
            TResult thenValue)
        {
            return new MapResult<TValue, TResult>(originalValue).Map(ifValue, thenValue);
        }

        public static MapResult<TValue, TResult> Map<TValue, TResult>(this TValue originalValue,
            Func<TValue, bool> ifFunc, TResult thenValue)
        {
            return new MapResult<TValue, TResult>(originalValue).Map(ifFunc, thenValue);
        }

        public static MapResult<TValue, TResult> Map<TValue, TResult>(this TValue originalValue, TValue ifValue,
            Func<TValue, TResult> thenFunc)
        {
            return new MapResult<TValue, TResult>(originalValue).Map(ifValue, thenFunc);
        }

        public static MapResult<TValue, TResult> Map<TValue, TResult>(this TValue originalValue,
            Func<TValue, bool> ifFunc, Func<TValue, TResult> thenFunc)
        {
            return new MapResult<TValue, TResult>(originalValue).Map(ifFunc, thenFunc);
        }
    }
}