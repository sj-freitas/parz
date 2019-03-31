using System;
using System.Collections.Generic;
using System.Linq;
using Parz.Models;

namespace Parz.Factories
{
    public class AggregateTokenFactory : ITokenFactory
    {
        private readonly IEnumerable<ITokenTypeParser> _symbolParsers;

        public AggregateTokenFactory(params ITokenTypeParser[] symbolParsers)
            : this((IEnumerable<ITokenTypeParser>) symbolParsers)
        {
        }

        public AggregateTokenFactory(IEnumerable<ITokenTypeParser> symbolParsers)
        {
            _symbolParsers = symbolParsers;
        }

        public IToken ToToken(ILeveledToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var parsed = _symbolParsers
                .Select(t => new
                {
                    CanParse = t.TryParse(token.Token, out TokenType tokenType,
                        out string adapted),
                    TokenType = tokenType,
                    Symbol = adapted
                })
                .FirstOrDefault(t => t.CanParse);

            return new Token
            {
                Level = token.Level,
                Symbol = parsed.Symbol,
                TokenType = parsed.TokenType
            };
        }
    }
}
