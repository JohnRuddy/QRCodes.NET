namespace QrCode.Web.Encoder.Versions
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal struct VersionControlStruct
    {
        /// <summary>
        /// Gets or sets the version detail.
        /// </summary>
        /// <value>The version detail.</value>
        /// <remarks></remarks>
        internal VersionDetail VersionDetail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is contain ECI.
        /// </summary>
        /// <value><c>true</c> if this instance is contain ECI; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        internal bool isContainECI { get; set; }

        /// <summary>
        /// Gets or sets the ECI header.
        /// </summary>
        /// <value>The ECI header.</value>
        /// <remarks></remarks>
        internal BitList ECIHeader { get; set; }
    }
}