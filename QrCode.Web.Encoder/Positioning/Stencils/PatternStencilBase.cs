using System;

namespace QrCode.Web.Encoder.Positioning.Stencils
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal abstract class PatternStencilBase : BitMatrix
    {
        /// <summary>
        /// 
        /// </summary>
        protected const bool o = false;

        /// <summary>
        /// 
        /// </summary>
        protected const bool x = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternStencilBase"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <remarks></remarks>
        internal PatternStencilBase(int version)
        {
            Version = version;
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <remarks></remarks>
        public int Version { get; private set; }

        /// <summary>
        /// Gets the stencil.
        /// </summary>
        /// <remarks></remarks>
        public abstract bool[,] Stencil { get; }

        /// <summary>
        /// Gets or sets the <see cref="System.Boolean"/> with the specified i.
        /// </summary>
        /// <remarks></remarks>
        public override bool this[int i, int j]
        {
            get { return Stencil[i, j]; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <remarks></remarks>
        public override int Width
        {
            get { return Stencil.GetLength(0); }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <remarks></remarks>
        public override int Height
        {
            get { return Stencil.GetLength(1); }
        }

        /// <summary>
        /// Gets the internal array.
        /// </summary>
        /// <remarks></remarks>
        public override bool[,] InternalArray
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Applies to.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <remarks></remarks>
        public abstract void ApplyTo(TriStateMatrix matrix);
    }
}