using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace DeShred
{
    public class ShreddedImage
    {
        #region Enums

        private enum EdgeSide
        {
            Right,
            Left
        }

        #endregion

        #region Readonly

        private readonly Dictionary<int, EdgeScore> _allEdgeScores = new Dictionary<int, EdgeScore>();

        private readonly Bitmap _bitmapSource;


        private readonly object _bitmapSourceLock = new object();
        private readonly object _myLock = new object();
        private readonly List<int> _results = new List<int>();

        #endregion

        #region Fields

        private int _segmentWidthBackingField;

        #endregion

        #region C'tors

        public ShreddedImage(Bitmap originalImage)
        {
            _bitmapSource = originalImage;
            bitmapSourceHeight = originalImage.Height;
            bitmapSourceWidth = originalImage.Width;
        }

        public ShreddedImage(Bitmap originalImage, int segmentWidth)
        {
            if (segmentWidth < 1 || segmentWidth > originalImage.Width || originalImage.Width % SegmentWidth != 0)
                throw new Exception(
                    "Invalid Segment Width Specified: Segment Width must be positive, less than the image size and divide the image size evenly");
            SegmentWidth = segmentWidth;
            _bitmapSource = originalImage;

            bitmapSourceHeight = originalImage.Height;
            bitmapSourceWidth = originalImage.Width;
        }

        #endregion

        #region Instance Properties

        public Bitmap OutputBitmap
        {
            get
            {
                var b = new Bitmap(_bitmapSource.Width, _bitmapSource.Height);
                for (int i = 0; i < _bitmapSource.Width / SegmentWidth; i++)
                {
                    int segmentNumber = _results[i];
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

        public int SegmentWidth
        {
            get
            {
                if (_segmentWidthBackingField == 0) CalculateSegmentWidth(_bitmapSource, .55);
                return _segmentWidthBackingField;
            }
            private set { _segmentWidthBackingField = value; }
        }

        private readonly int bitmapSourceHeight;


        private readonly int bitmapSourceWidth;


        #endregion

        #region Instance Methods

        public void CalculateEdges()
        {
            _results.Clear();
            _allEdgeScores.Clear();
            for (int i = 0; i < 20; i++)
            {
                _results.Add(i);
                _allEdgeScores.Add(i, new EdgeScore(GetDualEdgeScores(i)));
                if (EdgeCalculated != null)
                    EdgeCalculated(null, new EventArgs());
            }

            if (EdgeCalculationCompleted != null)
                EdgeCalculationCompleted(null, new EventArgs());
        }

        public void CalculateSegmentWidth(Bitmap bitmap, double sensitivity)
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

                if (SegmentWidthCalculated != null && i % 20 == 0)
                    SegmentWidthCalculated(null, new EventArgs());
            }

            double max = results.Max(x => x.Value);
            double min = results.Min(x => x.Value);
            double range = max - min;
            Dictionary<int, double> normalisedResults = results.ToDictionary(x => x.Key, x => (x.Value - min) / range);
            List<int> edgeIndexes = normalisedResults.Where(x => x.Value > sensitivity).Select(x => x.Key).ToList();
            IEnumerable<int> columnWidth =
                edgeIndexes.Where((x, idx) => idx < edgeIndexes.Count - 1 && idx >= 0).Select(
                    (x, idx) => Math.Abs(edgeIndexes[idx] - edgeIndexes[idx + 1]));
            var groupedColumnWidth = columnWidth.GroupBy(i => i).Select(g => new { g.Key, Count = g.Count() });
            SegmentWidth =
                groupedColumnWidth.Where(x => x.Count == groupedColumnWidth.Max(y => y.Count)).Select(x => x.Key).First();


            if (SegmentWidthCalculationCompleted != null)
                SegmentWidthCalculationCompleted(null, new EventArgs());
        }

        public void SortEdges()
        {
            // Find Left Edge - Where X's best left match is Y but X is not Y's best right match
            IEnumerable<int> leftEdges = _allEdgeScores
                .Where(x => x.Key != _allEdgeScores[x.Value.BestLeftMatchIndex].BestRightMatchIndex)
                .Select(x => x.Key);
            _results[0] = leftEdges.FirstOrDefault();

            // Find Right Edge - Where X's best right match is Y but X is not Y's best left match
            IEnumerable<int> rightEdges = _allEdgeScores
                .Where(x => x.Key != _allEdgeScores[x.Value.BestRightMatchIndex].BestLeftMatchIndex)
                .Select(x => x.Key);

            _results[19] = rightEdges.FirstOrDefault();

            for (int i = 1; i < 19; i++)
            {
                int rightNeighborIndex = GetRightNeighbor(_results[i - 1]);
                _results[i] = rightNeighborIndex;
            }
        }

        private Dictionary<int, PixelEdgeScore> GetDualEdgeScores(int source)
        {
            Bitmap s = ImageSegment(source);
            List<Color> sEdgeLeft = GetEdge(s, EdgeSide.Left);
            List<Color> sEdgeRight = GetEdge(s, EdgeSide.Right);

            var d = new Dictionary<int, PixelEdgeScore>();

            Parallel.For(0, 20, i =>
                                    {
                                        if (i == source) return;
                                        Bitmap c = ImageSegment(i);
                                        List<Color> cEdgeL = GetEdge(c, EdgeSide.Left);
                                        List<Color> cEdgeR = GetEdge(c, EdgeSide.Right);

                                        double leftScore = EdgeCompareScore(sEdgeLeft, cEdgeR);
                                        double rightScore = EdgeCompareScore(sEdgeRight, cEdgeL);

                                        var edgeScore = new PixelEdgeScore { Left = leftScore, Right = rightScore };
                                        lock (_myLock)
                                        {
                                            d.Add(i, edgeScore);
                                        }
                                    });

            Dictionary<int, PixelEdgeScore> sd = d.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            return sd;
        }

        private int GetRightNeighbor(int segment)
        {
            return _allEdgeScores[segment].BestRightMatchIndex;
        }

        /// <summary>
        /// Returns an image segment as a Bitmap
        /// </summary>
        /// <param name="segment">Zero based segment index of source image</param>
        /// <returns>Bitmap</returns>
        private Bitmap ImageSegment(int segment)
        {
            int start = segment * SegmentWidth;
            return ImageSegment(start, SegmentWidth);
        }

        private Bitmap ImageSegment(int start, int width)
        {
            Bitmap input = _bitmapSource;
            var output = new Bitmap(SegmentWidth, bitmapSourceHeight);

            for (int i = start; i < start + width; i++)
            {
                for (int j = 0; j < bitmapSourceHeight; j++)
                {
                    Color c;
                    lock (_bitmapSourceLock)
                    {
                        c = input.GetPixel(i, j);
                    }
                    output.SetPixel(i - start, j, c);
                }
            }

            return output;
        }

        #endregion

        #region Event Declarations

        public event EventHandler EdgeCalculated;
        public event EventHandler EdgeCalculationCompleted;

        public event EventHandler SegmentWidthCalculated;
        public event EventHandler SegmentWidthCalculationCompleted;

        #endregion

        #region Class Methods

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

            double score = aggregate / EdgeA.Count;
            return score;
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

        #endregion
    }
}