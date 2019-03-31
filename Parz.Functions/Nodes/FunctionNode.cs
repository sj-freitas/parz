using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Parz.Nodes
{
    [Serializable]
    public class FunctionNode : INode
    {
        public string FunctionName { get; internal set; }

        public NodeArrayNode Arguments { get; internal set; }

        public FunctionNode(string functionName, IEnumerable<INode> arguments)
        {
            FunctionName = functionName;
            Arguments = new NodeArrayNode(arguments);
        }

        public INode ShallowClone()
        {
            return new FunctionNode(FunctionName, Arguments.Nodes);
        }

        protected FunctionNode(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            Arguments = (NodeArrayNode)info.GetValue(nameof(Arguments).ToCamelCase(), typeof(NodeArrayNode));
            FunctionName = info.GetString(nameof(FunctionName).ToCamelCase());
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(Arguments).ToCamelCase(), Arguments);
            info.AddValue(nameof(FunctionName).ToCamelCase(), FunctionName);
        }

        public override string ToString() => $"{FunctionName}({Arguments})";
    }
}
