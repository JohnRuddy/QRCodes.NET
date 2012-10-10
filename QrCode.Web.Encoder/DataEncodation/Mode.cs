namespace QrCode.Web.Encoder.DataEncodation
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public enum Mode
    {
        /// <summary>
        /// 
        /// </summary>
        Numeric = 0001,

        /// <summary>
        /// 
        /// </summary>
        Alphanumeric = 0001 << 1,

        /// <summary>
        /// 
        /// </summary>
        EightBitByte = 0001 << 2,

        /// <summary>
        /// 
        /// </summary>
        Kanji = 0001 << 3,
    }
}