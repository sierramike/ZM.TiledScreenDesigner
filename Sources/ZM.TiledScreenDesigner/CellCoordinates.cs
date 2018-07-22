using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.TiledScreenDesigner
{
    public class CellCoordinates
    {
        public CellCoordinates()
        {
            Row = 0;
            Col = 0;
            Columns = 1;
        }

        public virtual int Row { get; set; }

        public virtual int Col { get; set; }

        public virtual int Columns { get; set; }

        public virtual void Next()
        {
            Col++;
            if (Col >= Columns)
            {
                Col = 0;
                Row++;
            }
        }

        public virtual CellCoordinates Clone()
        {
            return new CellCoordinates()
            {
                Row = Row,
                Col = Col,
                Columns = Columns
            };
        }
    }
}
