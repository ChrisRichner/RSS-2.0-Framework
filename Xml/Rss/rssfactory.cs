using System;
using System.Xml;
using System.Xml.Serialization;
using Raccoom.Xml.ComponentModel;

namespace Raccoom.Xml.Rss
{
    /// <summary>
    /// GenericRssFactory implements the operations to read and write rss related objects
    /// </summary>
    public abstract class GenericRssFactory<TReader, TWriter>
        where TReader : IRssReader, new()
        where TWriter : IRssWriter, new()
    {
        #region fields
        /// <summary>web proxy to use when get feeds from da web</summary>
        System.Net.WebProxy _webProxy;
        TReader _reader;
        TWriter _writer;
        #endregion

        #region constructors

        /// <summary>Initializes a new instance of GenericRssFactory</summary>
        protected GenericRssFactory()
        {
            _reader = new TReader();
            _writer = new TWriter();
        }

        #endregion

        #region public interface
        /// <summary>
        /// Gets or sets the WebProxy that is used to connect to the network, can be null
        /// </summary>
        public System.Net.WebProxy Proxy
        {
            get
            {
                return _webProxy;
            }

            set
            {
                _webProxy = value;
            }
        }
        public void Write(string feed, IRssChannel channel)
        {
            if (channel == null) throw new System.ArgumentNullException("channel");
            if (string.IsNullOrEmpty(feed)) throw new System.ArgumentNullException("feed");
            _writer.Write(feed, channel);
        }
        public IRssChannel Read(string feed)
        {
            if (string.IsNullOrEmpty(feed)) throw new System.ArgumentNullException("feed");
            return _reader.Read(feed);
        }
        public TWriter Writer
        {
            set
            {
                _writer = value;
            }
            get
            {
                return _writer;
            }
        }
        public TReader Reader
        {
            set
            {
                _reader = value;
            }
            get
            {
                return _reader;
            }
        }
        /// <summary>
        /// Gets (create) the requested <see cref="IRssChannel"/> instance
        /// </summary>
        /// <param name="feed">The URI or filename of the resource to receive the data.</param>
        /// <returns>The requested see cref="IRssChannel" instance</returns>		
        public virtual IRssChannel Create(string feed)
        {
            System.Diagnostics.Debug.Assert(_reader != null);
            //
            return _reader.Read(feed);
        }
        /// <summary>
        /// Create a new <see cref="IRssChannel"/> instance 
        /// </summary>
        /// <returns></returns>
        public virtual IRssChannel Create()
        {
            return new RssChannel();
        }

        #endregion
    }
    public class RssFactory : GenericRssFactory<Rss.RssXmlReader, Rss.RssXmlWriter> { }

    /// <summary>
    /// GenericRssXmlWriter implements the write operations needed
    /// </summary>
    public class RssXmlWriter : IRssWriter
    {
        #region IRssWriter Members
        public virtual void Write(string feed, IRssChannel channel)
        {
            using (System.IO.FileStream stream = new System.IO.FileStream(feed, System.IO.FileMode.Create))
            {
                Write(stream, channel);
            }
        }
        public virtual void Write(System.IO.Stream stream, IRssChannel channel)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (channel == null) throw new ArgumentNullException("channel");
            //
            System.Reflection.AssemblyName assemblyName = this.GetType().Assembly.GetName();
            string copyright = string.Format("Generated by {0} {1}, Copyright � 2007 by Christoph Richner. All rights reserved. http://www.raccoom.net", assemblyName.Name, assemblyName.Version);
            //
            System.Xml.XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = System.Text.Encoding.UTF8;            
            //
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {   
                //Write the XML delcaration. 
                writer.WriteStartDocument();
                // copyright
                writer.WriteComment(copyright);
                // rss start
                writer.WriteStartElement("rss");
                writer.WriteAttributeString(null, "version", null, "2.0");
                // namespace handling
                System.Xml.Serialization.XmlSerializerNamespaces namespaces = ((IExtendableObject)channel).Xmlns;
                if (namespaces != null)
                {
                    foreach (System.Xml.XmlQualifiedName entry in namespaces.ToArray())
                    {
                        if (entry.Namespace == "http://www.w3.org/2000/xmlns/") continue;
                        //
                        writer.WriteAttributeString("xmlns", entry.Name, null, entry.Namespace);
                        
                    }
                }
                // serialize the content
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializerFactory().CreateSerializer(channel.GetType());
                ser.Serialize(writer, channel, namespaces);
                // end rss
                writer.WriteEndElement();
                // end document
                writer.WriteEndDocument();
            }
        }
        public virtual string Write(IRssChannel channel)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            //
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                Write(stream, channel);
                //
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                //
                using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        #endregion
    }
    /// <summary>
    /// GenericRssXmlReader implements the read operations needed
    /// </summary>
    public class RssXmlReader : SyndicationObjectParser, IRssReader
    {
        #region fields
        /// <summary>web proxy to use when get feeds from da web</summary>
        System.Net.WebProxy _webProxy;
        #endregion

        #region public interface
        /// <summary>
        /// Gets or sets the WebProxy that is used to connect to the network, can be null
        /// </summary>
        public System.Net.WebProxy Proxy
        {
            get
            {
                return _webProxy;
            }

            set
            {
                _webProxy = value;
            }
        }
        public virtual IRssChannel Read(string feed)
        {
            System.Xml.XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreWhitespace = true;
            xmlReaderSettings.DtdProcessing = DtdProcessing.Ignore;
            //
            using (XmlReader reader = System.Xml.XmlReader.Create(feed, xmlReaderSettings))
            {
                return Read(reader);
            }
        }
        public virtual IRssChannel Read(System.IO.Stream stream)
        {
            System.Xml.XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreComments = true;
            xmlReaderSettings.IgnoreWhitespace = true;
            xmlReaderSettings.DtdProcessing = DtdProcessing.Ignore;
            //
            using (XmlReader reader = System.Xml.XmlReader.Create(stream, xmlReaderSettings))
            {
                return Read(reader);
            }
        }
        protected virtual IRssChannel Read(XmlReader xmlReader)
        {
            IRssChannel channel = new RssChannel();
            //
            Parse(channel, xmlReader);
            //
            return channel;
        }
        #endregion
    }

}