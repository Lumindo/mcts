namespace GameTreeCore {
    /// <summary>
    /// A child selection service based on the AMAF algorithm. This is a special case of the alpha AMAF version (alpha = 1).
    /// </summary>
    public sealed class ChildSelectionServiceAMAF : ChildSelectionServiceAlphaAMAF {
        /// <summary>
        /// Creates a new instance of the ChildSelectionServiceAMAF class.
        /// </summary>
        public ChildSelectionServiceAMAF() : base(1) {
            }

        /// <summary>
        /// Returns a string with informations about the child selection service.
        /// </summary>
        public override string ToString() {
            return string.Format("Child selection via AMAF");
            }
        }
    }
