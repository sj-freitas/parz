using System.Collections.Generic;
using Parz.Models;
using Parz.Nodes;

namespace Parz.NodeConverters
{
    public interface INodeConverter
    {
        INode ToNode(IEnumerable<IToken> tokens, INodeConverter next);

        bool CanConvert(IEnumerable<IToken> tokens);
    }
}
