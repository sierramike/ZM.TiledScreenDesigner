using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZM.TiledScreenDesigner
{
    public class BorderToolStripMenuItem : ToolStripMenuItem
    {
        public BorderToolStripMenuItem() : base()
        {

        }

        public virtual Type ItemType { get; set; }

        public virtual BorderElements Element { get; set; }

        public virtual BorderProperties Property { get; set; }
    }
}
