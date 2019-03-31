using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Parz.Nodes.Serilization;

namespace Parz.Nodes
{
    [Serializable]
    public class UnaryOperationNode : INode
    {
        public string Operator { get; internal set; }

        public INode Node { get; internal set; }

        public UnaryOperationNode(string operatorSymbol, INode node)
        {
            Operator = operatorSymbol;
            Node = node;
        }

        public INode ShallowClone()
        {
            return new UnaryOperationNode(Operator, Node);
        }

        protected UnaryOperationNode(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            Operator = info.GetString(nameof(Operator).ToCamelCase());
            var node = (NodeContainer)info.GetValue(
                nameof(Node).ToCamelCase(),
                typeof(NodeContainer));
            Node = node.Node;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(Operator).ToCamelCase(), Operator);
            info.AddValue(nameof(Node).ToCamelCase(), new NodeContainer(Node));
        }

        public override string ToString() => $"({Operator}{Node})";
    }
}
