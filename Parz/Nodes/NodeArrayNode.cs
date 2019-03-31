using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Parz.Nodes.Serilization;

namespace Parz.Nodes
{
    [Serializable]
    public class NodeArrayNode : INode
    {
        public IEnumerable<INode> Nodes { get; internal set; }

        public NodeArrayNode(IEnumerable<INode> nodes)
        {
            Nodes = nodes;
        }

        public INode ShallowClone()
        {
            return new NodeArrayNode(Nodes);
        }

        protected NodeArrayNode(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }


            var nodes = (NodeContainer[])info.GetValue(
                nameof(Nodes).ToCamelCase(),
                typeof(NodeContainer[]));
            Nodes = nodes.Select(t => t.Node);
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            var nodes = this
                .GetFlattenedNodes()
                .Select(t => new NodeContainer(t))
                .ToArray();
            info.AddValue(nameof(Nodes).ToCamelCase(), nodes);
        }

        public override string ToString()
        {
            var parameters = this
                .GetFlattenedNodes()
                .Select(t => t.ToString());

            return $"{string.Join(", ", parameters)}";
        }
    }
}
