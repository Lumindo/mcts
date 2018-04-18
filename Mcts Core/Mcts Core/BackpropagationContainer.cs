using System.Collections.Generic;

namespace MctsCore {
    internal sealed class BackpropagationContainer {
        public BackpropagationContainer(GameResult gameResult, List<IMove> pathToResult) {
            this.gameResult = gameResult;
            this.pathToResult = pathToResult;
            }

        public GameResult gameResult { get; private set; }

        public List<IMove> pathToResult { get; private set; }

        public void addMoveToPath(IMove move) {
            if (pathToResult == null) pathToResult = new List<IMove>(new IMove[] { move });
            else pathToResult.Insert(0, move);
            }
        }
    }
