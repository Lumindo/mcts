namespace GameTreeCore {
    /// <summary>
    /// A child selection service based on the UCT algorithm. This is a special case of the alpha AMAF version (alpha = 0).
    /// </summary>
    public sealed class ChildSelectionServiceUCT : ChildSelectionServiceAlphaAMAF {
        /// <summary>
        /// Creates a new instance of the ChildSelectionServiceUCT class.
        /// </summary>
        public ChildSelectionServiceUCT() : base(0) {
            }

        /// <summary>
        /// Returns a string with informations about the child selection service.
        /// </summary>
        public override string ToString() {
            return string.Format("Child selection via UCT");
            }
        }
    }
