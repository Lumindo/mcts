using System.Collections.Generic;

namespace MctsCore {
    /// <summary>
    /// Represents a game state for the mcts algorithm.
    /// </summary>
    public interface IMctsableGameState {
        int phasingPlayer { get; }

        void makeMove(IMove move);            

        bool isGameOver();                  

        IMctsableGameState duplicate();

        List<IMove> getPossibleMoves();

        GameResult getResultOfTheGame();
        };
    }
