using System.Linq;

namespace QrCode.Web.Encoder.Masking.Scoring
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal static class MatrixScoreCalculator
    {
        /// <summary>
        /// Gets the lowest penalty matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="errorlevel">The errorlevel.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static BitMatrix GetLowestPenaltyMatrix(this TriStateMatrix matrix, ErrorCorrectionLevel errorlevel)
        {
            var patternFactory = new PatternFactory();
            int score = int.MaxValue;
            int tempScore;
            var result = new TriStateMatrix(matrix.Width);
            TriStateMatrix triMatrix;
            foreach (Pattern pattern in patternFactory.AllPatterns())
            {
                triMatrix = matrix.Apply(pattern, errorlevel);
                tempScore = triMatrix.PenaltyScore();
                if (tempScore < score)
                {
                    score = tempScore;
                    result = triMatrix;
                }
            }

            return result;
        }


        /// <summary>
        /// Penalties the score.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static int PenaltyScore(this BitMatrix matrix)
        {
            var penaltyFactory = new PenaltyFactory();
            return
                penaltyFactory
                    .AllRules()
                    .Sum(penalty => penalty.PenaltyCalculate(matrix));
        }
    }
}