
// Copyright © 2009 by Christoph Richner. All rights are reserved.
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//
// website http://www.raccoom.net, email support@raccoom.net, msn chrisdarebell@msn.com

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raccoom.Xml.Rss;
using Raccoom.Xml.ComponentModel;
//using Raccoom.Xml.Rss.Extensions.Podcast;

namespace Raccoom.Xml.UnitTest
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	[TestClass]    
	public class RssTest
	{
		#region fields
			
		#endregion
        public RssTest()
		{			
		}
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
        [TestMethod]
        public void RssExtensionTest()
        {
            RssFactory factory = new RssFactory();
            //
            RssChannel channel = (RssChannel)factory.Read("http://www.engadget.com/rss.xml");
            factory.Write("extension.xml", channel);
            //
            RssChannel channel1 = factory.Read("extension.xml") as RssChannel;
            // compare
            GenericAssert.Instance.EqualValues<RssItem>(channel, channel1);
        }
		/// <summary>
		/// Create a new channel instance and fill all properties with data
		/// </summary>
		[TestMethod]        
		public void FullChannelCreate()
		{
            RssFactory factory = new RssFactory();
            //
            RssChannel channel = factory.Create() as RssChannel;
            //
            IExtendableObject extensions = channel as IExtendableObject;
            extensions.Xmlns.Add("itunes", "http://www.itunes.com/dtds/podcast-1.0.dtd");
            extensions.Xmlns.Add("cf", "http://www.microsoft.com/schemas/rss/core/2005");
            
            //Podcast podcast = channel.Extensions.GetExtension<Podcast>();
            //Assert.Equals(true, channel.Extensions.IsDefined<Podcast>());
            //podcast.Author = "";
            //podcast.Block = Block.Yes;
            //
            channel.Category.Add(new RssCategory("cat", "domain"));// "RssCategory";
            channel.Cloud.Domain = GenericAssert.Uri;
			channel.Cloud.Path = "mypath/doc";
			channel.Cloud.Port = 80;
			channel.Cloud.Protocol = CloudProtocol.Soap;
			channel.Cloud.RegisterProcedure = "myprocedure";
			channel.Copyright = "Copyright";
			channel.Description = "Description";
            channel.Docs = GenericAssert.Uri;
			channel.Generator = "Generator";
			channel.Image.Description = "Description";
			channel.Image.Height = 30;
            channel.Image.Link = GenericAssert.Uri;
			channel.Image.Title = "Title";
            channel.Image.Url = GenericAssert.Uri;
			channel.Image.Width = 80;
			channel.Language = System.Globalization.CultureInfo.CurrentCulture;
			channel.LastBuildDate = DateTime.Now;
            channel.Link = GenericAssert.Uri;
			channel.ManagingEditor = GenericAssert.EmailAdress;
			//channel.PubDate = RFCDateTime;
			channel.PubDate = DateTime.Now;
			channel.Rating = "Rating";
            //channel.SkipDays = SkipDays.Monday | SkipDays.Tuesday;
            //channel.SkipHours = new int[] {1,2,3,4,5,6};
			channel.TextInput.Description = "Description";
            channel.TextInput.Link = GenericAssert.Uri;
			channel.TextInput.Name = "Name";
			channel.TextInput.Title = "Title";
			channel.Title = "Title";
            channel.Ttl = 10;
			channel.WebMaster = GenericAssert.EmailAdress;
			//
            RssItem item = channel.Items.Add();
            //PodcastItem podcastItem = item.Extensions.GetExtension<PodcastItem>();
            //Assert.Equals(true, item.Extensions.IsDefined<PodcastItem>());
            //podcastItem.Author = "Author";
            //podcastItem.Block = Block.No;
            //
			item.Author = GenericAssert.EmailAdress;
			item.Category.Add(new RssCategory("Category", GenericAssert.Uri));
			item.Comments = "Comments";
			item.Description = "Description";
			item.Enclosure.Length = 100;
			item.Enclosure.Type = "text/xml";
            item.Enclosure.Url = GenericAssert.Uri;
            item.Guid.Value = GenericAssert.Uri;
            item.Link = GenericAssert.Uri;
			item.PubDate = DateTime.Now;
            item.Source.Url = GenericAssert.Uri;
			item.Source.Value = "Source";
			item.Title = "Title";			
			channel.Items.Add(item);
			// save channel
			string filename = System.IO.Path.GetFullPath("channelfull.xml");
            factory.Write(filename, channel);
			// load channel
			RssChannel channel1 = factory.Create(filename) as RssChannel;
			// compare
			GenericAssert.Instance.EqualValues<RssItem>(channel, channel1);
		}		
		/// <summary>
		/// Transforms the channel 
		/// </summary>
        //[TestMethod]
        //public void FullChannelTransform()
        //{
        //    string filename = System.IO.Path.GetFullPath("channelfull.xml");
        //    RssChannel channel = new RssChannel(new Uri(filename));
        //    using(System.IO.Stream styleSheetStream = this.GetType().Assembly.GetManifestResourceStream("Test.Resources.rss.xslt"))
        //    {				
        //        Helper.TestPersistentManager(channel, styleSheetStream);
        //    }
        //}
		/// <summary>
		/// Creates a new channel with only a few properties set
		/// </summary>
        [TestMethod]
		public void EmptyElementChannelBuild()
		{
            RssFactory factory = new RssFactory();
            //
			RssChannel channel = new RssChannel();
			channel.Category.Add(new RssCategory("Cat1", GenericAssert.Uri));
            channel.Category.Add(new RssCategory("Cat1", GenericAssert.Uri));
            channel.Category.Add(new RssCategory("Cat1", GenericAssert.Uri));
			channel.Copyright = "Copyright";
			channel.Description = "Description";
            channel.Docs = GenericAssert.Uri;
			channel.Generator = "Generator";
			channel.Language = System.Globalization.CultureInfo.CurrentCulture;
			channel.LastBuildDate = DateTime.Now;
            channel.Link = GenericAssert.Uri;
            channel.ManagingEditor = GenericAssert.EmailAdress;						
			channel.PubDate = DateTime.Now;
			channel.Rating = "Rating";
			//channel.SkipDays = SkipDays.Monday;
			//channel.SkipHours = "2";
			channel.Title = "Title";
            channel.Ttl = 10;
            channel.WebMaster = GenericAssert.EmailAdress;
			//
			RssItem item  = channel.Items.Add();
            item.Author = GenericAssert.EmailAdress;
            item.Category.Add(new RssCategory("Category", GenericAssert.Uri));
			item.Comments = "Comments";
			item.Description = "Description";
			//item.RssGuid.Value = Uri;
			item.Guid.IsPermaLink = false;
            item.Link = GenericAssert.Uri;
			item.PubDate = DateTime.Now;
			//item.RssSource.Value = "RssSource";
            item.Source.Url = GenericAssert.Uri;
			item.Title = "Title";			
			channel.Items.Add(item);
			//
			string filename = System.IO.Path.GetFullPath("channelEmtpyElements.xml");
            factory.Write(filename, channel);
			//
			RssChannel channel1 = factory.Create(filename) as RssChannel;
			//
            GenericAssert.Instance.EqualValues<RssItem>(channel, channel1);
			
		}
		/// <summary>
		/// Loads and parse a embedded xml resource (feed) that contains empty elements
		/// </summary>
		[TestMethod]
		public void EmptyElementChannel()
		{
            RssFactory factory = new RssFactory();
            //
			RssChannel channel = null;
            channel = factory.Create("rsschannel.rss") as RssChannel;
			Assert.IsNotNull(channel.Image.Url);
			Assert.IsNotNull(channel.Description);
			Assert.IsNotNull(channel.Items[0].PubDate);
			Assert.IsNotNull(channel.Items[1].PubDate);
			Assert.IsNotNull(channel.Items[2].PubDate);
			Assert.IsNotNull(channel.Items[3].Description);
			Assert.IsNotNull(channel.Items[4].Description);
			Assert.IsNotNull(channel.Items[4].PubDate);
			Assert.IsNotNull(channel.Items[5].Link);
			Assert.IsNotNull(channel.Items[5].PubDate);
		}

		/// <summary>
		/// Get feeds from the internet
		/// </summary>
		[TestMethod]
		public void RssTop100()
		{
            Raccoom.Xml.Opml.OpmlFactory factory = new Raccoom.Xml.Opml.OpmlFactory();
            factory.Reader = new Raccoom.Xml.Opml.OpmlXmlReader();
            RssFactory rssFactory = new RssFactory();
            int counter = 0;
            RssChannel channel, channel1;
            //            
            foreach (Raccoom.Xml.Opml.OpmlOutline outline in factory.GetDocument("http://share.opml.org/opml/top100.opml").Body.Items)
            {
                if (counter == 100) break;
                //
                channel = rssFactory.Create(outline.XmlUrl) as RssChannel;
                //
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    rssFactory.Writer.Write(stream, channel);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    channel1 = rssFactory.Reader.Read(stream) as RssChannel;
                    //
                    GenericAssert.Instance.EqualValues<RssItem>(channel, channel1);
                }
                counter++;
            }
		}
        [TestMethod]
        public void HarvardSpecSamples()
        {
            RssFactory rssFactory = new RssFactory();
            //
            rssFactory.Create("http://cyber.law.harvard.edu/blogs/gems/tech/sampleRss20.xml");
            rssFactory.Create("http://cyber.law.harvard.edu/blogs/gems/tech/sampleRss091.xml");
            rssFactory.Create("http://cyber.law.harvard.edu/blogs/gems/tech/sampleRss092.xml");
        }
		/// <summary>
		/// RssItem knows his RssChannel, after adding, when removing the RssItem.RssChannel property have to be null
		/// </summary>
		[TestMethod]
		public void CollectionAddRemove()
		{
			RssChannel channel = new RssChannel();
			RssItem item = new RssItem();
			channel.Items.Add(item);
			// must be equal
            Assert.AreSame(((ISyndicationObjectRelationItem)channel.Items[0]).Parent, channel);
			item.Remove();			
			// must be null
            Assert.IsNull(((ISyndicationObjectRelationItem)item).Parent);
		}

		#region private interface		
        
		#endregion
	}
}
