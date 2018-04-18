namespace GameTreeCore {
    /// <summary>
    /// Exposes a method to select a child node from a IGameTreeNode.
    /// </summary>
    public interface IChildSelectionService {
        IGameTreeNode childSelection(IGameTreeNode node, double explorationConstant);
        }
    }
