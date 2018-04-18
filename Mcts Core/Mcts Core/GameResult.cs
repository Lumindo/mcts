using System;

namespace MctsCore {
    /// <summary>
    /// Contains informations about the result of a game.
    /// </summary>
    public class GameResult {
        private static readonly int _NO_WINNER = -1;


        /// <summary>
        /// Represents a winner value in case of a draw.
        /// </summary>
        public static int noWinner { get { return _NO_WINNER; } }

        /// <summary>
        /// Creates a new instance of the GameResult class.
        /// </summary>
        /// <param name="winner">The player who has won the game. I case of a draw use noWinner.</param>
        /// <param name="relativeVictoryPoints">The ratio of achieved victory points to maximal victory points.</param>
        /// <exception cref="ArgumentException">Is thrown, if the given ratio is not in [0,1].</exception>
        public GameResult(int winner, double relativeVictoryPoints) {
            if (relativeVictoryPoints < 0 || relativeVictoryPoints > 1) throw new ArgumentException("CLASS: GameResult, CONSTRUCTOR - the given ratio has to be in [0,1]!");

            this.winner = winner;
            this.relativeVictoryPoints = relativeVictoryPoints;
            }

        /// <summary>
        /// Gets the winner corresponding to this game result.
        /// </summary>
        public int winner { get; private set; }

        /// <summary>
        /// Gets the achieved relative victory points corresponding to this game result.
        /// </summary>
        public double relativeVictoryPoints { get; private set; }
        }
    }
