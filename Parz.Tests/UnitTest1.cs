using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Newtonsoft.Json;
using Parz.Factories;
using Parz.Functions.Factories;
using Parz.Functions.NodeConverters;
using Parz.LambdaExpressions.NodeConverters;
using Parz.LambdaExpressions;
using Parz.LambdaExpressions.Factories;
using Parz.LambdaExpressions.Nodes;
using Parz.NodeConverters;
using Parz.Nodes;
using Parz.Nodes.Serilization;
using Xunit;
using FuncDefaults = Parz.Functions.Defaults;
using LambdaDefaults = Parz.LambdaExpressions.Defaults;

namespace Parz.Tests
{
    public static class NodeExtensions
    {
        public static IEnumerable<T> GetAll<T>(this INode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (node is T targetNode)
            {
                return new T[] { targetNode };
            }

            var propertiesOfNode = node
                .GetType()
                .GetProperties();
            var fromReference = propertiesOfNode
                .Where(t => typeof(INode).IsAssignableFrom(t.PropertyType))
                .Select(t => (INode)t.GetValue(node))
                .SelectMany(t => t.GetAll<T>());

            var fromLists = propertiesOfNode
                .Where(t => typeof(IEnumerable<INode>).IsAssignableFrom(t.PropertyType))
                .Select(t => (IEnumerable<INode>)t.GetValue(node))
                .SelectMany(nodes => nodes.SelectMany(t => t.GetAll<T>()));

            return fromReference.Concat(fromLists);
        }
    }

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var tokens = "+/*".Split();
            var result = "1 + 3 / 5 +(52/3)+56 * min(4,6,7)".SplitTokens(tokens).ToArray();

