using MctsCore;
using System;
using System.Collections.Generic;

namespace TestChanceGameCore {
	/// <summary>
	/// Provides properties and methods for a game state of the TestChanceGame game system.
    /// </summary>
	public class TestChanceGame_GameState : IMctsableGameState {
		private static readonly int _ID_FIRST_PLAYER = 0, _ID_SECOND_PLAYER = 1, _NUMBER_OF_PLAYERS = 2;
		private static readonly int _GAME_SITUATION_TAKE_DICE = 0, _GAME_SITUATION_ROLL_DICE = 1, _GAME_SITUATION_SELECT_POSITION = 2;
		private static readonly int _ID_FIRST_DIGIT = 0, _ID_SECOND_DIGIT = 1;
		private static readonly int _NO_DIGIT = -1;
		private static readonly int _MAXIMAL_VICTORY_POINTS = 66;

		private int[][] _board;

		private GameResult _gameResult;

		private TestChanceGame_GameState(int phasingPlayer, int gameSituation, int[][] board, int lastDrawnDigit) : this(phasingPlayer) {
			_board[firstPlayer][_ID_FIRST_DIGIT] = board[firstPlayer][_ID_FIRST_DIGIT];
			_board[firstPlayer][_ID_SECOND_DIGIT] = board[firstPlayer][_ID_SECOND_DIGIT];
			_board[secondPlayer][_ID_FIRST_DIGIT] = board[secondPlayer][_ID_FIRST_DIGIT];
			_board[secondPlayer][_ID_SECOND_DIGIT] = board[secondPlayer][_ID_SECOND_DIGIT];

			this.gameSituation = gameSituation;
			this.lastDrawnDigit = lastDrawnDigit;
		    }


        /// <summary>
        /// Creates an initial TestChanceGame game state.
        /// </summary>
		/// <exception cref="ArgumentException">Is thrown, if the given player is none of the TestChanceGame game.</exception>
		public TestChanceGame_GameState(int phasingplayer) {
			if (phasingplayer != firstPlayer && phasingplayer != secondPlayer) throw new ArgumentException("CLASS: TestChanceGame_GameState, CONSTRUCTOR - invalid given player!");

			_board = new int[_NUMBER_OF_PLAYERS][];

			for (int player = firstPlayer; player < _NUMBER_OF_PLAYERS; player++) _board[player] = new int[] { _NO_DIGIT, _NO_DIGIT };

			this.phasingPlayer = phasingplayer;
			this.gameSituation = _GAME_SITUATION_TAKE_DICE;
			this.lastDrawnDigit = _NO_DIGIT;
            }

		public static int firstPlayer => _ID_FIRST_PLAYER;
		public static int rollDiceGameState => _GAME_SITUATION_ROLL_DICE;
		public static int secondPlayer => _ID_SECOND_PLAYER; 
		public static int selectPositionGameState => _GAME_SITUATION_SELECT_POSITION; 
		public static int takeDiceGameState => _GAME_SITUATION_TAKE_DICE; 

		public int firstPlayersFirstDigit => _board[_ID_FIRST_PLAYER][_ID_FIRST_DIGIT]; 
		public int firstPlayersSecondDigit => _board[_ID_FIRST_PLAYER][_ID_SECOND_DIGIT]; 
		public int firstPlayersValue {
			get {
				if (firstPlayersFirstDigit == _NO_DIGIT || firstPlayersSecondDigit == _NO_DIGIT) return _NO_DIGIT;

				return firstPlayersFirstDigit * 10 + firstPlayersSecondDigit;
			    }     
		    }
		public int gameSituation { get; private set; }
		public int lastDrawnDigit { get; private set; }
		public int phasingPlayer { get; private set; }
		public int secondPlayersFirstDigit => _board[_ID_SECOND_PLAYER][_ID_FIRST_DIGIT]; 
		public int secondPlayersSecondDigit => _board[_ID_SECOND_PLAYER][_ID_SECOND_DIGIT]; 
		public int secondPlayersValue {
            get {
				if (secondPlayersFirstDigit == _NO_DIGIT || secondPlayersSecondDigit == _NO_DIGIT) return _NO_DIGIT;

				return secondPlayersFirstDigit * 10 + secondPlayersSecondDigit;
                }
            }
        
