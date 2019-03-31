using System;
using System.Collections.Generic;
using System.Linq;
using Parz.Models;
using Parz.NodeConverters;
using Parz.Nodes;

namespace Parz
{
    public static class TokenExntesions
    {
        /// <summary>
        /// Helper function that creates a new expression where the minimum
        /// level is zero.
        /// </summary>
        /// <param name="tokens">The expression to normalize.</param>
        /// <returns>A new immutable expression.</returns>
        public static IEnumerable<IToken> NormalizeLevel(this IEnumerable<IToken> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }
            if (!tokens.Any())
            {
                return tokens;
            }

            var lowestLevel = tokens
                .Select(t => t.Level)
                .Aggregate((curr, next) =>
                    curr < next ?
                        curr :
                        next);

            return tokens.Select(t => new Token
            {
                Level = t.Level - lowestLevel,
                Symbol = t.Symbol,
                TokenType = t.TokenType
            });
        }

        public static INode Treeify(this IEnumerable<IToken> tokens,
            INodeConverter converter)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            if (!tokens.Any())
            {
                throw new InvalidOperationException("Cannot treefy an empty expression!");
            }

            return converter.ToNode(tokens, converter);
        }
    }
}
