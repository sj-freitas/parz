using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Parz.Nodes
{
    [Serializable]
    public class VariableNode : INode
    {
        public string Symbol { get; internal set; }

        public VariableNode(string symbol)
        {
            Symbol = symbol;
        }

        public INode ShallowClone()
        {
            return new VariableNode(Symbol);
        }

        protected VariableNode(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            Symbol = info.GetString(nameof(Symbol).ToCamelCase());
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(Symbol).ToCamelCase(), Symbol);
        }

        public override string ToString() => Symbol;
    }
}
