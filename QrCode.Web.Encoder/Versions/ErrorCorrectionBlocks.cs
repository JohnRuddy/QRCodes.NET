using System;

namespace QrCode.Web.Encoder.Versions
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal struct ErrorCorrectionBlocks
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ErrorCorrectionBlock[] m_ECBlock;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorCorrectionBlocks"/> struct.
        /// </summary>
        /// <param name="numErrorCorrectionCodewords">The num error correction codewords.</param>
        /// <param name="ecBlock">The ec block.</param>
        /// <remarks></remarks>
        internal ErrorCorrectionBlocks(int numErrorCorrectionCodewords, ErrorCorrectionBlock ecBlock)
            : this()
        {
            NumErrorCorrectionCodewards = numErrorCorrectionCodewords;
            m_ECBlock = new[] {ecBlock};

            initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorCorrectionBlocks"/> struct.
        /// </summary>
        /// <param name="numErrorCorrectionCodewords">The num error correction codewords.</param>
        /// <param name="ecBlock1">The ec block1.</param>
        /// <param name="ecBlock2">The ec block2.</param>
        /// <remarks></remarks>
        internal ErrorCorrectionBlocks(int numErrorCorrectionCodewords, ErrorCorrectionBlock ecBlock1,
                                       ErrorCorrectionBlock ecBlock2)
            : this()
        {
            NumErrorCorrectionCodewards = numErrorCorrectionCodewords;
            m_ECBlock = new[] {ecBlock1, ecBlock2};

            initialize();
        }

        /// <summary>
        /// Gets or sets the num error correction codewards.
        /// </summary>
        /// <value>The num error correction codewards.</value>
        /// <remarks></remarks>
        internal int NumErrorCorrectionCodewards { get; private set; }

        /// <summary>
        /// Gets or sets the num blocks.
        /// </summary>
        /// <value>The num blocks.</value>
        /// <remarks></remarks>
        internal int NumBlocks { get; private set; }

        /// <summary>
        /// Gets or sets the error correction codewords per block.
        /// </summary>
        /// <value>The error correction codewords per block.</value>
        /// <remarks></remarks>
        internal int ErrorCorrectionCodewordsPerBlock { get; private set; }

        /// <summary>
        /// Get Error Correction Blocks
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        internal ErrorCorrectionBlock[] GetECBlocks()
        {
            return m_ECBlock;
        }

        /// <summary>
        /// Initialize for NumBlocks and ErrorCorrectionCodewordsPerBlock
        /// </summary>
        /// <remarks></remarks>
        private void initialize()
        {
            if (m_ECBlock == null)
                throw new ArgumentNullException("ErrorCorrectionBlocks array doesn't contain any value");

            NumBlocks = 0;
            int blockLength = m_ECBlock.Length;
            for (int i = 0; i < blockLength; i++)
            {
                NumBlocks += m_ECBlock[i].NumErrorCorrectionBlock;
            }


            ErrorCorrectionCodewordsPerBlock = NumErrorCorrectionCodewards/NumBlocks;
        }
    }
}