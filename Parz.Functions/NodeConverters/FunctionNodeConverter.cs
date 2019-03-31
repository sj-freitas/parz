using System;
using System.Collections.Generic;
using System.Linq;
using Parz.Models;
using Parz.NodeConverters;
using Parz.Nodes;

namespace Parz.Functions.NodeConverters
{
    public class FunctionNodeConverter : INodeConverter
    {
        public bool CanConvert(IEnumerable<IToken> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }
            return tokens.First().TokenType == FuncTokenType.Function;
        }

        public INode ToNode(IEnumerable<IToken> tokens, INodeConverter next)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            var first = tokens.First();

            tokens = tokens.Skip(1);
            var second = tokens.First();
            if (second.TokenType != ParzTokenType.Opener)
            {
                throw new InvalidOperationException(
                    "Functions must be followed by parentheses!");
            }

            var currentLevel = second.Level;
            var leftIdx = 0;
            var length = 0;

            tokens = tokens.Skip(1);

            var functionNodes = new List<IEnumerable<IToken>>();
            foreach (var curr in tokens)
            {
                if (curr.TokenType == ParzTokenType.Closer &&
                    curr.Level == currentLevel)
                {
                    break;
                }

                if (curr.TokenType == FuncTokenType.Separator &&
                    curr.Level == currentLevel)
                {
                    var segment = tokens
                        .Skip(leftIdx)
                        .Take(length);

                    functionNodes.Add(segment);

                    // Need to skip the separator
                    leftIdx += length + 1;
                    length = 0;
                }
                else
                {
                    length++;
                }
            }
            var lastSegment = tokens
                .Skip(leftIdx)
                .Take(length);

            // Funcions can be parameterless.
            if (tokens.Any())
            {
                functionNodes.Add(lastSegment);
            }

            var arguments = functionNodes
                .Select(t => t.NormalizeLevel())
                .Select(t => next.ToNode(t, next));
            return new FunctionNode(first.Symbol, arguments);
        }
    }
}
