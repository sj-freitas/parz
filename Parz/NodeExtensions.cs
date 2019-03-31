using System;
using System.Collections.Generic;
using System.Linq;
using Parz.Nodes;

namespace Parz
{
    public static class NodeExtensions
    {
        /// <summary>
        /// Recursively gets all values of the specific node type.
        /// </summary>
        /// <typeparam name="T">The node type to get.</typeparam>
        /// <param name="node">The root node instance.</param>
        /// <returns>All the instances of the node type.</returns>
        public static IEnumerable<T> GetAll<T>(this INode node)
            where T : INode
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (node is T targetNode)
            {
                return new T[] { targetNode };
            }

            var propertiesOfNode = node
                .GetType()
                .GetProperties();
            var fromReference = propertiesOfNode
                .Where(t => typeof(INode).IsAssignableFrom(t.PropertyType))
                .Select(t => (INode)t.GetValue(node))
                .SelectMany(t => t.GetAll<T>());

            var fromLists = propertiesOfNode
                .Where(t => typeof(IEnumerable<INode>).IsAssignableFrom(t.PropertyType))
                .Select(t => (IEnumerable<INode>)t.GetValue(node))
                .SelectMany(nodes => nodes.SelectMany(t => t.GetAll<T>()));

            return fromReference.Concat(fromLists);
        }

        /// <summary>
        /// Creates a new node tree with cloned values but replaces
        /// any node of T type by the function result.
        /// </summary>
        /// <typeparam name="T">The node type to replace.</typeparam>
        /// <param name="node">The root node.</param>
        /// <returns>A new cloned tree.</returns>
        public static INode ReplaceNodes<T>(this INode node,
            Func<T, INode> replaceFunc)
            where T : INode
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            if (replaceFunc == null)
            {
                throw new ArgumentNullException(nameof(replaceFunc));
            }

            if (node is T nodeToReplace)
            {
                return replaceFunc(nodeToReplace);
            }

            var cloned = node.ShallowClone();
            var propertiesOfNode = node
                .GetType()
                .GetProperties();

            var nodeProperties = propertiesOfNode
                .Where(t => typeof(INode).IsAssignableFrom(t.PropertyType));
            foreach (var curr in nodeProperties)
            {
                var currValue = (INode)curr.GetValue(node);
                var replaced = currValue.ReplaceNodes<T>(replaceFunc);

                curr.SetValue(cloned, replaced);
            }

            var nodeListProperties = propertiesOfNode
                .Where(t => typeof(IEnumerable<INode>).IsAssignableFrom(t.PropertyType));
            foreach (var curr in nodeListProperties)
            {
                var currList = (IEnumerable<INode>)curr.GetValue(node);
                var replaced = currList
                    .Select(t => t.ReplaceNodes<T>(replaceFunc));

                curr.SetValue(cloned, replaced);
            }

            return cloned;
        }
    }
}
