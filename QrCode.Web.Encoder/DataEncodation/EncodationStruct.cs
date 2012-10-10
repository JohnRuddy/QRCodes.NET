using QrCode.Web.Encoder.Versions;

namespace QrCode.Web.Encoder.DataEncodation
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal struct EncodationStruct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncodationStruct"/> struct.
        /// </summary>
        /// <param name="vcStruct">The vc struct.</param>
        /// <remarks></remarks>
        internal EncodationStruct(VersionControlStruct vcStruct)
            : this()
        {
            VersionDetail = vcStruct.VersionDetail;
        }

        /// <summary>
        /// Gets or sets the version detail.
        /// </summary>
        /// <value>The version detail.</value>
        /// <remarks></remarks>
        internal VersionDetail VersionDetail { get; set; }

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode.</value>
        /// <remarks></remarks>
        internal Mode Mode { get; set; }

        /// <summary>
        /// Gets or sets the data codewords.
        /// </summary>
        /// <value>The data codewords.</value>
        /// <remarks></remarks>
        internal BitList DataCodewords { get; set; }
    }
}