using QrCode.Web.Encoder.DataEncodation;
using QrCode.Web.Encoder.EncodingRegion;
using QrCode.Web.Encoder.ErrorCorrection;
using QrCode.Web.Encoder.Masking;
using QrCode.Web.Encoder.Masking.Scoring;
using QrCode.Web.Encoder.Positioning;

namespace QrCode.Web.Encoder
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal static class QRCodeEncode
    {
        /// <summary>
        /// Encodes the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="errorLevel">The error level.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static BitMatrix Encode(string content, ErrorCorrectionLevel errorLevel)
        {
            EncodationStruct encodeStruct = DataEncode.Encode(content, errorLevel);

            BitList codewords = ECGenerator.FillECCodewords(encodeStruct.DataCodewords, encodeStruct.VersionDetail);

            var triMatrix = new TriStateMatrix(encodeStruct.VersionDetail.MatrixWidth);
            PositioninngPatternBuilder.EmbedBasicPatterns(encodeStruct.VersionDetail.Version, triMatrix);

            triMatrix.EmbedVersionInformation(encodeStruct.VersionDetail.Version);
            triMatrix.EmbedFormatInformation(errorLevel, new Pattern0());
            triMatrix.TryEmbedCodewords(codewords);

            return triMatrix.GetLowestPenaltyMatrix(errorLevel);
        }
    }
}