using MctsCore;
using System;

namespace Hus {
    /// <summary>
    /// Provides properties and methods for a move of the HUS game system.
    /// </summary>
    public sealed class HusMove : IMove {
        /// <summary>
        /// Creates a move of the HUS game system.
        /// </summary>
        /// <param name="playerWhoDoesTheMove">The player who does the move.</param>
        /// <param name="nextPlayer">The player who is next.</param>
        /// <param name="pit">The pit on the hus board that corresponds to the move.</param>
        /// <exception cref="ArgumentException">Is thrown, if an invalid player was given.</exception>
        /// <exception cref="ArgumentException">Is thrown, if an invalid pit was given.</exception>
        public HusMove(int playerWhoDoesTheMove, int nextPlayer, int pit) {
            if ((playerWhoDoesTheMove != HusGameState.firstPlayer && playerWhoDoesTheMove != HusGameState.secondPlayer) || 
                (nextPlayer != HusGameState.firstPlayer && nextPlayer != HusGameState.secondPlayer)) throw new ArgumentException("CLASS: HusMove, CONSTRUCTOR - invalid given player!");
            if (pit < 0 || pit > HusGameState.maxPits) throw new ArgumentException("CLASS: HusMove, CONSTRUCTOR - invalid given pit!");

            this.playerWhoDoesTheMove = playerWhoDoesTheMove;
            this.nextPlayer = nextPlayer;
            this.pit = pit;
            }

        public int nextPlayer { get; private set; }
        public int pit { get; private set; }
        public int playerWhoDoesTheMove { get; private set; }

        /// <summary>
        /// Checks, if the two moves are equal. 
        /// 
        /// This is the case if and only if the given move is a move of the HUS game system and all three properties are equal.
        /// </summary>
        /// <param name="move">A move.</param>
        /// <exception cref="ArgumentNullException">Is thrown, if the given move is null.</exception>
        public bool isEqualTo(IMove move) {
            if (move == null) throw new ArgumentNullException("CLASS: HusMove, METHOD: isEqualTo - the given move is null!");

            HusMove hMove = move as HusMove;

            if (hMove == null) return false;

            if (hMove.playerWhoDoesTheMove == playerWhoDoesTheMove && hMove.nextPlayer == nextPlayer && hMove.pit == pit) return true;

            return false;
            }
        }
    }
