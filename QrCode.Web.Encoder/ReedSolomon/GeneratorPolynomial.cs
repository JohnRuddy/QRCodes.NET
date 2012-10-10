using System.Collections.Generic;

namespace QrCode.Web.Encoder.ReedSolomon
{
    /// <summary>
    /// Description of GeneratorPolynomial.
    /// </summary>
    /// <remarks></remarks>
    internal sealed class GeneratorPolynomial
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly List<Polynomial> m_cacheGenerator;

        /// <summary>
        /// 
        /// </summary>
        private readonly GaloisField256 m_gfield;

        /// <summary>
        /// After create GeneratorPolynomial. Keep it as long as possible.
        /// Unless QRCode encode is done or no more QRCode need to generate.
        /// </summary>
        /// <param name="gfield">The gfield.</param>
        /// <remarks></remarks>
        internal GeneratorPolynomial(GaloisField256 gfield)
        {
            m_gfield = gfield;
            m_cacheGenerator = new List<Polynomial>(10);
            m_cacheGenerator.Add(new Polynomial(m_gfield, new[] {1}));
        }

        /// <summary>
        /// Get generator by degree. (Largest degree for that generator)
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <returns>Generator</returns>
        /// <remarks></remarks>
        internal Polynomial GetGenerator(int degree)
        {
            if (degree >= m_cacheGenerator.Count)
                BuildGenerator(degree);
            return m_cacheGenerator[degree];
        }

        /// <summary>
        /// Build Generator if we can not find specific degree of generator from cache
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <remarks></remarks>
        private void BuildGenerator(int degree)
        {
            lock (m_cacheGenerator)
            {
                int currentCacheLength = m_cacheGenerator.Count;
                if (degree >= currentCacheLength)
                {
                    Polynomial lastGenerator = m_cacheGenerator[currentCacheLength - 1];

                    for (int d = currentCacheLength; d <= degree; d++)
                    {
                        Polynomial nextGenerator =
                            lastGenerator.Multiply(new Polynomial(m_gfield, new[] {1, m_gfield.Exponent(d - 1)}));
                        m_cacheGenerator.Add(nextGenerator);
                        lastGenerator = nextGenerator;
                    }
                }
            }
        }
    }
}