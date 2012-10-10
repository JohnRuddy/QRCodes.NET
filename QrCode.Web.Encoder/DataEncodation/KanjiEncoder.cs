using System;
using System.Text;

namespace QrCode.Web.Encoder.DataEncodation
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>ISO/IEC 18004:2000 Chapter 8.4.5 Page 23</remarks>
    internal class KanjiEncoder : EncoderBase
    {
        /// <summary>
        /// Bitcount according to ISO/IEC 18004:2000 Kanji mode Page 25
        /// </summary>
        private const int KANJI_BITCOUNT = 13;

        /// <summary>
        /// 
        /// </summary>
        private const int FST_GROUP_LOWER_BOUNDARY = 0x8140;

        /// <summary>
        /// 
        /// </summary>
        private const int FST_GROUP_UPPER_BOUNDARY = 0x9FFC;

        /// <summary>
        /// 
        /// </summary>
        private const int FST_GROUP_SUBTRACT_VALUE = 0x8140;

        /// <summary>
        /// 
        /// </summary>
        private const int SEC_GROUP_LOWER_BOUNDARY = 0xE040;

        /// <summary>
        /// 
        /// </summary>
        private const int SEC_GROUP_UPPER_BOUNDARY = 0xEBBF;

        /// <summary>
        /// 
        /// </summary>
        private const int SEC_GROUP_SUBTRACT_VALUE = 0xC140;


        /// <summary>
        /// Multiply value for Most significant byte.
        /// Chapter 8.4.5 P.24
        /// </summary>
        private const int MULTIPLY_FOR_msb = 0xC0;

        /// <summary>
        /// Gets the mode.
        /// </summary>
        /// <remarks></remarks>
        internal override Mode Mode
        {
            get { return Mode.Kanji; }
        }

        /// <summary>
        /// Returns the bit representation of input data.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal override BitList GetDataBits(string content)
        {
            byte[] contentBytes = EncodeContent(content);
            int contentLength = base.GetDataLength(content);

            return GetDataBitsByByteArray(contentBytes, contentLength);
        }

        /// <summary>
        /// Gets the data bits by byte array.
        /// </summary>
        /// <param name="encodeContent">Content of the encode.</param>
        /// <param name="contentLength">Length of the content.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal BitList GetDataBitsByByteArray(byte[] encodeContent, int contentLength)
        {
            var dataBits = new BitList();

            int bytesLength = encodeContent.Length;

            if (bytesLength == contentLength*2)
            {
                for (int i = 0; i < bytesLength; i += 2)
                {
                    int encoded = ConvertShiftJIS(encodeContent[i], encodeContent[i + 1]);
                    dataBits.Add(encoded, KANJI_BITCOUNT);
                }
            }
            else
                throw new ArgumentOutOfRangeException("Each char must be two byte length");

            return dataBits;
        }

        /// <summary>
        /// Encodes the content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected byte[] EncodeContent(string content)
        {
            byte[] contentBytes;
            try
            {
                contentBytes = Encoding.GetEncoding("shift_jis").GetBytes(content);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            return contentBytes;
        }

        /// <summary>
        /// Converts the shift JIS.
        /// </summary>
        /// <param name="FirstByte">The first byte.</param>
        /// <param name="SecondByte">The second byte.</param>
        /// <returns></returns>
        /// <remarks>See Chapter 8.4.5 P.24 Kanji Mode</remarks>
        private int ConvertShiftJIS(byte FirstByte, byte SecondByte)
        {
            int ShiftJISValue = (FirstByte << 8) + (SecondByte & 0xff);
            int Subtracted = -1;
            if (ShiftJISValue >= FST_GROUP_LOWER_BOUNDARY && ShiftJISValue <= FST_GROUP_UPPER_BOUNDARY)
            {
                Subtracted = ShiftJISValue - FST_GROUP_SUBTRACT_VALUE;
            }
            else if (ShiftJISValue >= SEC_GROUP_LOWER_BOUNDARY && ShiftJISValue <= SEC_GROUP_UPPER_BOUNDARY)
            {
                Subtracted = ShiftJISValue - SEC_GROUP_SUBTRACT_VALUE;
            }
            else
                throw new ArgumentOutOfRangeException("Char is not inside acceptable range.");

            return ((Subtracted >> 8)*MULTIPLY_FOR_msb) + (Subtracted & 0xFF);
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
            return CharCountIndicatorTable.GetBitCountInCharCountIndicator(Mode.Kanji, version);
        }
    }
}