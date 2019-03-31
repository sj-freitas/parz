using System;
using System.Collections.Generic;
using System.Linq;
using Parz.Factories;
using ParzDefaults = Parz.Defaults;

namespace Parz.LambdaExpressions.Factories
{
    public class LambdaExpressionParser : ITokenTypeParser
    {
        private readonly IEnumerable<string> _supportedQuotations;
        private readonly StringComparison _stringComparison;

        public LambdaExpressionParser(IEnumerable<string> supportedQuotations = null,
            StringComparison equalityComparer = ParzDefaults.StringComparisonType)
        {
            _supportedQuotations = supportedQuotations ?? Defaults.QuotationMarks;
            _stringComparison = equalityComparer;
        }

        public bool TryParse(string symbol, out TokenType tokenType,
            out string adapted)
        {
            adapted = symbol ?? throw new ArgumentNullException(nameof(symbol));
            tokenType = LambdaTokenType.LambdaExpression;

            var quotationMatch = _supportedQuotations
                .Select(t => new
                {
                    QuotationStyle = t,
                    IsBetweenQuotationMarks =
                        symbol.StartsWith(t, _stringComparison) &&
                        symbol.EndsWith(t, _stringComparison)
                })
                .FirstOrDefault(t => t.IsBetweenQuotationMarks);

            var isInQuotationMarks = quotationMatch != null;

            if (!isInQuotationMarks)
            {
                return false;
            }

            var quotationStyleLength = quotationMatch.QuotationStyle.Length;
            adapted = symbol.Substring(
                quotationStyleLength, 
                symbol.Length - quotationStyleLength*2);

            return IsArrowFunction(adapted);
        }

        public static bool IsArrowFunction(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return false;
            }
            
            return expression.IndexOf("=>") > 0;
        }
    }
}
