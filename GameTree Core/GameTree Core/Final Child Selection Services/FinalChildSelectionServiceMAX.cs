using System;
using System.Collections.Generic;

namespace GameTreeCore {
    public sealed class FinalChildSelectionServiceMAX : IFinalChildSelectionService {
        /// <summary>
        /// Returns a string with informations about the child selection service.
        /// </summary>
        public override string ToString() {
            return string.Format("MAX final child selection");
            }

        /// <summary>
        /// Selects the child with the highest relative reward.
        /// </summary>
        /// <param name="node">A node.</param>
        /// <exception cref="ArgumentNullException">Is thrown, if the given node is null.</exception>
        /// <exception cref="InvalidOperationException">Is thrown, if no child nodes are available.</exception>
        public IGameTreeNode finalChildSelection(IGameTreeNode node) {
            if (node == null) throw new ArgumentNullException("CLASS: FinalChildSelectionServiceMAX, METHOD: finalChildSelection - the given node is null!");
            if (!node.areChildNodesExpanded) throw new InvalidOperationException("CLASS: FinalChildSelectionServiceMAX, METHOD: finalChildSelection - no child nodes are available!");

            double maxScore = 0;

            List<IGameTreeNode> bestChilds = new List<IGameTreeNode>();

            foreach (IGameTreeNode child in node.getChildNodes()) {
                if (child.value / child.playouts >= maxScore) {
                    if (child.value / child.playouts > maxScore) {
                        maxScore = child.value / child.playouts;
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
