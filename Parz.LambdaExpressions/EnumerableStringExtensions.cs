using System;
using System.Collections.Generic;
using System.Linq;
using ParzDefaults = Parz.Defaults;

namespace Parz.LambdaExpressions
{
    public static class EnumerableStringExtensions
    {
        public static IEnumerable<string> AggregateByQuotationMarks(
            this IEnumerable<string> tokens, IEnumerable<string> separators = null,
            IEqualityComparer<string> equalityComparer = null)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            separators = separators ?? Defaults.QuotationMarks;
            equalityComparer = equalityComparer ?? ParzDefaults.StringEqualityComparer;
            foreach (var currSeparator in separators)
            {
                tokens = tokens.AggreageteByQuotationMark(currSeparator, equalityComparer);
            }

            return tokens;
        }

        private static IEnumerable<string> AggreageteByQuotationMark(
            this IEnumerable<string> tokens, string currentQuotationMarkStyle,
            IEqualityComparer<string> equalityComparer)
        {
            var currIdx = 0;
            var skipCount = 0;

            foreach (var curr in tokens)
            {
                if (equalityComparer.Equals(curr, currentQuotationMarkStyle) &&
                    skipCount <= 0)
                {
                    // Needs to be transformed!
                    var skipped = tokens.Skip(currIdx + 1);
                    var indexOfEnd = skipped.IndexOf(t => equalityComparer
                        .Equals(t, currentQuotationMarkStyle));

                    if (indexOfEnd >= 0)
                    {
                        var segment = string.Join(" ", skipped.Take(indexOfEnd));

                        yield return segment.ToQuote(currentQuotationMarkStyle);

                        // 2 because we have 2x the quote character to add
                        // as an offset.
                        skipCount = indexOfEnd + 2;
                    }
                }

                // Render the current element unless being
                // ordered to skip it.
                if (--skipCount < 0)
                {
                    yield return curr;
                }
                currIdx++;
            }
        }

        private static string ToQuote(this string text, string quoteChar)
        {
            return $"{quoteChar}{text}{quoteChar}";
        }
    }
}
