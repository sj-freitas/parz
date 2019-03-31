using System;
using System.Collections.Generic;
using System.Linq;
using Parz.Factories;
using ParzDefaults = Parz.Defaults;

namespace Parz.Functions.Factories
{
    public class SeparatorParser : ITokenTypeParser
    {
        private readonly IEnumerable<string> _supportedFunctionSeparators;
        private readonly IEqualityComparer<string> _equalityComparer;

        public SeparatorParser(IEnumerable<string> supportedFunctionSeparators = null,
            IEqualityComparer<string> equalityComparer = null)
        {
            _supportedFunctionSeparators = supportedFunctionSeparators ?? Defaults.FunctionSeparators;
            _equalityComparer = equalityComparer ?? ParzDefaults.StringEqualityComparer;
        }

        public bool TryParse(string symbol, out TokenType tokenType,
            out string adapted)
        {
            adapted = symbol ?? throw new ArgumentNullException(nameof(symbol));
            tokenType = FuncTokenType.Separator;

            return _supportedFunctionSeparators.Contains(symbol, _equalityComparer);
        }
    }
}
