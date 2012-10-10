using System;
using QrCode.Web.Encoder.DataEncodation;

namespace QrCode.Web.Encoder.Versions
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal static class VersionControl
    {
        /// <summary>
        /// 
        /// </summary>
        private const int NUM_BITS_MODE_INDICATOR = 4;

        /// <summary>
        /// 
        /// </summary>
        private const string DEFAULT_ENCODING = QRCodeConstantVariable.DefaultEncoding;

        /// <summary>
        /// 
        /// </summary>
        private static readonly int[] VERSION_GROUP = new[] {9, 26, 40};


        /// <summary>
        /// Determine which version to use
        /// </summary>
        /// <param name="dataBitsLength">Number of bits for encoded content</param>
        /// <param name="mode">The mode.</param>
        /// <param name="level">The level.</param>
        /// <param name="encodingName">Encoding name for EightBitByte</param>
        /// <returns>VersionDetail and ECI</returns>
        /// <remarks></remarks>
        internal static VersionControlStruct InitialSetup(int dataBitsLength, Mode mode, ErrorCorrectionLevel level,
                                                          string encodingName)
        {
            int totalDataBits = dataBitsLength;

            bool containECI = false;

            var eciHeader = new BitList();


            //Check ECI header
            if (mode == Mode.EightBitByte)
            {
                if (encodingName != DEFAULT_ENCODING && encodingName != QRCodeConstantVariable.UTF8Encoding)
                {
                    var eciSet = new ECISet(ECISet.AppendOption.NameToValue);
                    int eciValue = eciSet.GetECIValueByName(encodingName);

                    totalDataBits += ECISet.NumOfECIHeaderBits(eciValue);
                    eciHeader = eciSet.GetECIHeader(encodingName);
                    containECI = true;
                }
            }
            //Determine which version group it belong to
            int searchGroup = DynamicSearchIndicator(totalDataBits, level, mode);

            int[] charCountIndicator = CharCountIndicatorTable.GetCharCountIndicatorSet(mode);

            totalDataBits += (NUM_BITS_MODE_INDICATOR + charCountIndicator[searchGroup]);

            int lowerSearchBoundary = searchGroup == 0 ? 1 : (VERSION_GROUP[searchGroup - 1] + 1);
            int higherSearchBoundary = VERSION_GROUP[searchGroup];

            //Binary search to find proper version
            int versionNum = BinarySearch(totalDataBits, level, lowerSearchBoundary, higherSearchBoundary);

            VersionControlStruct vcStruct = FillVCStruct(versionNum, level, encodingName);

            vcStruct.isContainECI = containECI;

            vcStruct.ECIHeader = eciHeader;

            return vcStruct;
        }

        /// <summary>
        /// Fills the VC struct.
        /// </summary>
        /// <param name="versionNum">The version num.</param>
        /// <param name="level">The level.</param>
        /// <param name="encodingName">Name of the encoding.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static VersionControlStruct FillVCStruct(int versionNum, ErrorCorrectionLevel level, string encodingName)
        {
            if (versionNum < 1 || versionNum > 40)
            {
                throw new InvalidOperationException(string.Format("Unexpected version number: {0}", versionNum));
            }

            var vcStruct = new VersionControlStruct();

            int version = versionNum;

            QRCodeVersion versionData = VersionTable.GetVersionByNum(versionNum);

            int numTotalBytes = versionData.TotalCodewords;

            ErrorCorrectionBlocks ecBlocks = versionData.GetECBlocksByLevel(level);
            int numDataBytes = numTotalBytes - ecBlocks.NumErrorCorrectionCodewards;
            int numECBlocks = ecBlocks.NumBlocks;

            var vcDetail = new VersionDetail(version, numTotalBytes, numDataBytes, numECBlocks);

            vcStruct.VersionDetail = vcDetail;
            return vcStruct;
        }


        /// <summary>
        /// Decide which version group it belong to
        /// </summary>
        /// <param name="numBits">number of bits for bitlist where it contain DataBits encode from input content and ECI header</param>
        /// <param name="level">Error correction level</param>
        /// <param name="mode">Mode</param>
        /// <returns>Version group index for VERSION_GROUP</returns>
        /// <remarks></remarks>
        private static int DynamicSearchIndicator(int numBits, ErrorCorrectionLevel level, Mode mode)
        {
            int[] charCountIndicator = CharCountIndicatorTable.GetCharCountIndicatorSet(mode);
            int totalBits = 0;
            int loopLength = VERSION_GROUP.Length;
            for (int i = 0; i < loopLength; i++)
            {
                totalBits = numBits + NUM_BITS_MODE_INDICATOR + charCountIndicator[i];

                QRCodeVersion version = VersionTable.GetVersionByNum(VERSION_GROUP[i]);
                int numECCodewords = version.GetECBlocksByLevel(level).NumErrorCorrectionCodewards;

                int dataCodewords = version.TotalCodewords - numECCodewords;

                if (totalBits <= dataCodewords*8)
                {
                    return i;
                }
            }

            throw new InputOutOfBoundaryException(string.Format("QRCode do not have enough space for {0} bits",
                                                                (numBits + NUM_BITS_MODE_INDICATOR +
                                                                 charCountIndicator[2])));
        }

        /// <summary>
        /// Use number of data bits(header + eci header + data bits from EncoderBase) to search for proper version to use
        /// between min and max boundary.
        /// Boundary define by DynamicSearchIndicator method.
        /// </summary>
        /// <param name="numDataBits">The num data bits.</param>
        /// <param name="level">The level.</param>
        /// <param name="lowerVersionNum">The lower version num.</param>
        /// <param name="higherVersionNum">The higher version num.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static int BinarySearch(int numDataBits, ErrorCorrectionLevel level, int lowerVersionNum,
                                        int higherVersionNum)
        {
            int middleVersionNumber;

            while (lowerVersionNum <= higherVersionNum)
            {
                middleVersionNumber = (lowerVersionNum + higherVersionNum)/2;
                QRCodeVersion version = VersionTable.GetVersionByNum(middleVersionNumber);
                int numECCodewords = version.GetECBlocksByLevel(level).NumErrorCorrectionCodewards;
                int dataCodewords = version.TotalCodewords - numECCodewords;

                if (dataCodewords << 3 == numDataBits)
                    return middleVersionNumber;

                if (dataCodewords << 3 > numDataBits)
                {
                    higherVersionNum = middleVersionNumber - 1;
                }
                else
                {
                    lowerVersionNum = middleVersionNumber + 1;
                }
            }
            return lowerVersionNum;
        }
    }
}