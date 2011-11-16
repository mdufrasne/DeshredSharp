using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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

        public static readonly Dictionary<int, int> Results = new Dictionary<int, int>();

        private static readonly Dictionary<int, EdgeScore> AllEdgeScores =
            new Dictionary<int, EdgeScore>();

        #endregion

        #region Event Declarations

        public static event EventHandler EdgeCalculated;
        public static event EventHandler EdgeCalculationCompleted;

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
                            b.SetPixel(j + (i * 32), k, segment.GetPixel(j, k));
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
            for (int i = 0; i < 20; i++)
            {
                Results[i] = i;
                AllEdgeScores.Add(i, new EdgeScore(GetDualEdgeScores(i)));
                if (EdgeCalculated != null)
                    EdgeCalculated(null, new EventArgs());
            }

            if (EdgeCalculationCompleted != null)
                EdgeCalculationCompleted(null, new EventArgs());
        }

        public static List<int> GetNeighbors(int segment)
        {
            var l = new List<int>
                        {
                            AllEdgeScores[segment].BestLeftMatchIndex,
                            AllEdgeScores[segment].BestRightMatchIndex
                        };
            return l;
        }

        public static Bitmap ImageSegment(int segment)
        {
            return ImageSegment(segment * 32, 32);
        }

        public static void SortEdges()
        {
            for (int i = 0; i < 20; i++)
            {
                int t;
                int brm = AllEdgeScores[i].BestRightMatchIndex;
                int blm = AllEdgeScores[i].BestLeftMatchIndex;

                int rpos = Results.Where(x => x.Value == brm).Select(x => x.Key).First();
                if (i < 19 && AllEdgeScores[rpos].BestLeftMatchIndex == i)
                {
                    t = Results[i + 1];
                    Results[i + 1] = brm;
                    Results[rpos] = t;
                }

                int lpos = Results.Where(x => x.Value == blm).Select(x => x.Key).First();
                if (i > 0 && AllEdgeScores[lpos].BestRightMatchIndex == i)
                {
                    t = Results[i - 1];
                    Results[i - 1] = blm;
                    Results[lpos] = t;
                }
            }
        }

        private static double EdgeCompareScore(List<Color> EdgeA, List<Color> EdgeB)
        {
            if (EdgeA.Count != EdgeB.Count) throw new Exception("Edge Sample Counts Don't match lengths");

            double aggregate = 0;

            for (int i = 1; i < EdgeA.Count - 1; i++)
            {
                /*
                1 2
                S 4
                5 6  
                */

                int p2 = EdgeB[i - 1].ToArgb();
                int pS = EdgeA[i].ToArgb();
                int p4 = EdgeB[i].ToArgb();
                int p6 = EdgeB[i + 1].ToArgb();

                aggregate += (Math.Abs(pS - p2) + Math.Abs(pS - p4) + Math.Abs(pS - p6)) / 3.0;
                //aggregate += Math.Abs(pS - p4);
            }

            double score = (double)aggregate / EdgeA.Count;
            return score;
        }

        private static Dictionary<int, PixelEdgeScore> GetDualEdgeScores(int source)
        {
            var d = new Dictionary<int, PixelEdgeScore>();
            Bitmap s = ImageSegment(source);
            List<Color> sEdgeLeft = GetEdge(s, EdgeSide.Left);
            List<Color> sEdgeRight = GetEdge(s, EdgeSide.Right);

            for (int i = 0; i < 20; i++)
            {
                if (i == source) continue;
                Bitmap c = ImageSegment(i);
                List<Color> cEdgeL = GetEdge(c, EdgeSide.Left);
                List<Color> cEdgeR = GetEdge(c, EdgeSide.Right);

                double leftScore = EdgeCompareScore(sEdgeLeft, cEdgeR);
                double rightScore = EdgeCompareScore(sEdgeRight, cEdgeL);

                var edgeScore = new PixelEdgeScore { Left = leftScore, Right = rightScore };
                d.Add(i, edgeScore);
            }


            return d;
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