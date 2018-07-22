using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.TiledScreenDesigner
{
    public interface IBorder
    {
        Border BorderTop { get; set; }

        Border BorderLeft { get; set; }

        Border BorderRight { get; set; }

        Border BorderBottom { get; set; }
    }
}
