using System;
using System.Collections.Generic;
using System.Linq;
using Parz.Factories;
using Parz.Models;
using Parz.NodeConverters;
using Parz.Nodes;

namespace Parz
{
    /// <summary>
    /// A simple engine that uses all of the different components
    /// of the Parz library.
    /// </summary>
    public class ParzEngine : IEngine
    {
        private readonly Func<IEnumerable<string>, IEnumerable<ILeveledToken>> _toLevelTokens;
        private readonly IEnumerable<string> _separators;
        private readonly ITokenFactory _tokenFactory;
        private readonly INodeConverter _nodeConverter;

        public ParzEngine(IEnumerable<string> separators, ITokenFactory tokenFactory,
            INodeConverter nodeConverter, 
            Func<IEnumerable<string>, IEnumerable<ILeveledToken>> toLevelTokens = null)
        {
            _separators = separators;
            _tokenFactory = tokenFactory;
            _nodeConverter = nodeConverter;
            _toLevelTokens = toLevelTokens ?? ((t) => t.ToLevelTokens());
        }

        public INode Parse(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var splitExpression = expression
                .SplitTokens(_separators);
            var leveledTokens = _toLevelTokens(splitExpression);
            var toknified = leveledTokens
                .Tokenify(_tokenFactory)
                .ToList();

            return toknified.Treeify(_nodeConverter);
        }
    }
}
