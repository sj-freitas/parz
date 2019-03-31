using System;
using System.Collections.Generic;
using System.Linq;
using Parz.Factories;
using Parz.NodeConverters;

namespace Parz
{
    public static class Defaults
    {
        public const StringComparison StringComparisonType = StringComparison.InvariantCultureIgnoreCase;

        public static readonly IEqualityComparer<string> StringEqualityComparer =
            StringComparer.InvariantCultureIgnoreCase;

        private static readonly IDictionary<string, int> _operatorsMap =
            new Dictionary<string, int>
            {
                { "+", 0 },
                { "-", 0 },
                { "*", 1 },
                { "/", 1 },
            }
            .ToDictionary(t => t.Key, t => t.Value, StringEqualityComparer);

        public static readonly IEnumerable<string> Openers = new[]
        {
            "(",
            "[",
            "{"
        };
        public static readonly IEnumerable<string> Closers = new[]
        {
            ")",
            "]",
            "}"
        };
        public static readonly IEnumerable<string> Separators = Openers
            .Concat(Closers)
            .Concat(_operatorsMap.Keys);

        public static IDictionary<string, int> Operators => _operatorsMap
            .ToDictionary(t => t.Key, t => t.Value, StringEqualityComparer);

        public static readonly IEnumerable<string> OperatorSymbols = _operatorsMap.Keys;

        public static IEnumerable<Func<IEnumerable<string>, IEnumerable<string>>> PreProcessors =>
            Enumerable.Empty<Func<IEnumerable<string>, IEnumerable<string>>>();

        /// <summary>
        /// Gets the default token parsers. Keep in mind that this includes
        /// the variable parser, which will always default to true, so if
        /// you wish to add your own parsers, you have to concat with these
        /// instead of the other way around, always have your custom parsers
        /// in the begining.
        /// </summary>
        public static IEnumerable<ITokenTypeParser> TokenTypeParsers
        {
            get
            {
                yield return new ConstantNumberParser();
                yield return new OperatorParser(OperatorSymbols);
                yield return new OpenerParser();
                yield return new CloserParser();
                yield return new VariableParser();
            }
        }

        public static IEnumerable<INodeConverter> NodeConverters
        {
            get
            {
                yield return new ConstantNumberNodeConverter();
                yield return new BinaryOperatorNodeConverter(_operatorsMap);
                yield return new OpenerNodeConverter();
                yield return new VariableNodeConverter();
            }
        }
    }
}
