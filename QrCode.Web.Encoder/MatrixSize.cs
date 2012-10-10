namespace QrCode.Web.Encoder
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public struct MatrixSize
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixSize"/> struct.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <remarks></remarks>
        internal MatrixSize(int width, int height)
            : this()
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        /// <remarks></remarks>
        public int Width { get; private set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        /// <remarks></remarks>
        public int Height { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return string.Format("Size({0};{1})", Width, Height);
        }
    }
}