using System;

namespace QrCode.Web.Encoder.ReedSolomon
{
    /// <summary>
    /// Description of GaloisField256.
    /// </summary>
    /// <remarks></remarks>
    internal sealed class GaloisField256
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly int[] antiLogTable;

        /// <summary>
        /// 
        /// </summary>
        private readonly int[] logTable;

        /// <summary>
        /// 
        /// </summary>
        private readonly int m_primitive;


        /// <summary>
        /// Initializes a new instance of the <see cref="GaloisField256"/> class.
        /// </summary>
        /// <param name="primitive">The primitive.</param>
        /// <remarks></remarks>
        internal GaloisField256(int primitive)
        {
            antiLogTable = new int[256];
            logTable = new int[256];

            m_primitive = primitive;

            int gfx = 1;
            //Power cycle is from 0 to 254. 2^255 = 1 = 2^0 
            //Value cycle is from 1 to 255. Thus there should not have Log(0).			
            for (int powers = 0; powers < 256; powers++)
            {
                antiLogTable[powers] = gfx;
                if (powers != 255)
                    logTable[gfx] = powers;
                gfx <<= 1; //gfx = gfx * 2 where alpha is 2.

                if (gfx > 255)
                {
                    gfx ^= primitive;
                }
            }
        }

        /// <summary>
        /// Gets the primitive.
        /// </summary>
        /// <remarks></remarks>
        internal int Primitive
        {
            get { return m_primitive; }
        }

        /// <summary>
        /// Gets the QR code galois field.
        /// </summary>
        /// <remarks></remarks>
        internal static GaloisField256 QRCodeGaloisField
        {
            get { return new GaloisField256(QRCodeConstantVariable.QRCodePrimitive); }
        }

        /// <summary>
        /// Exponents the specified powers ofa.
        /// </summary>
        /// <param name="PowersOfa">The powers ofa.</param>
        /// <returns>Powers of a in GF table. Where a = 2</returns>
        /// <remarks></remarks>
        internal int Exponent(int PowersOfa)
        {
            return antiLogTable[PowersOfa];
        }

        /// <summary>
        /// Logs the specified gf value.
        /// </summary>
        /// <param name="gfValue">The gf value.</param>
        /// <returns>log ( power of a) in GF table. Where a = 2</returns>
        /// <remarks></remarks>
        internal int Log(int gfValue)
        {
            if (gfValue == 0)
                throw new ArgumentException("GaloisField value will not equal to 0, Log method");
            return logTable[gfValue];
        }

        /// <summary>
        /// Inverses the specified gf value.
        /// </summary>
        /// <param name="gfValue">The gf value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal int inverse(int gfValue)
        {
            if (gfValue == 0)
                throw new ArgumentException("GaloisField value will not equal to 0, Inverse method");
            return Exponent(255 - Log(gfValue));
        }

        /// <summary>
        /// Additions the specified gf value A.
        /// </summary>
        /// <param name="gfValueA">The gf value A.</param>
        /// <param name="gfValueB">The gf value B.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal int Addition(int gfValueA, int gfValueB)
        {
            return gfValueA ^ gfValueB;
        }

        /// <summary>
        /// Subtractions the specified gf value A.
        /// </summary>
        /// <param name="gfValueA">The gf value A.</param>
        /// <param name="gfValueB">The gf value B.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal int Subtraction(int gfValueA, int gfValueB)
        {
            //Subtraction is same as addition. 
            return Addition(gfValueA, gfValueB);
        }

        /// <summary>
        /// Products the specified gf value A.
        /// </summary>
        /// <param name="gfValueA">The gf value A.</param>
        /// <param name="gfValueB">The gf value B.</param>
        /// <returns>Product of two values.
        /// In other words. a multiply b</returns>
        /// <remarks></remarks>
        internal int Product(int gfValueA, int gfValueB)
        {
            if (gfValueA == 0 || gfValueB == 0)
            {
                return 0;
            }
            if (gfValueA == 1)
            {
                return gfValueB;
            }
            if (gfValueB == 1)
            {
                return gfValueA;
            }

            return Exponent((Log(gfValueA) + Log(gfValueB))%255);
        }

        /// <summary>
        /// Quotients the specified gf value A.
        /// </summary>
        /// <param name="gfValueA">The gf value A.</param>
        /// <param name="gfValueB">The gf value B.</param>
        /// <returns>Quotient of two values.
        /// In other words. a devided b</returns>
        /// <remarks></remarks>
        internal int Quotient(int gfValueA, int gfValueB)
        {
            if (gfValueA == 0)
                return 0;
            if (gfValueB == 0)
                throw new ArgumentException("gfValueB can not be zero");
            if (gfValueB == 1)
                return gfValueA;
            return Exponent(Math.Abs(Log(gfValueA) - Log(gfValueB))%255);
        }
    }
}