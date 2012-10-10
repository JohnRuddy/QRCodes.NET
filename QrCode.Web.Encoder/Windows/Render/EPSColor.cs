namespace QrCode.Web.Encoder.Windows.Render
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public abstract class EPSColor
    {
        /// <summary>
        /// Gets the R.
        /// </summary>
        /// <remarks></remarks>
        public abstract byte R { get; }

        /// <summary>
        /// Gets the G.
        /// </summary>
        /// <remarks></remarks>
        public abstract byte G { get; }

        /// <summary>
        /// Gets the B.
        /// </summary>
        /// <remarks></remarks>
        public abstract byte B { get; }

        /// <summary>
        /// Gets the A.
        /// </summary>
        /// <remarks></remarks>
        public abstract byte A { get; }
    }
}