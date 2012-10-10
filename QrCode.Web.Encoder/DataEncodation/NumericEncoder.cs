using System;

namespace QrCode.Web.Encoder.DataEncodation
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>ISO/IEC 18004:2000 Chapter 8.4.2 Page 19</remarks>
    internal class NumericEncoder : EncoderBase
    {
        /// <summary>
        /// Gets the mode.
        /// </summary>
        /// <remarks></remarks>
        internal override Mode Mode
        {
            get { return Mode.Numeric; }
        }

        /// <summary>
        /// Returns the bit representation of input data.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal override BitList GetDataBits(string content)
        {
            var dataBits = new BitList();
            int contentLength = content.Length;
            for (int i = 0; i < contentLength; i += 3)
            {
                int groupLength = Math.Min(3, contentLength - i);
                int value = GetDigitGroupValue(content, i, groupLength);
                int bitCount = GetBitCountByGroupLength(groupLength);
                dataBits.Add(value, bitCount);
            }

            return dataBits;
        }


        /// <summary>
        /// Defines the length of the Character Count Indicator,
        /// which varies according to themode and the symbol version in use
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>Number of bits in Character Count Indicator.</returns>
        /// <remarks></remarks>
        protected override int GetBitCountInCharCountIndicator(int version)
        {
            return CharCountIndicatorTable.GetBitCountInCharCountIndicator(Mode.Numeric, version);
        }

        /// <summary>
        /// Gets the digit group value.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private int GetDigitGroupValue(string content, int startIndex, int length)
        {
            int value = 0;
            int iThPowerOf10 = 1;
            for (int i = 0; i < length; i++)
            {
                int positionFromEnd = startIndex + length - i - 1;
                int digit = content[positionFromEnd] - '0';
                value += digit*iThPowerOf10;
                iThPowerOf10 *= 10;
            }
            return value;
        }

        /// <summary>
        /// Tries the get digit group value.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool TryGetDigitGroupValue(string content, int startIndex, int length, out int value)
        {
            value = 0;
            int iThPowerOf10 = 1;
            for (int i = 0; i < length; i++)
            {
                int positionFromEnd = startIndex + length - i - 1;
                int digit = content[positionFromEnd] - '0';
                //If not numeric. 
                if (digit < 0 || digit > 9)
                    return false;
                value += digit*iThPowerOf10;
                iThPowerOf10 *= 10;
            }
            return true;
        }

        /// <summary>
        /// Gets the length of the bit count by group.
        /// </summary>
        /// <param name="groupLength">Length of the group.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected int GetBitCountByGroupLength(int groupLength)
        {
            switch (groupLength)
            {
                case 0:
                    return 0;
                case 1:
                    return 4;
                case 2:
                    return 7;
                case 3:
                    return 10;
                default:
                    throw new InvalidOperationException("Unexpected group length:" + groupLength.ToString());
            }
        }
    }
}