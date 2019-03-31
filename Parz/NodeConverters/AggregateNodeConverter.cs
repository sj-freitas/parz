using System;
using System.Collections.Generic;
using System.Linq;
using Parz.Models;
using Parz.Nodes;

namespace Parz.NodeConverters
{
    public class AggregateNodeConverter : INodeConverter
    {
        private readonly IEnumerable<INodeConverter> _converters;

        public AggregateNodeConverter(params INodeConverter[] converters)
            : this ((IEnumerable<INodeConverter>)converters)
        {
        }

        public AggregateNodeConverter(IEnumerable<INodeConverter> converters)
        {
            _converters = converters;
        }

        public bool CanConvert(IEnumerable<IToken> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }
            return true;
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

            var converter = _converters
                .Where(t => t.CanConvert(tokens))
                .FirstOrDefault();

            if (converter == null)
            {
                throw new InvalidOperationException(
                    $"Expression \"{string.Join(" ", tokens.Select(t => t.Symbol))}\" " +
                    "is unconvertible, check if it has any unrecognized tokens.");
            }

            return converter.ToNode(tokens, next);
        }
    }
}
