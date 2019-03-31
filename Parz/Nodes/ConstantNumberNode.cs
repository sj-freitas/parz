using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Parz.Nodes
{
    [Serializable]
    public class ConstantNumberNode : INode
    {
        public decimal Value { get; internal set; }

        public ConstantNumberNode(decimal value)
        {
            Value = value;
        }

        public INode ShallowClone()
        {
            return new ConstantNumberNode(Value);
        }

        protected ConstantNumberNode(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            Value = info.GetDecimal(nameof(Value).ToCamelCase());
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(Value).ToCamelCase(), Value);
        }

        public override string ToString() => $"{Value}";
    }
}
