using System.Collections.Generic;
using System.Linq;

namespace Parz.Nodes
{
    public static class NodeArrayNodeExtensions
    {
        public static IEnumerable<INode> GetFlattenedNodes(this NodeArrayNode arrayNode)
        {
            return arrayNode
                .Nodes
                .SelectMany(t => (t is NodeArrayNode array) ?
                    array.GetFlattenedNodes() :
                    new[] { t });
        }
    }
}
