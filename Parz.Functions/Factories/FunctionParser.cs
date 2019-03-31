using System;
using System.Collections.Generic;
using System.Linq;
using Parz.Factories;

namespace Parz.Functions.Factories
{
    /// <summary>
    /// Parses any symbol that matches any of the function names.
    /// Multiple parsers of this type can be supported with other
    /// function names.
    /// </summary>
    public class FunctionParser : ITokenTypeParser
    {
        private readonly IEnumerable<string> _supportedFunctions;
        private readonly IEqualityComparer<string> _stringComparison;

        public FunctionParser(params string[] supportedFunctions)
            : this(StringComparer.CurrentCultureIgnoreCase, supportedFunctions)
        {
        }

        public FunctionParser(IEqualityComparer<string> stringComparison,
            params string[] supportedFunctions)
            : this(stringComparison, (IEnumerable<string>) supportedFunctions)
        {
        }

        public FunctionParser(IEqualityComparer<string> stringComparison,
            IEnumerable<string> supportedFunctions)
        {
            _supportedFunctions = supportedFunctions;
            _stringComparison = stringComparison;
        }

        public bool TryParse(string symbol, out TokenType tokenType,
            out string adapted)
        {
            adapted = symbol ?? throw new ArgumentNullException(nameof(symbol));
            tokenType = FuncTokenType.Function;

            return _supportedFunctions.Contains(symbol, _stringComparison);
        }
    }
}
