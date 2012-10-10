using System;
using System.Collections.Generic;

namespace QrCode.Web.Encoder.Masking
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal class PatternFactory
    {
        /// <summary>
        /// Creates the type of the by.
        /// </summary>
        /// <param name="maskPatternType">Type of the mask pattern.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal Pattern CreateByType(MaskPatternType maskPatternType)
        {
            switch (maskPatternType)
            {
                case MaskPatternType.Type0:
                    return new Pattern0();

                case MaskPatternType.Type1:
                    return new Pattern1();

                case MaskPatternType.Type2:
                    return new Pattern2();

                case MaskPatternType.Type3:
                    return new Pattern3();

                case MaskPatternType.Type4:
                    return new Pattern4();

                case MaskPatternType.Type5:
                    return new Pattern5();

                case MaskPatternType.Type6:
                    return new Pattern6();

                case MaskPatternType.Type7:
                    return new Pattern7();
            }

            throw new ArgumentException(string.Format("Usupported pattern type {0}", maskPatternType), "maskPatternType");
        }

        /// <summary>
        /// Alls the patterns.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        internal IEnumerable<Pattern> AllPatterns()
        {
            foreach (MaskPatternType patternType in Enum.GetValues(typeof (MaskPatternType)))
            {
                yield return CreateByType(patternType);
            }
        }
    }
}