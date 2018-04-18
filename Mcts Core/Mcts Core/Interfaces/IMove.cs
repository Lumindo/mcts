namespace MctsCore {
    /// <summary>
    /// Represents a move of a game for the mcts algorithm.
    /// </summary>
    public interface IMove {
        int nextPlayer { get; }

        int playerWhoDoesTheMove { get; }

        bool isEqualTo(IMove move);
        };
    }
