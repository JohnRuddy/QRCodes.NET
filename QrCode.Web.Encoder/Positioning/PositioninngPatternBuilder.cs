using QrCode.Web.Encoder.Positioning.Stencils;

namespace QrCode.Web.Encoder.Positioning
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal static class PositioninngPatternBuilder
    {
        /// <summary>
        /// Embeds the basic patterns.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="matrix">The matrix.</param>
        /// <remarks></remarks>
        internal static void EmbedBasicPatterns(int version, TriStateMatrix matrix)
        {
            new PositionDetectionPattern(version).ApplyTo(matrix);
            new DarkDotAtLeftBottom(version).ApplyTo(matrix);
            new AlignmentPattern(version).ApplyTo(matrix);
            new TimingPattern(version).ApplyTo(matrix);
        }
    }
}