namespace QrCode.Web.Encoder.Windows.Render
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public struct DrawingSize
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingSize"/> struct.
        /// </summary>
        /// <param name="moduleSize">Size of the module.</param>
        /// <param name="codeWidth">Width of the code.</param>
        /// <param name="quietZoneModules">The quiet zone modules.</param>
        /// <remarks></remarks>
        public DrawingSize(int moduleSize, int codeWidth, QuietZoneModules quietZoneModules)
            : this()
        {
            ModuleSize = moduleSize;
            CodeWidth = codeWidth;
            QuietZoneModules = quietZoneModules;
        }

        /// <summary>
        /// Module pixel width
        /// </summary>
        /// <value>The size of the module.</value>
        /// <remarks></remarks>
        public int ModuleSize { get; private set; }

        /// <summary>
        /// QrCode pixel width
        /// </summary>
        /// <value>The width of the code.</value>
        /// <remarks></remarks>
        public int CodeWidth { get; private set; }

        /// <summary>
        /// Gets or sets the quiet zone modules.
        /// </summary>
        /// <value>The quiet zone modules.</value>
        /// <remarks></remarks>
        public QuietZoneModules QuietZoneModules { get; private set; }
    }
}