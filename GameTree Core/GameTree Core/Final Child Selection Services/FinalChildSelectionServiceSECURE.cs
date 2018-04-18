using System;
using System.Collections.Generic;

namespace GameTreeCore {
    public sealed class FinalChildSelectionServiceSECURE : IFinalChildSelectionService {
        /// <summary>
        /// Returns a string with informations about the child selection service.
        /// </summary>
        public override string ToString() {
            return string.Format("SECURE final child selection");
            }

        /// <summary>
        /// Selects the root child with the highest relative reward.
        /// </summary>
        /// <param name="node">A node.</param>
        /// <returns>The root child with the highest relative reward.</returns>
        /// <exception cref="ArgumentNullException">Is thrown, if the given node is null.</exception>
        /// <exception cref="InvalidOperationException">Is thrown, if no child nodes are available.</exception>
        public IGameTreeNode finalChildSelection(IGameTreeNode node) {
            if (node == null) throw new ArgumentNullException("CLASS: FinalChildSelectionServiceSECURE, METHOD: finalChildSelection - the given node is null!");
            if (!node.areChildNodesExpanded) throw new InvalidOperationException("CLASS: FinalChildSelectionServiceSECURE, METHOD: finalChildSelection - no child nodes are available!");

            double score, maxScore = 0;

            List<IGameTreeNode> bestChilds = new List<IGameTreeNode>();

            foreach (IGameTreeNode child in node.getChildNodes()) {
                score = child.value / child.playouts + 1 / Math.Sqrt(child.playouts);

                if (score >= maxScore) {
                    if (score > maxScore) {
                        maxScore = score;
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
