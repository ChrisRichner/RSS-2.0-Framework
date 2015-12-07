using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Raccoom.Xml.Demo
{
    public partial class RssReader : Form
    {
        #region fields
        Raccoom.Xml.Rss.RssFactory rssFactory = new Raccoom.Xml.Rss.RssFactory();
        Raccoom.Xml.Opml.OpmlFactory factory = new Raccoom.Xml.Opml.OpmlFactory();
        static string _cacheDirectory = string.Empty;
        #endregion        

        public RssReader()
        {
            InitializeComponent();
        }

        #region internal interface
        private static string CacheDirectory
        {
            get
            {
                if (_cacheDirectory == string.Empty)
                {
                    _cacheDirectory = System.IO.Path.Combine(Application.StartupPath, "Cache");
                    if (!System.IO.Directory.Exists(_cacheDirectory)) System.IO.Directory.CreateDirectory(_cacheDirectory);
                }
                return _cacheDirectory;
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //
            if (DesignMode) return;
            //
            this.Icon = Properties.Resources.rss;
            //
            _tvRssChannels.ImageList = new ImageList();
            _tvRssChannels.ImageList.ColorDepth = ColorDepth.Depth32Bit;
            _tvRssChannels.ImageList.Images.Add(Properties.Resources.rss);
            _tvRssChannels.ImageList.Images.Add(Properties.Resources.folder);
            //
            listView1.Layout += delegate(object sender, System.Windows.Forms.LayoutEventArgs ex) { this.listView1.Columns[1].Width = -2; };
            listView1.SmallImageList = new ImageList();
            listView1.SmallImageList.Images.Add(Properties.Resources.error);
            //
            listView2.Layout += delegate(object sender, System.Windows.Forms.LayoutEventArgs ex) { this.listView2.Columns[1].Width = -2; };
            listView2.SmallImageList = listView1.SmallImageList;
        }
        private void _btnFetchOpml_Click(object sender, EventArgs e)
        {
            Raccoom.Xml.Opml.OpmlDocument document = null;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                SetStatus(string.Format("Fetching {0}, please wait...", toolStripTextBox1.Text));
                
                //
                string fileName = System.IO.Path.Combine(CacheDirectory, System.IO.Path.GetFileName(toolStripTextBox1.Text));
                toolStripButton1.Enabled = false;
                string uri = toolStripTextBox1.Text;
                // cache hit test
                if (System.IO.File.Exists(fileName))
                {
                    uri = fileName;
                }
                document = factory.Read(uri) as Raccoom.Xml.Opml.OpmlDocument;
                // cache item
                if (!System.IO.File.Exists(fileName))
                {
                    factory.Write(fileName, document);
                }
                //
                _tvRssChannels.BeginUpdate();
                _tvRssChannels.Nodes.Clear();
                //
                foreach (Raccoom.Xml.Opml.OpmlOutline outline in document.Body.Items)
                {
                    _tvRssChannels.Nodes.Add(CreateNode(outline));
                }
            }
            finally
            {
                _tvRssChannels.EndUpdate();
                //
                FillErrors(listView2, document.Errors);
                //
                toolStripButton1.Enabled = true;
                SetStatusReady();
                Cursor.Current = Cursors.Default;
            }
        }

        private TreeNodeOpml CreateNode(Raccoom.Xml.Opml.OpmlOutline outline)
        {
            TreeNodeOpml node= new TreeNodeOpml(outline);
            node.Text = outline.Text;            
            if (outline.IsInclusion || outline.Items.Count > 0)
            {
                node.ImageIndex = 1;
                node.SelectedImageIndex = 1;
                node.Nodes.Add("@@@DUMMYNODE@@@");
            }
            return node;
        }
        private void _tvRssChannels_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            SetStatus(string.Format("Fetching {0}, please wait...", e.Node.Text));

            this.splitContainer3.Enabled = false;
            //
            try
            {
                if (e.Node.Nodes.Count == 0)
                {
                    System.Collections.IDictionary dic;

                    try
                    {
                        Raccoom.Xml.Rss.RssChannel channel = ((TreeNodeOpml)e.Node).RssChannel;
                        dic = channel.Errors;
                        //
                        propertyGrid1.SelectedObject = channel;
                    }
                    catch (Exception ex)
                    {
                        dic = new System.Collections.Specialized.ListDictionary();
                        dic.Add("Feed", ex.Message);
                    }
                    //
                    FillErrors(listView1, dic);
                }
                else
                {
                    propertyGrid1.SelectedObject = null;
                    listView2.Items.Clear();
                }
            }
            finally
            {
                Cursor = Cursors.Default;
                this.splitContainer3.Enabled = true;
                SetStatusReady();
            }
        }
        private void FillErrors(ListView listView, System.Collections.IDictionary errors)
        {
            listView.Items.Clear();
            if (errors.Count > 0)
            {
                foreach (System.Collections.DictionaryEntry exceptionEntry in errors)
                {
                    listView.Items.Add(exceptionEntry.Key.ToString(), 0).SubItems.Add(exceptionEntry.Value.ToString());
                }
            }
        }
        private void SetStatus(string statusText)
        {
            toolStripStatusLabel1.Text = statusText;
            Application.DoEvents();
        }
        private void SetStatusReady()
        {
            SetStatus("Done");
        }
        
        #endregion

        #region events
        private void _tvRssChannels_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.splitContainer3.Enabled = false;
            //
            try
            {
                if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "@@@DUMMYNODE@@@")
                {
                    e.Node.Nodes[0].Remove();
                    TreeNodeOpml opmlNode = e.Node as TreeNodeOpml;
                    //
                    if (opmlNode.Outline.IsInclusion)
                    {
                        opmlNode.Outline.InclusionDocument =  factory.GetDocument(opmlNode.Outline.Url) as Raccoom.Xml.Opml.OpmlDocument;                        
                    }
                    //
                    foreach (Raccoom.Xml.Opml.OpmlOutline outline in opmlNode.Outline.Items)
                    {
                        e.Node.Nodes.Add(CreateNode(outline));
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                this.splitContainer3.Enabled = true;
            }
        }
        private void _tvRssChannels_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            //
            TreeNodeOpml node = e.Node as TreeNodeOpml;
            if (node == null) return;
            //
            //
            switch (Control.ModifierKeys)
            {
                case Keys.Control:
                    string fileName = System.IO.Path.GetFullPath("test.xml");
                    //node.RssChannel.Items.Clear();
                    rssFactory.Write(fileName, node.RssChannel);
                    System.Diagnostics.Process.Start(fileName);
                    break;
                case Keys.Shift:
                    System.Diagnostics.Process.Start(node.Outline.XmlUrl);
                    break;
            }
            //

        }
        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hold Ctrl to see parsed xml or Shift for original feed while clicking a node", Application.ProductName);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region nested classes
        internal class TreeNodeOpml : TreeNode
        {
            static Raccoom.Xml.Rss.RssFactory rssFactory = new Raccoom.Xml.Rss.RssFactory();
            Raccoom.Xml.Rss.RssChannel _rssChannel;
            Raccoom.Xml.Opml.OpmlOutline _outline;

            internal TreeNodeOpml(Raccoom.Xml.Opml.OpmlOutline outline)
                : base(outline.Text)
            {
                _outline = outline;
            }
            internal Raccoom.Xml.Rss.RssChannel RssChannel
            {
                get
                {
                    if (_rssChannel == null)
                    {
                        string filename = System.IO.Path.Combine(RssReader.CacheDirectory, System.IO.Path.GetFileName(Url.GetHashCode().ToString()));
                        filename= System.IO.Path.ChangeExtension(filename, "xml");
                        bool isCache = System.IO.File.Exists(filename);
                        _rssChannel = rssFactory.Read(isCache ? filename : Url) as Raccoom.Xml.Rss.RssChannel;
                        //
                        if (!isCache)
                        {
                            rssFactory.Write(filename, _rssChannel);
                        }
                        
                    }
                    return _rssChannel;
                }
            }
            internal string Url
            {
                get
                {
                    return string.IsNullOrEmpty(_outline.Url) ? _outline.XmlUrl : _outline.Url;
                }
            }
            internal Raccoom.Xml.Opml.OpmlOutline Outline
            {
                get
                {
                    return _outline;
                }
            }
        } 
        #endregion

        private void openToolStripButton_Click(object sender, EventArgs e)
        {   
            _tvRssChannels_NodeMouseClick(_tvRssChannels, new TreeNodeMouseClickEventArgs(this._tvRssChannels.SelectedNode, MouseButtons.Left,1,1,1));
        }

        
    }
}