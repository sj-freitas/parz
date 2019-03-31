using System;
using System.Collections.Generic;
using System.Linq;
using Parz.Models;
using Parz.Nodes;

namespace Parz.NodeConverters
{
    public class BinaryOperatorNodeConverter : INodeConverter
    {
        private readonly IDictionary<string, int> _operators;

        /// <summary>
        /// Creates an instance of OperatorNodeConverter defining how
        /// the operators are mapped to priority.
        /// </summary>
        /// <param name="operators">A priority dictionary that will
        /// define which operators have the lowest or highest priority,
        /// the key is the symbol and the value is an integer, where 0
        /// should be + and -, and 0 should be * and -.</param>
        public BinaryOperatorNodeConverter(IDictionary<string, int> operators = null)
        {
            _operators = operators ?? Defaults.Operators;
        }

        public bool CanConvert(IEnumerable<IToken> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            // Has operand followed by operator followed by operand
            var indexOfOperand = tokens.IndexOf(t => 
                t.TokenType != ParzTokenType.Operator);
            if (indexOfOperand < 0)
            {
                return false;
            }
            var indexOfOperator = tokens
                .Skip(indexOfOperand)
                .IndexOf(t => t.Level == 0 &&
                    t.TokenType == ParzTokenType.Operator);
            if (indexOfOperator < 0)
            {
                return false;
            }
            return tokens
                .Skip(indexOfOperator + indexOfOperand)
                .Any(t => t.TokenType != ParzTokenType.Operator);
        }

        private class OperatorHelper
        {
            public int Index { get; set; }

            public int Priority { get; set; }

            public string Symbol { get; set; }
        }

        public INode ToNode(IEnumerable<IToken> tokens, INodeConverter next)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            var intermediateOperators = new List<OperatorHelper>();
            var currIndex = 0;
            var hasOperand = false;
            foreach (var curr in tokens)
            {
                if (curr.TokenType == ParzTokenType.Operator &&
                    hasOperand && curr.Level == 0)
                {
                    // Add and reset the operand, this a new operator
                    hasOperand = false;
                    intermediateOperators.Add(new OperatorHelper
                    {
                        Index = currIndex,
                        Priority = _operators[curr.Symbol],
                        Symbol = curr.Symbol
                    });
                }

                if (curr.TokenType != ParzTokenType.Operator)
                {
                    hasOperand = true;
                }
                currIndex++;
            }

            if (!intermediateOperators.Any())
            {
                // There is no operator at this level!
                return next.ToNode(tokens, next);
            }

            // Need to check the lowest priority Operator
            var lowestPriorityOperator = intermediateOperators
                .Aggregate((prev, curr) => prev.Priority < curr.Priority ?
                    prev : curr);

            var left = tokens.Take(lowestPriorityOperator.Index);
            var right = tokens.Skip(lowestPriorityOperator.Index + 1);
            
            return new BinaryOperationNode(lowestPriorityOperator.Symbol,
                next.ToNode(left, next),
                next.ToNode(right, next));
        }
    }
}
