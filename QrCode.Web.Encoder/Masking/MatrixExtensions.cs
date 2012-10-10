using System;
using QrCode.Web.Encoder.EncodingRegion;

namespace QrCode.Web.Encoder.Masking
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public static class MatrixExtensions
    {
        /// <summary>
        /// Xors the specified first.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <param name="errorlevel">The errorlevel.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static TriStateMatrix Xor(this TriStateMatrix first, Pattern second, ErrorCorrectionLevel errorlevel)
        {
            TriStateMatrix result = XorMatrix(first, second);
            result.EmbedFormatInformation(errorlevel, second);
            return result;
        }


        /// <summary>
        /// Xors the matrix.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static TriStateMatrix XorMatrix(TriStateMatrix first, BitMatrix second)
        {
            int width = first.Width;
            var maskedMatrix = new TriStateMatrix(width);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    MatrixStatus states = first.MStatus(x, y);
                    switch (states)
                    {
                        case MatrixStatus.NoMask:
                            maskedMatrix[x, y, MatrixStatus.NoMask] = first[x, y];
                            break;
                        case MatrixStatus.Data:
                            maskedMatrix[x, y, MatrixStatus.Data] = first[x, y] ^ second[x, y];
                            break;
                        default:
                            throw new ArgumentException("TristateMatrix has None value cell.", "first");
                    }
                }
            }

            return maskedMatrix;
        }

        /// <summary>
        /// Applies the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="errorlevel">The errorlevel.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static TriStateMatrix Apply(this TriStateMatrix matrix, Pattern pattern, ErrorCorrectionLevel errorlevel)
        {
            return matrix.Xor(pattern, errorlevel);
        }

        /// <summary>
        /// Applies the specified pattern.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="matrix">The matrix.</param>
        /// <param name="errorlevel">The errorlevel.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static TriStateMatrix Apply(this Pattern pattern, TriStateMatrix matrix, ErrorCorrectionLevel errorlevel)
        {
            return matrix.Xor(pattern, errorlevel);
        }
    }
}