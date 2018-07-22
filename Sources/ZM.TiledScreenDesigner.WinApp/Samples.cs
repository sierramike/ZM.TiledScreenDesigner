using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ZM.TiledScreenDesigner.WinApp
{
    public static class Samples
    {
        public static Screen Blank()
        {
            var s = new Screen()
            {
                BackColor = Color.DarkBlue,
                Size = new Size() { Width = 1280, Height = 720 },
                HorizontalColumnGroupSpacing = 40,
                TitleColor = Color.Black,
                TitleFont = new Font("Arial", 20, FontStyle.Regular),
                HeaderContentAlign = System.Drawing.ContentAlignment.MiddleCenter,
                HeaderLocation = Position.Top,
                HeaderMargins = new Margins() { Bottom = 5, Top = 5, Left = 5, Right = 5 },
                HeaderThickness = 80,
                ContentMargins = new Margins() { Bottom = 20, Top = 20, Left = 20, Right = 20 },
                ContentAlign = System.Drawing.ContentAlignment.TopCenter
            };

            var ftGroup = new Font("Arial", 12, FontStyle.Regular);
            var ftTile = new Font("Arial", 10, FontStyle.Regular);

            var titleTile = new Tile()
            {
                Title = "Title",
                BackColor = Color.White,
                TitleFont = new Font("Arial", 20, FontStyle.Regular),
                TitleAlign = System.Drawing.ContentAlignment.MiddleCenter,
                TitleColor = Color.Black,
                ColSpan = 21
            };

            s.Header.BorderBottom.Width = 2;
            s.Header.Padding = new Margins(5, 5, 5, 5);
            s.Header.TileSize = 60;
            s.Header.HorizontalSpacing = 0;
            s.Header.WidthInColumns = 21;
            s.Header.Groups.Add(new Group());
            s.Header.Groups[0].Tiles.Add(titleTile);

            var cg1 = new ColumnGroup() { WidthInColumns = 3, HorizontalSpacing = 20, VerticalSpacing = 20, TileSize = 120, VerticalGroupSpacing = 35 };
            var g1 = new Group() { Title = "Group", TitleFont = ftGroup };
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan });
            cg1.Groups.Add(g1);
            s.ColumnGroups.Add(cg1);

            return s;
        }

        public static Screen MultiColumnGroups()
        {
            var s = new Screen()
            {
                BackColor = Color.DarkBlue,
                Size = new Size() { Width = 1280, Height = 720 },
                HorizontalColumnGroupSpacing = 40,
                Title = "Sample",
                TitleColor = Color.Black,
                TitleFont = new Font("Arial", 20, FontStyle.Regular),
                ContentMargins = new System.Drawing.Printing.Margins() { Bottom = 20, Top = 40, Left = 20, Right = 20 },
                ContentAlign = System.Drawing.ContentAlignment.TopCenter
            };

            var ftGroup = new Font("Arial", 12, FontStyle.Regular);
            var ftTile = new Font("Arial", 10, FontStyle.Regular);

            var cg1 = new ColumnGroup() { WidthInColumns = 3, HorizontalSpacing = 20, VerticalSpacing = 20, TileSize = 120, VerticalGroupSpacing = 35 };
            cg1.Groups.Add(new Group() { Title = "Group 1A", TitleFont = ftGroup, TitleOffset = new Point(0, 15) });
            cg1.Groups.Add(new Group() { Title = "Group 1B", TitleFont = ftGroup, TitleOffset = new Point(0, 15) });
            cg1.Groups[1].BorderTop.Width = 2;
            cg1.Groups[1].BorderLeft.Width = 2;
            cg1.Groups[1].BorderRight.Width = 2;
            cg1.Groups[1].BorderBottom.Width = 2;
            cg1.Groups.Add(new Group() { Title = "Group 1C", TitleFont = ftGroup, TitleOffset = new Point(0, 15) });
            s.ColumnGroups.Add(cg1);

            var cg2 = new ColumnGroup() { WidthInColumns = 3, HorizontalSpacing = 20, VerticalSpacing = 20, TileSize = 120, VerticalGroupSpacing = 35 };
            cg2.Groups.Add(new Group() { Title = "Group 2A", TitleFont = ftGroup, TitleOffset = new Point(0, 15) });
            cg1.Groups[0].BorderTop.Width = 2;
            cg1.Groups[0].BorderLeft.Width = 2;
            cg1.Groups[0].BorderRight.Width = 2;
            cg1.Groups[0].BorderBottom.Width = 2;
            cg2.Groups.Add(new Group() { Title = "Group 2B", TitleFont = ftGroup, TitleOffset = new Point(0, 15) });
            cg2.Groups.Add(new Group() { Title = "Group 2C", TitleFont = ftGroup, TitleOffset = new Point(0, 15) });
            cg1.Groups[2].BorderTop.Width = 2;
            cg1.Groups[2].BorderLeft.Width = 2;
            cg1.Groups[2].BorderRight.Width = 2;
            cg1.Groups[2].BorderBottom.Width = 2;
            s.ColumnGroups.Add(cg2);

            var cg3 = new ColumnGroup() { WidthInColumns = 3, HorizontalSpacing = 20, VerticalSpacing = 20, TileSize = 120, VerticalGroupSpacing = 35 };
            cg3.BorderTop.Width = 2;
            cg3.BorderLeft.Width = 2;
            cg3.BorderRight.Width = 2;
            cg3.BorderBottom.Width = 2;
            cg3.Groups.Add(new Group() { Title = "Group 3A", TitleFont = ftGroup, TitleOffset = new Point(0, 15) });
            cg3.Groups.Add(new Group() { Title = "Group 3B", TitleFont = ftGroup, TitleOffset = new Point(0, 15) });
            cg3.Groups.Add(new Group() { Title = "Group 3C", TitleFont = ftGroup, TitleOffset = new Point(0, 15) });
            s.ColumnGroups.Add(cg3);

            return s;
        }

        public static Screen TwoColumns()
        {
            var s = new Screen()
            {
                BackColor = Color.DarkBlue,
                Size = new Size() { Width = 1280, Height = 720 },
                HorizontalColumnGroupSpacing = 34,
                Title = "Sample",
                TitleAlign = System.Drawing.ContentAlignment.TopCenter,
                TitleColor = Color.White,
                TitleFont = new Font("Arial", 20, FontStyle.Regular),
                TitleOffset = new Point(0, 10),
                ContentMargins = new System.Drawing.Printing.Margins() { Bottom = 20, Top = 40, Left = 20, Right = 20 },
                ContentAlign = System.Drawing.ContentAlignment.TopCenter
            };

            var ftGroup = new Font("Arial", 12, FontStyle.Regular);
            var ftTile = new Font("Arial", 10, FontStyle.Regular);

            var cg1 = new ColumnGroup() { WidthInColumns = 3, HorizontalSpacing = 18, VerticalSpacing = 20, TileSize = 120, VerticalGroupSpacing = 35 };
            var g1 = new Group() { Title = "Group 1", TitleFont = ftGroup };
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan, ColSpan = 2, RowSpan = 2, Title = "Big size", TitleFont = ftTile, TitleColor = Color.White });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkGreen, RowSpan = 2 });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan, ColSpan = 2 });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan, ColSpan = 2 });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan, ColSpan = 3, Title = "Wide cell", TitleFont = ftTile, TitleColor = Color.White, TitleAlign = System.Drawing.ContentAlignment.TopLeft });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan });
            g1.Tiles.Add(new Tile() { BackColor = Color.Yellow, ColSpan = 2 });
            cg1.Groups.Add(g1);
            s.ColumnGroups.Add(cg1);

            var cg2 = new ColumnGroup() { WidthInColumns = 6, HorizontalSpacing = 18, VerticalSpacing = 20, TileSize = 120, VerticalGroupSpacing = 35 };
            var g2 = new Group() { Title = "Group 2", TitleFont = ftGroup };
            g2.BorderTop.Width = 2;
            g2.BorderLeft.Width = 2;
            g2.BorderRight.Width = 2;
            g2.BorderBottom.Width = 2;
            g2.Tiles.Add(new Tile() { BackColor = Color.DarkCyan, Title = "Tile", TitleFont = ftTile, TitleColor = Color.White });
            g2.Tiles.Add(new Tile() { BackColor = Color.DarkCyan, ColSpan = 2, RowSpan = 2 });
            g2.Tiles.Add(new Tile() { BackColor = Color.DarkCyan });
            g2.Tiles.Add(new Tile() { BackColor = Color.DarkGreen, ColSpan = 2, Title = "Double tile", TitleFont = ftTile, TitleColor = Color.White });
            g2.Tiles.Add(new Tile() { BackColor = Color.DarkCyan });
            g2.Tiles.Add(new Tile() { BackColor = Color.DarkCyan, RowSpan = 2 });
            g2.Tiles.Add(new Tile() { BackColor = Color.DarkCyan, ColSpan = 2 });
            g2.Tiles.Add(new Tile() { BackColor = Color.DarkCyan, ColSpan = 3 });
            g2.Tiles.Add(new Tile() { BackColor = Color.DarkCyan });
            g2.Tiles.Add(new Tile() { BackColor = Color.DarkCyan });
            g2.Tiles.Add(new Tile() { BackColor = Color.Yellow, ColSpan = 2 });
            g2.Tiles.Add(new Tile() { BackColor = Color.Yellow, ColSpan = 2 });
            g2.Tiles.Add(new Tile() { BackColor = Color.Yellow, ColSpan = 2 });
            g2.Tiles.Add(new Tile() { BackColor = Color.DarkMagenta, ColSpan = 6, RowSpan = 2, Title = "Extra large tile", TitleFont = ftTile, TitleColor = Color.White });
            cg2.Groups.Add(g2);
            s.ColumnGroups.Add(cg2);

            return s;
        }

        public static Screen FullScreen()
        {
            var s = new Screen()
            {
                BackColor = Color.DarkBlue,
                Size = new Size() { Width = 1280, Height = 720 },
                HorizontalColumnGroupSpacing = 40,
                Title = "Sample",
                TitleColor = Color.Black,
                TitleFont = new Font("Arial", 20, FontStyle.Regular),
                HeaderThickness = 60,
                HeaderMargins = new Margins(20, 20, 20, 0),
                HeaderContentAlign = System.Drawing.ContentAlignment.MiddleCenter,
                ContentMargins = new Margins(20, 20, 20, 20),
                ContentAlign = System.Drawing.ContentAlignment.TopCenter
            };

            var ftGroup = new Font("Arial", 12, FontStyle.Regular);
            var ftTile = new Font("Arial", 10, FontStyle.Regular);

            var titleTile = new Tile()
            {
                Title = "Sample",
                BackColor = Color.White,
                TitleFont = new Font("Arial", 20, FontStyle.Regular),
                TitleAlign = System.Drawing.ContentAlignment.MiddleCenter,
                TitleColor = Color.Black,
                ColSpan = 21
            };

            s.Header.TileSize = 60;
            s.Header.HorizontalSpacing = 0;
            s.Header.WidthInColumns = 21;
            s.Header.Groups.Add(new Group());
            s.Header.Groups[0].Tiles.Add(titleTile);

            var cg1 = new ColumnGroup() { WidthInColumns = 10, HorizontalSpacing = 20, VerticalSpacing = 20, TileSize = 106 };
            var g1 = new Group() { };
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan });
            g1.Tiles.Add(new Tile() { BackColor = Color.Gray, ColSpan = 4, RowSpan = 4, Title = "Big size", TitleFont = ftTile, TitleColor = Color.White, TitleAlign = System.Drawing.ContentAlignment.TopCenter });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkGreen, RowSpan = 2 });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkGreen, RowSpan = 2 });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkGreen, RowSpan = 2 });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan, ColSpan = 3 });
            g1.Tiles.Add(new Tile() { BackColor = Color.Yellow, ColSpan = 3, RowSpan = 2 });
            g1.Tiles.Add(new Tile() { BackColor = Color.Yellow, ColSpan = 3, RowSpan = 2 });
            g1.Tiles.Add(new Tile() { BackColor = Color.White });
            g1.Tiles.Add(new Tile() { BackColor = Color.White });
            g1.Tiles.Add(new Tile() { BackColor = Color.White });
            g1.Tiles.Add(new Tile() { BackColor = Color.White, ColSpan = 7 });
            cg1.Groups.Add(g1);
            s.ColumnGroups.Add(cg1);

            return s;
        }

        public static Screen NoTitle()
        {
            var s = new Screen()
            {
                BackColor = Color.DarkBlue,
                Size = new Size() { Width = 1280, Height = 720 },
                HorizontalColumnGroupSpacing = 40,
                TitleColor = Color.Black,
                TitleFont = new Font("Arial", 20, FontStyle.Regular),
                ContentMargins = new System.Drawing.Printing.Margins() { Bottom = 20, Top = 20, Left = 20, Right = 20 },
                ContentAlign = System.Drawing.ContentAlignment.TopCenter
            };

            var ftGroup = new Font("Arial", 12, FontStyle.Regular);
            var ftTile = new Font("Arial", 10, FontStyle.Regular);

            var cg1 = new ColumnGroup() { WidthInColumns = 9, HorizontalSpacing = 20, VerticalSpacing = 20, TileSize = 120 };
            var g1 = new Group() { };
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan });
            g1.Tiles.Add(new Tile() { BackColor = Color.Gray, ColSpan = 3, RowSpan = 4, Title = "Big size", TitleFont = ftTile, TitleColor = Color.White, TitleAlign = System.Drawing.ContentAlignment.TopCenter });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkGreen, RowSpan = 2 });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkGreen, RowSpan = 2 });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkGreen, RowSpan = 2 });
            g1.Tiles.Add(new Tile() { BackColor = Color.DarkCyan, ColSpan = 3 });
            g1.Tiles.Add(new Tile() { BackColor = Color.Yellow, ColSpan = 3, RowSpan = 2 });
            g1.Tiles.Add(new Tile() { BackColor = Color.Yellow, ColSpan = 3, RowSpan = 2 });
            g1.Tiles.Add(new Tile() { BackColor = Color.White });
            g1.Tiles.Add(new Tile() { BackColor = Color.White });
            g1.Tiles.Add(new Tile() { BackColor = Color.White });
            g1.Tiles.Add(new Tile() { BackColor = Color.White, ColSpan = 6 });
            cg1.Groups.Add(g1);
            s.ColumnGroups.Add(cg1);

            return s;
        }
    }
}
