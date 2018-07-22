using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.TiledScreenDesigner
{
    public static class Drawer
    {
        /// <summary>
        /// Draws a background.
        /// </summary>
        /// <param name="g">Graphics related to the image on which the background should be drawn.</param>
        /// <param name="s">Size of the object.</param>
        /// <param name="b">Object with the background properties.</param>
        public static void DrawBackground(Graphics g, Size s, IBackground b)
        {
            // Draw back color
            var clBackColor = Color.FromArgb(b.BackColorOpacity, b.BackColor);
            g.FillRectangle(new SolidBrush(clBackColor), 0, 0, s.Width, s.Height);

            // Draw background image
            if (b.BackgroundImage != null)
            {
                Rectangle rDestination = new Rectangle(0, 0, b.BackgroundImage.Width, b.BackgroundImage.Height);
                if (b.BackgroundImageLayout == ImageLayout.None)
                {
                    rDestination.Width = (int)(b.BackgroundImage.Width * b.BackgroundImageZoom);
                    rDestination.Height = (int)(b.BackgroundImage.Height * b.BackgroundImageZoom);
                }
                else if (b.BackgroundImageLayout == ImageLayout.Fill)
                {
                    rDestination.Width = s.Width;
                    rDestination.Height = s.Height;
                }
                else if (b.BackgroundImageLayout == ImageLayout.FillKeepRatio || b.BackgroundImageLayout == ImageLayout.FillKeepRatioAndCrop)
                {
                    float fRatio = (float)b.BackgroundImage.Size.Width / (float)b.BackgroundImage.Size.Height;
                    int iCalculatedWidth = (int)((float)s.Height * fRatio);
                    int iCalculatedHeight = (int)((float)s.Width / fRatio);
                    if ((iCalculatedWidth > s.Width && b.BackgroundImageLayout == ImageLayout.FillKeepRatio)
                        || (iCalculatedHeight > s.Height && b.BackgroundImageLayout == ImageLayout.FillKeepRatioAndCrop))
                    {
                        rDestination.Width = s.Width;
                        rDestination.Height = iCalculatedHeight;
                    }
                    else
                    {
                        rDestination.Width = iCalculatedWidth;
                        rDestination.Height = s.Height;
                    }
                }

                if (b.BackgroundImageAlign == ContentAlignment.TopCenter || b.BackgroundImageAlign == ContentAlignment.MiddleCenter || b.BackgroundImageAlign == ContentAlignment.BottomCenter)
                    rDestination.X = (s.Width - rDestination.Width) / 2;
                if (b.BackgroundImageAlign == ContentAlignment.TopRight || b.BackgroundImageAlign == ContentAlignment.MiddleRight || b.BackgroundImageAlign == ContentAlignment.BottomRight)
                    rDestination.X = s.Width - rDestination.Width;

                if (b.BackgroundImageAlign == ContentAlignment.MiddleLeft || b.BackgroundImageAlign == ContentAlignment.MiddleCenter || b.BackgroundImageAlign == ContentAlignment.MiddleRight)
                    rDestination.Y = (s.Height - rDestination.Height) / 2;
                if (b.BackgroundImageAlign == ContentAlignment.BottomLeft || b.BackgroundImageAlign == ContentAlignment.BottomCenter || b.BackgroundImageAlign == ContentAlignment.BottomRight)
                    rDestination.Y = s.Height - rDestination.Height;

                rDestination.X += b.BackgroundImageOffset.X;
                rDestination.Y += b.BackgroundImageOffset.Y;

                var mtx = new ColorMatrix
                {
                    Matrix33 = (float)b.BackgroundImageOpacity / 255f
                };
                var attr = new ImageAttributes();
                attr.SetColorMatrix(mtx, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                g.DrawImage(b.BackgroundImage, rDestination, 0, 0, b.BackgroundImage.Width, b.BackgroundImage.Height, GraphicsUnit.Pixel, attr);
            }

            // Draw background overlay
            if (b.BackgroundOverlayOpacity > 0)
            {
                var clOverlay = Color.FromArgb(b.BackgroundOverlayOpacity, b.BackgroundOverlayColor);
                g.FillRectangle(new SolidBrush(clOverlay), 0, 0, s.Width, s.Height);
            }
        }

        /// <summary>
        /// Draws borders around the object.
        /// </summary>
        /// <param name="g">Graphics related to the image on which the borders should be drawn.</param>
        /// <param name="s">Size of the object with borders.</param>
        /// <param name="b">Object with border properties.</param>
        public static void DrawBorder(Graphics g, Size s, IBorder b)
        {
            if (b.BorderTop.Width > 0)
            {
                var cl = Color.FromArgb(b.BorderTop.Opacity, b.BorderTop.Color);
                Pen p = new Pen(cl, 1);
                for (int i = 0; i < b.BorderTop.Width; i++)
                    g.DrawLine(p, 0, 0 + i, s.Width - 1, 0 + i);
            }
            if (b.BorderBottom.Width > 0)
            {
                var cl = Color.FromArgb(b.BorderBottom.Opacity, b.BorderBottom.Color);
                Pen p = new Pen(cl, 1);
                for (int i = 0; i < b.BorderBottom.Width; i++)
                    g.DrawLine(p, 0, s.Height - 1 - i, s.Width - 1, s.Height - 1 - i);
            }
            if (b.BorderLeft.Width > 0)
            {
                var cl = Color.FromArgb(b.BorderLeft.Opacity, b.BorderLeft.Color);
                Pen p = new Pen(cl, 1);
                for (int i = 0; i < b.BorderLeft.Width; i++)
                    g.DrawLine(p, 0 + i, 0, 0 + i, s.Height - 1);
            }
            if (b.BorderRight.Width > 0)
            {
                var cl = Color.FromArgb(b.BorderRight.Opacity, b.BorderRight.Color);
                Pen p = new Pen(cl, 1);
                for (int i = 0; i < b.BorderRight.Width; i++)
                    g.DrawLine(p, s.Width - 1 - i, 0, s.Width - 1 - i, s.Height - 1);
            }
        }

        public static void DrawIcon(Graphics g, Size s, IIcon i)
        {
            // Draw background image
            if (i.Icon != null)
            {
                Rectangle rDestination = new Rectangle(0, 0, (int)(i.Icon.Width * i.IconZoom), (int)(i.Icon.Height * i.IconZoom));

                if (i.IconAlign == ContentAlignment.TopCenter || i.IconAlign == ContentAlignment.MiddleCenter || i.IconAlign == ContentAlignment.BottomCenter)
                    rDestination.X = (s.Width - rDestination.Width) / 2;
                if (i.IconAlign == ContentAlignment.TopRight || i.IconAlign == ContentAlignment.MiddleRight || i.IconAlign == ContentAlignment.BottomRight)
                    rDestination.X = s.Width - rDestination.Width;

                if (i.IconAlign == ContentAlignment.MiddleLeft || i.IconAlign == ContentAlignment.MiddleCenter || i.IconAlign == ContentAlignment.MiddleRight)
                    rDestination.Y = (s.Height - rDestination.Height) / 2;
                if (i.IconAlign == ContentAlignment.BottomLeft || i.IconAlign == ContentAlignment.BottomCenter || i.IconAlign == ContentAlignment.BottomRight)
                    rDestination.Y = s.Height - rDestination.Height;

                rDestination.X += i.IconOffset.X;
                rDestination.Y += i.IconOffset.Y;

                var mtx = new ColorMatrix
                {
                    Matrix33 = (float)i.IconOpacity / 255f
                };
                var attr = new ImageAttributes();
                attr.SetColorMatrix(mtx, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                g.DrawImage(i.Icon, rDestination, 0, 0, i.Icon.Width, i.Icon.Height, GraphicsUnit.Pixel, attr);
            }
        }

        /// <summary>
        /// Draws a title in the object.
        /// </summary>
        /// <param name="g">Graphics related to the image on which the title should be drawn.</param>
        /// <param name="s">Size of the object.</param>
        /// <param name="t">Object with the title properties.</param>
        public static void DrawTitle(Graphics g, Size s, ITitle t)
        {
            if (t.TitleBarHeight > 0)
            {
                // Draw title bar
                var clTitleBar = Color.FromArgb(t.TitleBarOpacity, t.TitleBarColor);
                var brTitleBar = new SolidBrush(clTitleBar);

                var rTitleBar = new Rectangle(0, 0, s.Width, t.TitleBarHeight);
                if (t.TitleRotation == Rotation.Clockwise90 || t.TitleRotation == Rotation.Clockwise270)
                {
                    rTitleBar.Width = t.TitleBarHeight;
                    rTitleBar.Height = s.Height;
                }

                if (t.TitleAlign == ContentAlignment.TopCenter || t.TitleAlign == ContentAlignment.MiddleCenter || t.TitleAlign == ContentAlignment.BottomCenter)
                    rTitleBar.X = (s.Width - rTitleBar.Width) / 2;
                else if (t.TitleAlign == ContentAlignment.TopRight || t.TitleAlign == ContentAlignment.MiddleRight || t.TitleAlign == ContentAlignment.BottomRight)
                    rTitleBar.X = s.Width - rTitleBar.Width;

                if (t.TitleAlign == ContentAlignment.MiddleLeft || t.TitleAlign == ContentAlignment.MiddleCenter || t.TitleAlign == ContentAlignment.MiddleRight)
                    rTitleBar.Y = (s.Height - rTitleBar.Height) / 2;
                else if (t.TitleAlign == ContentAlignment.BottomLeft || t.TitleAlign == ContentAlignment.BottomCenter || t.TitleAlign == ContentAlignment.BottomRight)
                    rTitleBar.Y = s.Height - rTitleBar.Height;

                g.FillRectangle(brTitleBar, rTitleBar);
            }

            if (!string.IsNullOrEmpty(t.Title))
            {
                // Calculate title coordinates
                var szTitle = g.MeasureString(t.Title, t.TitleFont);

                // Draw title on temporary picture
                var bT = new Bitmap((int)szTitle.Width, (int)szTitle.Height);
                var gT = Graphics.FromImage(bT);
                gT.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                gT.FillRectangle(Brushes.Transparent, 0, 0, szTitle.Width, szTitle.Height);
                gT.DrawString(t.Title, t.TitleFont, new SolidBrush(t.TitleColor), 0, 0);

                // Rotate temporary picture if applicable
                if (t.TitleRotation == Rotation.Clockwise90)
                    ((Bitmap)bT).RotateFlip(RotateFlipType.Rotate90FlipNone);
                else if (t.TitleRotation == Rotation.Clockwise180)
                    ((Bitmap)bT).RotateFlip(RotateFlipType.Rotate180FlipNone);
                else if (t.TitleRotation == Rotation.Clockwise270)
                    ((Bitmap)bT).RotateFlip(RotateFlipType.Rotate270FlipNone);

                int iTitleY = t.TitleOffset.Y;
                int iTitleX = t.TitleOffset.X;

                if (t.TitleAlign == ContentAlignment.TopRight || t.TitleAlign == ContentAlignment.MiddleRight || t.TitleAlign == ContentAlignment.BottomRight)
                    iTitleX = s.Width - bT.Size.Width + t.TitleOffset.X;
                else if (t.TitleAlign == ContentAlignment.TopCenter || t.TitleAlign == ContentAlignment.MiddleCenter || t.TitleAlign == ContentAlignment.BottomCenter)
                    iTitleX = (s.Width - bT.Size.Width) / 2 + t.TitleOffset.X;

                if (t.TitleAlign == ContentAlignment.BottomLeft || t.TitleAlign == ContentAlignment.BottomCenter || t.TitleAlign == ContentAlignment.BottomRight)
                    iTitleY = s.Height - bT.Size.Height + t.TitleOffset.Y;
                else if (t.TitleAlign == ContentAlignment.MiddleLeft || t.TitleAlign == ContentAlignment.MiddleCenter || t.TitleAlign == ContentAlignment.MiddleRight)
                    iTitleY = (s.Height - bT.Size.Height) / 2 + t.TitleOffset.Y;

                g.DrawImage(bT, iTitleX, iTitleY);
            }
        }
    }
}
