namespace QrCode.Web.Encoder
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public sealed class StateMatrix
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly int m_Width;

        /// <summary>
        /// 
        /// </summary>
        private readonly MatrixStatus[,] m_matrixStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMatrix"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <remarks></remarks>
        public StateMatrix(int width)
        {
            m_Width = width;
            m_matrixStatus = new MatrixStatus[width,width];
        }

        /// <summary>
        /// Gets or sets the <see cref="QrCode.Web.Encoder.MatrixStatus"/> with the specified x.
        /// </summary>
        /// <remarks></remarks>
        public MatrixStatus this[int x, int y]
        {
            get { return m_matrixStatus[x, y]; }
            set { m_matrixStatus[x, y] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="QrCode.Web.Encoder.MatrixStatus"/> with the specified point.
        /// </summary>
        /// <remarks></remarks>
        internal MatrixStatus this[MatrixPoint point]
        {
            get { return this[point.X, point.Y]; }
            set { this[point.X, point.Y] = value; }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <remarks></remarks>
        public int Width
        {
            get { return m_Width; }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <remarks></remarks>
        public int Height
        {
            get { return Width; }
        }
    }
}