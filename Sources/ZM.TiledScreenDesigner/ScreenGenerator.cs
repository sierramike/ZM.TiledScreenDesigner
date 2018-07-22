using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ZM.TiledScreenDesigner
{
    public class ScreenGenerator
    {
        public ScreenGenerator()
        {

        }

        public virtual Screen Open(string data)
        {
            return JsonConvert.DeserializeObject<Screen>(data);
        }

        public virtual string Save(Screen s)
        {
            return JsonConvert.SerializeObject(s, Formatting.Indented);
        }

        public virtual Bitmap GenerateScreen(Screen s)
        {
            s.Calculate();

            // Create picture
            var b = new Bitmap(s.Size.Width, s.Size.Height);
            var gr = Graphics.FromImage(b);

            // Draw background
            Drawer.DrawBackground(gr, s.Size, s);

            // Draw tilte
            Drawer.DrawTitle(gr, s.Size, s);

            // Header Region
            int iHeaderHeight = 0;
            int iHeaderWidth = 0;
            int iHeaderX = 0;
            int iHeaderY = 0;
            int iContentHeight = 0;
            int iContentWidth = 0;
            int iContentX = 0;
            int iContentY = 0;

            switch (s.HeaderLocation)
            {
                case Position.Top:
                    iHeaderHeight = s.HeaderThickness;
                    iHeaderWidth = s.Size.Width - s.HeaderMargins.Left - s.HeaderMargins.Right;
                    iHeaderX = s.HeaderMargins.Left;
                    iHeaderY = s.HeaderMargins.Top;
                    iContentHeight = s.Size.Height - s.HeaderMargins.Top - s.HeaderThickness - s.HeaderMargins.Bottom - s.ContentMargins.Top - s.ContentMargins.Bottom;
                    iContentWidth = s.Size.Width - s.ContentMargins.Left - s.ContentMargins.Right;
                    iContentX = s.ContentMargins.Left;
                    iContentY = s.HeaderMargins.Top + s.HeaderThickness + s.HeaderMargins.Bottom + s.ContentMargins.Top;
                    break;
                case Position.Left:
                    iHeaderHeight = s.Size.Height - s.HeaderMargins.Top - s.HeaderMargins.Bottom;
                    iHeaderWidth = s.HeaderThickness;
                    iHeaderX = s.HeaderMargins.Left;
                    iHeaderY = s.HeaderMargins.Top;
                    iContentHeight = s.Size.Height;
                    iContentWidth = s.Size.Width - s.HeaderMargins.Left - s.HeaderThickness - s.HeaderMargins.Right;
                    iContentX = s.HeaderMargins.Left + s.HeaderThickness + s.HeaderMargins.Right + s.ContentMargins.Left;
                    iContentY = s.ContentMargins.Top;
                    break;
                case Position.Right:
                    iHeaderHeight = s.Size.Height - s.HeaderMargins.Top - s.HeaderMargins.Bottom;
                    iHeaderWidth = s.HeaderThickness;
                    iHeaderX = s.Size.Width - s.HeaderMargins.Right - s.HeaderThickness;
                    iHeaderY = s.HeaderMargins.Top;
                    iContentHeight = s.Size.Height;
                    iContentWidth = s.Size.Width - s.HeaderMargins.Left - s.HeaderThickness - s.HeaderMargins.Right;
                    iContentX = s.ContentMargins.Left;
                    iContentY = s.ContentMargins.Top;
                    break;
                case Position.Bottom:
                    iHeaderHeight = s.HeaderThickness;
                    iHeaderWidth = s.Size.Width - s.HeaderMargins.Left - s.HeaderMargins.Right;
                    iHeaderX = s.HeaderMargins.Left;
                    iHeaderY = s.Size.Height - s.HeaderMargins.Bottom - s.HeaderThickness;
                    iContentHeight = s.Size.Height - s.HeaderMargins.Top - s.HeaderThickness - s.HeaderMargins.Bottom - s.ContentMargins.Top - s.ContentMargins.Bottom;
                    iContentWidth = s.Size.Width - s.ContentMargins.Left - s.ContentMargins.Right;
                    iContentX = s.ContentMargins.Left;
                    iContentY = s.ContentMargins.Top;
                    break;
            }

            if (iHeaderWidth > 0 && iHeaderHeight > 0)
            {
                var bHeader = new Bitmap(iHeaderWidth, iHeaderHeight);
                var grHeader = Graphics.FromImage(bHeader);

                var imgHeader = GenerateColumnGroup(s, s.Header);

                int iHeaderSourceX = 0;
                int iHeaderSourceY = 0;

                if (s.HeaderContentAlign == System.Drawing.ContentAlignment.TopLeft || s.HeaderContentAlign == System.Drawing.ContentAlignment.MiddleLeft || s.HeaderContentAlign == System.Drawing.ContentAlignment.BottomLeft)
                    iHeaderSourceX = 0;
                if (s.HeaderContentAlign == System.Drawing.ContentAlignment.TopCenter || s.HeaderContentAlign == System.Drawing.ContentAlignment.MiddleCenter || s.HeaderContentAlign == System.Drawing.ContentAlignment.BottomCenter)
                    iHeaderSourceX = (imgHeader.Size.Width - iHeaderWidth) / 2;
                if (s.HeaderContentAlign == System.Drawing.ContentAlignment.TopRight || s.HeaderContentAlign == System.Drawing.ContentAlignment.MiddleRight || s.HeaderContentAlign == System.Drawing.ContentAlignment.BottomRight)
                    iHeaderSourceX = imgHeader.Size.Width - iHeaderWidth;

                if (s.HeaderContentAlign == System.Drawing.ContentAlignment.TopLeft || s.HeaderContentAlign == System.Drawing.ContentAlignment.TopCenter || s.HeaderContentAlign == System.Drawing.ContentAlignment.TopRight)
                    iHeaderSourceY = 0;
                if (s.HeaderContentAlign == System.Drawing.ContentAlignment.MiddleLeft || s.HeaderContentAlign == System.Drawing.ContentAlignment.MiddleCenter || s.HeaderContentAlign == System.Drawing.ContentAlignment.MiddleRight)
                    iHeaderSourceY = (imgHeader.Size.Height - iHeaderHeight) / 2;
                if (s.HeaderContentAlign == System.Drawing.ContentAlignment.BottomLeft || s.HeaderContentAlign == System.Drawing.ContentAlignment.BottomCenter || s.HeaderContentAlign == System.Drawing.ContentAlignment.BottomRight)
                    iHeaderSourceY = imgHeader.Size.Height - iHeaderHeight;

                //gr.DrawImage(imgHeader, new Rectangle(iHeaderX, iHeaderY, iHeaderWidth, iHeaderHeight), new Rectangle(new Point(), imgHeader.Size), GraphicsUnit.Pixel);
                gr.DrawImage(imgHeader, new Rectangle(iHeaderX, iHeaderY, iHeaderWidth, iHeaderHeight), new Rectangle(new Point(iHeaderSourceX, iHeaderSourceY), new Size(iHeaderWidth, iHeaderHeight)), GraphicsUnit.Pixel);
            }

            int iTop = s.HeaderMargins.Top + s.HeaderThickness + s.HeaderMargins.Bottom + s.ContentMargins.Top;

            //// Draw title tile
            //if (s.TitleTileHeight > 0)
            //{
            //    var bT = new Bitmap(s.GetRegionWidth(), s.TitleTileHeight);
            //    var grT = Graphics.FromImage(bT);

            //    // Draw tile
            //    var clTitleTile = Color.FromArgb(s.TitleTileOpacity, s.TitleTileColor);
            //    grT.FillRectangle(new SolidBrush(clTitleTile), 0, 0, s.GetRegionWidth(), s.TitleTileHeight);

            //    // Draw title
            //    Drawer.DrawTitle(grT, bT.Size, s);

            //    gr.DrawImage(bT, s.Margins.Left, iTop);
            //    iTop += s.TitleTileHeight + s.VerticalSpacing;
            //}

            // Content region
            //var bRegion = new Bitmap(s.GetGroupColumnWidth(), s.Size.Height - iTop - s.Margins.Bottom);
            var iRenderingHeight = s.ColumnGroups.Max(x => x.GetHeight());
            var bRegion = new Bitmap(s.GetGroupColumnWidth(), iRenderingHeight);
            //var bRegion = new Bitmap(s.GetGroupColumnWidth(), iContentHeight);
            var grRegion = Graphics.FromImage(bRegion);

            // Draw column groups on temporary region
            int iLeft = 0;
            foreach (var cg in s.ColumnGroups)
            {
                grRegion.DrawImage(GenerateColumnGroup(s, cg), iLeft, 0);
                iLeft += cg.RenderingSize.Width + s.HorizontalColumnGroupSpacing;
            }

            // Draw region on screen
            //int iScreenRegionWidth = s.GetRegionWidth();
            //int iScreenRegionHeight = s.Size.Height - iTop - s.Margins.Bottom;
            int iX = 0;
            int iY = 0;

            if (s.ContentAlign == System.Drawing.ContentAlignment.TopLeft || s.ContentAlign == System.Drawing.ContentAlignment.MiddleLeft || s.ContentAlign == System.Drawing.ContentAlignment.BottomLeft)
                iX = 0;
            if (s.ContentAlign == System.Drawing.ContentAlignment.TopCenter || s.ContentAlign == System.Drawing.ContentAlignment.MiddleCenter || s.ContentAlign == System.Drawing.ContentAlignment.BottomCenter)
                iX = (bRegion.Size.Width - iContentWidth) / 2;
            if (s.ContentAlign == System.Drawing.ContentAlignment.TopRight || s.ContentAlign == System.Drawing.ContentAlignment.MiddleRight || s.ContentAlign == System.Drawing.ContentAlignment.BottomRight)
                iX = bRegion.Size.Width - iContentWidth;

            if (s.ContentAlign == System.Drawing.ContentAlignment.TopLeft || s.ContentAlign == System.Drawing.ContentAlignment.TopCenter || s.ContentAlign == System.Drawing.ContentAlignment.TopRight)
                iY = 0;
            if (s.ContentAlign == System.Drawing.ContentAlignment.MiddleLeft || s.ContentAlign == System.Drawing.ContentAlignment.MiddleCenter || s.ContentAlign == System.Drawing.ContentAlignment.MiddleRight)
                iY = (bRegion.Size.Height - iContentHeight) / 2;
            if (s.ContentAlign == System.Drawing.ContentAlignment.BottomLeft || s.ContentAlign == System.Drawing.ContentAlignment.BottomCenter || s.ContentAlign == System.Drawing.ContentAlignment.BottomRight)
                iY = bRegion.Size.Height - iContentHeight;
            //iLeft = 0;
            //if (s.HorizontalAlign == HorizontalAlign.Right)
            //    iLeft = bRegion.Size.Width - s.GetRegionWidth();
            //else if (s.HorizontalAlign == HorizontalAlign.Center)
            //    iLeft = (bRegion.Size.Width - s.GetRegionWidth()) / 2;
            //gr.DrawImage(bRegion, new Rectangle(s.Margins.Left, iTop, iScreenRegionWidth, iScreenRegionHeight), new Rectangle(iLeft, 0, iScreenRegionWidth, iScreenRegionHeight), GraphicsUnit.Pixel);
            gr.DrawImage(bRegion, new Rectangle(iContentX, iContentY, iContentWidth, iContentHeight), new Rectangle(iX, iY, iContentWidth, iContentHeight), GraphicsUnit.Pixel);

            return b;
        }

        protected virtual Bitmap GenerateColumnGroup(Screen s, ColumnGroup cg)
        {
            var b = new Bitmap(cg.RenderingSize.Width, cg.RenderingSize.Height);

            var gr = Graphics.FromImage(b);

            Drawer.DrawBackground(gr, cg.RenderingSize, cg);

            int iTop = cg.Padding.Top;
            foreach (var g in cg.Groups)
            {
                // Draw title
                var bT = new Bitmap(cg.RenderingSize.Width, Math.Max(1, cg.VerticalGroupSpacing));
                var grT = Graphics.FromImage(bT);

                grT.FillRectangle(Brushes.Transparent, 0, 0, bT.Width, bT.Height);
                Drawer.DrawTitle(grT, bT.Size, g);

                gr.DrawImage(bT, cg.Padding.Left, iTop);
                iTop += cg.VerticalGroupSpacing;

                // Draw group
                gr.DrawImage(GenerateGroup(s, cg, g), cg.Padding.Left, iTop);

                // Draw border
                Drawer.DrawBorder(gr, cg.RenderingSize, cg);

                iTop += g.RenderingSize.Height;
            }

            return b;
        }

        protected virtual Bitmap GenerateGroup(Screen s, ColumnGroup cg, Group g)
        {
            g.Populate(cg);

            var b = new Bitmap(g.RenderingSize.Width, g.RenderingSize.Height);

            var gr = Graphics.FromImage(b);

            Drawer.DrawBackground(gr, g.RenderingSize, g);

            //gr.FillRectangle(Brushes.Transparent, 0, 0, g.RenderingSize.Width, g.RenderingSize.Height);

            for (int iR = 0; iR < g.Cells.GetLength(0); iR++)
            {
                for (int iC = 0; iC < g.Cells.GetLength(1); iC++)
                {
                    ScreenCell cell = g.Cells[iR, iC];
                    if (cell != null)
                        if (cell.Anchor)
                        {
                            int iX = iC * (cg.TileSize + cg.HorizontalSpacing);
                            int iY = iR * (cg.TileSize + cg.VerticalSpacing);
                            gr.DrawImage(GenerateTile(s, cg, g, cell.Tile), iX, iY);
                        }
                }
            }

            // Draw border
            Drawer.DrawBorder(gr, b.Size, g);

            return b;
        }

        protected virtual Bitmap GenerateTile(Screen s, ColumnGroup cg, Group g, Tile t)
        {
            var szTile = new Size()
            {
                Width = (cg.TileSize + cg.HorizontalSpacing) * t.ColSpan - cg.HorizontalSpacing,
                Height = (cg.TileSize + cg.VerticalSpacing) * t.RowSpan - cg.VerticalSpacing
            };

            var b = new Bitmap(szTile.Width, szTile.Height);

            var gr = Graphics.FromImage(b);

            //var clT = Color.FromArgb(128, Color.Red);
            //var pgb = new LinearGradientBrush(new Point(0, 0), new Point(iTileWidth, iTileHeight), t.BackColor, clT);
            //gr.FillRectangle(pgb, 0, 0, iTileWidth, iTileHeight);

            // Draw background
            Drawer.DrawBackground(gr, szTile, t);

            // Draw title
            Drawer.DrawTitle(gr, szTile, t);

            // Draw icon
            Drawer.DrawIcon(gr, szTile, t);

            // Draw border
            Drawer.DrawBorder(gr, b.Size, t);

            return b;
        }
    }
}
