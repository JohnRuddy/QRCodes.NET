using System;
using System.Collections.Generic;

namespace QrCode.Web.Encoder.Masking.Scoring
{
    /// <summary>
    /// Description of PenaltyFactory.
    /// </summary>
    /// <remarks></remarks>
    internal class PenaltyFactory
    {
        /// <summary>
        /// Creates the by rule.
        /// </summary>
        /// <param name="penaltyRule">The penalty rule.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal Penalty CreateByRule(PenaltyRules penaltyRule)
        {
            switch (penaltyRule)
            {
                case PenaltyRules.Rule01:
                    return new Penalty1();
                case PenaltyRules.Rule02:
                    return new Penalty2();
                case PenaltyRules.Rule03:
                    return new Penalty3();
                case PenaltyRules.Rule04:
                    return new Penalty4();
                default:
                    throw new ArgumentException(string.Format("Unsupport penalty rule : {0}", penaltyRule),
                                                "penaltyRule");
            }
        }


        /// <summary>
        /// Alls the rules.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        internal IEnumerable<Penalty> AllRules()
        {
            foreach (PenaltyRules penaltyRule in Enum.GetValues(typeof (PenaltyRules)))
            {
                yield return CreateByRule(penaltyRule);
            }
        }
    }
}