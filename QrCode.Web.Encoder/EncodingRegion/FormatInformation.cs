using System;
using QrCode.Web.Encoder.Masking;

namespace QrCode.Web.Encoder.EncodingRegion
{
    /// <summary>
    /// 6.9 Format information
    /// The Format Information is a 15 bit sequence containing 5 data bits, with 10 error correction bits calculated using the (15, 5) BCH code.
    /// </summary>
    /// <remarks>ISO/IEC 18004:2000 Chapter 8.9 Page 53</remarks>
    internal static class FormatInformation
    {
        /// <summary>
        /// From Appendix C in JISX0510:2004 (p.65).
        /// </summary>
        private const int s_FormatInfoPoly = 0x537;

        /// <summary>
        /// From Appendix C in JISX0510:2004 (p.65).
        /// </summary>
        private const int s_FormatInfoMaskPattern = 0x5412;

        /// <summary>
        /// Embed format information to tristatematrix.
        /// Process combination of create info bits, BCH error correction bits calculation, embed towards matrix.
        /// </summary>
        /// <param name="triMatrix">The tri matrix.</param>
        /// <param name="errorlevel">The errorlevel.</param>
        /// <param name="pattern">The pattern.</param>
        /// <remarks>ISO/IEC 18004:2000 Chapter 8.9 Page 53</remarks>
        internal static void EmbedFormatInformation(this TriStateMatrix triMatrix, ErrorCorrectionLevel errorlevel,
                                                    Pattern pattern)
        {
            BitList formatInfo = GetFormatInfoBits(errorlevel, pattern);
            int width = triMatrix.Width;
            for (int index = 0; index < 15; index++)
            {
                MatrixPoint point = PointForInfo1(index);
                bool bit = formatInfo[index];
                triMatrix[point.X, point.Y, MatrixStatus.NoMask] = bit;

                if (index < 7)
                {
                    triMatrix[8, width - 1 - index, MatrixStatus.NoMask] = bit;
                }
                else
                {
                    triMatrix[width - 8 + (index - 7), 8, MatrixStatus.NoMask] = bit;
                }
            }
        }

        /// <summary>
        /// Points for info1.
        /// </summary>
        /// <param name="bitsIndex">Index of the bits.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static MatrixPoint PointForInfo1(int bitsIndex)
        {
            if (bitsIndex <= 7)
            {
                return bitsIndex >= 6
                           ? new MatrixPoint(bitsIndex + 1, 8)
                           : new MatrixPoint(bitsIndex, 8);
            }
            else
            {
                return bitsIndex == 8
                           ? new MatrixPoint(8, 8 - (bitsIndex - 7))
                           : new MatrixPoint(8, 8 - (bitsIndex - 7) - 1);
            }
        }

        /// <summary>
        /// Gets the format info bits.
        /// </summary>
        /// <param name="errorlevel">The errorlevel.</param>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static BitList GetFormatInfoBits(ErrorCorrectionLevel errorlevel, Pattern pattern)
        {
            var formatInfo = (int) pattern.MaskPatternType;
            //Pattern bits length = 3
            formatInfo |= GetErrorCorrectionIndicatorBits(errorlevel) << 3;

            int bchCode = BCHCalculator.CalculateBCH(formatInfo, s_FormatInfoPoly);
            //bchCode length = 10
            formatInfo = (formatInfo << 10) | bchCode;

            //xor maskPattern
            formatInfo ^= s_FormatInfoMaskPattern;

            var resultBits = new BitList();
            resultBits.Add(formatInfo, 15);

            if (resultBits.Count != 15)
                throw new Exception("FormatInfoBits length is not 15");
            else
                return resultBits;
        }

        //According Table 25 — Error correction level indicators
        //Using this bits as enum values would destroy thir order which currently correspond to error correction strength.
        /// <summary>
        /// Gets the error correction indicator bits.
        /// </summary>
        /// <param name="errorLevel">The error level.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static int GetErrorCorrectionIndicatorBits(ErrorCorrectionLevel errorLevel)
        {
            //L 01
            //M 00
            //Q 11
            //H 10
            switch (errorLevel)
            {
                case ErrorCorrectionLevel.H:
                    return 0x02;

                case ErrorCorrectionLevel.L:
                    return 0x01;

                case ErrorCorrectionLevel.M:
                    return 0x00;

                case ErrorCorrectionLevel.Q:
                    return 0x03;
                default:
                    throw new ArgumentException(string.Format("Unsupported error correction level [{0}]", errorLevel),
                                                "errorLevel");
            }
        }
    }
}