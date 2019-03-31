using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Parz
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (value.Length == 0)
            {
                return value;
            }

            return $"{char.ToLowerInvariant(value[0])}{value.Substring(1)}";
        }

        public static IEnumerable<string> SplitTokens(this string expression,
            IEnumerable<string> knownSeparators, char splitAggregator = '\'',
            StringComparison comparer = Defaults.StringComparisonType)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return expression
                .Split(Array.Empty<char>())
                .SelectMany(t => t.SeparateNested(knownSeparators, comparer))
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => t.Trim());
        }

        private static IEnumerable<string> SeparateNested(this string expression,
            IEnumerable<string> knownSeparators, StringComparison comparer)
        {
            var aggregatorSeed = new
            {
                Index = -1,
                Symbol = default(string)
            };

            // Largest separators should be first to avoid unnecessary conflicts.
            knownSeparators = knownSeparators.OrderByDescending(t => t.Length);

            while (true)
            {
                var nextOperator = knownSeparators
                    .Select(t => new
                    {
                        Index = expression.IndexOf(t, comparer),
                        Symbol = t
                    })
                    .Aggregate(aggregatorSeed, (curr, next) =>
                    {
                        if (curr.Index < 0)
                        {
                            return next;
                        }

                        if (next.Index < 0)
                        {
                            return curr;
                        }

                        return next.Index < curr.Index ? next : curr;
                    });

                if (nextOperator.Index < 0)
                {
                    // No more operators, nothing else to do!
                    yield return expression;

                    break;
                }

                yield return expression.Substring(0, nextOperator.Index);
                yield return nextOperator.Symbol;

                expression = expression.Substring(
                    nextOperator.Index +
                    nextOperator.Symbol.Length);
            }
        }
    }
}
