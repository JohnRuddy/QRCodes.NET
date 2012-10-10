using System;

namespace QrCode.Web.Encoder.Terminate
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal static class Terminator
    {
        /// <summary>
        /// 
        /// </summary>
        private const int NumBitsForByte = 8;

        /// <summary>
        /// This method will create BitList that contain
        /// terminator, padding and pad codewords for given datacodewords.
        /// Use it to full fill the data codewords capacity. Thus avoid massive empty bits.
        /// </summary>
        /// <param name="baseList">Method will add terminator bits at end of baseList</param>
        /// <param name="dataCount">Num of bits for datacodewords without terminator</param>
        /// <param name="numTotalDataCodewords">Total number of datacodewords for specific version.
        /// Receive it under Version/VersionTable</param>
        /// <returns>Bitlist that contain Terminator, padding and padcodewords</returns>
        /// <remarks>ISO/IEC 18004:2006 P. 32 33.
        /// Terminator / Bit stream to codeword conversion</remarks>
        internal static void TerminateBites(this BitList baseList, int dataCount, int numTotalDataCodewords)
        {
            int numTotalDataBits = numTotalDataCodewords << 3;
            int numDataBits = dataCount;

            int numFillerBits = numTotalDataBits - numDataBits;
            int numBitsNeedForLastByte = numFillerBits & 0x7;
            int numFillerBytes = numFillerBits >> 3;

            //BitList result = new BitList();
            if (numBitsNeedForLastByte >= QRCodeConstantVariable.TerminatorLength)
            {
                baseList.TerminatorPadding(numBitsNeedForLastByte);
                baseList.PadeCodewords(numFillerBytes);
            }
            else if (numFillerBytes == 0)
            {
                baseList.TerminatorPadding(numBitsNeedForLastByte);
            }
            else if (numFillerBytes > 0)
            {
                baseList.TerminatorPadding(numBitsNeedForLastByte + NumBitsForByte);
                baseList.PadeCodewords(numFillerBytes - 1);
            }

            if (baseList.Count != numTotalDataBits)
                throw new ArgumentException(
                    string.Format("Generate terminator and Padding fail. Num of bits need: {0}, Actually length: {1}",
                                  numFillerBytes, baseList.Count - numDataBits));
        }


        /// <summary>
        /// Pades the codewords.
        /// </summary>
        /// <param name="mainList">The main list.</param>
        /// <param name="numOfPadeCodewords">The num of pade codewords.</param>
        /// <remarks></remarks>
        private static void PadeCodewords(this BitList mainList, int numOfPadeCodewords)
        {
            if (numOfPadeCodewords < 0)
                throw new ArgumentException("Num of pade codewords less than Zero");
            for (int numOfP = 1; numOfP <= numOfPadeCodewords; numOfP++)
            {
                if (numOfP%2 == 1)
                    mainList.Add(QRCodeConstantVariable.PadeCodewordsOdd, NumBitsForByte);
                else
                    mainList.Add(QRCodeConstantVariable.PadeCodewordsEven, NumBitsForByte);
            }
        }

        /// <summary>
        /// Terminators the padding.
        /// </summary>
        /// <param name="mainList">The main list.</param>
        /// <param name="numBits">The num bits.</param>
        /// <remarks></remarks>
        private static void TerminatorPadding(this BitList mainList, int numBits)
        {
            mainList.Add(QRCodeConstantVariable.TerminatorNPaddingBit, numBits);
        }
    }
}