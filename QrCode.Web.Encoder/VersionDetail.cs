namespace QrCode.Web.Encoder
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public struct VersionDetail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionDetail"/> struct.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="numTotalBytes">The num total bytes.</param>
        /// <param name="numDataBytes">The num data bytes.</param>
        /// <param name="numECBlocks">The num EC blocks.</param>
        /// <remarks></remarks>
        internal VersionDetail(int version, int numTotalBytes, int numDataBytes, int numECBlocks)
            : this()
        {
            Version = version;
            NumTotalBytes = numTotalBytes;
            NumDataBytes = numDataBytes;
            NumECBlocks = numECBlocks;
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        /// <remarks></remarks>
        internal int Version { get; private set; }

        /// <summary>
        /// Gets or sets the num total bytes.
        /// </summary>
        /// <value>The num total bytes.</value>
        /// <remarks></remarks>
        internal int NumTotalBytes { get; private set; }

        /// <summary>
        /// Gets or sets the num data bytes.
        /// </summary>
        /// <value>The num data bytes.</value>
        /// <remarks></remarks>
        internal int NumDataBytes { get; private set; }

        /// <summary>
        /// Gets or sets the num EC blocks.
        /// </summary>
        /// <value>The num EC blocks.</value>
        /// <remarks></remarks>
        internal int NumECBlocks { get; private set; }

        /// <summary>
        /// Width for current version
        /// </summary>
        /// <remarks></remarks>
        internal int MatrixWidth
        {
            get { return Width(Version); }
        }

        /// <summary>
        /// number of Error correction blocks for group 1
        /// </summary>
        /// <remarks></remarks>
        internal int ECBlockGroup1
        {
            get { return NumECBlocks - ECBlockGroup2; }
        }

        /// <summary>
        /// Number of error correction blocks for group 2
        /// </summary>
        /// <remarks></remarks>
        internal int ECBlockGroup2
        {
            get { return NumTotalBytes%NumECBlocks; }
        }

        /// <summary>
        /// Number of data bytes per block for group 1
        /// </summary>
        /// <remarks></remarks>
        internal int NumDataBytesGroup1
        {
            get { return NumDataBytes/NumECBlocks; }
        }

        /// <summary>
        /// Number of data bytes per block for group 2
        /// </summary>
        /// <remarks></remarks>
        internal int NumDataBytesGroup2
        {
            get { return NumDataBytesGroup1 + 1; }
        }

        /// <summary>
        /// Number of error correction bytes per block
        /// </summary>
        /// <remarks></remarks>
        internal int NumECBytesPerBlock
        {
            get { return (NumTotalBytes - NumDataBytes)/NumECBlocks; }
        }

        /// <summary>
        /// Widthes the specified version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static int Width(int version)
        {
            return 17 + 4*version;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return Version + ";" + NumTotalBytes + ";" + NumDataBytes + ";" + NumECBlocks;
        }
    }
}