using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QrCode.Web.Encoder;
using QrCode.Web.Encoder.Windows.Render;

namespace QrCode.Web.Control
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:QrControl runat=server></{0}:QrControl>")]
    public class QrControl : WebControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? "[" + this.ID + "]" : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            //this.GetQrImage();
            // output.Write();   
        }

        private void GetQrImage()
        {
            HttpContext.Current.Response.ContentType = "image/jpeg";
            
 
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode.Web.Encoder.QrCode qrCode = qrEncoder.Encode(this.Text);

            GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);

            renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, HttpContext.Current.Response.OutputStream);
             
            //using (FileStream stream = new FileStream(@"c:\temp\HelloWorld.png", FileMode.Create))
            //{
            //    renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);

            //    img.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //}

            //image.Save(context.Response.OutputStream, ImageFormat.Jpeg)
        }
    }
}
