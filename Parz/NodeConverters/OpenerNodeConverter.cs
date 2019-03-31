using System;
using System.Collections.Generic;
using System.Linq;
using Parz.Models;
using Parz.Nodes;

namespace Parz.NodeConverters
{
    public class OpenerNodeConverter : INodeConverter
    {
        public bool CanConvert(IEnumerable<IToken> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            var first = tokens.First();
            if (first.TokenType != ParzTokenType.Opener)
            {
                return false;
            }

            // Find the location of the closing tag
            var idx = 1;
            foreach (var curr in tokens.Skip(1))
            {
                if (curr.TokenType == ParzTokenType.Closer &&
                    curr.Level == first.Level)
                {
                    break;
                }
                idx++;
            }

            // Check if it's the expression's end
            var count = tokens.Count();
            return idx == count - 1;
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

            // Find the location of the closing tag
            var idx = 1;
            var first = tokens.First();
            foreach (var curr in tokens.Skip(1))
            {
                if (curr.TokenType == ParzTokenType.Closer &&
                    curr.Level == first.Level)
                {
                    break;
                }
                idx++;
            }

            // Check if it's the expression's end
            var count = tokens.Count();
            if (idx != count - 1)
            {
                return next.ToNode(tokens, next);
            }

            // Trim the parentheses. Normalize the level
            // to avoid any inconsistencies.
            tokens = tokens
                .Skip(1)
                .Take(count - 2)
                .NormalizeLevel();

            return next.ToNode(tokens, next);
        }
    }
}
