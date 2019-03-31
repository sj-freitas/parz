using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Parz.Models;
using Parz.Nodes;

namespace Parz.NodeConverters
{
    public class ConstantNumberNodeConverter : INodeConverter
    {
        private readonly NumberStyles _numberStyles;
        private readonly IFormatProvider _formatProvider;
        private readonly Func<string, string> _adapt;

        public ConstantNumberNodeConverter(NumberStyles numberStyles = NumberStyles.Any,
            IFormatProvider formatProvider = null,
            Func<string, string> adapt = null)
        {
            _numberStyles = numberStyles;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
            _adapt = adapt ?? (t => t.Replace(',', '.'));
        }

        public bool CanConvert(IEnumerable<IToken> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }
            
            var firstIsConstant = tokens
                .First()
                .TokenType == ParzTokenType.Constant;
            var hasMore = tokens
                .Skip(1)
                .Any();
            return firstIsConstant && !hasMore;
        }

        public INode ToNode(IEnumerable<IToken> token, INodeConverter next)
        {
            return new ConstantNumberNode(decimal.Parse(
                _adapt(token.First().Symbol),
                _numberStyles, _formatProvider));
        }
    }
}
