using System;

namespace Parz.Factories
{
    public class VariableParser : ITokenTypeParser
    {
        public bool TryParse(string symbol, out TokenType tokenType,
            out string adapted)
        {
            adapted = symbol ?? throw new ArgumentNullException(nameof(symbol));
            tokenType = ParzTokenType.Variable;

            return true;
        }
    }
}
