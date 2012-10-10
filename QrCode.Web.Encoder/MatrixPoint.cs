namespace QrCode.Web.Encoder
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public struct MatrixPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixPoint"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <remarks></remarks>
        internal MatrixPoint(int x, int y)
            : this()
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>The X.</value>
        /// <remarks></remarks>
        public int X { get; private set; }

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>The Y.</value>
        /// <remarks></remarks>
        public int Y { get; private set; }

        /// <summary>
        /// Offsets the specified offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public MatrixPoint Offset(MatrixPoint offset)
        {
            return new MatrixPoint(offset.X + X, offset.Y + Y);
        }

        /// <summary>
        /// Offsets the specified offset X.
        /// </summary>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal MatrixPoint Offset(int offsetX, int offsetY)
        {
            return Offset(new MatrixPoint(offsetX, offsetY));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return string.Format("Point({0};{1})", X, Y);
        }
    }
}