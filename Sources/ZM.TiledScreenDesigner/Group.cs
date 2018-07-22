using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ZM.TiledScreenDesigner
{
    public class Group : IBackground, IBorder, IChildrens, ITitle
    {
        public Group()
        {
            BackColor = Color.Transparent;
            BackColorOpacity = 0;
            BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            BackgroundImageLayout = ImageLayout.None;
            BackgroundImageOffset = new Point();
            BackgroundImageOpacity = 255;
            BackgroundImageZoom = 1;
            BackgroundOverlayColor = Color.Transparent;
            BackgroundOverlayOpacity = 0;

            TitleColor = Color.White;
            TitleAlign = System.Drawing.ContentAlignment.MiddleLeft;
            TitleOffset = new Point();
            TitleFont = new Font("Arial", 12, FontStyle.Regular);
            TitleBarColor = Color.Black;
            TitleBarHeight = 0;
            TitleBarOpacity = 0;

            BorderTop = new Border();
            BorderLeft = new Border();
            BorderRight = new Border();
            BorderBottom = new Border();

            Tiles = new List<Tile>();
        }

        [Category("Background")]
        [Description("Background color")]
        public virtual Color BackColor { get; set; }

        [Category("Background")]
        [Description("Background color opacity (0=transparent, 255=solid)")]
        public virtual byte BackColorOpacity { get; set; }

        [Category("Background")]
        [Description("Background image")]
        [DefaultValue(typeof(Image), null)]
        [JsonConverter(typeof(ImageJsonConverter))]
        public virtual Image BackgroundImage { get; set; }

        [Category("Background")]
        [Description("Background image")]
        public virtual System.Drawing.ContentAlignment BackgroundImageAlign { get; set; }

        [Category("Background")]
        [Description("Background image layout (None=Original size, Fill=Fill the whole region without keeping image ratio, FillKeepRatio=Zoom keeping orignial ratio so that whole image is inside the region, FillKeepRatioAndCrop=Zoom keeping original aspect ratio filling the region and cropping image if necessary)")]
        public virtual ImageLayout BackgroundImageLayout { get; set; }

        [Category("Background")]
        [Description("Background image offset")]
        public Point BackgroundImageOffset { get; set; }

        [Category("Background")]
        [Description("Background image opacity (0=transparent, 255=solid)")]
        public virtual byte BackgroundImageOpacity { get; set; }

        [Category("Background")]
        [Description("Background zoom factor (Decimal value, 1=No zoom, set BackgroundImageLayout to none to use zoom)")]
        public float BackgroundImageZoom { get; set; }

        [Category("Background")]
        [Description("Background overlay color")]
        public virtual Color BackgroundOverlayColor { get; set; }

        [Category("Background")]
        [Description("Background overlay opacity (0=transparent, 255=solid)")]
        public virtual byte BackgroundOverlayOpacity { get; set; }


        [Category("Title")]
        [Description("Title of the group")]
        public virtual string Title { get; set; }

        [Category("Title")]
        [Description("Title horizontal alignment")]
        public virtual System.Drawing.ContentAlignment TitleAlign { get; set; }

        [Category("Title")]
        [Description("Color of the title bar")]
        public virtual Color TitleBarColor { get; set; }

        [Category("Title")]
        [Description("Height of the title bar")]
        public virtual int TitleBarHeight { get; set; }

        [Category("Title")]
        [Description("Title bar opacity (0=transparent, 255=solid)")]
        public virtual byte TitleBarOpacity { get; set; }

        [Category("Title")]
        [Description("Color of the title text")]
        public virtual Color TitleColor { get; set; }

        [Category("Title")]
        [Description("Font of the title")]
        public virtual Font TitleFont { get; set; }

        [Category("Title")]
        [Description("Title offset")]
        public virtual Point TitleOffset { get; set; }

        [Category("Title")]
        [Description("Title rotation")]
        public virtual Rotation TitleRotation { get; set; }


        [Category("Borders")]
        [Description("Top border")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Border BorderTop { get; set; }

        [Category("Borders")]
        [Description("Left border")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Border BorderLeft { get; set; }

        [Category("Borders")]
        [Description("Right border")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Border BorderRight { get; set; }

        [Category("Borders")]
        [Description("Bottom border")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Border BorderBottom { get; set; }


        [Browsable(false)]
        public virtual List<Tile> Tiles { get; private set; }

        [Browsable(false)]
        [JsonIgnore]
        public virtual Size RenderingSize { get; set; }


        [Browsable(false)]
        [JsonIgnore]
        public virtual ScreenCell[,] Cells { get; private set; }


        public virtual void Calculate(ColumnGroup cg) //, int TileSize, int VerticalSpacing)
        {
            Populate(cg);

            // Count used rows
            int i = Cells.GetLength(0);
            while (i > 0)
            {
                bool bUsed = false;
                for (int j = 0; j < Cells.GetLength(1); j++)
                    if (Cells[i - 1, j] != null)
                        bUsed = true;

                if (!bUsed)
                    i--;
                else
                    break;
            }

            int iHeight = 1;
            if (i > 0)
                iHeight = (cg.TileSize + cg.VerticalSpacing) * i - cg.VerticalSpacing;

            RenderingSize = new Size(cg.RenderingSize.Width, iHeight);
        }

        protected virtual bool EmptyCell(ScreenCell[,] s, int row, int col)
        {
            bool b = false;

            if (row < s.GetLength(0) && col < s.GetLength(1))
                return s[row, col] == null;

            return b;
        }

        public virtual void Populate(ColumnGroup cg)
        {
            // Count maximum possible rows
            // Sum number of cells, which is sum of tile sizes, then add tallest tile size
            var iMaxRows = 1;
            if (Tiles.Count > 0)
                iMaxRows = Tiles.Sum(x => x.ColSpan * x.RowSpan) / cg.WidthInColumns + Tiles.Max(x => x.RowSpan);

            // Populate table of cells
            Cells = new ScreenCell[iMaxRows, cg.WidthInColumns];
            var cc = new CellCoordinates() { Columns = cg.WidthInColumns };
            foreach (var tile in Tiles)
            {
                // Skip tiles that are larger than the column group
                if (tile.ColSpan <= cg.WidthInColumns)
                {
                    // Find next available cell
                    bool bFound = false;
                    while (!bFound && cc.Row < iMaxRows)
                    {
                        bFound = true;
                        for (int iR = 0; iR < tile.RowSpan; iR++)
                        {
                            for (int iC = 0; iC < tile.ColSpan; iC++)
                            {
                                bFound &= EmptyCell(Cells, cc.Row + iR, cc.Col + iC);
                            }
                        }
                        if (!bFound)
                            cc.Next();
                    }

                    // If available cell found, set tile in cell(s)
                    if (bFound)
                    {
                        for (int iR = 0; iR < tile.RowSpan; iR++)
                        {
                            for (int iC = 0; iC < tile.ColSpan; iC++)
                            {
                                Cells[cc.Row + iR, cc.Col + iC] = new ScreenCell() { Tile = tile, Anchor = (iR == 0 && iC == 0) };
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", string.IsNullOrEmpty(Title) ? "(untitled group)" : Title);
        }

        public void ChildAdd(object o)
        {
            Tiles.Add(o as Tile);
        }

        [Browsable(false)]
        [JsonIgnore]
        public int ChildCount
        {
            get { return Tiles.Count; }
        }

        public int ChildIndexOf(object o)
        {
            return Tiles.IndexOf(o as Tile);
        }

        public void ChildInsert(int index, object o)
        {
            Tiles.Insert(index, o as Tile);
        }

        public void ChildRemove(object o)
        {
            Tiles.Remove(o as Tile);
        }

        public void ChildRemoveAt(int index)
        {
            Tiles.RemoveAt(index);
        }

        [Browsable(false)]
        [JsonIgnore]
        public Type ChildType
        {
            get { return typeof(Tile); }
        }

        [Browsable(false)]
        [JsonIgnore]
        public List<object> Childs
        {
            get { return Tiles.Select(x => x as object).ToList(); }
        }
    }
}
