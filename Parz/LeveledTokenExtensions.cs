using System;
using System.Collections.Generic;
using System.Linq;
using Parz.Factories;
using Parz.Models;

namespace Parz
{
    public static class LeveledTokenExtensions
    {
        public static IEnumerable<IToken> Tokenify(this IEnumerable<ILeveledToken> tokens,
            ITokenFactory tokenFactory)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            return tokens.Select(tokenFactory.ToToken);
        }

        public static IEnumerable<ILeveledToken> ToLevelTokens(this IEnumerable<string> tokens,
            IEnumerable<string> openers = null, IEnumerable<string> closers = null,
            IEqualityComparer<string> equalityComparer = null)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            openers = openers ?? Defaults.Openers;
            closers = closers ?? Defaults.Closers;
            equalityComparer = equalityComparer ?? Defaults.StringEqualityComparer;

            var closersAndOpeners = openers
                .Zip(closers, (opener, closer) => new KeyValuePair<string, string>(opener, closer))
                .ToDictionary(t => t.Key, t => t.Value, equalityComparer);

            var level = 0;
            var expectingClosers = new Stack<string>();
            foreach (var curr in tokens)
            {
                if (openers.Contains(curr, equalityComparer))
                {
                    expectingClosers.Push(closersAndOpeners[curr]);
                    level++;
                }

                yield return new LeveledToken
                {
                    Level = level,
                    Token = curr
                };

                if (closers.Contains(curr, equalityComparer))
                {
                    var expectedCloser = expectingClosers.Pop();

                    if (!equalityComparer.Equals(curr, expectedCloser))
                    {
                        var correspondingOpener = closersAndOpeners
                            .First(t => t.Value == expectedCloser)
                            .Key;

                        throw new InvalidOperationException(
                            $"The closing bracket \"{curr}\" does not close \"{correspondingOpener}\"!");
                    }

                    level--;
                }
            }
        }
    }
}
