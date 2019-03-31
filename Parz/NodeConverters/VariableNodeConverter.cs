using System;
using System.Collections.Generic;
using System.Linq;
using Parz.Models;
using Parz.Nodes;

namespace Parz.NodeConverters
{
    public class VariableNodeConverter : INodeConverter
    {
        public bool CanConvert(IEnumerable<IToken> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            var firstIsVariable = tokens
                .First()
                .TokenType == ParzTokenType.Variable;
            var hasMore = tokens
                .Skip(1)
                .Any();
            return firstIsVariable && !hasMore;
        }

        public INode ToNode(IEnumerable<IToken> token, INodeConverter next)
        {
            return new VariableNode(token.First().Symbol);
        }
    }
}
