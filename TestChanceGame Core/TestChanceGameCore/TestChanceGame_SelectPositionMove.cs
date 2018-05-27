using MctsCore;
using System;

namespace TestChanceGameCore {
	public class TestChanceGame_SelectPositionMove : IMove {
        public TestChanceGame_SelectPositionMove(int playerWhoDoesTheMove, int nextPlayer, int digitPosition) {
			if ((playerWhoDoesTheMove != TestChanceGame_GameState.firstPlayer && playerWhoDoesTheMove != TestChanceGame_GameState.secondPlayer) ||
			   (nextPlayer != TestChanceGame_GameState.firstPlayer && nextPlayer != TestChanceGame_GameState.secondPlayer)) throw new ArgumentException("CLASS: TestChanceGame_SelectPositionMove, CONSTRUCTOR - invalid given player!");

			this.playerWhoDoesTheMove = playerWhoDoesTheMove;
			this.nextPlayer = nextPlayer;
			this.digitPosition = digitPosition;
            }

		public int digitPosition { get; private set; }
		public int nextPlayer { get; private set; }
		public int playerWhoDoesTheMove { get; private set; }

		public bool isEqualTo(IMove move) {
			if (move == null) throw new ArgumentNullException();

			if (move is TestChanceGame_SelectPositionMove selectPositionMove &&
			   selectPositionMove.playerWhoDoesTheMove == playerWhoDoesTheMove && selectPositionMove.nextPlayer == nextPlayer && selectPositionMove.digitPosition == digitPosition) return true;

			return false;
		    }
	    }
    }