        /// <summary>
		/// Makes the given move.
        /// </summary>
		/// <exception cref="ArgumentNullException">Is thrown, if the given move is null.</exception>
		/// <exception cref="InvalidOperationException">Is thrown, if the given move is invalid.</exception>
        public void makeMove(IMove move) {
			if (move == null) throw new ArgumentNullException();

			if(gameSituation == _GAME_SITUATION_TAKE_DICE) {
				if (!(move is TestChanceGame_TakeDiceMove)) throw new InvalidOperationException("CLASS: TestChanceGame_GameState, METHOD: makeMove - invalid given move!");

				gameSituation = _GAME_SITUATION_ROLL_DICE;
			    }
			else if (gameSituation == _GAME_SITUATION_ROLL_DICE) {
				if (!(move is TestChanceGame_RollDiceMove rollDiceMove)) throw new InvalidOperationException("CLASS: TestChanceGame_GameState, METHOD: makeMove - invalid given move!");

				lastDrawnDigit = rollDiceMove.drawnDigit;

				gameSituation = _GAME_SITUATION_SELECT_POSITION;
                }
			else {
				if (!(move is TestChanceGame_SelectPositionMove selectPositionMove)) throw new InvalidOperationException("CLASS: TestChanceGame_GameState, METHOD: makeMove - invalid given move!");

				_board[selectPositionMove.playerWhoDoesTheMove][selectPositionMove.digitPosition] = lastDrawnDigit;

				gameSituation = _GAME_SITUATION_TAKE_DICE;
				phasingPlayer = selectPositionMove.nextPlayer;
			    }
		    }

        /// <summary>
		/// Checks, if the game state is a terminal game state.
        /// </summary>
		public bool isGameOver() {
			if (firstPlayersFirstDigit == _NO_DIGIT || firstPlayersSecondDigit == _NO_DIGIT || secondPlayersFirstDigit == _NO_DIGIT || secondPlayersSecondDigit == _NO_DIGIT) return false;

			if (firstPlayersValue > secondPlayersValue) _gameResult = new GameResult(firstPlayer, (double)firstPlayersValue / _MAXIMAL_VICTORY_POINTS);
			else if (secondPlayersValue > firstPlayersValue) _gameResult = new GameResult(secondPlayer, (double)secondPlayersValue / _MAXIMAL_VICTORY_POINTS);
			else _gameResult = new GameResult(GameResult.noWinner, 0);

			return true;
		    }

        /// <summary>
		/// Duplicates the game state.
        /// </summary>
		public IMctsableGameState duplicate() {
			return new TestChanceGame_GameState(phasingPlayer, gameSituation, _board, lastDrawnDigit);
		    }

        /// <summary>
		/// Gets a list of all possible moves.
        /// </summary>
		public List<IMove> getPossibleMoves() {
			List<IMove> possibleMoves = new List<IMove>();

			if (gameSituation == _GAME_SITUATION_TAKE_DICE) possibleMoves.Add(new TestChanceGame_TakeDiceMove(phasingPlayer));
			else if(gameSituation == _GAME_SITUATION_ROLL_DICE) {
				for (int diceValue = 1; diceValue <= 6; diceValue++) possibleMoves.Add(new TestChanceGame_RollDiceMove(phasingPlayer, diceValue));
			    }
			else {
				int nextPlayer;

				if(phasingPlayer == firstPlayer) {
					nextPlayer = secondPlayer;

					if (firstPlayersFirstDigit == _NO_DIGIT) possibleMoves.Add(new TestChanceGame_SelectPositionMove(phasingPlayer, nextPlayer, _ID_FIRST_DIGIT));
					if (firstPlayersSecondDigit == _NO_DIGIT) possibleMoves.Add(new TestChanceGame_SelectPositionMove(phasingPlayer, nextPlayer, _ID_SECOND_DIGIT));
				    }
				else {
					nextPlayer = firstPlayer;

					if (secondPlayersFirstDigit == _NO_DIGIT) possibleMoves.Add(new TestChanceGame_SelectPositionMove(phasingPlayer, nextPlayer, _ID_FIRST_DIGIT));
					if (secondPlayersSecondDigit == _NO_DIGIT) possibleMoves.Add(new TestChanceGame_SelectPositionMove(phasingPlayer, nextPlayer, _ID_SECOND_DIGIT));
				    }
			    }

			return possibleMoves;
		    }

        /// <summary>
		/// Gets the result of a terminal game state.
        /// </summary>
		/// <exception cref="InvalidOperationException">Is thrown, if the game state is not a terminal game state.</exception>
		public GameResult getResultOfTheGame() {
			if (!isGameOver()) throw new InvalidOperationException("CLASS: TestChanceGame_GameState, METHOD: getResultOfTheGame - the game state does not represent a terminal game state!");

			return _gameResult;
		    }
	    }
    }
