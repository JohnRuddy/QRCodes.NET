namespace QrCode.Web.Encoder.Positioning.Stencils
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal class PositionDetectionPattern : PatternStencilBase
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly bool[,] s_PositionDetection = new[,]
                                                                  {
                                                                      {o, o, o, o, o, o, o, o, o},
                                                                      {o, x, x, x, x, x, x, x, o},
                                                                      {o, x, o, o, o, o, o, x, o},
                                                                      {o, x, o, x, x, x, o, x, o},
                                                                      {o, x, o, x, x, x, o, x, o},
                                                                      {o, x, o, x, x, x, o, x, o},
                                                                      {o, x, o, o, o, o, o, x, o},
                                                                      {o, x, x, x, x, x, x, x, o},
                                                                      {o, o, o, o, o, o, o, o, o}
                                                                  };

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternStencilBase"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <remarks></remarks>
        public PositionDetectionPattern(int version)
            : base(version)
        {
        }

        /// <summary>
        /// Gets the stencil.
        /// </summary>
        /// <remarks></remarks>
        public override bool[,] Stencil
        {
            get { return s_PositionDetection; }
        }

        /// <summary>
        /// Applies to.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <remarks></remarks>
        public override void ApplyTo(TriStateMatrix matrix)
        {
            MatrixSize size = GetSizeOfSquareWithSeparators();

            var leftTopCorner = new MatrixPoint(0, 0);
            CopyTo(matrix, new MatrixRectangle(new MatrixPoint(1, 1), size), leftTopCorner, MatrixStatus.NoMask);

            var rightTopCorner = new MatrixPoint(matrix.Width - Width + 1, 0);
            CopyTo(matrix, new MatrixRectangle(new MatrixPoint(0, 1), size), rightTopCorner, MatrixStatus.NoMask);


            var leftBottomCorner = new MatrixPoint(0, matrix.Width - Width + 1);
            CopyTo(matrix, new MatrixRectangle(new MatrixPoint(1, 0), size), leftBottomCorner, MatrixStatus.NoMask);
        }

        /// <summary>
        /// Gets the size of square with separators.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private MatrixSize GetSizeOfSquareWithSeparators()
        {
            return new MatrixSize(Width - 1, Height - 1);
        }
    }
}