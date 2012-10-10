using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using QrCode.Web.Encoder.Windows.Render;

namespace QrCode.Web.Encoder
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal class QrCodeImageHandler : IHttpHandler
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
            try
            {
                context.Response.ContentType = "image/jpeg";
                string text = context.Request.QueryString[0];

                var qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                QrCode qrCode = qrEncoder.Encode(text);

                var renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black,
                                                    Brushes.White);
                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Jpeg, context.Response.OutputStream);
            }
            catch (Exception)
            {
            }
        }

        #endregion Methods
    }
}