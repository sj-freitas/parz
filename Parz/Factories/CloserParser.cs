using System;
using System.Collections.Generic;
using System.Linq;

namespace Parz.Factories
{
    public class CloserParser : ITokenTypeParser
    {
        private readonly IEnumerable<string> _supportedClosers;
        private readonly IEqualityComparer<string> _equalityComparer;

        public CloserParser(IEnumerable<string> supportedOpeners = null,
            IEqualityComparer<string> equalityComparer = null)
        {
            _supportedClosers = supportedOpeners ?? Defaults.Closers;
            _equalityComparer = equalityComparer ?? Defaults.StringEqualityComparer;
        }

        public bool TryParse(string symbol, out TokenType tokenType,
            out string adapted)
        {
            adapted = symbol ?? throw new ArgumentNullException(nameof(symbol));
            tokenType = ParzTokenType.Closer;

            return _supportedClosers.Contains(symbol, _equalityComparer);
        }
    }
}
