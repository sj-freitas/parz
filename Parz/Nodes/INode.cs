using System;
using System.Runtime.Serialization;
using Parz.Nodes.Serilization;

namespace Parz.Nodes
{
    /// <summary>
    /// An abstract data structure that should represent any
    /// point of an expression. It is serializable so that it
    /// can be marshalled into any context.
    /// </summary>
    public interface INode : ISerializable, IShallowCloneable
    {
    }
}
