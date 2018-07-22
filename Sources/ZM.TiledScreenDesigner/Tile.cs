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
    public class Tile : IBackground, IBorder, IIcon, ITitle
    {
        public Tile()
        {
            BackColor = Color.White;
            BackColorOpacity = 255;
            BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            BackgroundImageLayout = ImageLayout.None;
            BackgroundImageOffset = new Point();
            BackgroundImageOpacity = 255;
            BackgroundImageZoom = 1;
            BackgroundOverlayColor = Color.White;
            BackgroundOverlayOpacity = 0;

            TitleColor = Color.Black;
            TitleAlign = System.Drawing.ContentAlignment.TopCenter;
            TitleOffset = new Point();
            TitleFont = new Font("Arial", 10, FontStyle.Regular);
            TitleBarColor = Color.Black;
            TitleBarHeight = 0;
            TitleBarOpacity = 0;

            BorderTop = new Border();
            BorderLeft = new Border();
            BorderRight = new Border();
            BorderBottom = new Border();

            IconAlign = System.Drawing.ContentAlignment.TopLeft;
            IconOffset = new Point();
            IconOpacity = 255;
            IconZoom = 1;
            ColSpan = 1;
            RowSpan = 1;
        }


        [Category("Title")]
        [Description("Title of the tile")]
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


        [Category("Background")]
        [Description("Background color of the tile")]
        public virtual Color BackColor { get; set; }

        [Category("Background")]
        [Description("Background color opacity (0=transparent, 255=solid)")]
        public virtual byte BackColorOpacity { get; set; }

        [Category("Background")]
        [Description("Background image of the screen")]
        [DefaultValue(typeof(Image), null)]
        [JsonConverter(typeof(ImageJsonConverter))]
        public virtual Image BackgroundImage { get; set; }

        [Category("Background")]
        [Description("Background image of the screen")]
        public virtual System.Drawing.ContentAlignment BackgroundImageAlign { get; set; }

        [Category("Background")]
        [Description("Background image layout")]
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


        [Category("Layout")]
        [Description("Width of the tile in number of columns")]
        public virtual int RowSpan { get; set; }

        [Category("Layout")]
        [Description("Height of the tile in unmber of rows")]
        public virtual int ColSpan { get; set; }


        [Category("Icon")]
        [Description("Icon picture")]
        public Image Icon { get; set; }

        [Category("Icon")]
        [Description("Icon alignment")]
        public System.Drawing.ContentAlignment IconAlign { get; set; }

        [Category("Icon")]
        [Description("Icon offset")]
        public Point IconOffset { get; set; }

        [Category("Icon")]
        [Description("Icon opacity (0=transparent, 255=solid)")]
        public byte IconOpacity { get; set; }

        [Category("Icon")]
        [Description("Icon zoom factor (Decimal value, 1=No zoom)")]
        public float IconZoom { get; set; }


        public override string ToString()
        {
            return string.Format("{0}x{1} ({2}) {3}", RowSpan, ColSpan, BackColor, string.IsNullOrEmpty(Title) ? "(untitled tile)" : Title);
        }
    }
}
