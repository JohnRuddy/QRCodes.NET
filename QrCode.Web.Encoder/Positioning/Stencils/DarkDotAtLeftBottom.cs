using System;

namespace QrCode.Web.Encoder.Positioning.Stencils
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal class DarkDotAtLeftBottom : PatternStencilBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DarkDotAtLeftBottom"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <remarks></remarks>
        public DarkDotAtLeftBottom(int version) : base(version)
        {
        }

        /// <summary>
        /// Gets the stencil.
        /// </summary>
        /// <remarks></remarks>
        public override bool[,] Stencil
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Applies to.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <remarks></remarks>
        public override void ApplyTo(TriStateMatrix matrix)
        {
            matrix[8, matrix.Width - 8, MatrixStatus.NoMask] = true;
        }
    }
}