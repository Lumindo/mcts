using MctsCore;
using System;

namespace TestChanceGameCore {
	public class TestChanceGame_RollDiceMove : IMove {
        public TestChanceGame_RollDiceMove(int playerWhoDoesTheMove, int drawnDigit) {
			if (playerWhoDoesTheMove != TestChanceGame_GameState.firstPlayer && playerWhoDoesTheMove != TestChanceGame_GameState.secondPlayer) throw new ArgumentException("CLASS: TestChanceGame_RollDiceMove, CONSTRUCTOR - invalid given player!");
			if (drawnDigit < 1 || drawnDigit > 6) throw new ArgumentException("CLASS: TestChanceGame_RollDiceMove, CONSTRUCTOR - invalid given digit!");

			this.playerWhoDoesTheMove = playerWhoDoesTheMove;
			this.drawnDigit = drawnDigit;
            }

		public int drawnDigit { get; private set; }
		public int nextPlayer { get; private set; }
		public int playerWhoDoesTheMove { get; private set; }

		public bool isEqualTo(IMove move) {
			if (move == null) throw new ArgumentNullException();

			if (move is TestChanceGame_RollDiceMove rollDiceMove && 
			    rollDiceMove.playerWhoDoesTheMove == playerWhoDoesTheMove && rollDiceMove.nextPlayer == nextPlayer) return true;

			return false;
		    }
	    }
    }
