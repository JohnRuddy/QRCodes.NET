namespace QrCode.Web.Encoder
{
    /// <summary>
    /// Contain most of common constant variables. S
    /// </summary>
    /// <remarks></remarks>
    public static class QRCodeConstantVariable
    {
        /// <summary>
        /// 
        /// </summary>
        public const int MinVersion = 1;

        /// <summary>
        /// 
        /// </summary>
        public const int MaxVersion = 40;

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultEncoding = "iso-8859-1";

        /// <summary>
        /// 
        /// </summary>
        public const string UTF8Encoding = "utf-8";

        /// <summary>
        /// ISO/IEC 18004:2006(E) Page 45 Chapter Generating the error correction codewords
        /// Primative Polynomial = Bin 100011101 = Dec 285
        /// </summary>
        public const int QRCodePrimitive = 285;

        /// <summary>
        /// 
        /// </summary>
        internal const int TerminatorNPaddingBit = 0;

        /// <summary>
        /// 
        /// </summary>
        internal const int TerminatorLength = 4;

        /// <summary>
        /// 0xEC
        /// </summary>
        internal const int PadeCodewordsOdd = 0xec;

        /// <summary>
        /// 0x11
        /// </summary>
        internal const int PadeCodewordsEven = 0x11;

        /// <summary>
        /// 
        /// </summary>
        internal const int PositionStencilWidth = 7;

        /// <summary>
        /// 
        /// </summary>
        internal static bool[] PadeOdd = new[]
                                             {
                                                 true, true, true, false,
                                                 true, true, false, false
                                             };

        /// <summary>
        /// 
        /// </summary>
        internal static bool[] PadeEven = new[]
                                              {
                                                  false, false, false, true,
                                                  false, false, false, true
                                              };

        /// <summary>
        /// URL:http://en.wikipedia.org/wiki/Byte-order_mark
        /// </summary>
        /// <remarks></remarks>
        public static byte[] UTF8ByteOrderMark
        {
            get { return new byte[] {0xEF, 0xBB, 0xBF}; }
        }
    }
}