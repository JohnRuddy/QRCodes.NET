using System.Drawing;

namespace QrCode.Web.Encoder.Windows.Render
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class EPSFormColor : EPSColor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EPSFormColor"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <remarks></remarks>
        public EPSFormColor(Color color)
        {
            Color = color;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        /// <remarks></remarks>
        public Color Color { get; set; }

        /// <summary>
        /// Gets the A.
        /// </summary>
        /// <remarks></remarks>
        public override byte A
        {
            get { return Color.A; }
        }

        /// <summary>
        /// Gets the B.
        /// </summary>
        /// <remarks></remarks>
        public override byte B
        {
            get { return Color.B; }
        }

        /// <summary>
        /// Gets the G.
        /// </summary>
        /// <remarks></remarks>
        public override byte G
        {
            get { return Color.G; }
        }

        /// <summary>
        /// Gets the R.
        /// </summary>
        /// <remarks></remarks>
        public override byte R
        {
            get { return Color.R; }
        }
    }
}