using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.TiledScreenDesigner
{
    public interface IIcon
    {
        Image Icon { get; set; }

        ContentAlignment IconAlign { get; set; }

        Point IconOffset { get; set; }

        byte IconOpacity { get; set; }

        float IconZoom { get; set; }
    }
}
