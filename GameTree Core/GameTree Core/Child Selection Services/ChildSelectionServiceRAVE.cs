using System;
using System.Collections.Generic;

namespace GameTreeCore {
    /// <summary>
    /// A child selection service based on the RAVE algorithm.
    /// </summary>
    public class ChildSelectionServiceRAVE : IChildSelectionService {
        private readonly int _playoutsBound;


        /// <summary>
        /// Creates a new instance of the ChildSelectionServiceRAVE class.
        /// </summary>
        /// <param name="playoutsBound">A nonzero playouts bound.</param>
        /// <exception cref="ArgumentException">Is thrown, if the given bound is negative.</exception>
        public ChildSelectionServiceRAVE(int playoutsBound) {
            if (playoutsBound < 0) throw new ArgumentException("CLASS: ChildSelectionServiceRAVE, CONSTRUCTOR - the given bound is negative!");

            _playoutsBound = playoutsBound;
            }

        /// <summary>
        /// Returns a string with informations about the child selection service.
        /// </summary>
        public override string ToString() {
            return string.Format("Child selection via RAVE, playoutsBound = {0}", _playoutsBound);
            }

        /// <summary>
        /// Selects the best child of the given node via the RAVE algorithm.
        /// </summary>
        /// <param name="node">A node in the game tree.</param>
        /// <param name="explorationConstant">A tunable exploration constant.</param>
        /// <exception cref="ArgumentNullException">Is thrown, if the given node is null.</exception>
        /// <exception cref="InvalidOperationException">Is thrown, if no child nodes are available.</exception>
        public IGameTreeNode childSelection(IGameTreeNode node, double explorationConstant) {
            if (node == null) throw new ArgumentNullException("CLASS: ChildSelectionServiceRAVE, METHOD: childSelection - given node is null!");
            if (!node.areChildNodesExpanded) throw new InvalidOperationException("CLASS: ChildSelectionServiceRAVE, METHOD: childSelection - no child nodes are available!");

            double score, maxScore = 0, alpha;

            List<IGameTreeNode> bestChildren = new List<IGameTreeNode>();

            foreach (IGameTreeNode child in node.getChildNodes()) {
                if (child.playouts == 0) return child;

                alpha = Math.Max(0, (double)(_playoutsBound - child.playouts) / _playoutsBound);

                score = alpha * child.valueAMAF + (1 - alpha) * child.value + explorationConstant * Math.Sqrt(Math.Log(node.playouts) / child.playouts);

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
