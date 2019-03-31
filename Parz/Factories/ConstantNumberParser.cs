using System;
using System.Globalization;

namespace Parz.Factories
{
    public class ConstantNumberParser : ITokenTypeParser
    {
        private readonly NumberStyles _numberStyles;
        private readonly IFormatProvider _formatProvider;
        private readonly Func<string, string> _adapt;

        public ConstantNumberParser(NumberStyles numberStyles = NumberStyles.Any, 
            IFormatProvider formatProvider = null,
            Func<string, string> adapt = null)
        {
            _numberStyles = numberStyles;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
            _adapt = adapt ?? (t => t.Replace(',','.'));
        }

        public bool TryParse(string symbol, out TokenType tokenType,
            out string adapted)
        {
            adapted = symbol ?? throw new ArgumentNullException(nameof(symbol));
            tokenType = ParzTokenType.Constant;

            var canParse = decimal
                .TryParse(_adapt(symbol), _numberStyles, _formatProvider,
                    out decimal result);

            adapted = canParse ? 
                result.ToString(CultureInfo.InvariantCulture) :
                adapted;

            return canParse;
        }
    }
}
