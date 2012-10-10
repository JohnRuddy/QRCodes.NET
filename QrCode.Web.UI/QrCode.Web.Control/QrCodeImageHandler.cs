namespace QrCode.Web.Control
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Text;
    using System.Web;

    using QrCode.Web.Encoder;
    using QrCode.Web.Encoder.Windows.Render;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    class QrCodeImageHandler : IHttpHandler
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        ///   </returns>
        /// <remarks></remarks>
        public bool IsReusable
        {
            get { return false; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        /// <remarks></remarks>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/jpeg";

            var text = context.Request.QueryString["Text"];

            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode.Web.Encoder.QrCode qrCode = qrEncoder.Encode(text);

            GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);

            renderer.WriteToStream(qrCode.Matrix, ImageFormat.Jpeg, context.Response.OutputStream);
        }

        #endregion Methods
    }
}