using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raccoom.Xml.Atom;

namespace Raccoom.Xml.UnitTest
{
    [TestClass]    
    public class Atom
    {
        [TestMethod]
        public void CreateBasicFeed()
        {
            Raccoom.Xml.Atom.AtomFeed feed = new AtomFeed();
            //
            AtomContentConstruct link = new AtomContentConstruct();
            link.Type = AtomContentType.Text;
            link.Value = "Link1";
            feed.Links.Add(link);
            link.Remove();
            //
            link = new AtomContentConstruct();
            link.Type = AtomContentType.Text;
            link.Value = "Link2";
            feed.Links.Add(link);
            Assert.AreEqual(feed.Links.Count, 1);
            feed.Links.Add(link);
            Assert.AreEqual(feed.Links.Count, 2);
            //
            AtomPersonConstruct author = new AtomPersonConstruct();
            author.XmlLanguage = System.Globalization.CultureInfo.CurrentCulture;
            author.Name = "Christoph Richner";
            author.Email = "support@raccoom.net";
            author.Uri = new Uri("http://www.raccoom.net");
            feed.Authors.Add(author);
            Assert.AreEqual(feed.Authors.Count, 1);
            author.Remove();
            Assert.AreEqual(feed.Authors.Count, 0);
            feed.Authors.Add(author);
            Assert.AreEqual(feed.Authors.Count, 1);
            //
            feed.Updated = DateTime.Now;
            //
            AtomEntry entry = new AtomEntry();
            entry.Contributors.Add(author);
            feed.Entries.Add(entry);
            //
            entry.Remove();
            feed.Entries.Add(entry);
            //
            System.Xml.Serialization.XmlSerializer ser;
            try
            {
                ser = new System.Xml.Serialization.XmlSerializerFactory().CreateSerializer(feed.GetType());
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create("atom.xml"))
                {
                    System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();
                    //ns.Add("atom", "http://www.w3.org/2005/Atom");
                    ser.Serialize(writer, feed, ns);                    
                }
                //
                AtomParser parser = new AtomParser();
                using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create("atom.xml"))
                {
                    parser.Parse(reader);
                }
                
            }
            catch (Exception ex)
            {
                Exception e = ex.GetBaseException();               
            }            
        }
        class AtomParser : Raccoom.Xml.ComponentModel.SyndicationObjectParser
        {
            internal AtomFeed Parse(System.Xml.XmlReader reader)
            {
                AtomFeed feed = new AtomFeed();
                base.Parse(feed, reader);
                return feed;
            }
        }
    }
}
