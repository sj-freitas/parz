using System;
using System.Collections.Generic;
using System.Linq;
using Parz.Models;
using Parz.Nodes;

namespace Parz.NodeConverters
{
    public class UnaryOperatorNodeConverter : INodeConverter
    {
        public bool CanConvert(IEnumerable<IToken> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            var indexOfNonOperator = tokens
                .IndexOf(t => t.Level != 0 ||
                    t.TokenType != ParzTokenType.Operator);

            // Check if the non operator is preceded by operators and
            // there is nothing after the non operator with the same
            // level.
            return indexOfNonOperator > 0 && !tokens
                .Skip(indexOfNonOperator + 1)
                .Where(t => t.Level == 0)
                .Any();
        }

        public INode ToNode(IEnumerable<IToken> tokens, INodeConverter next)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            var first = tokens.First();

            var operatorSymbol = first.Symbol;
            var node = next.ToNode(tokens.Skip(1), next);

            return new UnaryOperationNode(operatorSymbol, node);
        }
    }
}
