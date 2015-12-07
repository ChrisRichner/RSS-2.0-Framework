using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Raccoom.Xml.DemoWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>

    public partial class Window1 : System.Windows.Window
    {
        #region fields
        Raccoom.Xml.Rss.RssFactory rssFactory = new Raccoom.Xml.Rss.RssFactory();
        Raccoom.Xml.Opml.OpmlFactory factory = new Raccoom.Xml.Opml.OpmlFactory();
        #endregion        

        public Window1()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            //
            factory.Reader = new Raccoom.Xml.Opml.OpmlXmlReader();
            //
            
            //
            Raccoom.Xml.Opml.IOpmlDocument document = factory.GetDocument("http://share.opml.org/opml/top100.opml");
            listBox1.ItemsSource = document.Body.Items;
            listBox1.SelectionChanged += new SelectionChangedEventHandler(listBox1_SelectionChanged);
        }

        void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Raccoom.Xml.Opml.OpmlOutline outline = e.AddedItems[0] as Raccoom.Xml.Opml.OpmlOutline;
            if (outline == null) return;
            //
            this.listBox2.ItemsSource =  ((Raccoom.Xml.Rss.RssChannel)rssFactory.Read(outline.XmlUrl)).Items;
        }
    }
}