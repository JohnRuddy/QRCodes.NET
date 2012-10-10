using QrCode.Web.Encoder.Versions;

namespace QrCode.Web.Encoder
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class QrEncoder
    {
        /// <summary>
        /// Default QrEncoder will set ErrorCorrectionLevel as M
        /// </summary>
        /// <remarks></remarks>
        public QrEncoder()
            : this(ErrorCorrectionLevel.M)
        {
        }

        /// <summary>
        /// QrEncoder with parameter ErrorCorrectionLevel.
        /// </summary>
        /// <param name="errorCorrectionLevel">The error correction level.</param>
        /// <remarks></remarks>
        public QrEncoder(ErrorCorrectionLevel errorCorrectionLevel)
        {
            ErrorCorrectionLevel = errorCorrectionLevel;
        }

        /// <summary>
        /// Gets or sets the error correction level.
        /// </summary>
        /// <value>The error correction level.</value>
        /// <remarks></remarks>
        public ErrorCorrectionLevel ErrorCorrectionLevel { get; set; }

        /// <summary>
        /// Encode string content to QrCode matrix
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        /// <exception cref="InputOutOfBoundaryException">
        /// This exception for string content is null, empty or too large</exception>
        /// <remarks></remarks>
        public QrCode Encode(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new InputOutOfBoundaryException("Input should not be empty or null");
            }
            else
                return new QrCode(QRCodeEncode.Encode(content, ErrorCorrectionLevel));
        }

        /// <summary>
        /// Try to encode content
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="qrCode">The qr code.</param>
        /// <returns>False if input content is empty, null or too large.</returns>
        /// <remarks></remarks>
        public bool TryEncode(string content, out QrCode qrCode)
        {
            try
            {
                qrCode = Encode(content);
                return true;
            }
            catch (InputOutOfBoundaryException)
            {
                qrCode = new QrCode();
                return false;
            }
        }
    }
}