using MctsCore;
using System;
using System.Collections.Generic;

namespace Hus {
    /// <summary>
    /// Provides properties and methods for a game state of the HUS game system.
    /// </summary>
    public sealed class HusGameState : IMctsableGameState {
        private static readonly int _ID_FIRST_PLAYER = 0, _ID_SECOND_PLAYER = 1, _NUMBER_OF_PLAYERS = 2, _MAX_PITS = 16;

        private int[][] _board;

        private bool[] _playerIsAllowedToStealStones;

        private GameResult _gameResult;

        private HusGameState(int phasingPlayer, int[][] board, bool[] playerIsAllowedToStealStones) : this(phasingPlayer) {
            for (int player = 0; player < numberOfPlayers; player++) {
                for (int pit = 0; pit < maxPits; pit++) _board[player][pit] = board[player][pit];

                _playerIsAllowedToStealStones[player] = playerIsAllowedToStealStones[player];
                }
            }    


        public static int firstPlayer => _ID_FIRST_PLAYER;
        public static int numberOfPlayers => _NUMBER_OF_PLAYERS;
        public static int maxPits => _MAX_PITS;
        public static int secondPlayer => _ID_SECOND_PLAYER;

        /// <summary>
        /// Creates an inital HUS game state.
        /// </summary>
        /// <exception cref="ArgumentException">Is thrown, if the given player is none of the Hus game.</exception>
        public HusGameState(int phasingPlayer) {
            if (phasingPlayer != firstPlayer && phasingPlayer != secondPlayer) throw new ArgumentException("CLASS: HusGameState, CONSTRUCTOR - invalid given player!");

            _board = new int[numberOfPlayers][];

            for (int player = 0; player < numberOfPlayers; player++) _board[player] = new int[] { 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };

            _playerIsAllowedToStealStones = new bool[] { false, false };

            this.phasingPlayer = phasingPlayer;
            }

        public int phasingPlayer { get; private set; }

        /// <summary>
        /// Makes the given move.
        /// </summary>
        /// <exception cref="ArgumentNullException">Is thrown, if the given move is null.</exception>
        /// <exception cref="ArgumentException">Is thrown, if the given move in invalid, because the pit does not contain enough gems.</exception>
        /// <exception cref="InvalidCastException">Is thrown, if the given move is not a Hus move.</exception>
        public void makeMove(IMove move) {
            if (move == null) throw new ArgumentNullException("CLASS: HusGameState, METHOD: makeMove - the given move is null");

            HusMove hMove = move as HusMove;

            if (hMove == null) throw new InvalidCastException("CLASS: HusGameState, METHOD: makeMove - unable to cast given move to HusMove!");
            if (_board[phasingPlayer][hMove.pit] < 2) throw new ArgumentException("CLASS: HusGameState, METHOD: makeMove - invalid given move. Pit does not contain enough gems!");

            int currentPit = hMove.pit, numberOfGems;

            bool firstTurn = true;

            do {
                if (!_playerIsAllowedToStealStones[phasingPlayer] || firstTurn || currentPit >= maxPits / 2 || _board[hMove.nextPlayer][(maxPits / 2 - 1) - currentPit] == 0)
                    numberOfGems = _board[phasingPlayer][currentPit];
                else {
                    numberOfGems = _board[phasingPlayer][currentPit] + _board[hMove.nextPlayer][7 - currentPit] + _board[hMove.nextPlayer][maxPits / 2 + currentPit];

                    _board[hMove.nextPlayer][(maxPits / 2 - 1) - currentPit] = 0;
                    _board[hMove.nextPlayer][maxPits / 2 + currentPit] = 0;
                    }

                _board[phasingPlayer][currentPit] = 0;

                for (int i = numberOfGems; i > 0; i--) {
                    currentPit = (currentPit + 1) % maxPits;

                    _board[phasingPlayer][currentPit]++;
                    }

                firstTurn = false;
                }
            while (_board[phasingPlayer][currentPit] >= 2 && !isGameOver());

            _playerIsAllowedToStealStones[phasingPlayer] = true;

            phasingPlayer = hMove.nextPlayer;
            }

        /// <summary>
        /// Checks, if the game state is a terminal game state.
        /// </summary>
        public bool isGameOver() {
            if (_gameResult != null) return true;

            int sumInnerRow = 0;

            bool stillValidMoves = false;

            // Has the first player lost the game?
            for (int pit = 0; pit < maxPits; pit++) {
                if (pit < maxPits / 2) sumInnerRow += _board[firstPlayer][pit];

                if (_board[firstPlayer][pit] >= 2) stillValidMoves = true;
                }

            if(sumInnerRow == 0 || !stillValidMoves) {
                _gameResult = new GameResult(secondPlayer, 1);
                return true;
                }

            // Has the second player lost the game?
            sumInnerRow = 0;
            stillValidMoves = false;

            for (int pit = 0; pit < maxPits; pit++) {
                if (pit < maxPits / 2) sumInnerRow += _board[secondPlayer][pit];

                if (_board[secondPlayer][pit] >= 2) stillValidMoves = true;
                }

            if (sumInnerRow == 0 || !stillValidMoves) {
                _gameResult = new GameResult(firstPlayer, 1);
                return true;
                }

            return false;
            }

        /// <summary>
        /// Duplicates the game state.
        /// </summary>
        public IMctsableGameState duplicate() {
            return new HusGameState(phasingPlayer, _board, _playerIsAllowedToStealStones);
            }

        /// <summary>
        /// Gets a list of all possible moves.
        /// </summary>
        public List<IMove> getPossibleMoves() {
            List<IMove> possibleMoves = new List<IMove>();

            int nextPlayer;

            if (phasingPlayer == firstPlayer) nextPlayer = secondPlayer;
            else nextPlayer = firstPlayer;

            for (int pit = 0; pit < maxPits; pit++) {
                if (_board[phasingPlayer][pit] >= 2) possibleMoves.Add(new HusMove(phasingPlayer, nextPlayer, pit));   
                }

            return possibleMoves;
            }

        /// <summary>
        /// Gets the result of a terminal game state.
        /// </summary>
        /// <exception cref="InvalidOperationException">Is thrown, if the game state is not a terminal game state.</exception>
        public GameResult getResultOfTheGame() {
            if (!isGameOver()) throw new InvalidOperationException("CLASS: HusGameState, getResultOfTheGame - the game state is not a terminal game state!");

            return _gameResult;
            }
        }
    }
