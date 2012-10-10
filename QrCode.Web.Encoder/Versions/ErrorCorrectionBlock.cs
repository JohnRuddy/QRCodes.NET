namespace QrCode.Web.Encoder.Versions
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal struct ErrorCorrectionBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorCorrectionBlock"/> struct.
        /// </summary>
        /// <param name="numErrorCorrectionBlock">The num error correction block.</param>
        /// <param name="numDataCodewards">The num data codewards.</param>
        /// <remarks></remarks>
        internal ErrorCorrectionBlock(int numErrorCorrectionBlock, int numDataCodewards)
            : this()
        {
            NumErrorCorrectionBlock = numErrorCorrectionBlock;
            NumDataCodewords = numDataCodewards;
        }

        /// <summary>
        /// Gets or sets the num error correction block.
        /// </summary>
        /// <value>The num error correction block.</value>
        /// <remarks></remarks>
        internal int NumErrorCorrectionBlock { get; private set; }

        /// <summary>
        /// Gets or sets the num data codewords.
        /// </summary>
        /// <value>The num data codewords.</value>
        /// <remarks></remarks>
        internal int NumDataCodewords { get; private set; }
    }
}