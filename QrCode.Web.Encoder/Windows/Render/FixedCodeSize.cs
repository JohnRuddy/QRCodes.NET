using System;

namespace QrCode.Web.Encoder.Windows.Render
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class FixedCodeSize : ISizeCalculation
    {
        /// <summary>
        /// 
        /// </summary>
        private int m_QrCodeWidth;

        /// <summary>
        /// 
        /// </summary>
        private int m_QuietZoneModules = 2;

        /// <summary>
        /// FixedCodeSize is strategy for rendering QrCode at fixed Size.
        /// </summary>
        /// <param name="qrCodeWidth">Width of the qr code.</param>
        /// <param name="quietZone">The quiet zone.</param>
        /// <remarks></remarks>
        public FixedCodeSize(int qrCodeWidth, QuietZoneModules quietZone)
        {
            m_QrCodeWidth = qrCodeWidth;
            m_QuietZoneModules = (int) quietZone;
        }

        /// <summary>
        /// QrCodeWidth is pixel size of QrCode you would like to print out.
        /// It have to be greater than QrCode's matrix width(include quiet zone).
        /// QrCode matrix width is between 25 ~ 182(version 1 ~ 40).
        /// </summary>
        /// <value>The width of the qr code.</value>
        /// <remarks></remarks>
        public int QrCodeWidth
        {
            get { return m_QrCodeWidth; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("QrCodeWidth", value,
                                                          "QrCodeWidth can not be equal or less than zero");
                m_QrCodeWidth = value;
            }
        }

        /// <summary>
        /// Number of quietZone modules
        /// </summary>
        /// <value>The quiet zone modules.</value>
        /// <remarks></remarks>
        public QuietZoneModules QuietZoneModules
        {
            get { return (QuietZoneModules) m_QuietZoneModules; }
            set { m_QuietZoneModules = (int) value; }
        }

        #region ISizeCalculation Members

        /// <summary>
        /// Interface function that use by Rendering class.
        /// </summary>
        /// <param name="matrixWidth">QrCode matrix width</param>
        /// <returns>Module pixel size and QrCode pixel width</returns>
        /// <remarks></remarks>
        public DrawingSize GetSize(int matrixWidth)
        {
            int moduleSize = m_QrCodeWidth/(matrixWidth + m_QuietZoneModules*2);
            return new DrawingSize(moduleSize, m_QrCodeWidth, (QuietZoneModules) m_QuietZoneModules);
        }

        #endregion
    }
}