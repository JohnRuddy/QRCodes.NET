using System;

namespace QrCode.Web.Encoder.Versions
{
    /// <summary>
    /// Use this exception for null or empty input string or when input string is too large.
    /// </summary>
    /// <remarks></remarks>
    public class InputOutOfBoundaryException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public InputOutOfBoundaryException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputOutOfBoundaryException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public InputOutOfBoundaryException(string message) : base(message)
        {
        }
    }
}