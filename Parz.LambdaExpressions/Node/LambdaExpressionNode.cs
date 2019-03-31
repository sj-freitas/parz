using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Parz.Nodes;

namespace Parz.LambdaExpressions.Nodes
{
    [Serializable]
    public class LambdaExpressionNode : INode
    {
        public string LambdaExpression { get; internal set; }

        public LambdaExpressionNode(string lambdaExpression)
        {
            LambdaExpression = lambdaExpression;
        }

        public INode ShallowClone()
        {
            return new LambdaExpressionNode(LambdaExpression);
        }

        protected LambdaExpressionNode(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            LambdaExpression = info.GetString(nameof(LambdaExpression).ToCamelCase());
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(LambdaExpression).ToCamelCase(), LambdaExpression);
        }

        public override string ToString() => $"'{LambdaExpression}'";
    }
}
