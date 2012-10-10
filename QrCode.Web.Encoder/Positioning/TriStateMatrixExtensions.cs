using System.Collections.Generic;

namespace QrCode.Web.Encoder.Positioning
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal static class TriStateMatrixExtensions
    {
        /// <summary>
        /// Embeds the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="stencil">The stencil.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static TriStateMatrix Embed(this TriStateMatrix matrix, BitMatrix stencil, MatrixPoint location)
        {
            stencil.CopyTo(matrix, location, MatrixStatus.NoMask);
            return matrix;
        }

        /// <summary>
        /// Embeds the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="stencil">The stencil.</param>
        /// <param name="locations">The locations.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static TriStateMatrix Embed(this TriStateMatrix matrix, BitMatrix stencil,
                                             IEnumerable<MatrixPoint> locations)
        {
            foreach (MatrixPoint location in locations)
            {
                Embed(matrix, stencil, location);
            }
            return matrix;
        }
    }
}