using System;

namespace QrCode.Web.Encoder.Versions
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal struct QRCodeVersion
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ErrorCorrectionBlocks[] m_ECBlocks;

        /// <summary>
        /// Initializes a new instance of the <see cref="QRCodeVersion"/> struct.
        /// </summary>
        /// <param name="versionNum">The version num.</param>
        /// <param name="totalCodewords">The total codewords.</param>
        /// <param name="ecblocksL">The ecblocks L.</param>
        /// <param name="ecblocksM">The ecblocks M.</param>
        /// <param name="ecblocksQ">The ecblocks Q.</param>
        /// <param name="ecblocksH">The ecblocks H.</param>
        /// <remarks></remarks>
        internal QRCodeVersion(int versionNum, int totalCodewords, ErrorCorrectionBlocks ecblocksL,
                               ErrorCorrectionBlocks ecblocksM, ErrorCorrectionBlocks ecblocksQ,
                               ErrorCorrectionBlocks ecblocksH)
            : this()
        {
            VersionNum = versionNum;
            TotalCodewords = totalCodewords;
            m_ECBlocks = new[] {ecblocksL, ecblocksM, ecblocksQ, ecblocksH};
            DimensionForVersion = 17 + versionNum*4;
        }

        /// <summary>
        /// Gets or sets the version num.
        /// </summary>
        /// <value>The version num.</value>
        /// <remarks></remarks>
        internal int VersionNum { get; private set; }

        /// <summary>
        /// Gets or sets the total codewords.
        /// </summary>
        /// <value>The total codewords.</value>
        /// <remarks></remarks>
        internal int TotalCodewords { get; private set; }

        /// <summary>
        /// Gets or sets the dimension for version.
        /// </summary>
        /// <value>The dimension for version.</value>
        /// <remarks></remarks>
        internal int DimensionForVersion { get; private set; }

        /// <summary>
        /// Get Error Correction Blocks by level
        /// </summary>
        /// <param name="ECLevel">The EC level.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        //[method
        internal ErrorCorrectionBlocks GetECBlocksByLevel(ErrorCorrectionLevel ECLevel)
        {
            switch (ECLevel)
            {
                case ErrorCorrectionLevel.L:
                    return m_ECBlocks[0];
                case ErrorCorrectionLevel.M:
                    return m_ECBlocks[1];
                case ErrorCorrectionLevel.Q:
                    return m_ECBlocks[2];
                case ErrorCorrectionLevel.H:
                    return m_ECBlocks[3];
                default:
                    throw new ArgumentOutOfRangeException("Invalide ErrorCorrectionLevel");
            }
        }
    }
}