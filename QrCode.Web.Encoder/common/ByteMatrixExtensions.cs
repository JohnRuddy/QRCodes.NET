namespace QrCode.Web.Encoder.common
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal static class ByteMatrixExtensions
    {
        /// <summary>
        /// Toes the bit matrix.
        /// </summary>
        /// <param name="byteMatrix">The byte matrix.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static TriStateMatrix ToBitMatrix(this ByteMatrix byteMatrix)
        {
            var matrix = new TriStateMatrix(byteMatrix.Width);
            for (int i = 0; i < byteMatrix.Width; i++)
            {
                for (int j = 0; j < byteMatrix.Height; j++)
                {
                    if (byteMatrix[j, i] != -1)
                    {
                        matrix[i, j, MatrixStatus.NoMask] = byteMatrix[j, i] != 0;
                    }
                }
            }
            return matrix;
        }

        /// <summary>
        /// Toes the pattern bit matrix.
        /// </summary>
        /// <param name="byteMatrix">The byte matrix.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static TriStateMatrix ToPatternBitMatrix(this ByteMatrix byteMatrix)
        {
            var matrix = new TriStateMatrix(byteMatrix.Width);
            for (int i = 0; i < byteMatrix.Width; i++)
            {
                for (int j = 0; j < byteMatrix.Height; j++)
                {
                    if (byteMatrix[j, i] != -1)
                    {
                        matrix[i, j, MatrixStatus.Data] = byteMatrix[j, i] != 0;
                    }
                }
            }
            return matrix;
        }
    }
}