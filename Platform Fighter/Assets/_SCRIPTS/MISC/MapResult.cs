using System;

namespace MISC
{
    public class MapResult<TValue, TResult>
    {
        internal MapResult(TValue value, TResult result = default, bool matchedPreviously = false)
        {
            OriginalValue = value;
            Result = result;
            MatchedPreviously = matchedPreviously;
        }

        private TValue OriginalValue { get; }
        public TResult Result { get; }
        private bool MatchedPreviously { get; }

        public MapResult<TValue, TResult> Map(TValue ifValue, TResult thenValue)
        {
            return Map(x => x.Equals(ifValue), x => thenValue);
        }

        public MapResult<TValue, TResult> Map(TValue ifValue, Func<TValue, TResult> thenFunc)
        {
            return Map(x => x.Equals(ifValue), thenFunc);
        }

        public MapResult<TValue, TResult> Map(Func<TValue, bool> ifFunc, TResult thenValue)
        {
            return Map(ifFunc, x => thenValue);
        }

        public MapResult<TValue, TResult> Map(Func<TValue, bool> ifFunc, Func<TValue, TResult> thenFunc)
        {
            if (MatchedPreviously || !ifFunc(OriginalValue)) return this;
            var result = new MapResult<TValue, TResult>(OriginalValue, thenFunc(OriginalValue), true);
            return result;
        }

        public TResult Else(TResult elseValue)
        {
            return Else(x => elseValue);
        }

        public TResult Else(Func<TValue, TResult> elseFunc)
        {
            if (MatchedPreviously) return Result;
            var result = elseFunc(OriginalValue);
            return result;
        }

        public TResult ElseDo(Action<TValue> doThis)
        {
            if (MatchedPreviously) return Result;
            doThis(OriginalValue);
            return Result;
        }

        public static implicit operator TResult(MapResult<TValue, TResult> value) => value.Result;
    }
}