using System;
using System.Collections.ObjectModel;

namespace MctsCore {
    internal sealed class MctsChanceNode : MctsNode {
		private static readonly int _FLOATING_PRECISSION = 6;


        public MctsChanceNode(MctsNode previousNode, INonDeterministicMove move, int phasingPlayer, ReadOnlyCollection<double> childDistribution) : base(previousNode, move, phasingPlayer) {
            if(previousNode == null || move == null || childDistribution == null) throw new ArgumentNullException("CLASS: MctsChanceNode, CONSTRUCTOR - at least one of the given parameter is null!");

            expandNode();

            if(childDistribution.Count != getChildNodes().Count) throw new ArgumentException("CLASS: MctsChanceNode, CONSTRUCTOR - the number of the given probabilities does not coincide with the number of possible moves!!");

            double checksum = 0;

            for(int i = 0; i < childDistribution.Count; i++) {
                if (childDistribution[i] < 0 || childDistribution[i] > 1) throw new ArgumentException("CLASS: MctsChanceNode, CONSTRUCTOR - invalid given probability!");

                checksum += childDistribution[i];
                }

			if(Math.Abs(checksum - 1.0) > Math.Pow(10, _FLOATING_PRECISSION)) throw new ArgumentException("CLASS: MctsChanceNode, CONSTRUCTOR - invalid distribution (sum has to be 1)!");

            this.childDistribution = childDistribution;
            }

        public ReadOnlyCollection<double> childDistribution { get; private set; }

        public void addResult(GameResult gameResult, MctsNode backpropagatingNode) {
            if (gameResult == null || backpropagatingNode == null) throw new ArgumentNullException("CLASS: MctsChanceNode, METHOD: addResult - at least one of the given parameter is null!");

            int indexOfBackpropagatingNode;

            try {
                indexOfBackpropagatingNode = getChildNodes().IndexOf(backpropagatingNode);
                }
            catch(InvalidOperationException) {
                throw new InvalidOperationException("CLASS: MctsChanceNode, METHOD: addResult - the child nodes do not have been expanded yet!");
                }
             
            if (indexOfBackpropagatingNode == -1) throw new ArgumentException("CLASS: MctsChanceNode, METHOD: addResult - the given backpropagating node is not a child node!");

            playouts++;

            if (gameResult.winner == move.playerWhoDoesTheMove) value += childDistribution[indexOfBackpropagatingNode] * (1 + gameResult.relativeVictoryPoints);
            }
        }
    }
