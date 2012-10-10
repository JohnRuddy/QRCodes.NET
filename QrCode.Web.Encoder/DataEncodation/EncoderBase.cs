namespace QrCode.Web.Encoder.DataEncodation
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public abstract class EncoderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        internal EncoderBase()
        {
        }

        /// <summary>
        /// Gets the mode.
        /// </summary>
        /// <remarks></remarks>
        internal abstract Mode Mode { get; }

        /// <summary>
        /// Gets the length of the data.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected virtual int GetDataLength(string content)
        {
            return content.Length;
        }

        /// <summary>
        /// Returns the bit representation of input data.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal abstract BitList GetDataBits(string content);


        /// <summary>
        /// Returns bit representation of <see cref="Mode"/> value.
        /// </summary>
        /// <returns></returns>
        /// <remarks>See Chapter 8.4 Data encodation, Table 2 — Mode indicators</remarks>
        internal BitList GetModeIndicator()
        {
            var modeIndicatorBits = new BitList();
            modeIndicatorBits.Add((int) Mode, 4);
            return modeIndicatorBits;
        }

        /// <summary>
        /// Gets the char count indicator.
        /// </summary>
        /// <param name="characterCount">The character count.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal BitList GetCharCountIndicator(int characterCount, int version)
        {
            var characterCountBits = new BitList();
            int bitCount = GetBitCountInCharCountIndicator(version);
            characterCountBits.Add(characterCount, bitCount);
            return characterCountBits;
        }

        /// <summary>
        /// Defines the length of the Character Count Indicator,
        /// which varies according to themode and the symbol version in use
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>Number of bits in Character Count Indicator.</returns>
        /// <remarks>See Chapter 8.4 Data encodation, Table 3 — Number of bits in Character Count Indicator.</remarks>
        protected abstract int GetBitCountInCharCountIndicator(int version);
    }
}