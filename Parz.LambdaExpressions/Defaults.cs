using System.Collections.Generic;
using System.Linq;

namespace Parz.LambdaExpressions
{
    public static class Defaults
    {
        public static readonly IEnumerable<string> QuotationMarks = new[]
        {
            "\"",
            "'",
            "`"
        };

        public static readonly IEnumerable<string> ArrowMarks = new[]
        {
            "=>",
            "->"
        };

        public static readonly IEnumerable<string> LambdaSeparators = QuotationMarks
            .Concat(ArrowMarks);
    }
}
