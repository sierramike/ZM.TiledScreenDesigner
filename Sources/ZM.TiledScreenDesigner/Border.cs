using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.TiledScreenDesigner
{
    [JsonConverter(typeof(NoTypeConverterJsonConverter<Border>))]
    [TypeConverter(typeof(BorderTypeConverter))]
    [Editor(typeof(BorderTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public class Border
    {
        public Border()
        {
            Color = Color.Red;
            Opacity = 255;
            Width = 0;
        }

        [NotifyParentProperty(true)]
        public virtual Color Color { get; set; }

        [NotifyParentProperty(true)]
        public virtual byte Opacity { get; set; }

        [NotifyParentProperty(true)]
        public virtual int Width { get; set; }

        public override string ToString()
        {
            return string.Format("{0}px {1} Op:{2}", Width, Color.Name, Opacity);
        }
    }
}
