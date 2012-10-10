using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using QrCode.Web.Encoder.Windows.Render;

namespace QrCode.Web.Encoder.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class QrCodeGraphicControl : Control
    {
        /// <summary>
        /// 
        /// </summary>
        private QrCode m_QrCode = new QrCode();

        /// <summary>
        /// 
        /// </summary>
        private bool m_isFreezed;

        /// <summary>
        /// 
        /// </summary>
        private bool m_isLocked;

        /// <summary>
        /// Return whether if class is freezed.
        /// </summary>
        /// <remarks></remarks>
        public bool IsFreezed
        {
            get { return m_isFreezed; }
        }

        /// <summary>
        /// Return whether if class is locked
        /// </summary>
        /// <remarks></remarks>
        public bool IsLocked
        {
            get { return m_isLocked; }
        }

        /// <summary>
        /// Occurs when [dark brush changed].
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler DarkBrushChanged;

        /// <summary>
        /// Occurs when [light brush changed].
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler LightBrushChanged;

        /// <summary>
        /// Occurs when [quiet zone module changed].
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler QuietZoneModuleChanged;

        /// <summary>
        /// Occurs when [error correct level changed].
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler ErrorCorrectLevelChanged;

        /// <summary>
        /// Occurs when [qr matrix changed].
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler QrMatrixChanged;

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        /// <remarks></remarks>
        protected override void OnPaint(PaintEventArgs e)
        {
            int offsetX, offsetY, width;
            if (Width <= Height)
            {
                offsetX = 0;
                offsetY = (Height - Width)/2;
                width = Width;
            }
            else
            {
                offsetX = (Width - Height)/2;
                offsetY = 0;
                width = Height;
            }

            new GraphicsRenderer(new FixedCodeSize(width, m_QuietZoneModule), m_darkBrush, m_LightBrush).Draw(
                e.Graphics, m_QrCode.Matrix, new Point(offsetX, offsetY));

            base.OnPaint(e);
        }

        /// <summary>
        /// Updates the qr code cache.
        /// </summary>
        /// <remarks></remarks>
        internal void UpdateQrCodeCache()
        {
            if (!m_isLocked)
            {
                new QrEncoder(m_ErrorCorrectLevel).TryEncode(Text, out m_QrCode);
                OnQrMatrixChanged(new EventArgs());
                Invalidate();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:QrMatrixChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void OnQrMatrixChanged(EventArgs e)
        {
            if (QrMatrixChanged != null)
                QrMatrixChanged(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.TextChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        /// <remarks></remarks>
        protected override void OnTextChanged(EventArgs e)
        {
            UpdateQrCodeCache();
            base.OnTextChanged(e);
        }

        /// <summary>
        /// Lock Class, that any change to Text or ErrorCorrectLevel won't cause it to update QrCode Matrix
        /// </summary>
        /// <remarks></remarks>
        public void Lock()
        {
            m_isLocked = true;
        }

        /// <summary>
        /// Unlock Class, then update QrCodeMatrix and repaint
        /// </summary>
        /// <remarks></remarks>
        public void UnLock()
        {
            m_isLocked = false;
            UpdateQrCodeCache();
        }

        /// <summary>
        /// Freeze Class, Any value change to Brush, QuietZoneModule won't cause immediately repaint.
        /// </summary>
        /// <remarks>It won't stop any repaint cause by other action.</remarks>
        public void Freeze()
        {
            m_isFreezed = true;
        }

        /// <summary>
        /// Unfreeze and repaint immediately.
        /// </summary>
        /// <remarks></remarks>
        public void UnFreeze()
        {
            m_isFreezed = false;
            Invalidate();
        }

        /// <summary>
        /// Get Qr SquareBitMatrix as two dimentional bool array.
        /// It will be deep copy of control's internal bitmatrix.
        /// </summary>
        /// <returns>null if matrix is null, else full matrix</returns>
        /// <remarks></remarks>
        public SquareBitMatrix GetQrMatrix()
        {
            if (m_QrCode.Matrix == null)
                return null;
            else
            {
                BitMatrix matrix = m_QrCode.Matrix;
                bool[,] internalArray = matrix.InternalArray;
                return new SquareBitMatrix(internalArray);
            }
        }

        #region DarkBrush

        /// <summary>
        /// 
        /// </summary>
        private Brush m_darkBrush = Brushes.Black;

        /// <summary>
        /// Gets or sets the dark brush.
        /// </summary>
        /// <value>The dark brush.</value>
        /// <remarks></remarks>
        [Category("Qr Code"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always),
         RefreshProperties(RefreshProperties.All), Localizable(false)]
        public Brush DarkBrush
        {
            get { return m_darkBrush; }
            set
            {
                if (m_darkBrush != value)
                {
                    m_darkBrush = value;
                    OnDarkBrushChanged(new EventArgs());
                    if (!m_isFreezed)
                        Invalidate();
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DarkBrushChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void OnDarkBrushChanged(EventArgs e)
        {
            if (DarkBrushChanged != null)
            {
                DarkBrushChanged(this, e);
            }
        }

        #endregion

        #region LightBrush

        /// <summary>
        /// 
        /// </summary>
        private Brush m_LightBrush = Brushes.White;

        /// <summary>
        /// Gets or sets the light brush.
        /// </summary>
        /// <value>The light brush.</value>
        /// <remarks></remarks>
        [Category("Qr Code"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always),
         RefreshProperties(RefreshProperties.All), Localizable(false)]
        public Brush LightBrush
        {
            get { return m_LightBrush; }
            set
            {
                if (m_LightBrush != value)
                {
                    m_LightBrush = value;
                    OnLightBrushChanged(new EventArgs());
                    if (!m_isFreezed)
                        Invalidate();
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:LightBrushChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void OnLightBrushChanged(EventArgs e)
        {
            if (LightBrushChanged != null)
                LightBrushChanged(this, e);
        }

        #endregion

        #region QuietZoneModule

        /// <summary>
        /// 
        /// </summary>
        private QuietZoneModules m_QuietZoneModule = QuietZoneModules.Two;

        /// <summary>
        /// Gets or sets the quiet zone module.
        /// </summary>
        /// <value>The quiet zone module.</value>
        /// <remarks></remarks>
        [Category("Qr Code"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always),
         RefreshProperties(RefreshProperties.All), Localizable(false)]
        public QuietZoneModules QuietZoneModule
        {
            get { return m_QuietZoneModule; }
            set
            {
                if (m_QuietZoneModule != value)
                {
                    m_QuietZoneModule = value;
                    OnQuietZoneModuleChanged(new EventArgs());
                    if (!m_isFreezed)
                        Invalidate();
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:QuietZoneModuleChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void OnQuietZoneModuleChanged(EventArgs e)
        {
            if (QuietZoneModuleChanged != null)
                QuietZoneModuleChanged(this, e);
        }

        #endregion

        #region ErrorCorrectLevel

        /// <summary>
        /// 
        /// </summary>
        private ErrorCorrectionLevel m_ErrorCorrectLevel = ErrorCorrectionLevel.M;

        /// <summary>
        /// Gets or sets the error correct level.
        /// </summary>
        /// <value>The error correct level.</value>
        /// <remarks></remarks>
        [Category("Qr Code"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always),
         RefreshProperties(RefreshProperties.All), Localizable(false)]
        public ErrorCorrectionLevel ErrorCorrectLevel
        {
            get { return m_ErrorCorrectLevel; }
            set
            {
                if (m_ErrorCorrectLevel != value)
                {
                    m_ErrorCorrectLevel = value;

                    UpdateQrCodeCache();
                    OnErrorCorrectLevelChanged(new EventArgs());
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:ErrorCorrectLevelChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void OnErrorCorrectLevelChanged(EventArgs e)
        {
            if (ErrorCorrectLevelChanged != null)
                ErrorCorrectLevelChanged(this, e);
        }

        #endregion
    }
}