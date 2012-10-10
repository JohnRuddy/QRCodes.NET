namespace QrCode.Web.Encoder.Masking.Scoring
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public abstract class Penalty
    {
        /// <summary>
        /// Penalties the calculate.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal abstract int PenaltyCalculate(BitMatrix matrix);
    }
}