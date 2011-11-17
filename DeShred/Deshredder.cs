using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace DeShred
{
    public static class Deshredder
    {
        #region Enums

        private enum EdgeSide
        {
            Right,
            Left
        }

        #endregion

        #region Readonly & Static Fields

        public static int segmentWidth;

        private static readonly Dictionary<int, EdgeScore> AllEdgeScores = new Dictionary<int, EdgeScore>();
        private static readonly List<int> Results = new List<int>();
        private static readonly object myLock = new object();

        #endregion

        #region Event Declarations

        public static event EventHandler EdgeCalculated;
        public static event EventHandler EdgeCalculationCompleted;

        public static event EventHandler SegmentWidthCalculated;
        public static event EventHandler SegmentWidthCalculationCompleted;

        #endregion

        #region Class Properties

        public static Bitmap OutputBitmap
        {
            get
            {
                var b = new Bitmap(640, 359);
                for (int i = 0; i < 20; i++)
                {
                    int segmentNumber = Results[i];
                    Bitmap segment = ImageSegment(segmentNumber);
                    for (int j = 0; j < 32; j++)
                    {
                        for (int k = 0; k < 359; k++)
                        {
                            b.SetPixel(j + (i*32), k, segment.GetPixel(j, k));
                        }
                    }
                }

                return b;
            }
        }

        #endregion

        #region Class Methods

        public static void CalculateEdges()
        {
            Results.Clear();
            AllEdgeScores.Clear();
            for (int i = 0; i < 20; i++)
            {
                Results.Add(i);
                AllEdgeScores.Add(i, new EdgeScore(GetDualEdgeScores(i)));
                if (EdgeCalculated != null)
                    EdgeCalculated(null, new EventArgs());
            }

            if (EdgeCalculationCompleted != null)
                EdgeCalculationCompleted(null, new EventArgs());
        }

        public static void SegmentWidth(Bitmap bitmap, double sensitivity)
        {
            int Height = bitmap.Height;
            int Width = bitmap.Width;
            var results = new Dictionary<int, double>();

            for (int i = 0; i < Width - 1; i++)
            {
                var l = new List<Color>();
                var r = new List<Color>();
                for (int j = 0; j < Height; j++)
                {
                    l.Add(bitmap.GetPixel(i, j));
                    r.Add(bitmap.GetPixel(i + 1, j));
                }
                results.Add(i, EdgeCompareScore(l, r));

                if (SegmentWidthCalculated != null && i%20 == 0)
                    SegmentWidthCalculated(null, new EventArgs());
            }

            double max = results.Max(x => x.Value);
            double min = results.Min(x => x.Value);
            double range = max - min;
            Dictionary<int, double> normalisedResults = results.ToDictionary(x => x.Key, x => (x.Value - min)/range);
            List<int> edgeIndexes = normalisedResults.Where(x => x.Value > sensitivity).Select(x => x.Key).ToList();
            IEnumerable<int> columnWidth =
                edgeIndexes.Where((x, idx) => idx < edgeIndexes.Count - 1 && idx >= 0).Select(
                    (x, idx) => Math.Abs(edgeIndexes[idx] - edgeIndexes[idx + 1]));
            var groupedColumnWidth = columnWidth.GroupBy(i => i).Select(g => new {g.Key, Count = g.Count()});
            segmentWidth =
                groupedColumnWidth.Where(x => x.Count == groupedColumnWidth.Max(y => y.Count)).Select(x => x.Key).First();


            if (SegmentWidthCalculationCompleted != null)
                SegmentWidthCalculationCompleted(null, new EventArgs());
        }

        public static void SortEdges()
        {
            // Find Left Edge - Where X's best left match is Y but X is not Y's best right match
            IEnumerable<int> leftEdges = AllEdgeScores
                .Where(x => x.Key != AllEdgeScores[x.Value.BestLeftMatchIndex].BestRightMatchIndex)
                .Select(x => x.Key);
            Results[0] = leftEdges.FirstOrDefault();

            // Find Right Edge - Where X's best right match is Y but X is not Y's best left match
            IEnumerable<int> rightEdges = AllEdgeScores
                .Where(x => x.Key != AllEdgeScores[x.Value.BestRightMatchIndex].BestLeftMatchIndex)
                .Select(x => x.Key);

            Results[19] = rightEdges.FirstOrDefault();

            for (int i = 1; i < 19; i++)
            {
                int rightNeighborIndex = GetRightNeighbor(Results[i - 1]);
                Results[i] = rightNeighborIndex;
            }
        }

        private static double EdgeCompareScore(List<Color> EdgeA, List<Color> EdgeB)
        {
            if (EdgeA.Count != EdgeB.Count) throw new Exception("Edge Sample Counts Don't match lengths");

            double aggregate = 0;

            var l = new List<int>();
            for (int i = 0; i < 20; i++)
            {
                l.Add(i);
            }

            for (int i = 0; i < EdgeA.Count; i += 1)
            {
                /*
                1 2
                S 4
                5 6  
                */

                int pS = EdgeA[i].ToArgb();
                int p4 = EdgeB[i].ToArgb();

                // All neighbor compare pattern
                //int p2 = EdgeB[i - 1].ToArgb();
                //int p6 = EdgeB[i + 1].ToArgb();
                //aggregate += (Math.Abs(pS - p2) + Math.Abs(pS - p4) + Math.Abs(pS - p6)) / 3.0;

                // 1-1 compare pattern
                aggregate += Math.Abs(pS - p4);
            }

            double score = aggregate/EdgeA.Count;
            return score;
        }

        private static Dictionary<int, PixelEdgeScore> GetDualEdgeScores(int source)
        {
            Bitmap s = ImageSegment(source);
            List<Color> sEdgeLeft = GetEdge(s, EdgeSide.Left);
            List<Color> sEdgeRight = GetEdge(s, EdgeSide.Right);

            var l = new List<int>();
            for (int i = 0; i < 20; i++)
            {
                l.Add(i);
            }
            var d = new Dictionary<int, PixelEdgeScore>();
            Parallel.ForEach(l, i =>
                                    {
                                        if (i == source) return;
                                        Bitmap c = ImageSegment(i);
                                        List<Color> cEdgeL = GetEdge(c, EdgeSide.Left);
                                        List<Color> cEdgeR = GetEdge(c, EdgeSide.Right);

                                        double leftScore = EdgeCompareScore(sEdgeLeft, cEdgeR);
                                        double rightScore = EdgeCompareScore(sEdgeRight, cEdgeL);

                                        var edgeScore = new PixelEdgeScore {Left = leftScore, Right = rightScore};
                                        lock (myLock)
                                        {
                                            d.Add(i, edgeScore);
                                        }
                                    });

            Dictionary<int, PixelEdgeScore> sd = d.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            return sd;
        }

        private static List<Color> GetEdge(Bitmap input, EdgeSide edgeSide)
        {
            int height = input.Height;
            var l = new List<Color>();
            if (edgeSide == EdgeSide.Left)
            {
                for (int i = 0; i < height; i++)
                    l.Add(input.GetPixel(0, i));

                return l;
            }

            // Calculate zero-based width
            int width = input.Width - 1;

            for (int i = 0; i < height; i++)
                l.Add(input.GetPixel(width, i));

            return l;
        }

        private static int GetRightNeighbor(int segment)
        {
            return AllEdgeScores[segment].BestRightMatchIndex;
        }

        private static Bitmap ImageSegment(int segment)
        {
            return ImageSegment(segment*32, 32);
        }

        private static Bitmap ImageSegment(int start, int width)
        {
            var input = new Bitmap(@"unshred.png");
            var output = new Bitmap(32, 359);

            for (int i = start; i < start + width; i++)
            {
                for (int j = 0; j < 359; j++)
                {
                    Color c = input.GetPixel(i, j);
                    output.SetPixel(i - start, j, c);
                }
            }

            return output;
        }

        #endregion
    }
}