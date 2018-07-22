using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ZM.TiledScreenDesigner
{
    public class Screen : IBackground, IChildrens, ITitle
    {
        public Screen()
        {
            BackColor = Color.Black;
            BackColorOpacity = 255;
            BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            BackgroundImageLayout = ImageLayout.None;
            BackgroundImageOffset = new Point();
            BackgroundImageOpacity = 255;
            BackgroundImageZoom = 1;
            BackgroundOverlayColor = Color.White;
            BackgroundOverlayOpacity = 0;
            HeaderContentAlign = System.Drawing.ContentAlignment.MiddleCenter;
            HeaderLocation = Position.Top;
            HeaderMargins = new Margins(0, 0, 0, 0);
            HeaderOffset = new Point();
            HeaderThickness = 0;
            TitleAlign = System.Drawing.ContentAlignment.MiddleCenter;
            TitleColor = Color.Black;
            TitleOffset = new Point();
            TitleBarColor = Color.Black;
            TitleBarHeight = 0;
            TitleBarOpacity = 0;
            Size = new Size() { Width = 1920, Height = 1080 };
            ContentMargins = new Margins(0, 0, 0, 0);
            ContentAlign = System.Drawing.ContentAlignment.TopCenter;
            ColumnGroups = new List<ColumnGroup>();
            Header = new ColumnGroup();
        }

        [Category("Background")]
        [Description("Background color")]
        public virtual Color BackColor { get; set; }

        [Category("Background")]
        [Description("Background color opacity (0=transparent, 255=solid), always solid for the screen background")]
        [Browsable(false)]
        public virtual byte BackColorOpacity { get{ return 255; } set { } }

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
        [Description("Title of the screen")]
        public virtual string Title { get; set; }

        [Category("Title")]
        [Description("Title alignment")]
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


        [Category("Header")]
        [Description("Header position")]
        public virtual Position HeaderLocation { get; set; }

        [Category("Header")]
        [Description("Header thickness")]
        public virtual int HeaderThickness { get; set; }

        [Category("Header")]
        [Description("Header alignment")]
        public virtual System.Drawing.ContentAlignment HeaderContentAlign { get; set; }

        [Category("Header")]
        [Description("Header margins")]
        public virtual Margins HeaderMargins { get; set; }

        [Category("Header")]
        [Description("Header offset")]
        public virtual Point HeaderOffset { get; set; }


        [Category("Content")]
        [Description("Content alignment")]
        public virtual System.Drawing.ContentAlignment ContentAlign { get; set; }

        [Category("Content")]
        [Description("Minimum margins around the screen, can be higher after centering the result")]
        public virtual Margins ContentMargins { get; set; }


        [Category("Size")]
        [Description("Size of the screen")]
        public virtual Size Size { get; set; }

        [Category("Size")]
        [Description("Horizontal spacing between column groups")]
        public virtual int HorizontalColumnGroupSpacing { get; set; }


        [Browsable(false)]
        public virtual List<ColumnGroup> ColumnGroups { get; private set; }

        [Browsable(false)]
        public virtual ColumnGroup Header { get; private set; }

        public virtual void Calculate()
        {
            Header.Calculate();

            foreach (var cg in ColumnGroups)
                cg.Calculate();
        }

        public virtual int GetGroupColumnWidth()
        {
            Calculate();

            int iRegionWidth = 0;
            foreach (var cg in ColumnGroups)
                iRegionWidth += cg.RenderingSize.Width + HorizontalColumnGroupSpacing;
            if (iRegionWidth > 0)
                iRegionWidth -= HorizontalColumnGroupSpacing;
            else
                iRegionWidth = 1;

            return iRegionWidth;
        }

        public override string ToString()
        {
            return "Screen";
        }

        public void ChildAdd(object o)
        {
            ColumnGroups.Add(o as ColumnGroup);
        }

        [Browsable(false)]
        [JsonIgnore]
        public int ChildCount
        {
            get { return ColumnGroups.Count; }
        }

        public int ChildIndexOf(object o)
        {
            return ColumnGroups.IndexOf(o as ColumnGroup);
        }

        public void ChildInsert(int index, object o)
        {
            ColumnGroups.Insert(index, o as ColumnGroup);
        }

        public void ChildRemove(object o)
        {
            ColumnGroups.Remove(o as ColumnGroup);
        }

        public void ChildRemoveAt(int index)
        {
            ColumnGroups.RemoveAt(index);
        }

        [Browsable(false)]
        [JsonIgnore]
        public Type ChildType
        {
            get { return typeof(ColumnGroup); }
        }

        [Browsable(false)]
        [JsonIgnore]
        public List<object> Childs
        {
            get { return ColumnGroups.Select(x => x as object).ToList(); }
        }
    }
}
