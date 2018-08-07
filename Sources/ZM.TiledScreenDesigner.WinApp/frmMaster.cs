using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZM.TiledScreenDesigner.WinApp
{
    public partial class frmMaster : Form
    {
        protected Screen _screen;

        private bool _modified;
        protected bool Modified {
            get { return _modified; }
            set
            {
                _modified = value;
            }
        }

        private string _fileName;
        protected string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                this.Text = string.Format("{0} - Tiled Screen Designer", (_fileName != null ? (new FileInfo(_fileName)).Name : "(untitled)"));
            }
        }

        public frmMaster()
        {
            Modified = false;

            InitializeComponent();

            AddBorderMenuItems(ctxTVApplyCGBdAll, BorderElements.All, typeof(ColumnGroup));
            AddBorderMenuItems(ctxTVApplyCGBdTop, BorderElements.Top, typeof(ColumnGroup));
            AddBorderMenuItems(ctxTVApplyCGBdBottom, BorderElements.Bottom, typeof(ColumnGroup));
            AddBorderMenuItems(ctxTVApplyCGBdLeft, BorderElements.Left, typeof(ColumnGroup));
            AddBorderMenuItems(ctxTVApplyCGBdRight, BorderElements.Right, typeof(ColumnGroup));

            AddBorderMenuItems(ctxTVApplyGBdAll, BorderElements.All, typeof(Group));
            AddBorderMenuItems(ctxTVApplyGBdTop, BorderElements.Top, typeof(Group));
            AddBorderMenuItems(ctxTVApplyGBdBottom, BorderElements.Bottom, typeof(Group));
            AddBorderMenuItems(ctxTVApplyGBdLeft, BorderElements.Left, typeof(Group));
            AddBorderMenuItems(ctxTVApplyGBdRight, BorderElements.Right, typeof(Group));

            AddBorderMenuItems(ctxTVApplyTBdAll, BorderElements.All, typeof(Tile));
            AddBorderMenuItems(ctxTVApplyTBdTop, BorderElements.Top, typeof(Tile));
            AddBorderMenuItems(ctxTVApplyTBdBottom, BorderElements.Bottom, typeof(Tile));
            AddBorderMenuItems(ctxTVApplyTBdLeft, BorderElements.Left, typeof(Tile));
            AddBorderMenuItems(ctxTVApplyTBdRight, BorderElements.Right, typeof(Tile));

            AddBorderMenuItems(ctxTVApplyBd, BorderElements.All, null);

            ctxPVNew_Click(ctxPVNew, new EventArgs());
        }

        private void AddBorderMenuItems(ToolStripMenuItem item, BorderElements element, Type objectType)
        {
            AddBorderMenuItem(item, "Color ...", element, BorderProperties.Color, objectType);
            AddBorderMenuItem(item, "Opacity ...", element, BorderProperties.Opacity, objectType);
            AddBorderMenuItem(item, "Width ...", element, BorderProperties.Width, objectType);
        }

        private void AddBorderMenuItem(ToolStripMenuItem item, string text, BorderElements element, BorderProperties property, Type objectType)
        {
            var tsmi = new BorderToolStripMenuItem() { Text = text, ItemType = objectType, Element = element, Property = property };
            tsmi.Click += ctxTVApplyBorders_Click;
            item.DropDownItems.Add(tsmi);
        }

        public static ImageCodecInfo GetCodecInfo(Image i)
        {
            var imgguid = i.RawFormat.Guid;
            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageDecoders())
            {
                if (codec.FormatID == imgguid)
                    return codec;
            }
            return ImageCodecInfo.GetImageDecoders().Where(x => x.FormatID == ImageFormat.Jpeg.Guid).First();
        }

        private int GetImageIndex(object o)
        {
            if (_screen.Header.Equals(o)) // Special case of header
                return 1;

            Type t = o.GetType();
            if (t == typeof(Screen))
                return 0;
            else if (t == typeof(ColumnGroup))
                return 2;
            else if (t == typeof(Group))
                return 3;
            else if (t == typeof(Tile))
                return 4;
            else
                return 0;
        }

        private TreeNode LoadNodes(Screen s)
        {
            var tnMain = new TreeNode() { Text = "Screen", Tag = s, ImageIndex = GetImageIndex(s), SelectedImageIndex = GetImageIndex(s) };

            var tnHeader = LoadNodeColumnGroup(s.Header);
            tnHeader.Text = "Header";
            tnMain.Nodes.Add(tnHeader);

            foreach (var cg in s.ColumnGroups)
                tnMain.Nodes.Add(LoadNodeColumnGroup(cg));

            return tnMain;
        }

        private TreeNode LoadNodeColumnGroup(ColumnGroup cg)
        {
            var tn = new TreeNode() { Text = cg.ToString(), Tag = cg, ImageIndex = GetImageIndex(cg), SelectedImageIndex = GetImageIndex(cg) };

            foreach (var g in cg.Groups)
                tn.Nodes.Add(LoadNodeGroup(g));

            return tn;
        }

        private TreeNode LoadNodeGroup(Group g)
        {
            var tn = new TreeNode() { Text = g.ToString(), Tag = g, ImageIndex = GetImageIndex(g), SelectedImageIndex = GetImageIndex(g) };

            foreach (var t in g.Tiles)
                tn.Nodes.Add(LoadNodeTile(t));

            return tn;
        }

        private TreeNode LoadNodeTile(Tile t)
        {
            return new TreeNode() { Text = t.ToString(), Tag = t, ImageIndex = GetImageIndex(t), SelectedImageIndex = GetImageIndex(t) };
        }

        private void LoadScreen(Screen s)
        {
            _screen = s;

            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(LoadNodes(s));
            treeView1.ExpandAll();
            treeView1.SelectedNode = treeView1.Nodes[0];

            pg.SelectedObject = s;

            pg_PropertyValueChanged(pg, null);
        }

        private void pg_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            RefreshTN(treeView1.Nodes[0], pg.SelectedObject, true);
            RefreshPreview(true);
        }

        private void pnlPreview_DoubleClick(object sender, EventArgs e)
        {
            ctxPVFullScreen_Click(sender, e);
        }

        private void pnlPreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ctxPV.Show(Cursor.Position);
            }
        }

        private void pnlPreview_Resize(object sender, EventArgs e)
        {
            if (pnlPreview.Width >= pnlPreview.BackgroundImage.Width && pnlPreview.Height >= pnlPreview.BackgroundImage.Height)
                pnlPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            else
                pnlPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
        }

        private void RefreshPreview(bool propertyModified)
        {
            var g = new ScreenGenerator();

            var b = g.GenerateScreen(_screen);

            pnlPreview.BackgroundImage = b;

            if (propertyModified)
            {
                Modified = propertyModified;
                pg.SelectedObject = pg.SelectedObject;
            }

            //if (_screen.GetGroupColumnWidth() > _screen.GetRegionWidth())
            //    statusMainLabel.Text = string.Format("WARNING: Content is wider than region (content is {0} pixels wide, region is {1} pixels wide)", _screen.GetGroupColumnWidth(), _screen.GetRegionWidth());
            //else if (_screen.GetGroupColumnWidth() != _screen.GetRegionWidth())
            //    statusMainLabel.Text = string.Format("INFO: Content width is different from region width (content is {0} pixels wide, region is {1} pixels wide)", _screen.GetGroupColumnWidth(), _screen.GetRegionWidth());
            //else
            //    statusMainLabel.Text = "-";
        }

        private void RefreshTN(TreeNode root, object o, bool propertyModified)
        {
            if (o != null && !_screen.Header.Equals(o))
                foreach (TreeNode tn in root.Nodes)
                {
                    if (o.Equals(tn.Tag))
                        tn.Text = o.ToString();
                    else
                        RefreshTN(tn, o, propertyModified);
                }

            if (propertyModified)
            {
                Modified = propertyModified;
                pg.SelectedObject = pg.SelectedObject;
            }
        }

        private void SaveImage(Image i)
        {
            var ci = GetCodecInfo(i);

            MessageBox.Show(ci.FilenameExtension);

            var sfd = new SaveFileDialog()
            {
                Filter = string.Format("{0} files ({1})|{1}|All files|*.*", ci.FormatDescription, ci.FilenameExtension.ToLower()),
                Title = "Save picture ..."
            };

            if (sfd.ShowDialog() == DialogResult.OK)
                _screen.BackgroundImage.Save(sfd.FileName, new ImageFormat(i.RawFormat.Guid));
        }

        private void mnuSamplesFullScreen_Click(object sender, EventArgs e)
        {
            LoadScreen(Samples.FullScreen());
        }

        private void mnuSamplesMultigroups_Click(object sender, EventArgs e)
        {
            LoadScreen(Samples.MultiColumnGroups());
        }

        private void mnuSamplesNoTitle_Click(object sender, EventArgs e)
        {
            LoadScreen(Samples.NoTitle());
        }

        private void mnuSamplesTwoColumns_Click(object sender, EventArgs e)
        {
            LoadScreen(Samples.TwoColumns());
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var obj = treeView1.SelectedNode.Tag;
            pg.SelectedObject = obj;

            if (obj == null || obj is Screen || _screen.Header.Equals(obj))
            {
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }
            else if (treeView1.SelectedNode.Parent.Tag is IChildrens)
            {
                var c = treeView1.SelectedNode.Parent.Tag as IChildrens;
                int i = c.ChildIndexOf(obj);
                btnUp.Enabled = (i > 0);
                btnDown.Enabled = (i < c.ChildCount - 1);
            }
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = treeView1.GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            // Confirm that the node at the drop location is not 
            // the dragged node and that target node isn't null
            // (for example if you drag outside the control)
            if (!draggedNode.Equals(targetNode) && targetNode != null)
            {
                if (targetNode.Tag is IChildrens)
                {
                    var o = draggedNode.Parent.Tag as IChildrens;
                    o.ChildRemove(draggedNode.Tag);
                    draggedNode.Remove();
                    var t = targetNode.Tag as IChildrens;
                    t.ChildAdd(draggedNode.Tag);
                    targetNode.Nodes.Add(draggedNode);
                    RefreshPreview(true);
                }
            }
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            treeViewDrag((TreeView)sender, e);
        }

        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            treeViewDrag((TreeView)sender, e);
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var tv = (TreeView)sender;

                // Retrieve the node at the click location.
                var targetNode = tv.GetNodeAt(new Point(e.X, e.Y));

                if (targetNode != null)
                {
                    var o = targetNode.Tag as IChildrens;
                    ctxTV.Tag = targetNode;
                    ctxTVAdd.Tag = o?.ChildType;
                    ctxTVAdd.Text = string.Format("Add {0}", o?.ChildType?.Name);
                    ctxTVAdd.Enabled = (o != null);
                    ctxTVRemove.Enabled = !_screen.Header.Equals(targetNode.Tag) && !_screen.Equals(targetNode.Tag);

                    var b = targetNode.Tag as IBackground;
                    var i = targetNode.Tag as IIcon;
                    bool bSaveBG_Visible = (b != null);
                    bool bSaveIcon_Visible = (i != null);
                    ctxTVSaveBGImage.Visible = bSaveBG_Visible;
                    ctxTVSaveBGImage.Enabled = (b?.BackgroundImage != null);
                    ctxTVSaveIcon.Visible = bSaveIcon_Visible;
                    ctxTVSaveIcon.Enabled = (i?.Icon != null);
                    ctxTVSepSaveImage.Visible = bSaveBG_Visible || bSaveIcon_Visible;

                    bool bApplyAll_Visible = targetNode.Tag is IChildrens;
                    bool bApplyCG_Visible = _screen.Equals(targetNode.Tag);
                    bool bApplyG_Visible = bApplyCG_Visible || targetNode.Tag is ColumnGroup;
                    bool bApplyT_Visible = bApplyG_Visible || targetNode.Tag is Group;
                    bool bApplyBd_Visible = targetNode.Tag is IBorder;
                    ctxTVApplyAll.Visible = bApplyAll_Visible;
                    ctxTVApplyCG.Visible = bApplyCG_Visible;
                    ctxTVApplyG.Visible = bApplyG_Visible;
                    ctxTVApplyT.Visible = bApplyT_Visible;
                    ctxTVApplyBd.Visible = bApplyBd_Visible;
                    //ctxTVSepApply.Visible = bApplyCG_Visible || bApplyG_Visible || bApplyT_Visible || bApplyBd_Visible;

                    ctxTV.Show(Cursor.Position);
                }
            }
        }

        private void treeViewDrag(TreeView tv, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            var targetPoint = tv.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            var targetNode = tv.GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.
            var draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            statusMainLabel.Text = string.Format("From: {0} To: {1}", draggedNode.Text, targetNode?.Text);

            if (draggedNode.Equals(targetNode) || targetNode == null)
                e.Effect = DragDropEffects.None;
            else if (draggedNode.Tag is Tile && targetNode.Tag is Group && !draggedNode.Parent.Equals(targetNode))
                e.Effect = DragDropEffects.Move;
            else if (draggedNode.Tag is Group && targetNode.Tag is ColumnGroup && !draggedNode.Parent.Equals(targetNode))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ctxTVAdd_Click(object sender, EventArgs e)
        {
            var o = Activator.CreateInstance((Type)ctxTVAdd.Tag);
            AddTN(ctxTV.Tag as TreeNode, o);
        }

        private void ctxTVRemove_Click(object sender, EventArgs e)
        {
            RemoveTN(ctxTV.Tag as TreeNode);
        }

        private void AddTN(TreeNode tn, object o)
        {
            (tn.Tag as IChildrens).ChildAdd(o);
            tn.Nodes.Add(new TreeNode() { Text = o.ToString(), Tag = o, ImageIndex = GetImageIndex(o), SelectedImageIndex = GetImageIndex(o) });
            tn.Expand();
            RefreshPreview(true);
        }

        private void RemoveTN(TreeNode tn)
        {
            if (MessageBox.Show(string.Format("Are you sure to delete object \"{0}\"?\n\nCaution: if this item has childrens, they will be also deleted.", tn.Text), "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                (tn.Parent.Tag as IChildrens).ChildRemove(tn.Tag);
                tn.Parent.Nodes.Remove(tn);
                RefreshPreview(true);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            var movedNode = treeView1.SelectedNode;

            var parent = movedNode.Parent;
            var parentObject = parent.Tag as IChildrens;

            int i = parentObject.ChildIndexOf(movedNode.Tag);
            parentObject.ChildRemoveAt(i);
            parent.Nodes.Remove(movedNode);

            parentObject.ChildInsert(i - 1, movedNode.Tag);
            if (parent.Tag is Screen) i++;
            parent.Nodes.Insert(i - 1, movedNode);

            treeView1.SelectedNode = movedNode;

            RefreshPreview(true);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            var movedNode = treeView1.SelectedNode;

            var parent = movedNode.Parent;
            var parentObject = parent.Tag as IChildrens;

            int i = parentObject.ChildIndexOf(movedNode.Tag);
            parentObject.ChildRemoveAt(i);
            parent.Nodes.Remove(movedNode);

            parentObject.ChildInsert(i + 1, movedNode.Tag);
            if (parent.Tag is Screen) i++;
            parent.Nodes.Insert(i + 1, movedNode);

            treeView1.SelectedNode = movedNode;

            RefreshPreview(true);
        }

        private List<T> GetAll<T>(IChildrens c) where T : class
        {
            var l = new List<T>();

            foreach (var elt in c.Childs)
            {
                if (elt is T)
                    l.Add(elt as T);
                if (elt is IChildrens)
                    l.AddRange(GetAll<T>(elt as IChildrens));
            }

            return l;
        }

        private T GetFirst<T>(IChildrens c) where T : class
        {
            foreach(var elt in c.Childs)
            {
                T t = null;
                if (elt is T)
                    t = elt as T;
                else if (elt is IChildrens)
                    t = GetFirst<T>(elt as IChildrens);

                if (t != null)
                    return t;
            }
            return null;
        }

        private void ApplyBool<T>(TreeNode root, PropertyInfo pi, bool value) where T : class
        {
            IChildrens ch = root.Tag as IChildrens;

            var l = GetAll<T>(ch);
            foreach (var elt in l)
            {
                if (elt is IBackground o)
                {
                    pi.SetValue(o, value);
                    RefreshTN(treeView1.Nodes[0], o, true);
                }
            }

            RefreshPreview(false);
        }

        private void ApplyByte<T>(TreeNode root, PropertyInfo pi, string message, byte defaultValue, byte min = byte.MinValue, byte max = byte.MaxValue) where T : class
        {
            IChildrens ch = root.Tag as IChildrens;
            var t = GetFirst<T>(ch) as IBackground;

            int? i = frmGetInt.GetInt((t != null ? (byte)pi.GetValue(t) : defaultValue), message, min, max);

            if (i != null)
            {
                var l = GetAll<T>(ch);
                foreach (var elt in l)
                {
                    if (elt is IBackground o)
                    {
                        pi.SetValue(o, (byte)i);
                        RefreshTN(treeView1.Nodes[0], o, true);
                    }
                }

                RefreshPreview(false);
            }
        }

        private void ApplyByteBorder<T>(TreeNode root, bool applyToChildren, BorderElements borderElement, PropertyInfo pi, string message, byte defaultValue, byte min = byte.MinValue, byte max = byte.MaxValue) where T : class
        {
            IBorder t = null;
            var l = new List<IBorder>();

            if (applyToChildren)
            {
                IChildrens ch = root.Tag as IChildrens;
                t = GetFirst<T>(ch) as IBorder;
                l.AddRange(GetAll<T>(ch).Select(x => x as IBorder).Where(x => x != null).ToArray());
            }
            else
            {
                t = root.Tag as IBorder;
                l.Add(t);
            }

            byte b = defaultValue;
            if (borderElement == BorderElements.Bottom)
                b = (t != null ? (byte)pi.GetValue(t.BorderBottom) : defaultValue);
            else if (borderElement == BorderElements.Left)
                b = (t != null ? (byte)pi.GetValue(t.BorderLeft) : defaultValue);
            else if (borderElement == BorderElements.Right)
                b = (t != null ? (byte)pi.GetValue(t.BorderRight) : defaultValue);
            else
                b = (t != null ? (byte)pi.GetValue(t.BorderTop) : defaultValue);

            int? i = frmGetInt.GetInt(b, message, min, max);

            if (i != null)
            {
                foreach (var elt in l)
                {
                    if (elt is IBorder o)
                    {
                        if (borderElement == BorderElements.All || borderElement == BorderElements.Bottom)
                            pi.SetValue(o.BorderBottom, (byte)i);
                        if (borderElement == BorderElements.All || borderElement == BorderElements.Left)
                            pi.SetValue(o.BorderLeft, (byte)i);
                        if (borderElement == BorderElements.All || borderElement == BorderElements.Right)
                            pi.SetValue(o.BorderRight, (byte)i);
                        if (borderElement == BorderElements.All || borderElement == BorderElements.Top)
                            pi.SetValue(o.BorderTop, (byte)i);

                        RefreshTN(treeView1.Nodes[0], o, true);
                    }
                }

                RefreshPreview(false);
            }
        }

        private void ApplyColor<T>(TreeNode root, PropertyInfo pi, Color defaultValue) where T : class
        {
            IChildrens ch = root.Tag as IChildrens;
            var t = GetFirst<T>(ch) as IBackground;

            var cd = new ColorDialog();
            cd.Color = (t != null ? (Color)pi.GetValue(t) : defaultValue);
            if (cd.ShowDialog() == DialogResult.OK)
            {
                var l = GetAll<T>(ch);
                foreach (var elt in l)
                {
                    if (elt is IBackground o)
                    {
                        pi.SetValue(o, cd.Color);
                        RefreshTN(treeView1.Nodes[0], o, true);
                    }
                }

                RefreshPreview(false);
            }
        }

        private void ApplyColorBorder<T>(TreeNode root, bool applyToChildren, BorderElements borderElement, PropertyInfo pi, Color defaultValue) where T : class
        {
            IBorder t = null;
            var l = new List<IBorder>();

            if (applyToChildren)
            {
                IChildrens ch = root.Tag as IChildrens;
                t = GetFirst<T>(ch) as IBorder;
                l.AddRange(GetAll<T>(ch).Select(x => x as IBorder).Where(x => x != null).ToArray());
            }
            else
            {
                t = root.Tag as IBorder;
                l.Add(t);
            }

            var cd = new ColorDialog();
            if (borderElement == BorderElements.Bottom)
                cd.Color = (t != null ? (Color)pi.GetValue(t.BorderBottom) : defaultValue);
            else if (borderElement == BorderElements.Left)
                cd.Color = (t != null ? (Color)pi.GetValue(t.BorderLeft) : defaultValue);
            else if (borderElement == BorderElements.Right)
                cd.Color = (t != null ? (Color)pi.GetValue(t.BorderRight) : defaultValue);
            else
                cd.Color = (t != null ? (Color)pi.GetValue(t.BorderTop) : defaultValue);

            if (cd.ShowDialog() == DialogResult.OK)
            {
                foreach (var elt in l)
                {
                    if (borderElement == BorderElements.All || borderElement == BorderElements.Bottom)
                        pi.SetValue(elt.BorderBottom, cd.Color);
                    if (borderElement == BorderElements.All || borderElement == BorderElements.Left)
                        pi.SetValue(elt.BorderLeft, cd.Color);
                    if (borderElement == BorderElements.All || borderElement == BorderElements.Right)
                        pi.SetValue(elt.BorderRight, cd.Color);
                    if (borderElement == BorderElements.All || borderElement == BorderElements.Top)
                        pi.SetValue(elt.BorderTop, cd.Color);

                    RefreshTN(treeView1.Nodes[0], elt, true);
                }

                RefreshPreview(false);
            }
        }

        private void ApplyFont<T>(TreeNode root, PropertyInfo pi, Font defaultValue) where T : class
        {
            IChildrens ch = root.Tag as IChildrens;
            var t = GetFirst<T>(ch) as IBackground;

            var fd = new FontDialog();
            fd.Font = (t != null ? (Font)pi.GetValue(t) : defaultValue);
            if (fd.ShowDialog() == DialogResult.OK)
            {
                var l = GetAll<T>(ch);
                foreach (var elt in l)
                {
                    if (elt is IBackground o)
                    {
                        pi.SetValue(o, fd.Font);
                        RefreshTN(treeView1.Nodes[0], o, true);
                    }
                }

                RefreshPreview(false);
            }
        }

        private void ApplyInt<T>(TreeNode root, PropertyInfo pi, string message, int defaultValue, int min = int.MinValue, int max = int.MaxValue) where T : class
        {
            IChildrens ch = root.Tag as IChildrens;
            var t = GetFirst<T>(ch) as IBackground;

            int? i = frmGetInt.GetInt((t != null ? (int)pi.GetValue(t) : defaultValue), message, min, max);

            if (i != null)
            {
                var l = GetAll<T>(ch);
                foreach (var elt in l)
                {
                    if (elt is IBackground o)
                    {
                        pi.SetValue(o, (int)i);
                        RefreshTN(treeView1.Nodes[0], o, true);
                    }
                }

                RefreshPreview(false);
            }
        }

        private void ApplyIntBorder<T>(TreeNode root, bool applyToChildren, BorderElements borderElement, PropertyInfo pi, string message, int defaultValue, int min = int.MinValue, int max = int.MaxValue) where T : class
        {
            IBorder t = null;
            var l = new List<IBorder>();

            if (applyToChildren)
            {
                IChildrens ch = root.Tag as IChildrens;
                t = GetFirst<T>(ch) as IBorder;
                l.AddRange(GetAll<T>(ch).Select(x => x as IBorder).Where(x => x != null).ToArray());
            }
            else
            {
                t = root.Tag as IBorder;
                l.Add(t);
            }

            int v = defaultValue;
            if (borderElement == BorderElements.Bottom)
                v = (t != null ? (int)pi.GetValue(t.BorderBottom) : defaultValue);
            else if (borderElement == BorderElements.Left)
                v = (t != null ? (int)pi.GetValue(t.BorderLeft) : defaultValue);
            else if (borderElement == BorderElements.Right)
                v = (t != null ? (int)pi.GetValue(t.BorderRight) : defaultValue);
            else
                v = (t != null ? (int)pi.GetValue(t.BorderTop) : defaultValue);

            int? i = frmGetInt.GetInt(v, message, min, max);

            if (i != null)
            {
                foreach (var elt in l)
                {
                    if (elt is IBorder o)
                    {
                        if (borderElement == BorderElements.All || borderElement == BorderElements.Bottom)
                            pi.SetValue(o.BorderBottom, (int)i);
                        if (borderElement == BorderElements.All || borderElement == BorderElements.Left)
                            pi.SetValue(o.BorderLeft, (int)i);
                        if (borderElement == BorderElements.All || borderElement == BorderElements.Right)
                            pi.SetValue(o.BorderRight, (int)i);
                        if (borderElement == BorderElements.All || borderElement == BorderElements.Top)
                            pi.SetValue(o.BorderTop, (int)i);

                        RefreshTN(treeView1.Nodes[0], o, true);
                    }
                }

                RefreshPreview(false);
            }
        }

        private void ApplyAlign<T>(TreeNode root, PropertyInfo pi, ContentAlignment defaultValue) where T : class
        {
            IChildrens ch = root.Tag as IChildrens;
            var t = GetFirst<T>(ch) as ITitle;

            var newValue = frmGetContentAlignment.GetContentAlignment(t != null ? (ContentAlignment)pi.GetValue(t) : defaultValue);

            if (newValue != null)
            {
                var l = GetAll<T>(ch);
                foreach (var elt in l)
                {
                    if (elt is ITitle o)
                    {
                        pi.SetValue(o, (ContentAlignment)newValue);
                        RefreshTN(treeView1.Nodes[0], o, true);
                    }
                }

                RefreshPreview(false);
            }
        }

        private void ApplyOffset<T>(TreeNode root, PropertyInfo pi, Point defaultValue) where T : class
        {
            IChildrens ch = root.Tag as IChildrens;
            var t = GetFirst<T>(ch) as ITitle;

            var newValue = frmGetOffset.GetOffset(t != null ? (Point)pi.GetValue(t) : defaultValue);

            if (newValue != null)
            {
                var l = GetAll<T>(ch);
                foreach (var elt in l)
                {
                    if (elt is ITitle o)
                    {
                        pi.SetValue(o, (Point)newValue);
                        RefreshTN(treeView1.Nodes[0], o, true);
                    }
                }

                RefreshPreview(false);
            }
        }

        private void ApplyRotation<T>(TreeNode root, PropertyInfo pi, Rotation defaultValue) where T : class
        {
            IChildrens ch = root.Tag as IChildrens;
            var t = GetFirst<T>(ch) as ITitle;

            var newValue = frmGetRotation.GetRotation(t != null ? (Rotation)pi.GetValue(t) : defaultValue);

            if (newValue != null)
            {
                var l = GetAll<T>(ch);
                foreach (var elt in l)
                {
                    if (elt is ITitle o)
                    {
                        pi.SetValue(o, (Rotation)newValue);
                        RefreshTN(treeView1.Nodes[0], o, true);
                    }
                }

                RefreshPreview(false);
            }
        }

        private void ctxTVApplyCGBGColor_Click(object sender, EventArgs e)
        {
            var pi = typeof(IBackground).GetProperty(nameof(IBackground.BackColor));
            ApplyColor<ColumnGroup>(ctxTV.Tag as TreeNode, pi, Color.Black);
        }

        private void ctxTVApplyGBGColor_Click(object sender, EventArgs e)
        {
            var pi = typeof(IBackground).GetProperty(nameof(IBackground.BackColor));
            ApplyColor<Group>(ctxTV.Tag as TreeNode, pi, Color.Black);
        }

        private void ctxTVApplyTBGColor_Click(object sender, EventArgs e)
        {
            var pi = typeof(IBackground).GetProperty(nameof(IBackground.BackColor));
            ApplyColor<Tile>(ctxTV.Tag as TreeNode, pi, Color.Black);
        }

        private void ctxTVApplyCGBGColorOpacity_Click(object sender, EventArgs e)
        {
            var pi = typeof(IBackground).GetProperty(nameof(IBackground.BackColorOpacity));
            ApplyByte<ColumnGroup>(ctxTV.Tag as TreeNode, pi, "Enter background color opacity:", 255);
        }

        private void ctxTVApplyGBGColorOpacity_Click(object sender, EventArgs e)
        {
            var pi = typeof(IBackground).GetProperty(nameof(IBackground.BackColorOpacity));
            ApplyByte<Group>(ctxTV.Tag as TreeNode, pi, "Enter background color opacity:", 255);
        }

        private void ctxTVApplyTBGColorOpacity_Click(object sender, EventArgs e)
        {
            var pi = typeof(IBackground).GetProperty(nameof(IBackground.BackColorOpacity));
            ApplyByte<Tile>(ctxTV.Tag as TreeNode, pi, "Enter background color opacity:", 255);
        }

        private void ctxTVApplyCGBGOverlayColor_Click(object sender, EventArgs e)
        {
            var pi = typeof(IBackground).GetProperty(nameof(IBackground.BackgroundOverlayColor));
            ApplyColor<ColumnGroup>(ctxTV.Tag as TreeNode, pi, Color.Black);
        }

        private void ctxTVApplyGBGOverlayColor_Click(object sender, EventArgs e)
        {
            var pi = typeof(IBackground).GetProperty(nameof(IBackground.BackgroundOverlayColor));
            ApplyColor<Group>(ctxTV.Tag as TreeNode, pi, Color.Black);
        }

        private void ctxTVApplyTBGOverlayColor_Click(object sender, EventArgs e)
        {
            var pi = typeof(IBackground).GetProperty(nameof(IBackground.BackgroundOverlayColor));
            ApplyColor<Tile>(ctxTV.Tag as TreeNode, pi, Color.Black);
        }

        private void ctxTVApplyCGBGOverlayOpacity_Click(object sender, EventArgs e)
        {
            var pi = typeof(IBackground).GetProperty(nameof(IBackground.BackgroundOverlayOpacity));
            ApplyByte<ColumnGroup>(ctxTV.Tag as TreeNode, pi, "Enter background overlay opacity:", 255);
        }

        private void ctxTVApplyGBGOverlayOpacity_Click(object sender, EventArgs e)
        {
            var pi = typeof(IBackground).GetProperty(nameof(IBackground.BackgroundOverlayOpacity));
            ApplyByte<Group>(ctxTV.Tag as TreeNode, pi, "Enter background overlay opacity:", 255);
        }

        private void ctxTVApplyTBGOverlayOpacity_Click(object sender, EventArgs e)
        {
            var pi = typeof(IBackground).GetProperty(nameof(IBackground.BackgroundOverlayOpacity));
            ApplyByte<Tile>(ctxTV.Tag as TreeNode, pi, "Enter background overlay opacity:", 255);
        }

        private void ctxTVApplyGTitleFont_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleFont));
            ApplyFont<Group>(ctxTV.Tag as TreeNode, pi, new Font("Arial", 12, FontStyle.Regular));
        }

        private void ctxTVApplyTTitleFont_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleFont));
            ApplyFont<Tile>(ctxTV.Tag as TreeNode, pi, new Font("Arial", 12, FontStyle.Regular));
        }

        private void ctxTVApplyGTitleColor_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleColor));
            ApplyColor<Group>(ctxTV.Tag as TreeNode, pi, Color.Black);
        }

        private void ctxTVApplyTTitleColor_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleColor));
            ApplyColor<Tile>(ctxTV.Tag as TreeNode, pi, Color.Black);
        }

        private void ctxTVApplyGTitleAlign_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleAlign));
            ApplyAlign<Group>(ctxTV.Tag as TreeNode, pi, ContentAlignment.TopCenter);
        }

        private void ctxTVApplyTTitleAlign_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleAlign));
            ApplyAlign<Tile>(ctxTV.Tag as TreeNode, pi, ContentAlignment.TopCenter);
        }

        private void ctxTVApplyGTitleOffset_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleOffset));
            ApplyOffset<Group>(ctxTV.Tag as TreeNode, pi, new Point());
        }

        private void ctxTVApplyTTitleOffset_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleOffset));
            ApplyOffset<Tile>(ctxTV.Tag as TreeNode, pi, new Point());
        }

        private void ctxTVApplyGTitleRotation_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleRotation));
            ApplyRotation<Group>(ctxTV.Tag as TreeNode, pi, Rotation.None);
        }

        private void ctxTVApplyTTitleRotation_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleRotation));
            ApplyRotation<Tile>(ctxTV.Tag as TreeNode, pi, Rotation.None);
        }

        private void ctxTVApplyGTitleBarColor_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleBarColor));
            ApplyColor<Group>(ctxTV.Tag as TreeNode, pi, Color.Black);
        }

        private void ctxTVApplyTTitleBarColor_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleBarColor));
            ApplyColor<Tile>(ctxTV.Tag as TreeNode, pi, Color.Black);
        }

        private void ctxTVApplyGTitleBarHeight_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleBarHeight));
            ApplyInt<Group>(ctxTV.Tag as TreeNode, pi, "Enter title bar height:", 0, 0, 4000);
        }

        private void ctxTVApplyTTitleBarHeight_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleBarHeight));
            ApplyInt<Tile>(ctxTV.Tag as TreeNode, pi, "Enter title bar height:", 0, 0, 4000);
        }

        private void ctxTVApplyGTitleBarOpacity_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleBarOpacity));
            ApplyByte<Group>(ctxTV.Tag as TreeNode, pi, "Enter title bar opacity:", 0);
        }

        private void ctxTVApplyTTitleBarOpacity_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleBarOpacity));
            ApplyByte<Tile>(ctxTV.Tag as TreeNode, pi, "Enter title bar opacity:", 0);
        }

        private void ctxTVApplyCGTilesSize_Click(object sender, EventArgs e)
        {
            var pi = typeof(ColumnGroup).GetProperty(nameof(ColumnGroup.TileSize));
            ApplyInt<ColumnGroup>(ctxTV.Tag as TreeNode, pi, "Enter tile size:", 100, 1, 4000);
        }

        private void ctxTVApplyCGHSpacing_Click(object sender, EventArgs e)
        {
            var pi = typeof(ColumnGroup).GetProperty(nameof(ColumnGroup.HorizontalSpacing));
            ApplyInt<ColumnGroup>(ctxTV.Tag as TreeNode, pi, "Enter horizontal spacing:", 20, 0, 4000);
        }

        private void ctxTVApplyCGVSpacing_Click(object sender, EventArgs e)
        {
            var pi = typeof(ColumnGroup).GetProperty(nameof(ColumnGroup.VerticalSpacing));
            ApplyInt<ColumnGroup>(ctxTV.Tag as TreeNode, pi, "Enter vertical spacing:", 20, 0, 4000);
        }

        private void ctxTVApplyCGVGroupSpacing_Click(object sender, EventArgs e)
        {
            var pi = typeof(ColumnGroup).GetProperty(nameof(ColumnGroup.VerticalGroupSpacing));
            ApplyInt<ColumnGroup>(ctxTV.Tag as TreeNode, pi, "Enter vertical group spacing:", 20, 0, 4000);
        }

        private void ctxTVApplyBorders_Click(object sender, EventArgs e)
        {
            var tsmi = sender as BorderToolStripMenuItem;
            Type chType = tsmi.ItemType;
            if (chType == null)
                chType = (ctxTV.Tag as TreeNode).Tag.GetType();

            PropertyInfo pi = null;
            if (tsmi.Property == BorderProperties.Color)
            {
                pi = typeof(Border).GetProperty(nameof(Border.Color));

                MethodInfo mi = this.GetType().GetMethod("ApplyColorBorder", BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo mig = mi.MakeGenericMethod(chType);
                mig.Invoke(this, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { ctxTV.Tag as TreeNode, tsmi.ItemType != null, tsmi.Element, pi, Color.Red }, null);

            }
            else if (tsmi.Property == BorderProperties.Opacity)
            {
                pi = typeof(Border).GetProperty(nameof(Border.Opacity));

                MethodInfo mi = this.GetType().GetMethod("ApplyByteBorder", BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo mig = mi.MakeGenericMethod(chType);
                mig.Invoke(this, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { ctxTV.Tag as TreeNode, tsmi.ItemType != null, tsmi.Element, pi, "Enter border opacity:", (byte)255, byte.MinValue, byte.MaxValue }, null);
            }
            else if (tsmi.Property == BorderProperties.Width)
            {
                pi = typeof(Border).GetProperty(nameof(Border.Width));

                MethodInfo mi = this.GetType().GetMethod("ApplyIntBorder", BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo mig = mi.MakeGenericMethod(chType);
                mig.Invoke(this, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { ctxTV.Tag as TreeNode, tsmi.ItemType != null, tsmi.Element, pi, "Enter border width:", 2, int.MinValue, int.MaxValue }, null);
            }
        }

        private void ctxTVSaveBGImage_Click(object sender, EventArgs e)
        {
            var bg = treeView1.Nodes[0].Tag as IBackground;
            SaveImage(bg.BackgroundImage);
        }

        private void ctxTVSaveIcon_Click(object sender, EventArgs e)
        {
            var ic = treeView1.Nodes[0].Tag as IIcon;
            SaveImage(ic.Icon);
        }

        private void ctxPVFullScreen_Click(object sender, EventArgs e)
        {
            var f = new frmPreview(pnlPreview.BackgroundImage);
            f.ShowDialog();
        }

        private void ctxPVExportJPG_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog()
            {
                Filter = "JPG files|*.jpg|All files|*.*",
                Title = "Save to JPG"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
                pnlPreview.BackgroundImage.Save(sfd.FileName, ImageFormat.Jpeg);
        }

        private void ctxPVExportPNG_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog()
            {
                Filter = "PNG files|*.png|All files|*.*",
                Title = "Save to PNG"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
                pnlPreview.BackgroundImage.Save(sfd.FileName, ImageFormat.Png);
        }

        private void ctxTVApplyAllTitleFont_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleFont));
            ApplyFont<ITitle>(ctxTV.Tag as TreeNode, pi, new Font("Arial", 12, FontStyle.Regular));
        }

        private void ctxTVApplyAllTitleColor_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleColor));
            ApplyColor<ITitle>(ctxTV.Tag as TreeNode, pi, Color.Black);
        }

        private void ctxTVApplyAllTitleAlign_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleAlign));
            ApplyAlign<ITitle>(ctxTV.Tag as TreeNode, pi, ContentAlignment.TopCenter);
        }

        private void ctxTVApplyAllTitleOffset_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleOffset));
            ApplyOffset<ITitle>(ctxTV.Tag as TreeNode, pi, new Point());
        }

        private void ctxTVApplyAllTitleRotation_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleRotation));
            ApplyRotation<ITitle>(ctxTV.Tag as TreeNode, pi, Rotation.None);
        }

        private void ctxTVApplyAllTitleBarColor_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleBarColor));
            ApplyColor<ITitle>(ctxTV.Tag as TreeNode, pi, Color.Black);
        }

        private void ctxTVApplyAllTitleBarHeight_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleBarHeight));
            ApplyInt<ITitle>(ctxTV.Tag as TreeNode, pi, "Enter title bar height:", 0, 0, 4000);
        }

        private void ctxTVApplyAllTitleBarOpacity_Click(object sender, EventArgs e)
        {
            var pi = typeof(ITitle).GetProperty(nameof(ITitle.TitleBarOpacity));
            ApplyByte<ITitle>(ctxTV.Tag as TreeNode, pi, "Enter title bar opacity:", 0);
        }

        private void ctxPVNew_Click(object sender, EventArgs e)
        {
            if (CheckSaveModel())
            {
                LoadScreen(Samples.Blank());
                Modified = false;
                FileName = null;
            }
        }

        private void ctxPVOpen_Click(object sender, EventArgs e)
        {
            if (CheckSaveModel())
            {
                var ofd = new OpenFileDialog()
                {
                    Filter = "Tiled screen files|*.tiled|All files|*.*",
                };

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string s = File.ReadAllText(ofd.FileName);
                    if (s.Length > 13)
                    {
                        if (s.StartsWith("VERSION 1.0\r\n"))
                        {
                            LoadScreen(new ScreenGenerator().Open(s.Substring(13)));
                            Modified = false;
                            FileName = ofd.FileName;
                        }
                    }
                    else
                        MessageBox.Show("Cannot open file: Invalid format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ctxPVSave_Click(object sender, EventArgs e)
        {
            if (FileName != null)
                Save(FileName);
            else
                ctxPVSaveAs_Click(ctxPVSave, new EventArgs());

        }

        private void ctxPVSaveAs_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog()
            {
                Filter = "Tiled screen files|*.tiled|All files|*.*",
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Save(sfd.FileName);
            }
        }

        private void Save(string fileName)
        {
            var s = new ScreenGenerator().Save(_screen);
            s = string.Format("VERSION 1.0\r\n{0}", s);
            File.WriteAllText(fileName, s);
            Modified = false;
            FileName = fileName;
        }

        private bool CheckSaveModel()
        {
            if (!Modified)
                return true;
            else
            {
                var dr = MessageBox.Show("Current model has been modified, do you want to save changes?", "Save changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                    return true;
                else if (dr == DialogResult.Yes)
                {
                    ctxPVSave_Click(ctxPVSave, new EventArgs());
                    return !Modified;
                }
                else
                    return false;
            }
        }

        private void ctxPVHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://tiledscreendesigner.ztb.fr");
        }
    }
}
