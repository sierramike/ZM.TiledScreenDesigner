using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.TiledScreenDesigner
{
    public class ColumnGroup : IBackground, IBorder, IChildrens
    {
        public ColumnGroup()
        {
            BackColor = Color.Transparent;
            BackColorOpacity = 0;
            BackgroundImageAlign = ContentAlignment.MiddleCenter;
            BackgroundImageLayout = ImageLayout.None;
            BackgroundImageOffset = new Point();
            BackgroundImageOpacity = 255;
            BackgroundImageZoom = 1;
            BackgroundOverlayColor = Color.Transparent;
            BackgroundOverlayOpacity = 0;

            WidthInColumns = 1;

            BorderTop = new Border();
            BorderLeft = new Border();
            BorderRight = new Border();
            BorderBottom = new Border();

            Groups = new List<Group>();

            TileSize = 100;
            HorizontalSpacing = 20;
            Padding = new Margins(0, 0, 0, 0);
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
        public virtual ContentAlignment BackgroundImageAlign { get; set; }

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


        [Category("Tiles")]
        [Description("Horizontal spacing between tiles")]
        public virtual int HorizontalSpacing { get; set; }

        [Category("Tiles")]
        [Description("Vertical spacing between tiles")]
        public virtual int VerticalSpacing { get; set; }

        [Category("Tiles")]
        [Description("Standard square tile size (width and height)")]
        public virtual int TileSize { get; set; }


        [Category("Size")]
        [Description("Vertical spacing between groups")]
        public virtual int VerticalGroupSpacing { get; set; }

        [Category("Size")]
        [Description("Width of the group in number of columns")]
        public virtual int WidthInColumns { get; set; }

        [Category("Size")]
        [Description("Padding around the column group")]
        public virtual Margins Padding { get; set; }


        [Category("Borders")]
        [Description("Top border")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [JsonProperty]
        public Border BorderTop { get; set; }

        [Category("Borders")]
        [Description("Left border")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [JsonProperty]
        public Border BorderLeft { get; set; }

        [Category("Borders")]
        [Description("Right border")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [JsonProperty]
        public Border BorderRight { get; set; }

        [Category("Borders")]
        [Description("Bottom border")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [JsonProperty]
        public Border BorderBottom { get; set; }


        [Browsable(false)]
        public virtual List<Group> Groups { get; private set; }

        [Browsable(false)]
        [JsonIgnore]
        public virtual Size RenderingSize { get; set; }


        public virtual void Calculate() //Screen s)
        {
            RenderingSize = new Size((TileSize + HorizontalSpacing) * WidthInColumns - HorizontalSpacing + Padding.Left + Padding.Right, RenderingSize.Height);

            int columnHeight = 0;
            foreach (var g in Groups)
            {
                g.Calculate(this); //, s.TileSize, s.VerticalSpacing);
                columnHeight += g.RenderingSize.Height + VerticalGroupSpacing;
            }

            RenderingSize = new Size(RenderingSize.Width, Math.Max(1, columnHeight) + Padding.Top + Padding.Bottom);
        }

        public virtual int GetHeight()
        {
            int i = 0;
            foreach(var g in Groups)
                i += (VerticalGroupSpacing + g.RenderingSize.Height);

            return i;
        }

        public void ChildAdd(object o)
        {
            Groups.Add(o as Group);
        }

        [Browsable(false)]
        [JsonIgnore]
        public int ChildCount
        {
            get { return Groups.Count; }
        }

        public int ChildIndexOf(object o)
        {
            return Groups.IndexOf(o as Group);
        }

        public void ChildInsert(int index, object o)
        {
            Groups.Insert(index, o as Group);
        }

        public void ChildRemove(object o)
        {
            Groups.Remove(o as Group);
        }

        public void ChildRemoveAt(int index)
        {
            Groups.RemoveAt(index);
        }

        [Browsable(false)]
        [JsonIgnore]
        public Type ChildType
        {
            get { return typeof(Group); }
        }

        [Browsable(false)]
        [JsonIgnore]
        public List<object> Childs
        {
            get { return Groups.Select(x => x as object).ToList(); }
        }

        public override string ToString()
        {
            return string.Format("Column group (Width: {0} col)", WidthInColumns);
        }
    }
}
