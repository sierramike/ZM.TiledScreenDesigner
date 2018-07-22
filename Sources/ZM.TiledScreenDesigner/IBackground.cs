using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.TiledScreenDesigner
{
    public interface IBackground
    {
        Color BackColor { get; set; }

        byte BackColorOpacity { get; set; }

        Image BackgroundImage { get; set; }

        ContentAlignment BackgroundImageAlign { get; set; }

        ImageLayout BackgroundImageLayout { get; set; }

        Point BackgroundImageOffset { get; set; }

        byte BackgroundImageOpacity { get; set; }

        float BackgroundImageZoom { get; set; }

        Color BackgroundOverlayColor { get; set; }

        byte BackgroundOverlayOpacity { get; set; }
    }
}
