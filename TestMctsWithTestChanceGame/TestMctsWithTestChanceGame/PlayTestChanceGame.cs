﻿using GameTreeCore;
using MctsCore;
using System;
using System.Diagnostics;
using TestChanceGameCore;

public class PlayTestChanceGame {
    public static void Main(string[] args) {
		int winsFirstPlayer = 0, winsSecondPlayer = 0, draws = 0;

        double scoreStartPlayer = 0, scoreSecondPlayer = 0;

        int simulations = 1000, iterations = 0, duration = 100, turns = 0, possibleMoves = 0;

		TestChanceGame_GameState gameState;

        IChildSelectionService childSelectionFirstPlayer = new ChildSelectionServiceRAVE(50), childSelectionSecondPlayer = new ChildSelectionServiceUCT();

        IFinalChildSelectionService finalMoveSelectionFirstPlayer = new FinalChildSelectionServiceROBUST(), finalMoveSelectionSecondPlayer = new FinalChildSelectionServiceSECURE();

        IMove bestMove;

		Random rng = new Random();

        Stopwatch timer = new Stopwatch();
        TimeSpan timerOutput = new TimeSpan();

        timer.Start();

        for (int i = 1; i <= simulations; i++) {
            Console.Write("Fortschritt: {0:F1} %\r", 100 * (double)i / simulations);

			gameState = new TestChanceGame_GameState(TestChanceGame_GameState.firstPlayer);

            while (!gameState.isGameOver()) {
				if (gameState.gameSituation == TestChanceGame_GameState.takeDiceGameState) bestMove = new TestChanceGame_TakeDiceMove(gameState.phasingPlayer);
				else if (gameState.gameSituation == TestChanceGame_GameState.rollDiceGameState) bestMove = new TestChanceGame_RollDiceMove(gameState.phasingPlayer, rng.Next(1, 6));
				else {
					turns++;

					if (gameState.phasingPlayer == TestChanceGame_GameState.firstPlayer)
						bestMove = MctsAlgorithm.calculateOptimalMoveInGivenTime(gameState, duration, childSelectionFirstPlayer, finalMoveSelectionFirstPlayer);
					else
						bestMove = MctsAlgorithm.calculateOptimalMoveInGivenTime(gameState, duration, childSelectionSecondPlayer, finalMoveSelectionSecondPlayer);

					iterations += MctsAlgorithm.iterations;

					possibleMoves += gameState.getPossibleMoves().Count;
				    }

                gameState.makeMove(bestMove);
                }

			if (gameState.getResultOfTheGame().winner == TestChanceGame_GameState.firstPlayer) {
                winsFirstPlayer++;
                scoreStartPlayer += gameState.getResultOfTheGame().relativeVictoryPoints;
                }
			else if (gameState.getResultOfTheGame().winner == TestChanceGame_GameState.secondPlayer) {
                winsSecondPlayer++;
                scoreSecondPlayer += gameState.getResultOfTheGame().relativeVictoryPoints;
                }
            else draws++;
            }

        timer.Stop();

        timerOutput = timer.Elapsed;

        Console.WriteLine("First Player: {0}, {1}", childSelectionFirstPlayer, finalMoveSelectionFirstPlayer);
        Console.WriteLine("Second Player: {0}, {1}\n", childSelectionSecondPlayer, finalMoveSelectionSecondPlayer);

        Console.WriteLine("{0} games, {1}ms until final move selection", simulations, duration);

        if (timerOutput.Hours > 0) Console.WriteLine("\n{0} h {1} Min {2} Sek", timerOutput.Hours, timerOutput.Minutes, timerOutput.Seconds);
        else if (timerOutput.Minutes > 0) Console.WriteLine("\n{0} Min {1} Sek", timerOutput.Minutes, timerOutput.Seconds);
        else Console.WriteLine("\n{0} Sek", timerOutput.Seconds);

        Console.WriteLine("\nWon games of the first player: {0} ({1:F2} %)", winsFirstPlayer, 100 * (double)winsFirstPlayer / simulations);
        Console.WriteLine("Won games of the second player: {0} ({1:F2} %)", winsSecondPlayer, 100 * (double)winsSecondPlayer / simulations);
        Console.WriteLine("Draws: {0} ({1:F2} %)\n", draws, 100 * (double)draws / simulations);

		Console.WriteLine("Possible turns per turn: {0}", (double)possibleMoves / turns);
        Console.WriteLine("Turns per game: {0}", (double)turns / simulations);
        Console.WriteLine("Iterations per turn: {0}", (double)iterations / turns);

		Console.ReadLine();
        }   
    }