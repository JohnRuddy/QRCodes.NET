using System;

namespace QrCode.Web.Encoder.Windows.Render
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class FixedModuleSize : ISizeCalculation
    {
        /// <summary>
        /// 
        /// </summary>
        private int m_ModuleSize;

        /// <summary>
        /// 
        /// </summary>
        private int m_QuietZoneModule;

        /// <summary>
        /// FixedModuleSize is strategy for rendering QrCode with fixed module pixel size.
        /// </summary>
        /// <param name="moduleSize">Size of the module.</param>
        /// <param name="quietZoneModules">The quiet zone modules.</param>
        /// <remarks></remarks>
        public FixedModuleSize(int moduleSize, QuietZoneModules quietZoneModules)
        {
            m_ModuleSize = moduleSize;
            m_QuietZoneModule = (int) quietZoneModules;
        }

        /// <summary>
        /// Module pixel size. Have to greater than zero
        /// </summary>
        /// <value>The size of the module.</value>
        /// <remarks></remarks>
        public int ModuleSize
        {
            get { return m_ModuleSize; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("ModuleSize", value,
                                                          "ModuleSize can not be equal or less than zero");
                m_ModuleSize = value;
            }
        }

        /// <summary>
        /// Number of quietZone modules
        /// </summary>
        /// <value>The quiet zone modules.</value>
        /// <remarks></remarks>
        public QuietZoneModules QuietZoneModules
        {
            get { return (QuietZoneModules) m_QuietZoneModule; }
            set { m_QuietZoneModule = (int) value; }
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
            int width = (m_QuietZoneModule*2 + matrixWidth)*m_ModuleSize;
            return new DrawingSize(m_ModuleSize, width, (QuietZoneModules) m_QuietZoneModule);
        }

        #endregion
    }
}