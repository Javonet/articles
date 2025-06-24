using SkiaSharp;
using Svg.Skia;

namespace Charts
{
    public class ChartDisplayPanel : PictureBox
    {
        public ChartDisplayPanel()
        {
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        public void DisplayImage(string svgContent)
        {
            Image = RenderSvgToBitmap(svgContent, Width, Height);
        }

        private Bitmap RenderSvgToBitmap(string svg, int width, int height)
        {
            using (var svgStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(svg)))
            {
                var skSvg = new SKSvg();
                skSvg.Load(svgStream);

                var bitmap = new SKBitmap(width, height);
                using (var canvas = new SKCanvas(bitmap))
                {
                    canvas.Clear(SKColors.Transparent);

                    var scale = Math.Min(width / skSvg.Picture.CullRect.Width, height / skSvg.Picture.CullRect.Height);
                    canvas.Scale((float)scale);

                    canvas.DrawPicture(skSvg.Picture);
                }

                return SkiaSharp.Views.Desktop.Extensions.ToBitmap(bitmap);
            }
        }
    }
}
