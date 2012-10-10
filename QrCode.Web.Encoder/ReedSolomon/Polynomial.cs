using System;

namespace QrCode.Web.Encoder.ReedSolomon
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal sealed class Polynomial
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly int[] m_Coefficients;

        /// <summary>
        /// 
        /// </summary>
        private readonly GaloisField256 m_GField;

        /// <summary>
        /// 
        /// </summary>
        private readonly int m_primitive;

        /// <summary>
        /// Initializes a new instance of the <see cref="Polynomial"/> class.
        /// </summary>
        /// <param name="gfield">The gfield.</param>
        /// <param name="coefficients">The coefficients.</param>
        /// <remarks></remarks>
        internal Polynomial(GaloisField256 gfield, int[] coefficients)
        {
            int coefficientsLength = coefficients.Length;

            if (coefficientsLength == 0 || coefficients == null)
                throw new ArithmeticException("Can not create empty Polynomial");

            m_GField = gfield;

            m_primitive = gfield.Primitive;

            if (coefficientsLength > 1 && coefficients[0] == 0)
            {
                int firstNonZeroIndex = 1;
                while (firstNonZeroIndex < coefficientsLength && coefficients[firstNonZeroIndex] == 0)
                {
                    firstNonZeroIndex++;
                }

                if (firstNonZeroIndex == coefficientsLength)
                    m_Coefficients = new[] {0};
                else
                {
                    int newLength = coefficientsLength - firstNonZeroIndex;
                    m_Coefficients = new int[newLength];
                    Array.Copy(coefficients, firstNonZeroIndex, m_Coefficients, 0, newLength);
                }
            }
            else
            {
                m_Coefficients = new int[coefficientsLength];
                Array.Copy(coefficients, m_Coefficients, coefficientsLength);
            }
        }

        /// <summary>
        /// Gets the coefficients.
        /// </summary>
        /// <remarks></remarks>
        internal int[] Coefficients
        {
            get { return m_Coefficients; }
        }

        /// <summary>
        /// Gets the G field.
        /// </summary>
        /// <remarks></remarks>
        internal GaloisField256 GField
        {
            get { return m_GField; }
        }


        /// <summary>
        /// Gets the degree.
        /// </summary>
        /// <remarks></remarks>
        internal int Degree
        {
            get { return Coefficients.Length - 1; }
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
        /// Gets a value indicating whether this instance is monomial zero.
        /// </summary>
        /// <remarks></remarks>
        internal bool isMonomialZero
        {
            get { return m_Coefficients[0] == 0; }
        }

        /// <summary>
        /// Gets the coefficient.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <returns>coefficient position. where (coefficient)x^degree</returns>
        /// <remarks></remarks>
        internal int GetCoefficient(int degree)
        {
            //eg: x^2 + x + 1. degree 1, reverse position = degree + 1 = 2. 
            //Pos = 3 - 2 = 1
            return m_Coefficients[m_Coefficients.Length - (degree + 1)];
        }

        /// <summary>
        /// Add another Polynomial to current one
        /// </summary>
        /// <param name="other">The polynomial need to add or subtract to current one</param>
        /// <returns>Result polynomial after add or subtract</returns>
        /// <remarks></remarks>
        internal Polynomial AddOrSubtract(Polynomial other)
        {
            if (Primitive != other.Primitive)
            {
                throw new ArgumentException(
                    "Polynomial can not perform AddOrSubtract as they don't have same Primitive for GaloisField256");
            }
            if (isMonomialZero)
                return other;
            else if (other.isMonomialZero)
                return this;

            int otherLength = other.Coefficients.Length;
            int thisLength = Coefficients.Length;

            if (otherLength > thisLength)
                return CoefficientXor(Coefficients, other.Coefficients);
            else
                return CoefficientXor(other.Coefficients, Coefficients);
        }


        /// <summary>
        /// Coefficients the xor.
        /// </summary>
        /// <param name="smallerCoefficients">The smaller coefficients.</param>
        /// <param name="largerCoefficients">The larger coefficients.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal Polynomial CoefficientXor(int[] smallerCoefficients, int[] largerCoefficients)
        {
            if (smallerCoefficients.Length > largerCoefficients.Length)
                throw new ArgumentException(
                    "Can not perform CoefficientXor method as smaller Coefficients length is larger than larger one.");
            int targetLength = largerCoefficients.Length;
            var xorCoefficient = new int[targetLength];
            int lengthDiff = largerCoefficients.Length - smallerCoefficients.Length;

            Array.Copy(largerCoefficients, 0, xorCoefficient, 0, lengthDiff);

            for (int index = lengthDiff; index < targetLength; index++)
            {
                xorCoefficient[index] = GField.Addition(largerCoefficients[index],
                                                        smallerCoefficients[index - lengthDiff]);
            }

            return new Polynomial(GField, xorCoefficient);
        }

        /// <summary>
        /// Multiply current Polynomial to anotherone.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>Result polynomial after multiply</returns>
        /// <remarks></remarks>
        internal Polynomial Multiply(Polynomial other)
        {
            if (Primitive != other.Primitive)
            {
                throw new ArgumentException(
                    "Polynomial can not perform Multiply as they don't have same Primitive for GaloisField256");
            }
            if (isMonomialZero || other.isMonomialZero)
                return new Polynomial(GField, new[] {0});

            int[] aCoefficients = Coefficients;
            int aLength = aCoefficients.Length;
            int[] bCoefficient = other.Coefficients;
            int bLength = bCoefficient.Length;
            var rCoefficients = new int[aLength + bLength - 1];

            for (int aIndex = 0; aIndex < aLength; aIndex++)
            {
                int aCoeff = aCoefficients[aIndex];
                for (int bIndex = 0; bIndex < bLength; bIndex++)
                {
                    rCoefficients[aIndex + bIndex] =
                        GField.Addition(rCoefficients[aIndex + bIndex], GField.Product(aCoeff, bCoefficient[bIndex]));
                }
            }
            return new Polynomial(GField, rCoefficients);
        }

        /// <summary>
        /// Multiplay scalar to current polynomial
        /// </summary>
        /// <param name="scalar">The scalar.</param>
        /// <returns>result of polynomial after multiply scalar</returns>
        /// <remarks></remarks>
        internal Polynomial MultiplyScalar(int scalar)
        {
            if (scalar == 0)
            {
                return new Polynomial(GField, new[] {0});
            }
            else if (scalar == 1)
            {
                return this;
            }

            int length = Coefficients.Length;
            var rCoefficient = new int[length];

            for (int index = 0; index < length; index++)
            {
                rCoefficient[index] = GField.Product(Coefficients[index], scalar);
            }

            return new Polynomial(GField, rCoefficient);
        }

        /// <summary>
        /// divide current polynomial by "other"
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>Result polynomial after divide</returns>
        /// <remarks></remarks>
        internal PolyDivideStruct Divide(Polynomial other)
        {
            if (Primitive != other.Primitive)
            {
                throw new ArgumentException(
                    "Polynomial can not perform Devide as they don't have same Primitive for GaloisField256");
            }
            if (other.isMonomialZero)
            {
                throw new ArgumentException("Can not devide by Polynomial Zero");
            }
            //this devide by other = a devide by b
            int aLength = Coefficients.Length;
            //We will make change to aCoefficient. It will return as remainder
            var aCoefficients = new int[aLength];
            Array.Copy(Coefficients, 0, aCoefficients, 0, aLength);


            int bLength = other.Coefficients.Length;

            if (aLength < bLength)
                return new PolyDivideStruct(new Polynomial(GField, new[] {0}), this);
            else
            {
                //quotient coefficients
                //qLastIndex = alength - blength  qlength = qLastIndex + 1
                var qCoefficients = new int[(aLength - bLength) + 1];

                //Denominator
                int otherLeadingTerm = other.GetCoefficient(other.Degree);
                int inverseOtherLeadingTerm = GField.inverse(otherLeadingTerm);

                for (int aIndex = 0; aIndex <= aLength - bLength; aIndex++)
                {
                    if (aCoefficients[aIndex] != 0)
                    {
                        int aScalar = GField.Product(inverseOtherLeadingTerm, aCoefficients[aIndex]);
                        Polynomial term = other.MultiplyScalar(aScalar);
                        qCoefficients[aIndex] = aScalar;

                        int[] bCoefficient = term.Coefficients;
                        if (bCoefficient[0] != 0)
                        {
                            for (int bIndex = 0; bIndex < bLength; bIndex++)
                            {
                                aCoefficients[aIndex + bIndex] = GField.Subtraction(aCoefficients[aIndex + bIndex],
                                                                                    bCoefficient[bIndex]);
                            }
                        }
                    }
                }

                return new PolyDivideStruct(new Polynomial(GField, qCoefficients),
                                            new Polynomial(GField, aCoefficients));
            }
        }
    }
}