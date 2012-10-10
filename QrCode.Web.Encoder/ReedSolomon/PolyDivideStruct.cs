namespace QrCode.Web.Encoder.ReedSolomon
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal struct PolyDivideStruct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolyDivideStruct"/> struct.
        /// </summary>
        /// <param name="quotient">The quotient.</param>
        /// <param name="remainder">The remainder.</param>
        /// <remarks></remarks>
        internal PolyDivideStruct(Polynomial quotient, Polynomial remainder)
            : this()
        {
            Quotient = quotient;
            Remainder = remainder;
        }

        /// <summary>
        /// Gets or sets the quotient.
        /// </summary>
        /// <value>The quotient.</value>
        /// <remarks></remarks>
        internal Polynomial Quotient { get; private set; }

        /// <summary>
        /// Gets or sets the remainder.
        /// </summary>
        /// <value>The remainder.</value>
        /// <remarks></remarks>
        internal Polynomial Remainder { get; private set; }
    }
}