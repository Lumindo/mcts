using System;
using System.Collections.Generic;

namespace GameTreeCore {
    /// <summary>
    /// A child selection service based on the alpha AMAF algorithm from David Silver.
    /// </summary>
    public sealed class ChildSelectionServiceSilverAlphaAMAF : IChildSelectionService {
        private readonly double _bias;


        /// <summary>
        /// Creates a new instance of the ChildSelectionServiceSilverAlphaAMAF class.
        /// </summary>
        /// <param name="bias">A bias.</param>
        public ChildSelectionServiceSilverAlphaAMAF(double bias) {
            _bias = bias;
            }

        /// <summary>
        /// Returns a string with informations about the child selection service.
        /// </summary>
        public override string ToString() {
            return string.Format("Child selection via SilverAlphaAMAF, bias = {0}", _bias);
            }

        /// <summary>
        /// Selects the best child of the given node via the alpha AMAF algorithm from Silver.
        /// </summary>
        /// <param name="node">A node in the game tree.</param>
        /// <param name="explorationConstant">A tunable exploration constant.</param>
        /// <exception cref="ArgumentNullException">Is thrown, if the given node is null.</exception>
        /// <exception cref="InvalidOperationException">Is thrown, if no child nodes are available.</exception>
        public IGameTreeNode childSelection(IGameTreeNode node, double explorationConstant) {
            if (node == null) throw new ArgumentNullException("CLASS: ChildSelectionServiceSilverAlphaAMAF, METHOD: childSelection - given node is null!");
            if (!node.areChildNodesExpanded) throw new InvalidOperationException("CLASS: ChildSelectionServiceSilverAlphaAMAF, METHOD: childSelection - no child nodes are available!");

            double score, maxScore = 0, silverAlpha;

            List<IGameTreeNode> bestChildren = new List<IGameTreeNode>();

            foreach (IGameTreeNode child in node.getChildNodes()) {
                if (child.playouts == 0) return child;

                silverAlpha = child.playoutsAMAF / (child.playouts + child.playoutsAMAF + 4 * Math.Pow(_bias, 2) * child.playouts * child.playoutsAMAF);

                score = silverAlpha * child.valueAMAF + (1 - silverAlpha) * child.value + explorationConstant * Math.Sqrt(Math.Log(node.playouts) / child.playouts);

                if (score >= maxScore) {
                    if (score > maxScore) {
                        maxScore = score;
                        bestChildren.Clear();
                        }

                    bestChildren.Add(child);
                    }
                }

            Random rng = new Random();

            return bestChildren[rng.Next(bestChildren.Count)];
            }
        }
    }
