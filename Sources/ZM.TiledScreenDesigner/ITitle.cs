using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.TiledScreenDesigner
{
    public interface ITitle
    {
        string Title { get; set; }

        ContentAlignment TitleAlign { get; set; }

        Color TitleBarColor { get; set; }

        int TitleBarHeight { get; set; }

        byte TitleBarOpacity { get; set; }

        Color TitleColor { get; set; }

        Font TitleFont { get; set; }

        Point TitleOffset { get; set; }

        Rotation TitleRotation { get; set; }
    }
}
