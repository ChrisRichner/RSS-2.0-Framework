using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raccoom.Xml.Opml;

namespace Raccoom.Xml.UnitTest
{
    [TestClass()]
    public class OpmlTest
    {  
        public OpmlTest()
        {
        }

        [TestMethod]
        public void FullDocumentCreateAddBodyNow()
        {
            OpmlFactory factory = new OpmlFactory();
            factory.Writer = new OpmlXmlWriter();
            factory.Reader = new OpmlXmlReader();
            //
            OpmlDocument doc = new OpmlDocument();
            doc.Head.DateCreated = DateTime.Now;
            doc.Head.DateModified = DateTime.Now;
            doc.Head.ExpansionState = "ExpansionState";
            doc.Head.OwnerEmail = GenericAssert.EmailAdress;
            doc.Head.OwnerId = GenericAssert.Uri;
            doc.Head.OwnerName = "OwnerName";
            doc.Head.Title = "Title";
            doc.Head.VertScrollState = 10;
            doc.Head.WindowBottom = 10;
            doc.Head.WindowLeft = 10;
            doc.Head.WindowRight = 10;
            doc.Head.WindowTop = 10;
            //
            int itemCount = 3;
            for (int i = 0; i < itemCount; i++)
            {
                // root outline
                OpmlOutline outline = CreateOutline();
                // sub outlines
                for (int j = 0; j < itemCount; j++)
                {
                    OpmlOutline o = CreateOutline();
                    outline.Items.Add(o);
                    o.Items.Add(CreateOutline());
                }
                //
                doc.Body.Items.Add(outline);
                // single outline
                doc.Body.Items.Add(CreateOutline());
            }
            //
            string filename = System.IO.Path.GetFullPath("opmldocument.xml");
            factory.Write(filename, doc);

            //
            OpmlDocument doc1 = factory.Read(filename) as OpmlDocument;
            //
            EqualValues(doc, doc1);
        }

        [TestMethod]
        public void FullDocumentCreateAddBodyAfter()
        {
            OpmlFactory factory = new OpmlFactory();
            factory.Writer = new OpmlXmlWriter();
            factory.Reader = new OpmlXmlReader();
            //
            OpmlDocument doc = new OpmlDocument();
            doc.Head.DateCreated = DateTime.Now;
            doc.Head.DateModified = DateTime.Now;
            doc.Head.ExpansionState = "ExpansionState";
            doc.Head.OwnerEmail = GenericAssert.EmailAdress;
            doc.Head.OwnerId = GenericAssert.Uri;
            doc.Head.OwnerName = "OwnerName";
            doc.Head.Title = "Title";
            doc.Head.VertScrollState = 10;
            doc.Head.WindowBottom = 10;
            doc.Head.WindowLeft = 10;
            doc.Head.WindowRight = 10;
            doc.Head.WindowTop = 10;
            //
            int itemCount = 3;
            OpmlOutline outlineRoot = CreateOutline();
            for (int i = 0; i < itemCount; i++)
            {
                // root outline
                OpmlOutline outline = CreateOutline();
                // sub outlines
                for (int j = 0; j < itemCount; j++)
                {
                    OpmlOutline o = CreateOutline();
                    outline.Items.Add(o);
                    o.Items.Add(CreateOutline());
                }
                //
                outlineRoot.Items.Add(outline);
                // single outline
                outlineRoot.Items.Add(CreateOutline());
            }
            doc.Body.Items.Add(outlineRoot);
            //
            foreach (OpmlOutline outline in doc.Body.Items)
            {
                outline.IsBreakpoint.CompareTo(null);
            }
            //
            string filename = System.IO.Path.GetFullPath("opmldocument.xml");
            factory.Write(filename, doc);
            //
            OpmlDocument doc1 = factory.GetDocument(filename) as OpmlDocument;
            //
            EqualValues(doc, doc1);
        }

        [TestMethod]
        public void ParseMsdnOpml()
        {
            Raccoom.Xml.Opml.OpmlFactory factory = new Raccoom.Xml.Opml.OpmlFactory();
            factory.Reader = new Opml.OpmlXmlReader();
            //
            Raccoom.Xml.Opml.IOpmlDocument document  = factory.GetDocument("http://blogs.msdn.com/Opml.aspx");
            //
            Assert.IsTrue(document.Body.Items.Count == 1);
            TestContext.WriteLine("{0} outlines parsed", document.Body.Items.Count);
        }

