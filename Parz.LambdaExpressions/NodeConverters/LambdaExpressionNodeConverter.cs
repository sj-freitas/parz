using System;
using System.Collections.Generic;
using System.Linq;
using Parz.LambdaExpressions;
using Parz.LambdaExpressions.Nodes;
using Parz.Models;
using Parz.NodeConverters;
using Parz.Nodes;

namespace Parz.LambdaExpressions.NodeConverters
{
    public class LambdaExpressionNodeConverter : INodeConverter
    {
        public bool CanConvert(IEnumerable<IToken> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }
            
            var firstIsConstant = tokens
                .First()
                .TokenType == LambdaTokenType.LambdaExpression;
            var hasMore = tokens
                .Skip(1)
                .Any();
            return firstIsConstant && !hasMore;
        }

        public INode ToNode(IEnumerable<IToken> token, INodeConverter next)
        {
            return new LambdaExpressionNode(token.First().Symbol);
        }
    }
}
