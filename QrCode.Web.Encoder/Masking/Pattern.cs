using System;

namespace QrCode.Web.Encoder.Masking
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public abstract class Pattern : BitMatrix
    {
        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <remarks></remarks>
        public override int Width
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <remarks></remarks>
        public override int Height
        {
            get { throw new NotSupportedException(); }
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
        /// Gets the type of the mask pattern.
        /// </summary>
        /// <remarks></remarks>
        public abstract MaskPatternType MaskPatternType { get; }
    }
}