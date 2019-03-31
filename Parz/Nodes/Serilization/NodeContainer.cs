using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Parz.Nodes.Serilization
{
    /// <summary>
    /// A helper class that stores the metadata relative to a node,
    /// since the node type is unknown during serialization.
    /// </summary>
    [Serializable]
    public class NodeContainer : ISerializable
    {
        private const string NodeTypeKey = "nodeType";

        public INode Node { get; }

        public NodeContainer(INode node)
        {
            Node = node;
        }

        protected NodeContainer(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            var nodeType = (Type)info.GetValue(NodeTypeKey, typeof(Type));
            Node = (INode)info.GetValue(nameof(Node).ToCamelCase(), nodeType);
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(NodeTypeKey, Node.GetType());
            info.AddValue(nameof(Node).ToCamelCase(), Node);
        }
    }
}
