using Parz.Nodes;

namespace Parz
{
    /// <summary>
    /// A parze engine is any type that can convert strings
    /// to INodes.
    /// </summary>
    public interface IEngine
    {
        INode Parse(string expression);
    }
}
