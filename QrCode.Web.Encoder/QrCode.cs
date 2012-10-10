namespace QrCode.Web.Encoder
{
    /// <summary>
    /// This class contain two variables.
    /// BitMatrix for QrCode
    /// isContainMatrix for indicate whether QrCode contains BitMatrix or not.
    /// BitMatrix will equal to null if isContainMatrix is false.
    /// </summary>
    /// <remarks></remarks>
    public class QrCode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QrCode"/> class.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <remarks></remarks>
        internal QrCode(BitMatrix matrix)
        {
            Matrix = matrix;
            isContainMatrix = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public QrCode()
        {
            isContainMatrix = false;
            Matrix = null;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is contain matrix.
        /// </summary>
        /// <remarks></remarks>
        public bool isContainMatrix { get; private set; }

        /// <summary>
        /// Gets the matrix.
        /// </summary>
        /// <remarks></remarks>
        public BitMatrix Matrix { get; private set; }
    }
}