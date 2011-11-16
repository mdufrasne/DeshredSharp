using System.Collections.Generic;
using System.Linq;

namespace DeShred
{
    public class EdgeScore
    {
        #region Readonly & Static Fields

        private readonly Dictionary<int, PixelEdgeScore> PixelEdgeScores;

        #endregion

        #region C'tors

        public EdgeScore(Dictionary<int, PixelEdgeScore> pixelEdgeScores)
        {
            PixelEdgeScores = pixelEdgeScores;
        }

        #endregion

        #region Instance Properties

        public int BestLeftMatchIndex
        {
            get { return PixelEdgeScores.OrderBy(p => p.Value.Left).First().Key; }
        }

        public int BestRightMatchIndex
        {
            get { return PixelEdgeScores.OrderBy(p => p.Value.Right).First().Key; }
        }

        #endregion
    }
}