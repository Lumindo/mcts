using GameTreeCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MctsCore {
    /// <summary>
    /// Provides static methods to calculate optimal moves using the mcts algorithm.
    /// </summary>
    public static class MctsAlgorithm {
        private static readonly double _DEFAULT_EXPLORATION_CONSTANT = 4;

        private static BackpropagationContainer mctsSearch(IMctsableGameState gameState, MctsNode node) {
            if(!node.areChildNodesExpanded) {
                if (node.playouts == 0) return defaultPolicy(gameState);

                node.expandNode();
                }

            MctsNode bestNode = MctsNode.childSelection(node, _DEFAULT_EXPLORATION_CONSTANT);

            gameState.makeMove(bestNode.move);

            BackpropagationContainer result;

            if (!gameState.isGameOver()) result = mctsSearch(gameState, bestNode);
            else result = new BackpropagationContainer(gameState.getResultOfTheGame(), null);
               
            bestNode.addResult(result.gameResult);
            bestNode.addResultToAMAFChilds(result);

            if(result.pathToResult != null) result.addMoveToPath(bestNode.move);  

            return result;
            }

        private static BackpropagationContainer defaultPolicy(IMctsableGameState gameState) {
            IMctsableGameState gameForSimulation = gameState.duplicate();

            Random rng = new Random();

            List<IMove> pathToResult = new List<IMove>(), possibleMoves;
            IMove chosenMove;

            while(!gameForSimulation.isGameOver()) {
                possibleMoves = gameForSimulation.getPossibleMoves();

                chosenMove = possibleMoves[rng.Next(possibleMoves.Count)];

                gameForSimulation.makeMove(chosenMove);
                pathToResult.Add(chosenMove);
                }

            return new BackpropagationContainer(gameForSimulation.getResultOfTheGame(), pathToResult);
            }


        /// <summary>
        /// Gets the number of done iterations until the final move was selected.
        /// </summary>
        public static int iterations { get; private set; }

        /// <summary>
        /// Calculates the best move depending on the given game state and the given constraints.
        /// </summary>
        /// <param name="initalGameState">A game state.</param>
        /// <param name="iterations">A number of iterations until a move is finally selected.</param>
        /// <param name="childSelectionService"> A child selection service which is used during the mcts algorithm.</param>
        /// <param name="finalChildSelectionService"> A child selection service to finally select a child.</param>
        /// <exception cref="ArgumentNullException">Is thrown, if at least one of the given parameters is null.</exception>
        /// <exception cref="ArgumentException">Is thrown, if the given game state is a terminal game state.</exception>
        /// <exception cref="ArgumentException">Is thrown, if the number given iterations is lower or equal than zero.</exception>
        public static IMove calculateOptimalMoveWithGivenIterations(IMctsableGameState initalGameState, int iterations, IChildSelectionService childSelectionService, 
                                                                    IFinalChildSelectionService finalChildSelectionService) {
            if (initalGameState == null || childSelectionService == null || finalChildSelectionService == null) throw new ArgumentNullException("CLASS: MctsAlgorithm, METHOD: calculateOptimalMoveWithGivenIterations - at least one of the given parameters is null!");
            if (initalGameState.isGameOver()) throw new ArgumentException("CLASS: MctsAlgorithm, METHOD: calculateOptimalMoveWithGivenIterations - the given game state is a terminal game state!");
            if (iterations <= 0) throw new ArgumentException("CLASS: MctsAlgorithm, METHOD: calculateOptimalMoveWithGivenIterations - the number of iterations is negative or zero!");

            MctsNode.childSelectionService = childSelectionService.childSelection;
            MctsRootNode.finalChildSelectionService = finalChildSelectionService.finalChildSelection;

            MctsRootNode root = new MctsRootNode(initalGameState);

            BackpropagationContainer result;

            root.expandNode();

            for (int i = 1; i <= iterations; i++) {
                result = mctsSearch(initalGameState.duplicate(), root);

                root.addResult(result.gameResult);
                root.addResultToAMAFChilds(result);
                }

            MctsAlgorithm.iterations = iterations;

            return MctsRootNode.finalChildSelection(root).move;
            }

        /// <summary>
        /// Calculates the best move depending on the given game state and the given constraints.
        /// </summary>
        /// <param name="initalGameState">A game state.</param>
        /// <param name="time">A time in milliseconds until a move is finally selected.</param>
        /// <param name="childSelectionService"> A child selection service which is used during the mcts algorithm.</param>
        /// <param name="finalChildSelectionService"> A child selection service to finally select a child.</param>
        /// <exception cref="ArgumentNullException">Is thrown, if at least one of the given parameters is null.</exception>
        /// <exception cref="ArgumentException">Is thrown, if the given game state is a terminal game state.</exception>
        /// <exception cref="ArgumentException">Is thrown, if the given number of milliseconds is lower or equal than zero.</exception>
        public static IMove calculateOptimalMoveInGivenTime(IMctsableGameState initalGameState, int time, IChildSelectionService childSelectionService,
                                                            IFinalChildSelectionService finalChildSelectionService) {
            if (initalGameState == null || childSelectionService == null || finalChildSelectionService == null) throw new ArgumentNullException("CLASS: MctsAlgorithm, METHOD: calculateOptimalMoveInGivenTime - at least one of the given parameters is null!");
            if (initalGameState.isGameOver()) throw new ArgumentException("CLASS: MctsAlgorithm, METHOD: calculateOptimalMoveInGivenTime - the given game state is a terminal game state!");
            if (time <= 0) throw new ArgumentException("CLASS: MctsAlgorithm, METHOD: calculateOptimalMoveInGivenTime - the number of milliseconds is negative or zero!");

            MctsNode.childSelectionService = childSelectionService.childSelection;
            MctsRootNode.finalChildSelectionService = finalChildSelectionService.finalChildSelection;

            MctsRootNode root = new MctsRootNode(initalGameState);

            BackpropagationContainer result;

            root.expandNode();

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            iterations = 0;

            Task workingTask = Task.Factory.StartNew(() => {
                while (!tokenSource.Token.IsCancellationRequested) {
                    result = mctsSearch(initalGameState.duplicate(), root);

                    iterations++;

                    root.addResult(result.gameResult);
                    root.addResultToAMAFChilds(result);
                    }
                }, tokenSource.Token);

            tokenSource.CancelAfter(time);

            workingTask.Wait();

            tokenSource.Dispose();

            return MctsRootNode.finalChildSelection(root).move;
            }
        }
    }
