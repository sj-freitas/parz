using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Parz.Nodes.Serilization;

namespace Parz.Nodes
{
    [Serializable]
    public class BinaryOperationNode : INode
    {
        public string Operator { get; internal set; }

        public INode Left { get; internal set; }

        public INode Right { get; internal set; }

        public BinaryOperationNode(string operatorSymbol, INode left, INode right)
        {
            Operator = operatorSymbol;
            Left = left;
            Right = right;
        }

        public INode ShallowClone()
        {
            return new BinaryOperationNode(Operator, Left, Right);
        }

        protected BinaryOperationNode(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            Operator = info.GetString(nameof(Operator).ToCamelCase());
            var left = (NodeContainer)info.GetValue(
                nameof(Left).ToCamelCase(),
                typeof(NodeContainer));
            var right = (NodeContainer)info.GetValue(
                nameof(Right).ToCamelCase(),
                typeof(NodeContainer));
            Left = left.Node;
            Right = right.Node;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(Operator).ToCamelCase(), Operator);
            info.AddValue(nameof(Left).ToCamelCase(), new NodeContainer(Left));
            info.AddValue(nameof(Right).ToCamelCase(), new NodeContainer(Right));
        }

        public override string ToString() => $"({Left} {Operator} {Right})";
    }
}
