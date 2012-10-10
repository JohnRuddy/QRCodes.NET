using System;

namespace QrCode.Web.Encoder.Masking
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal class Pattern5 : Pattern
    {
        /// <summary>
        /// Gets or sets the <see cref="System.Boolean"/> with the specified i.
        /// </summary>
        /// <remarks></remarks>
        public override bool this[int i, int j]
        {
            get { return (i*j)%2 + (i*j)%3 == 0; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets the type of the mask pattern.
        /// </summary>
        /// <remarks></remarks>
        public override MaskPatternType MaskPatternType
        {
            get { return MaskPatternType.Type5; }
        }
    }
}