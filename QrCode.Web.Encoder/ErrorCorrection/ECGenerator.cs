using System;
using System.Collections.Generic;
using QrCode.Web.Encoder.ReedSolomon;

namespace QrCode.Web.Encoder.ErrorCorrection
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal static class ECGenerator
    {
        /// <summary>
        /// Generate error correction blocks. Then out put with codewords BitList
        /// ISO/IEC 18004/2006 P45, 46. Chapter 6.6 Constructing final message codewords sequence.
        /// </summary>
        /// <param name="dataCodewords">Datacodewords from DataEncodation.DataEncode</param>
        /// <param name="vd">The vd.</param>
        /// <returns>codewords BitList contain datacodewords and ECCodewords</returns>
        /// <remarks></remarks>
        internal static BitList FillECCodewords(BitList dataCodewords, VersionDetail vd)
        {
            List<byte> dataCodewordsByte = dataCodewords.List;
            int ecBlockGroup2 = vd.ECBlockGroup2;
            int ecBlockGroup1 = vd.ECBlockGroup1;
            int numDataBytesGroup1 = vd.NumDataBytesGroup1;
            int numDataBytesGroup2 = vd.NumDataBytesGroup2;

            int ecBytesPerBlock = vd.NumECBytesPerBlock;

            int dataBytesOffset = 0;
            var dByteJArray = new byte[vd.NumECBlocks][];
            var ecByteJArray = new byte[vd.NumECBlocks][];

            GaloisField256 gf256 = GaloisField256.QRCodeGaloisField;
            var generator = new GeneratorPolynomial(gf256);

            for (int blockID = 0; blockID < vd.NumECBlocks; blockID++)
            {
                if (blockID < ecBlockGroup1)
                {
                    dByteJArray[blockID] = new byte[numDataBytesGroup1];
                    for (int index = 0; index < numDataBytesGroup1; index++)
                    {
                        dByteJArray[blockID][index] = dataCodewordsByte[dataBytesOffset + index];
                    }
                    dataBytesOffset += numDataBytesGroup1;
                }
                else
                {
                    dByteJArray[blockID] = new byte[numDataBytesGroup2];
                    for (int index = 0; index < numDataBytesGroup2; index++)
                    {
                        dByteJArray[blockID][index] = dataCodewordsByte[dataBytesOffset + index];
                    }
                    dataBytesOffset += numDataBytesGroup2;
                }

                ecByteJArray[blockID] = ReedSolomonEncoder.Encode(dByteJArray[blockID], ecBytesPerBlock, generator);
            }
            if (vd.NumDataBytes != dataBytesOffset)
                throw new ArgumentException("Data bytes does not match offset");

            var codewords = new BitList();

            int maxDataLength = ecBlockGroup1 == vd.NumECBlocks ? numDataBytesGroup1 : numDataBytesGroup2;

            for (int dataID = 0; dataID < maxDataLength; dataID++)
            {
                for (int blockID = 0; blockID < vd.NumECBlocks; blockID++)
                {
                    if (!(dataID == numDataBytesGroup1 && blockID < ecBlockGroup1))
                        codewords.Add(dByteJArray[blockID][dataID], 8);
                }
            }

            for (int ECID = 0; ECID < ecBytesPerBlock; ECID++)
            {
                for (int blockID = 0; blockID < vd.NumECBlocks; blockID++)
                {
                    codewords.Add(ecByteJArray[blockID][ECID], 8);
                }
            }

            if (vd.NumTotalBytes != codewords.Count >> 3)
                throw new ArgumentException(string.Format("total bytes: {0}, actual bits: {1}", vd.NumTotalBytes,
                                                          codewords.Count));

            return codewords;
        }
    }
}