using System;

namespace QrCode.Web.Encoder.EncodingRegion
{
    /// <summary>
    /// Embed version information for version larger or equal to 7.
    /// </summary>
    /// <remarks>ISO/IEC 18004:2000 Chapter 8.10 Page 54</remarks>
    internal static class VersionInformation
    {
        /// <summary>
        /// 
        /// </summary>
        private const int s_VIRectangleHeight = 3;

        /// <summary>
        /// 
        /// </summary>
        private const int s_VIRectangleWidth = 6;

        /// <summary>
        /// 
        /// </summary>
        private const int s_LengthDataBits = 6;

        /// <summary>
        /// 
        /// </summary>
        private const int s_LengthECBits = 12;

        /// <summary>
        /// 
        /// </summary>
        private const int s_VersionBCHPoly = 0x1f25;

        /// <summary>
        /// Embed version information to Matrix
        /// Only for version greater or equal to 7
        /// </summary>
        /// <param name="tsMatrix">Matrix</param>
        /// <param name="version">version number</param>
        /// <remarks></remarks>
        internal static void EmbedVersionInformation(this TriStateMatrix tsMatrix, int version)
        {
            if (version < 7)
                return;
            BitList versionInfo = VersionInfoBitList(version);

            int matrixWidth = tsMatrix.Width;
            //1 cell between version info and position stencil
            int shiftLength = QRCodeConstantVariable.PositionStencilWidth + s_VIRectangleHeight + 1;
            //Reverse order input
            int viIndex = s_LengthDataBits + s_LengthECBits - 1;

            for (int viWidth = 0; viWidth < s_VIRectangleWidth; viWidth++)
            {
                for (int viHeight = 0; viHeight < s_VIRectangleHeight; viHeight++)
                {
                    bool bit = versionInfo[viIndex];
                    viIndex--;
                    //Bottom left
                    tsMatrix[viWidth, (matrixWidth - shiftLength + viHeight), MatrixStatus.NoMask] = bit;
                    //Top right
                    tsMatrix[(matrixWidth - shiftLength + viHeight), viWidth, MatrixStatus.NoMask] = bit;
                }
            }
        }

        /// <summary>
        /// Versions the info bit list.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static BitList VersionInfoBitList(int version)
        {
            var result = new BitList();
            result.Add(version, s_LengthDataBits);
            result.Add(BCHCalculator.CalculateBCH(version, s_VersionBCHPoly), s_LengthECBits);

            if (result.Count != (s_LengthECBits + s_LengthDataBits))
                throw new Exception("Version Info creation error. Result is not 18 bits");
            return result;
        }
    }
}