        [TestMethod]
        public void CollectionAddRemove()
        {
            // document
            // - outline
            //   - outline
            OpmlDocument doc = new OpmlDocument();
            // add item
            OpmlOutline item = new OpmlOutline();
            doc.Body.Items.Add(item);
            Assert.IsNotNull(item.Document);
            Assert.IsNull(item.Parent);
            // add subitem
            OpmlOutline subItem = new OpmlOutline();
            item.Items.Add(subItem);
            Assert.IsNotNull(subItem.Document);
            Assert.IsNotNull(subItem.Parent);
            Assert.AreEqual(subItem.Document, doc);
            Assert.AreEqual(subItem.Parent, item);
            // remove subitem
            subItem.Remove();
            //item.Items.Remove(subItem);
            Assert.IsNull(subItem.Parent);
            Assert.IsNull(subItem.Document);
            // remove item
            item.Remove();
            //doc.OpmlBody.Items.Remove(item);
            Assert.IsNull(item.Document);
            Assert.IsNull(item.Parent);

        }
        [TestMethod]
        public void UserLandSpecSamples()
        {
            OpmlFactory opmlFactory = new OpmlFactory();
            opmlFactory.Reader = new OpmlXmlReader();
            OpmlDocument document = opmlFactory.GetDocument("http://hosting.opml.org/dave/spec/subscriptionList.opml") as OpmlDocument;
            //
            document = opmlFactory.GetDocument("http://hosting.opml.org/dave/spec/states.opml") as OpmlDocument;
            Assert.AreEqual(document.Body.Items.Count, 1);
            Assert.AreEqual(document.Body.Items[0].Text, "United States");
            Assert.AreEqual(document.Body.Items[0].Items.Count, 8);
            //
            document = opmlFactory.GetDocument("http://hosting.opml.org/dave/spec/directory.opml") as OpmlDocument;
            Assert.AreEqual(document.Body.Items.Count , 8);

        }
        #region private interface
        private void EqualValues(object obj1, object obj2)
        {
            foreach (System.Reflection.PropertyInfo pi in obj1.GetType().GetProperties())
            {
                if (pi.Name == "Document" || pi.Name == "Parent" | pi.Name == "InclusionDocument")
                {
                    continue;
                }
                else if (pi.PropertyType.BaseType == typeof(System.Collections.CollectionBase))
                {
                    OpmlOutlineCollection col = pi.GetValue(obj1, null) as OpmlOutlineCollection;
                    OpmlOutlineCollection col1 = pi.GetValue(obj2, null) as OpmlOutlineCollection;
                    for (int i = 0; i < col.Count; i++)
                    {
                        EqualValues(col[i], col1[i]);
                    }
                }
                else if (pi.PropertyType.Assembly.Equals(typeof(OpmlDocument).Assembly))
                {
                    EqualValues(pi.GetValue(obj1, null), pi.GetValue(obj2, null));
                }
                else if (pi.PropertyType == typeof(DateTime))
                {
                    Assert.AreEqual(System.Convert.ToString(pi.GetValue(obj1, null)), System.Convert.ToString(pi.GetValue(obj2, null)), pi.ReflectedType.Name + " " + pi.Name);
                }
                else if (pi.PropertyType == typeof(string))
                {
                    Assert.AreEqual(pi.GetValue(obj1, null), pi.GetValue(obj2, null), pi.ReflectedType.Name + " " + pi.Name);
                }

            }
        }
        private OpmlOutline CreateOutline()
        {
            OpmlOutline outline = new OpmlOutline();
            outline.Description = "Description";
            outline.HtmlUrl = GenericAssert.Uri;
            outline.IsBreakpoint = false;
            outline.IsComment = false;
            outline.Text = "Text";
            outline.Category = "Category1, Category2";
            outline.Created = DateTime.Now;
            outline.Version = "2.0";
            outline.Type = "rss";
            outline.Url = GenericAssert.Uri;
            outline.Language = System.Threading.Thread.CurrentThread.CurrentCulture;
            return outline;
        }
        #endregion

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        private TestContext _testContext;

        public TestContext TestContext
        {
            get { return _testContext; }
            set { _testContext = value; }
        }

        #endregion
    }
   
}