            Console.WriteLine(string.Join(Environment.NewLine, result));
        }

        private static IDictionary<string, int> Operators = new Dictionary<string, int>
        {
            { "+", 0 },
            { "-", 0 },
            { "*", 1 },
            { "/", 1 },
            { "^", 2 },
        };

        [Fact]
        public void Test2()
        {
            var parser = new ParzEngine(
                toLevelTokens: tokens => tokens
                    .AggregateByQuotationMarks(LambdaDefaults.QuotationMarks)
                    .ToLevelTokens(),
                separators: LambdaDefaults
                    .QuotationMarks
                    .Concat(FuncDefaults.FunctionSeparators)
                    .Concat(Defaults.Separators),
                tokenFactory: new AggregateTokenFactory(
                    new ConstantNumberParser(),
                    new OperatorParser(),
                    new OpenerParser(),
                    new CloserParser(),
                    new SeparatorParser(),
                    new FunctionParser("avg", "sum", "min", "max"),
                    new LambdaExpressionParser(),
                    new VariableParser()),
                nodeConverter: new AggregateNodeConverter(
                    new OpenerNodeConverter(),
                    new BinaryOperatorNodeConverter(),
                    new UnaryOperatorNodeConverter(),
                    new FunctionNodeConverter(),
                    new ConstantNumberNodeConverter(),
                    new VariableNodeConverter(),
                    new LambdaExpressionNodeConverter()));

            var expression = "1 + 3 / 5 +(52/3 + (-5))+56 * min(4, 3, 6 * 2 ,7) + min(0 + max(1, min(2, min(3, 4))))";

            var result = parser.Parse(expression);
            var decimalResult = Evaluate(result);
        }

        [Fact]
        public void Test3()
        {
            var operators = Operators.Keys;
            var factory = new AggregateTokenFactory(
                new ConstantNumberParser(),
                new OperatorParser(operators),
                new OpenerParser(),
                new CloserParser(),
                new VariableParser());

            var nodeConverter = new AggregateNodeConverter(
                new ConstantNumberNodeConverter(),
                new BinaryOperatorNodeConverter(Operators),
                new OpenerNodeConverter(),
                new VariableNodeConverter());

            var tokens = "(2 + 2) * 2 + rui"
                .SplitTokens(operators.Concat(Defaults.Separators))
                .ToLevelTokens()
                .Tokenify(factory)
                .ToList();

            var result = tokens
                .Treeify(nodeConverter);

            Console.WriteLine(result);
        }

        [Fact]
        public void Test4()
        {
            var operators = Operators.Keys;
            var factory = new AggregateTokenFactory(
                new ConstantNumberParser(),
                new OperatorParser(operators),
                new OpenerParser(),
                new CloserParser(),
                new FunctionParser("min"),
                new SeparatorParser(),
                new LambdaExpressionParser(LambdaDefaults.QuotationMarks),
                new VariableParser());

            var nodeConverter = new AggregateNodeConverter(
                new ConstantNumberNodeConverter(),
                new BinaryOperatorNodeConverter(Operators),
                new OpenerNodeConverter(),
                new LambdaExpressionNodeConverter(),
                new VariableNodeConverter());

            var result = "(2 + 2) * 2 + 't => t.Cenas == \"Sérgio\"' + 5"
                .SplitTokens(operators
                    .Concat(LambdaDefaults.QuotationMarks)
                    .Concat(Defaults.Separators))
                .AggregateByQuotationMarks(LambdaDefaults.QuotationMarks)
                .ToLevelTokens()
                .Tokenify(factory)
                .Treeify(nodeConverter);
        }

        [Fact]
        public void Test5()
        {
            var equalsOperator = new[] { "==" };
            var exponentialOperator = new[] { "^" };
            var extraOpeners = new[] { "<" };
            var extraClosers = new[] { ">" };
            var parser = new ParzEngine(
                toLevelTokens: tokens => tokens
                    .AggregateByQuotationMarks(LambdaDefaults.QuotationMarks)
                    .ToLevelTokens(
                        Defaults.Openers.Concat(extraOpeners),
                        Defaults.Closers.Concat(extraClosers)),
                separators: FuncDefaults
                    .FunctionSeparators
                    .Concat(LambdaDefaults.LambdaSeparators)
                    .Concat(equalsOperator)
                    .Concat(exponentialOperator)
                    .Concat(Defaults.Separators)
                    .Concat(extraOpeners)
                    .Concat(extraClosers),
                tokenFactory: new AggregateTokenFactory(
                    new ConstantNumberParser(),
                    new OperatorParser(Defaults.OperatorSymbols
                        .Concat(equalsOperator)
                        .Concat(exponentialOperator)),
                    new OpenerParser(Defaults.Openers
                        .Concat(extraOpeners)),
                    new CloserParser(Defaults.Closers
                        .Concat(extraClosers)),
                    new SeparatorParser(),
                    new FunctionParser("avg", "sum", "min", "max"),
                    new LambdaExpressionParser(),
                    new VariableParser()),
                nodeConverter: new AggregateNodeConverter(
                    new OpenerNodeConverter(),
                    new BinaryOperatorNodeConverter(Defaults.Operators
                        .AddEntry(equalsOperator.First(), -1)
                        .AddEntry(exponentialOperator.Single(), 2)
                        .ToDictionary(Defaults.StringEqualityComparer)),
                    new UnaryOperatorNodeConverter(),
                    new FunctionNodeConverter(),
                    new ConstantNumberNodeConverter(),
                    new VariableNodeConverter(),
                    new LambdaExpressionNodeConverter()));

            //var expression = "min(avg('t => t.Money >= 500'), allValues) ^ 2";
            //var expression = "(5+4)*max(4,-1-5.3435345,'t => cenas',sum(4.323,5,dasdasd,5,ddd,4))";
            var expression = "(5+4)*max(4,-1-5.3435345,'t => a.Coisas == \"cenas\"',sum(4.323,5,dasdasd,5,ddd,4))";
            //var expression = "(5+4)*max(--3----5)+max(4,-1-5.3435345,'t => cenas',sum(asd+4.32*3,5,dasdasd,5,ddd,4))";
            //var expression = "((5+4)*(--3----5))-4--1-5.3435345+(4.32*3-5+4)+(4---(4*3)*--5)";
            //var expression = "(5+4)*max(--3----5)+max(4,-1-5.3435345,avg(3,57,8),sum(4.32*3,5,4))";
            //var expression = "(5+4)*sum(5,8*avg(4,5,-------6,7),9,15)";

            var result = parser.Parse(expression);
            var variableNames = result
                .GetAll<VariableNode>()
                .Select(t => t.Symbol)
                .ToList();

            var values = new Dictionary<string, int>
            {
                { "dasdasd", 69 },
                { "ddd", 42 }
            };

            var replaced = result
                .ReplaceNodes<VariableNode>(t => new ConstantNumberNode(values[t.Symbol]))
                .ReplaceNodes<LambdaExpressionNode>(t => new NodeArrayNode(new INode[] 
                {
                    new ConstantNumberNode(5m),
                    new ConstantNumberNode(900m),
                    new ConstantNumberNode(8m),
                }));

            var root = new Root<string>(replaced, "banana");
            var lel = JsonConvert.SerializeObject(root);

            var resultDecimal = Evaluate(root.RootNode);

            var asObj = JsonConvert.DeserializeObject<Root<string>>(lel);
        }

        public static Func<decimal, decimal, decimal> MapSymbolToOperation(string symbol)
        {
            switch(symbol)
            {
                case ("+"):
                    return (a, b) => a + b;
                case ("-"):
                    return (a, b) => a - b;
                case ("*"):
                    return (a, b) => a * b;
                case ("/"):
                    return (a, b) => a / b;
                default:
                    throw new Exception();
            }
        }

        public static Func<IEnumerable<decimal>, decimal> MapSymbolToFunction(string symbol)
        {
            decimal Sum(IEnumerable<decimal> values) => values.Aggregate((prev, curr) => curr + prev);
            var mappedFunctions = new Dictionary<string, Func<IEnumerable<decimal>, decimal>>
                (StringComparer.InvariantCultureIgnoreCase)
            {
                { "avg", (values) => Sum(values) / values.Count() },
                { "max", (values) => values.Aggregate((prev, curr) => curr > prev? curr : prev) },
                { "min", (values) => values.Aggregate((prev, curr) => curr < prev? curr : prev) },
                { "sum", Sum }
            };

            return mappedFunctions[symbol];
        }

        public static decimal Evaluate(INode node)
        {
            if (node is ConstantNumberNode constantNode)
            {
                return constantNode.Value;
            }
            if (node is NodeArrayNode nodeArray)
            {
                // There are more nodes than expected, this is fine,
                // However, it should be aggregated as the first value,
                // this throws an exception if more than one value exists.
                return Evaluate(nodeArray.Nodes.Single());
            }
            if (node is UnaryOperationNode unaryNode)
            {
                var function = MapSymbolToOperation(unaryNode.Operator);

                return function(0m, Evaluate(unaryNode.Node));
            }
            if (node is BinaryOperationNode binaryNode)
            {
                var function = MapSymbolToOperation(binaryNode.Operator);

                return function(Evaluate(binaryNode.Left), Evaluate(binaryNode.Right));
            }
            if (node is FunctionNode functionNode)
            {
                var function = MapSymbolToFunction(functionNode.FunctionName);
                var values = functionNode
                    .Arguments
                    .GetFlattenedNodes()
                    .Select(t => Evaluate(t));

                return function(values);
            }

            throw new Exception();
        }

        [Serializable]
        public class Root<TPayload> : ISerializable
        {
            public TPayload Payload { get; }
            public INode RootNode { get; }

            public string Expression => RootNode.ToString();

            public Root(INode root, TPayload payload)
            {
                Payload = payload;
                RootNode = root;
            }

            protected Root(SerializationInfo info, StreamingContext context)
            {
                if (info == null)
                {
                    throw new ArgumentNullException(nameof(info));
                }

                var container = (NodeContainer)info.GetValue(
                    nameof(RootNode).ToCamelCase(),
                    typeof(NodeContainer));
                RootNode = container.Node;
                Payload = (TPayload)info.GetValue(nameof(Payload).ToCamelCase(), typeof(TPayload));
            }

            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                if (info == null)
                {
                    throw new ArgumentNullException(nameof(info));
                }

                info.AddValue(nameof(RootNode).ToCamelCase(), new NodeContainer(RootNode));
                info.AddValue(nameof(Payload).ToCamelCase(), Payload);
                info.AddValue(nameof(Expression).ToCamelCase(), Expression);
            }
        }
    }
}
