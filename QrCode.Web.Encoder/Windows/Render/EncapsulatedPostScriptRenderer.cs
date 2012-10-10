using System;
using System.Globalization;
using System.IO;

namespace QrCode.Web.Encoder.Windows.Render
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class EncapsulatedPostScriptRenderer
    {
        /// <summary>
        /// 
        /// </summary>
        private EpsModuleDrawingTechnique m_DrawingTechnique;

        /// <summary>
        /// 
        /// </summary>
        private ISizeCalculation m_iSize;

        /// <summary>
        /// Initializes a Encapsulated PostScript renderer.
        /// </summary>
        /// <param name="iSize">Size of the i.</param>
        /// <param name="darkColor">Color of the dark.</param>
        /// <param name="lightColor">Color of the light.</param>
        /// <remarks></remarks>
        public EncapsulatedPostScriptRenderer(ISizeCalculation iSize, EPSColor darkColor, EPSColor lightColor)
        {
            m_iSize = iSize;
            DarkColor = darkColor;
            LightColor = lightColor;
            m_DrawingTechnique = EpsModuleDrawingTechnique.Squares;
        }

        /// <summary>
        /// ISizeCalculation for the way to calculate QrCode's pixel size.
        /// Ex for ISizeCalculation:FixedCodeSize, FixedModuleSize
        /// </summary>
        /// <value>The size calculator.</value>
        /// <remarks></remarks>
        public ISizeCalculation SizeCalculator
        {
            set { m_iSize = value; }
            get { return m_iSize; }
        }

        /// <summary>
        /// DarkColor used to draw Dark modules of the QrCode
        /// </summary>
        /// <value>The color of the dark.</value>
        /// <remarks></remarks>
        public EPSColor DarkColor { set; get; }

        /// <summary>
        /// LightColor used to draw Light modules and QuietZone of the QrCode.
        /// Setting to a transparent color (A = 0) allows transparent light modules so the QR Code blends in the existing background.
        /// In that case the existing background should remain light and rather uniform, and higher error correction levels are recommended.
        /// </summary>
        /// <value>The color of the light.</value>
        /// <remarks></remarks>
        public EPSColor LightColor { set; get; }

        /// <summary>
        /// Selection of the technique used to draw the modules. 'Squares' draws vector squares one by one; 'Image' uses the 'image' or 'colorimage' PostScript command.
        /// 'Squares' supports transparency of light modules and often has smaller file size for color QR Codes.
        /// 'Image' might be faster to render on some devices as it is a single command.
        /// </summary>
        /// <value>The drawing technique.</value>
        /// <remarks></remarks>
        public EpsModuleDrawingTechnique DrawingTechnique
        {
            get { return m_DrawingTechnique; }
            set { m_DrawingTechnique = value; }
        }

        /// <summary>
        /// Renders the matrix in an Encapsuled PostScript format.
        /// </summary>
        /// <param name="matrix">The matrix to be rendered</param>
        /// <param name="stream">Output stream that must be writable</param>
        /// <remarks></remarks>
        public void WriteToStream(BitMatrix matrix, Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                int width = matrix == null ? 21 : matrix.Width;

                DrawingSize drawingSize = m_iSize.GetSize(width);

                OutputHeader(drawingSize, writer);
                OutputBackground(writer);

                if (matrix != null)
                {
                    switch (m_DrawingTechnique)
                    {
                        case EpsModuleDrawingTechnique.Squares:
                            DrawSquares(matrix, writer);
                            break;
                        case EpsModuleDrawingTechnique.Image:
                            DrawImage(matrix, writer);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("DrawingTechnique");
                    }
                }

                OutputFooter(writer);
            }
        }

        /// <summary>
        /// Outputs the EPS header with mandatory declarations, variable declarations and function definitions.
        /// </summary>
        /// <param name="drawingSize">Size of the drawing.</param>
        /// <param name="stream">Output text stream</param>
        /// <remarks></remarks>
        private void OutputHeader(DrawingSize drawingSize, StreamWriter stream)
        {
            string strHeader =
                @"%!PS-Adobe-3.0 EPSF-3.0
%%Creator: Gma.QrCodeNet
%%Title: QR Code
%%CreationDate: {0:yyyyMMdd}
%%Pages: 1
%%BoundingBox: 0 0 {1} {2}
%%Document-Fonts: Times-Roman
%%LanguageLevel: 1
%%EndComments
%%BeginProlog
/w {{ {3} }} def
/h {{ {4} }} def
/q {{ {5} }} def
/s {{ {6} }} def
/W {{ w q q add add }} def
/H {{ h q q add add }} def";

            string strBoxFunctions =
                @"% Define the box functions taking X and Y coordinates of the top left corner and filling a 1 point large square
/b { newpath moveto 1 0 rlineto 0 1 rlineto -1 0 rlineto closepath fill } def
/br { newpath moveto 1.01 0 rlineto 0 1 rlineto -1.01 0 rlineto closepath fill } def
/bb { newpath moveto 1 0 rlineto 0 1.01 rlineto -1 0 rlineto closepath fill } def
/brb { newpath moveto 1.01 0 rlineto 0 1 rlineto -0.01 0 rlineto 0 0.01 rlineto -1 0 rlineto closepath fill } def";

            string strHeaderEnd =
                @"%%EndProlog
%%Page: 1 1

% Save the current state
save

% Invert the Y axis
0 W s mul translate
s s neg scale";

            stream.WriteLine(string.Format(strHeader,
                                           DateTime.UtcNow,
                                           // Use invariant culture to ensure that the dot is used as the decimal separator
                                           (drawingSize.CodeWidth).ToString(CultureInfo.InvariantCulture.NumberFormat),
                                           // Size in points of the matrix with the quiet zone
                                           (drawingSize.CodeWidth).ToString(CultureInfo.InvariantCulture.NumberFormat),
                                           drawingSize.CodeWidth/drawingSize.ModuleSize -
                                           ((int) drawingSize.QuietZoneModules*2),
                                           // Number of modules of the matrix without the quiet zone
                                           drawingSize.CodeWidth/drawingSize.ModuleSize -
                                           ((int) drawingSize.QuietZoneModules*2),
                                           (int) drawingSize.QuietZoneModules, // Number of quiet zone modules
                                           drawingSize.ModuleSize.ToString(CultureInfo.InvariantCulture.NumberFormat)));
            // Size in points of a single module

            if (m_DrawingTechnique == EpsModuleDrawingTechnique.Squares)
                stream.WriteLine(strBoxFunctions);

            stream.WriteLine(strHeaderEnd);
        }

        /// <summary>
        /// Outputs the background unless it is defined as transparent. The background is used for light modules and quiet zone.
        /// </summary>
        /// <param name="stream">Output text stream</param>
        /// <remarks></remarks>
        private void OutputBackground(StreamWriter stream)
        {
            string strBackground =
                @"
% Create the background
{0} 255 div {1} 255 div {2} 255 div setrgbcolor
newpath 0 0 moveto W 0 rlineto 0 H rlineto W neg 0 rlineto closepath fill";

            if (LightColor.A != 0)
                stream.WriteLine(string.Format(strBackground,
                                               LightColor.R,
                                               LightColor.G,
                                               LightColor.B));
        }

        /// <summary>
        /// Draw a square for each dark module
        /// </summary>
        /// <param name="matrix">The matrix to be rendered</param>
        /// <param name="stream">Output text stream</param>
        /// <remarks></remarks>
        private void DrawSquares(BitMatrix matrix, StreamWriter stream)
        {
            string strSquaresHeader =
                @"
% Draw squares
{0} 255 div {1} 255 div {2} 255 div setrgbcolor
q q translate";

            stream.WriteLine(string.Format(strSquaresHeader,
                                           DarkColor.R,
                                           DarkColor.G,
                                           DarkColor.B));

            for (int y = 0; y < matrix.Height; ++y)
            {
                for (int x = 0; x < matrix.Width; ++x)
                {
                    if (matrix[x, y]) // Only draw dark modules
                    {
                        bool bHasRightNeighbor = x + 1 < matrix.Width && matrix[x + 1, y];
                        bool bHasBottomNeighbor = y + 1 < matrix.Height && matrix[x, y + 1];
                        // If the dark module has a dark neighbor on the right, make the two boxed overlap 1% by calling "br" instead of simply "b"
                        // Same applies for bottom neighbor with "bb" command, or "brb" for right and bottom dark neighbors.
                        // Overlapping two modules avoids having tiny gaps appearing on certain zoom scales or output devices.

                        // Output the coordinates of the upper left corner and call to the box function
                        stream.WriteLine(string.Format("{0} {1} b{2}{3}", x, y, bHasRightNeighbor ? "r" : "",
                                                       bHasBottomNeighbor ? "b" : ""));
                    }
                }
            }
        }

        /// <summary>
        /// Use the 'image' or 'colorimage' PostScript command to render modules
        /// </summary>
        /// <param name="matrix">The matrix to be rendered</param>
        /// <param name="stream">Output text stream</param>
        /// <remarks></remarks>
        private void DrawImage(BitMatrix matrix, StreamWriter stream)
        {
            bool bGrayScale =
                LightColor.R == LightColor.G &&
                LightColor.G == LightColor.B &&
                DarkColor.R == DarkColor.G &&
                DarkColor.G == DarkColor.B;
            string strLightColor;
            string strDarkColor;
            if (bGrayScale)
            {
                strLightColor = LightColor.R.ToString("x2");
                strDarkColor = DarkColor.R.ToString("x2");
            }
            else
            {
                strLightColor = string.Format("{0:x2}{1:x2}{2:x2}", LightColor.R, LightColor.G, LightColor.B);
                strDarkColor = string.Format("{0:x2}{1:x2}{2:x2}", DarkColor.R, DarkColor.G, DarkColor.B);
            }

            stream.WriteLine(@"% Draw squares
q q translate
w h scale
w h 8 [w 0 0 h 0 0]
{<");
            for (int y = 0; y < matrix.Height; ++y)
            {
                for (int x = 0; x < matrix.Width; ++x)
                    stream.Write(matrix[x, y] ? strDarkColor : strLightColor);
                stream.WriteLine();
            }
            stream.WriteLine(">}");
            stream.WriteLine(bGrayScale ? "image" : "false 3 colorimage");
        }

        /// <summary>
        /// Outputs the mandatory EPS footer.
        /// </summary>
        /// <param name="stream">Output text stream</param>
        /// <remarks></remarks>
        private void OutputFooter(StreamWriter stream)
        {
            string strFooter = @"
% Restore the initial state
restore showpage
%
% End of page
%
%%Trailer
%%EOF";

            stream.Write(strFooter);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public enum EpsModuleDrawingTechnique
    {
        /// <summary>
        /// 
        /// </summary>
        Squares,

        /// <summary>
        /// 
        /// </summary>
        Image
    };
}