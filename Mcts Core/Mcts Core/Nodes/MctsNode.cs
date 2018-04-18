using GameTreeCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MctsCore {
    internal class MctsNode : IGameTreeNode {
        private MctsNode[] _childNodes;

        private void addResultAMAF(GameResult gameResult) {
            playoutsAMAF++;

            if (gameResult.winner == move.playerWhoDoesTheMove) valueAMAF += 1 + gameResult.relativeVictoryPoints;
            }


        public delegate IGameTreeNode childSelectionPolicy(IGameTreeNode node, double constant);

        public static IMctsableGameState calculateGameStateFromNode(MctsNode node) {
            List<IGameTreeNode> pathInTheGameTree = new List<IGameTreeNode>();
            IGameTreeNode currentNode = node;

            pathInTheGameTree.Add(node);

            while (currentNode.previousNode != null) {
                pathInTheGameTree.Insert(0, currentNode.previousNode);
                currentNode = currentNode.previousNode;
                }

            MctsRootNode rootNode = currentNode as MctsRootNode;

            if (rootNode == null) throw new InvalidOperationException("CLASS: MctsNode, METHOD: calculateGameStateFromNode - no root node available in game tree!");

            IMctsableGameState gameState = rootNode.initialGameState.duplicate();

            pathInTheGameTree.RemoveAt(0);                                      

            foreach (MctsNode pathNode in pathInTheGameTree) gameState.makeMove(pathNode.move);

            return gameState;
            }

        public static MctsNode childSelection(MctsNode node, double explorationConstant) {
            MctsNode selectedChild = childSelectionService(node, explorationConstant) as MctsNode;

            if (selectedChild == null) throw new InvalidOperationException("CLASS: MctsNode, METHOD: childSelection - child selection service returns no mcts node!");

            return selectedChild;
            }

        public static childSelectionPolicy childSelectionService;

        public MctsNode(MctsNode previousNode, IMove move, int phasingPlayer) {
            this.move = move;
            this.previousNode = previousNode;
            this.phasingPlayer = phasingPlayer;

            areChildNodesExpanded = false;
            }

        public int phasingPlayer { get; private set; }
        public int playouts { get; protected set; }
        public int playoutsAMAF { get; protected set; }

        public double value { get; protected set; }
        public double valueAMAF { get; protected set; }

        public bool areChildNodesExpanded { get; private set; }

        public IMove move { get; private set; } 

        public IGameTreeNode previousNode { get; private set; }

        public void addResult(GameResult gameResult) {
            if (gameResult == null) throw new ArgumentNullException("CLASS: MctsNode, METHOD: addResult - the given result is null!");

            playouts++;

            if (gameResult.winner == move.playerWhoDoesTheMove) value += 1 + gameResult.relativeVictoryPoints;
            }

        public void addResultToAMAFChilds(BackpropagationContainer result) {
            if (result == null) throw new ArgumentNullException("CLASS: MctsNode, METHOD: addResultToAMAFChilds - the given result is null!");

            if (result.pathToResult == null || _childNodes == null) return;

            foreach(MctsNode child in _childNodes) {
                foreach(IMove move in result.pathToResult) {
                    if (phasingPlayer != move.playerWhoDoesTheMove) continue;

                    if (child.move.isEqualTo(move)) child.addResultAMAF(result.gameResult);
                    }
                }
            }

        public void expandNode() {
            if (areChildNodesExpanded) return;

            areChildNodesExpanded = true;

            IMctsableGameState gameState = MctsNode.calculateGameStateFromNode(this);

            List<IMove> possibleMoves = gameState.getPossibleMoves();

            INonDeterministicMove possibleNonDeterministicMove; 

            MctsNode[] childs = new MctsNode[possibleMoves.Count];

            for (int i = 0; i < possibleMoves.Count; i++) {
                possibleNonDeterministicMove = possibleMoves[i] as INonDeterministicMove;

                if (possibleNonDeterministicMove == null) childs[i] = new MctsNode(this, possibleMoves[i], possibleMoves[i].nextPlayer);
                else childs[i] = new MctsChanceNode(this, possibleNonDeterministicMove, possibleNonDeterministicMove.nextPlayer, possibleNonDeterministicMove.getChildDistribution());
                }

            _childNodes = childs;
            }

        public ReadOnlyCollection<IGameTreeNode> getChildNodes() {
            if (_childNodes == null) throw new InvalidOperationException("CLASS: MctsNode, METHOD: getChildNodes - the child nodes do not have been expanded yet!");

            return new ReadOnlyCollection<IGameTreeNode>(_childNodes);
            }
        }
    }
