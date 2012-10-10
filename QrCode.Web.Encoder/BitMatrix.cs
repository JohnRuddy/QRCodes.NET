namespace QrCode.Web.Encoder
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public abstract class BitMatrix
    {
        /// <summary>
        /// Gets or sets the <see cref="System.Boolean"/> with the specified i.
        /// </summary>
        /// <remarks></remarks>
        public abstract bool this[int i, int j] { get; set; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <remarks></remarks>
        public abstract int Width { get; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <remarks></remarks>
        public abstract int Height { get; }

        /// <summary>
        /// Gets the internal array.
        /// </summary>
        /// <remarks></remarks>
        public abstract bool[,] InternalArray { get; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <remarks></remarks>
        internal MatrixSize Size
        {
            get { return new MatrixSize(Width, Height); }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Boolean"/> with the specified point.
        /// </summary>
        /// <remarks></remarks>
        internal bool this[MatrixPoint point]
        {
            get { return this[point.X, point.Y]; }
            set { this[point.X, point.Y] = value; }
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="sourceArea">The source area.</param>
        /// <param name="targetPoint">The target point.</param>
        /// <param name="mstatus">The mstatus.</param>
        /// <remarks></remarks>
        internal void CopyTo(TriStateMatrix target, MatrixRectangle sourceArea, MatrixPoint targetPoint,
                             MatrixStatus mstatus)
        {
            for (int j = 0; j < sourceArea.Size.Height; j++)
            {
                for (int i = 0; i < sourceArea.Size.Width; i++)
                {
                    bool value = this[sourceArea.Location.X + i, sourceArea.Location.Y + j];
                    target[targetPoint.X + i, targetPoint.Y + j, mstatus] = value;
                }
            }
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="targetPoint">The target point.</param>
        /// <param name="mstatus">The mstatus.</param>
        /// <remarks></remarks>
        internal void CopyTo(TriStateMatrix target, MatrixPoint targetPoint, MatrixStatus mstatus)
        {
            CopyTo(target, new MatrixRectangle(new MatrixPoint(0, 0), new MatrixSize(Width, Height)), targetPoint,
                   mstatus);
        }
    }
}