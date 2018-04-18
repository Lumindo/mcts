using System;
using System.Collections.Generic;

namespace GameTreeCore {
    public sealed class FinalChildSelectionServiceROBUST : IFinalChildSelectionService {
        /// <summary>
        /// Returns a string with informations about the child selection service.
        /// </summary>
        public override string ToString() {
            return string.Format("ROBUST final child selection");
            }

        /// <summary>
        /// Selects the root child with the highest relative reward.
        /// </summary>
        /// <param name="node">A node.</param>
        /// <exception cref="ArgumentNullException">Is thrown, if the given node is null.</exception>
        /// <exception cref="InvalidOperationException">Is thrown, if no child nodes are available.</exception>
        public IGameTreeNode finalChildSelection(IGameTreeNode node) {
            if (node == null) throw new ArgumentNullException("CLASS: FinalChildSelectionServiceROBUST, METHOD: finalChildSelection - the given node is null!");
            if (!node.areChildNodesExpanded) throw new InvalidOperationException("CLASS: FinalChildSelectionServiceROBUST, METHOD: finalChildSelection - no child nodes are available!");

            int maxPlayouts = 0;

            List<IGameTreeNode> bestChilds = new List<IGameTreeNode>();

            foreach (IGameTreeNode child in node.getChildNodes()) {
                if (child.playouts >= maxPlayouts) {
                    if (child.playouts > maxPlayouts) {
                        maxPlayouts = child.playouts;
                        bestChilds.Clear();
                        }

                    bestChilds.Add(child);
                    }
                }

            Random rng = new Random();

            return bestChilds[rng.Next(bestChilds.Count)];
            }
        }
    }
