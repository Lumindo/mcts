using System.Collections.ObjectModel;

namespace MctsCore {
    /// <summary>
    /// Represents a non deterministic move of a game for the mcts algorithm.
    /// </summary>
    public interface INonDeterministicMove : IMove {
        ReadOnlyCollection<double> getChildDistribution();
        }
    }
