namespace GameTreeCore {
    /// <summary>
    /// Esposes a method to finally select a child node from a IGameTreeNode.
    /// </summary>
    public interface IFinalChildSelectionService {
        IGameTreeNode finalChildSelection(IGameTreeNode node);
        }
    }
