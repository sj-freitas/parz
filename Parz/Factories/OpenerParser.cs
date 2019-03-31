using System;
using System.Collections.Generic;
using System.Linq;

namespace Parz.Factories
{
    public class OpenerParser : ITokenTypeParser
    {
        private readonly IEnumerable<string> _supportedOpeners;
        private readonly IEqualityComparer<string> _equalityComparer;

        public OpenerParser(IEnumerable<string> supportedOpeners = null,
            IEqualityComparer<string> equalityComparer = null)
        {
            _supportedOpeners = supportedOpeners ?? Defaults.Openers;
            _equalityComparer = equalityComparer ?? Defaults.StringEqualityComparer;
        }

        public bool TryParse(string symbol, out TokenType tokenType,
            out string adapted)
        {
            adapted = symbol ?? throw new ArgumentNullException(nameof(symbol));
            tokenType = ParzTokenType.Opener;

            return _supportedOpeners.Contains(symbol, _equalityComparer);
        }
    }
}
