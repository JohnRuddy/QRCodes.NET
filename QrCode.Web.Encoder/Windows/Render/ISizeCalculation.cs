//using System.Collections.Generic;

namespace QrCode.Web.Encoder.Windows.Render
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public interface ISizeCalculation
    {
        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <param name="matrixWidth">Width of the matrix.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        DrawingSize GetSize(int matrixWidth);
    }
}