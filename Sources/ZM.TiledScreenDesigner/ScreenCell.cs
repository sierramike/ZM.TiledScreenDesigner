using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.TiledScreenDesigner
{
    public class ScreenCell
    {
        public ScreenCell()
        {

        }

        public virtual Tile Tile { get; set; }

        public virtual bool Anchor { get; set; }

        public override string ToString()
        {
            return string.Format("{0}{1}", Anchor ? "*" : "", Tile.ToString());
        }
    }
}
