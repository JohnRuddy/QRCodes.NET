using System;

namespace QrCode.Web.Encoder.common
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal static class BitListExtensions
    {
        /// <summary>
        /// Toes the byte array.
        /// </summary>
        /// <param name="bitList">The bit list.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static byte[] ToByteArray(this BitList bitList)
        {
            int bitLength = bitList.Count;
            if ((bitLength & 0x7) != 0)
                throw new ArgumentException("bitList count % 8 is not equal to zero");

            int numByte = bitLength >> 3;

            var result = new byte[numByte];

            for (int bitIndex = 0; bitIndex < bitLength; bitIndex++)
            {
                int numBitsInLastByte = bitIndex & 0x7;

                if (numBitsInLastByte == 0)
                    result[bitIndex >> 3] = 0;
                result[bitIndex >> 3] |= (byte) (ToBit(bitList[bitIndex]) << InverseShiftValue(numBitsInLastByte));
            }

            return result;
        }

        /// <summary>
        /// Toes the bit list.
        /// </summary>
        /// <param name="bArray">The b array.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static BitList ToBitList(byte[] bArray)
        {
            int bLength = bArray.Length;
            var result = new BitList();
            for (int bIndex = 0; bIndex < bLength; bIndex++)
            {
                result.Add(bArray[bIndex], 8);
            }
            return result;
        }

        /// <summary>
        /// Toes the bit.
        /// </summary>
        /// <param name="bit">if set to <c>true</c> [bit].</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static int ToBit(bool bit)
        {
            switch (bit)
            {
                case true:
                    return 1;
                case false:
                    return 0;
                default:
                    throw new ArgumentException("Invalide bit value");
            }
        }

        /// <summary>
        /// Inverses the shift value.
        /// </summary>
        /// <param name="numBitsInLastByte">The num bits in last byte.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static int InverseShiftValue(int numBitsInLastByte)
        {
            return 7 - numBitsInLastByte;
        }
    }
}