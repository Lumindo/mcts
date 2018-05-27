using MctsCore;
using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace TestChanceGameCore {
	public class TestChanceGame_TakeDiceMove : INonDeterministicMove {
		private static ReadOnlyCollection<double> _CHILD_DISTRIBUTION = new ReadOnlyCollection<double>(Enumerable.Repeat((double)1 / 6, 6).ToArray());
		
		public TestChanceGame_TakeDiceMove(int playerWhoDoesTheMove) {
			if (playerWhoDoesTheMove != TestChanceGame_GameState.firstPlayer && playerWhoDoesTheMove != TestChanceGame_GameState.secondPlayer) throw new ArgumentException("CLASS: TestChanceGame_TakeDiceMove, CONSTRUCTOR - invalid given player!");

			this.playerWhoDoesTheMove = playerWhoDoesTheMove;
			this.nextPlayer = playerWhoDoesTheMove;
            }

		public int nextPlayer { get; private set; }
		public int playerWhoDoesTheMove { get; private set; }

		public bool isEqualTo(IMove move) {
			if (move == null) throw new ArgumentNullException();

			if (move is TestChanceGame_TakeDiceMove takeDiceMove && 
			    takeDiceMove.playerWhoDoesTheMove == playerWhoDoesTheMove && takeDiceMove.nextPlayer == nextPlayer) return true;

			return false;
            }

		public ReadOnlyCollection<double> getChildDistribution() {
			return _CHILD_DISTRIBUTION;
		    }
	    }
    }   
