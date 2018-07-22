using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.TiledScreenDesigner
{
    public interface IChildrens
    {
        void ChildAdd(object o);
        int ChildCount { get; }
        int ChildIndexOf(object o);
        void ChildInsert(int index, object o);
        void ChildRemove(object o);
        void ChildRemoveAt(int index);
        Type ChildType { get; }
        List<object> Childs { get; }
    }
}
