using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.TiledScreenDesigner
{
    public class BorderTypeEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            var o = e.Value as Border;

            e.Graphics.FillRectangle(new SolidBrush(o.Color), e.Bounds);
        }
    }
}
