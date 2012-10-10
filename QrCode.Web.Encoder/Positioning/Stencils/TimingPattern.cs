using System;

namespace QrCode.Web.Encoder.Positioning.Stencils
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal class TimingPattern : PatternStencilBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatternStencilBase"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <remarks></remarks>
        public TimingPattern(int version)
            : base(version)
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
            // -8 is for skipping position detection patterns (size 7), and two horizontal/vertical
            // separation patterns (size 1). Thus, 8 = 7 + 1.
            for (int i = 8; i < matrix.Width - 8; ++i)
            {
                bool value = (sbyte) ((i + 1)%2) == 1;
                // Horizontal line.

                if (matrix.MStatus(6, i) == MatrixStatus.None)
                {
                    matrix[6, i, MatrixStatus.NoMask] = value;
                }

                // Vertical line.
                if (matrix.MStatus(i, 6) == MatrixStatus.None)
                {
                    matrix[i, 6, MatrixStatus.NoMask] = value;
                }
            }
        }
    }
}