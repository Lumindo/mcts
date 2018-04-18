using System;
using System.Collections.Generic;

namespace GameTreeCore {
    /// <summary>
    /// A child selection service based on the alpha AMAF algorithm.
    /// </summary>
    public class ChildSelectionServiceAlphaAMAF : IChildSelectionService {
        private readonly double _alpha;

        /// <summary>
        /// Creates a new instance of the ChildSelectionServiceAlphaAMAF class.
        /// </summary>
        /// <param name="alpha">A parameter in [0,1].</param>
        /// <exception cref="ArgumentException">Is thrown if the given alpha is not in [0,1].</exception>
        public ChildSelectionServiceAlphaAMAF(double alpha) {
            if (alpha < 0 || alpha > 1) throw new ArgumentException("CLASS: ChildSelectionServiceAlphaAMAF, CONSTRUCTOR - the given alpha has to be in [0,1]!");

            _alpha = alpha;
            }

        /// <summary>
        /// Returns a string with informations about the child selection service.
        /// </summary>
        public override string ToString() {
            return string.Format("Child selection via AlphaAMAF, alpha = {0}", _alpha);
            }

        /// <summary>
        /// Selects the best child of the given node via the alpha AMAF algorithm.
        /// </summary>
        /// <param name="node">A node in the game tree.</param>
        /// <param name="explorationConstant">A tunable exploration constant.</param>
        /// <exception cref="ArgumentNullException">Is thrown, if the given node is null.</exception>
        /// <exception cref="InvalidOperationException">Is thrown, if no child nodes are available.</exception>
        public IGameTreeNode childSelection(IGameTreeNode node, double explorationConstant) {
            if (node == null) throw new ArgumentNullException("CLASS: ChildSelectionServiceAlphaAMAF, METHOD: childSelection - given node is null!");
            if (!node.areChildNodesExpanded) throw new InvalidOperationException("CLASS: ChildSelectionServiceAlphaAMAF, METHOD: childSelection - no child nodes are available!");

            double score, maxScore = 0;

            List<IGameTreeNode> bestChildren = new List<IGameTreeNode>();

            foreach (IGameTreeNode child in node.getChildNodes()) {
                if (child.playouts == 0) return child;

                score = _alpha * child.valueAMAF + (1 - _alpha) * child.value + explorationConstant * Math.Sqrt(Math.Log(node.playouts) / child.playouts);

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
