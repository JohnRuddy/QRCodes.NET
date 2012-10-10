using System.Windows;
using System.Windows.Media;

namespace QrCode.Web.Encoder.Windows.Render
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal static class GeometryExtensions
    {
        /// <summary>
        /// Draws the rect geometry.
        /// </summary>
        /// <param name="ctx">The CTX.</param>
        /// <param name="rect">The rect.</param>
        /// <remarks></remarks>
        internal static void DrawRectGeometry(this StreamGeometryContext ctx, Int32Rect rect)
        {
            if (rect.IsEmpty)
                return;

            ctx.BeginFigure(new Point(rect.X, rect.Y), true, true);
            ctx.LineTo(new Point(rect.X, rect.Y + rect.Height), false, false);
            ctx.LineTo(new Point(rect.X + rect.Width, rect.Y + rect.Height), false, false);
            ctx.LineTo(new Point(rect.X + rect.Width, rect.Y), false, false);
        }
    }
}