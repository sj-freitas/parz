using System;
using System.Collections.Generic;
using System.Linq;

namespace Parz.Factories
{
    /// <summary>
    /// Parses any symbol that matches the single character operator.
    /// Multiple parsers of this type can be supported with other
    /// operators.
    /// </summary>
    public class OperatorParser : ITokenTypeParser
    {
        private readonly IEnumerable<string> _supportedOperators;
        private readonly IEqualityComparer<string> _equalityComparer;

        public OperatorParser(IEnumerable<string> supportedOperators = null,
            IEqualityComparer<string> equalityComparer = null)
        {
            _supportedOperators = supportedOperators ?? Defaults.OperatorSymbols;
            _equalityComparer = equalityComparer ?? Defaults.StringEqualityComparer;
        }

        public bool TryParse(string symbol, out TokenType tokenType,
            out string adapted)
        {
            adapted = symbol ?? throw new ArgumentNullException(nameof(symbol));
            tokenType = ParzTokenType.Operator;

            return _supportedOperators.Contains(symbol, _equalityComparer); 
        }
    }
}
