using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Raccoom.Xml.UnitTest
{
    internal class GenericAssert : Raccoom.Xml.ComponentModel.SyndicationObjectParser
    {
        #region fields
        static GenericAssert _instance;
        public const string Uri = "http://www.mydomain.com";
        public const string EmailAdress = "someone@somewhere.com";
        public const string RFCDateTime = "Mon, 03 Nov 2003 14:00:00 GMT";	
        #endregion

        #region ctor
        private GenericAssert() { }
        #endregion

        #region public interface
        public static GenericAssert Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GenericAssert();
                }
                return _instance;
            }
        }
        #endregion

        #region internal interface
        internal System.Collections.Generic.Dictionary<string, System.Reflection.PropertyInfo> Get(object target)
        {
            return base.GetComplexProperties(target);
        }
        internal void EqualValues<TItem>(Raccoom.Xml.ComponentModel.SyndicationObjectBase obj1, Raccoom.Xml.ComponentModel.SyndicationObjectBase obj2)
        where TItem : Raccoom.Xml.ComponentModel.SyndicationObjectBase, Raccoom.Xml.ComponentModel.ISyndicationObjectRelationItem, new()
        {
            // skip not specified elements
            if (!obj1.Specified) return;
            //
            System.Type parentCollectionType = typeof(Raccoom.Xml.ComponentModel.SyndicationObjectRelationCollection<>);
            System.Collections.Generic.Dictionary<string, System.Reflection.PropertyInfo> _complexProperties = Get(obj1);
            //
            foreach (System.Reflection.PropertyInfo pi in obj1.GetType().GetProperties())
            {
                if (pi.Name == "Channel" | pi.Name == "Errors" | pi.Name == "LastBuildDate" | pi.Name == "PubDate" | pi.Name == "ExtendedContent")
                {
                    continue;
                }
                // rss specific
                else if (pi.PropertyType == typeof(Raccoom.Xml.ComponentModel.SyndicationObjectRelationCollection<Rss.RssItem>))
                {
                    Raccoom.Xml.ComponentModel.SyndicationObjectRelationCollection<Rss.RssItem> col1 = pi.GetValue(obj1, null) as Raccoom.Xml.ComponentModel.SyndicationObjectRelationCollection<Rss.RssItem>;
                    Raccoom.Xml.ComponentModel.SyndicationObjectRelationCollection<Rss.RssItem> col2 = pi.GetValue(obj2, null) as Raccoom.Xml.ComponentModel.SyndicationObjectRelationCollection<Rss.RssItem>;
                    //
                    for (int i = 0; i < col1.Count; i++)
                    {
                        EqualValues<Rss.RssItem>(col1[0], col2[0]);
                    }
                    
                }
                else if (pi.PropertyType == typeof(Raccoom.Xml.ComponentModel.SyndicationObjectRelationCollection<Rss.RssCategory>))
                {
                    Raccoom.Xml.ComponentModel.SyndicationObjectRelationCollection<Rss.RssCategory> col1 = pi.GetValue(obj1, null) as Raccoom.Xml.ComponentModel.SyndicationObjectRelationCollection<Rss.RssCategory>;
                    Raccoom.Xml.ComponentModel.SyndicationObjectRelationCollection<Rss.RssCategory> col2 = pi.GetValue(obj2, null) as Raccoom.Xml.ComponentModel.SyndicationObjectRelationCollection<Rss.RssCategory>;
                    //
                    for (int i = 0; i < col1.Count; i++)
                    {
                        EqualValues<Rss.RssCategory>(col1[0], col2[0]);
                    }
                }               
                else if (_complexProperties.ContainsValue(pi))
                {
                    EqualValues<TItem>(pi.GetValue(obj1, null) as Raccoom.Xml.ComponentModel.SyndicationObjectBase, pi.GetValue(obj2, null) as Raccoom.Xml.ComponentModel.SyndicationObjectBase);
                }
                else if (pi.PropertyType == typeof(DateTime))
                {
                    Assert.AreEqual(System.Convert.ToString(pi.GetValue(obj1, null)), System.Convert.ToString(pi.GetValue(obj2, null)), pi.ReflectedType.Name + " " + pi.Name);
                }
                else if (pi.PropertyType == typeof(System.Xml.XmlNode[]))
                {
                    // compare rss extension content
                    System.Xml.XmlNode[] n1 = (System.Xml.XmlNode[])pi.GetValue(obj1, null);
                    System.Xml.XmlNode[] n2 = (System.Xml.XmlNode[])pi.GetValue(obj2, null);
                    //
                    if (n1 != null)
                    {
                        for (int i = 0; i < n1.Length; i++)
                        {
                            Assert.AreEqual(n1[i].Name, n2[i].Name, pi.ReflectedType.Name + " " + pi.Name);
                        }
                    }
                }
                else
                {
                    Assert.AreEqual(pi.GetValue(obj1, null), pi.GetValue(obj2, null), pi.ReflectedType.Name + " " + pi.Name);
                }
                

            }
        }
        #endregion
    }
}
