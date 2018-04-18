using System.Collections.ObjectModel;

namespace GameTreeCore {
    /// <summary>
    /// Represents a node in a game tree.  
    /// </summary>
    public interface IGameTreeNode {
        int playouts { get; }
        int playoutsAMAF { get; }

        double value { get; }
        double valueAMAF { get; }

        bool areChildNodesExpanded { get; }

        IGameTreeNode previousNode { get; }

        ReadOnlyCollection<IGameTreeNode> getChildNodes();
        }
    }
