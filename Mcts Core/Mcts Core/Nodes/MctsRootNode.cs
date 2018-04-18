using GameTreeCore;
using System;

namespace MctsCore {
    internal sealed class MctsRootNode : MctsNode {
        public delegate IGameTreeNode finalChildSelectionPolicy(IGameTreeNode rootNode);

        public static MctsNode finalChildSelection(MctsRootNode node) {
            MctsNode selectedChild = finalChildSelectionService(node) as MctsNode;

            if (selectedChild == null) throw new InvalidOperationException("CLASS: MctsRootNode, METHOD: finalChildSelection - final child selection service returns no mcts node!");

            return selectedChild;
            }

        public static finalChildSelectionPolicy finalChildSelectionService;

        public MctsRootNode(IMctsableGameState initialGameState) : base(null, null, initialGameState.phasingPlayer) {
            this.initialGameState = initialGameState ?? throw new ArgumentNullException("CLASS: MctsRootNode, CONSTRUCTOR - the given game state is null!");
            }

        public IMctsableGameState initialGameState { get; private set; }

        public new void addResult(GameResult result)    {
            playouts++;
            }
        }
    }
