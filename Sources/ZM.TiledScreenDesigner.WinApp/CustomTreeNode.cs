using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZM.TiledScreenDesigner.WinApp
{
    public class CustomTreeNode : TreeNode
    {
        public CustomTreeNode() : base()
        {

        }

        public CustomTreeNode(string text) : base(text)
        {

        }

        public CustomTreeNode(string text, TreeNode[] children) : base(text, children)
        {

        }

        public CustomTreeNode(string text, int imageIndex, int selectedImageIndex) : base(text, imageIndex, selectedImageIndex)
        {

        }

        public CustomTreeNode(string text, int imageIndex, int selectedImageIndex, TreeNode[] children) : base(text, imageIndex, selectedImageIndex, children)
        {

        }

        public new string Text
        {
            get { return (Tag != null ? Tag.ToString() : "(untitled)"); }
            set { }
        }
    }
}
