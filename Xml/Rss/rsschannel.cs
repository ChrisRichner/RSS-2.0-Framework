using System;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Net;
using Raccoom.Xml.ComponentModel;

namespace Raccoom.Xml.Rss
{	
	/// <summary>
	/// RSS RssChannel element which contains information about the channel (metadata) and its contents.
	/// </summary>
	/// <remarks><a href="http://blogs.law.harvard.edu/tech/rss" target="_blank">RSS 2.0 Specification</a></remarks>		
	/// <example>
	/// <h4>Consume feeds</h4>
	/// This sample shows how to consume the code project "Last 10 updates (category: All Topics)" rss feed.
	/// <code>
	/// Raccoom.Xml.RssChannel myRssChannel = new Raccoom.Xml.RssChannel(new Uri("http://www.codeproject.com/webservices/articlerss.aspx?cat=1"));
	/// // write the channel title to the standard output stream. 
	/// System.Console.WriteLine(myRssChannel.Title);
	/// // write each item's title to the standard output stream. 
	/// foreach(Raccoom.Xml.RssItem item in myRssChannel.Items)
	/// {
	/// 	System.Console.WriteLine(item.Title);
	/// }
	/// </code>
	/// This sample shows how to create rss feeds
	/// <code>
	/// Raccoom.Xml.RssChannel myRssChannel = new Raccoom.Xml.RssChannel();
	/// myRssChannel.Title = "Sample rss feed";
	/// myRssChannel.Copyright = "(c) 2003 by Christoph Richner";
	/// // add item to channel
	/// Raccoom.Xml.RssItem item = new Raccoom.Xml.RssItem();
	/// item.Title = "Raccoom RSS 2.0 Framework announced";
	/// item.Link = "http://jerrymaguire.sytes.net";
	/// myRssChannel.Items.Add(item);
	/// </code>
	/// <h4>Save feeds</h4>
	/// This example saves the channel to a file
	/// <code>
	/// // save feed to local storage
	/// myRssChannel.Save(@"c:\cp.xml");
	/// </code>
	/// This example saves the channel to <c>System.IO.Stream</c>.
	/// <code>
	/// // create stream
	/// System.IO.MemoryStream stream = new System.IO.MemoryStream();
	/// myRssChannel.Write(stream);
	/// stream.Close();
	/// </code>	
	/// This sample shows how to publish your feed (Default Proxy)
	/// <code>
	/// // password-based authentication for web resource
	/// System.Net.NetworkCredential providerCredential = new System.Net.NetworkCredential("username", "password", "domain");
	/// // use default system proxy
	/// Uri uri = new Uri("http://domain.net");
	/// myChannel.Publish(uri, null, "POST", providerCredential);
	/// </code>
	/// This sample shows how to publish your feed (Custom Proxy)
	/// <code>
	/// // password-based authentication for web resource
	/// System.Net.NetworkCredential providerCredential = new System.Net.NetworkCredential("username", "password", "domain");
	/// // password-based authentication for web proxy
	/// System.Net.NetworkCredential proxyCredential = new System.Net.NetworkCredential("username", "password", "domain");
	/// // create custom proxy
	/// System.Net.WebProxy webProxy = new System.Net.WebProxy("http://proxyurl:8080",false);
	/// webProxy.Credentials = proxyCredential;
	/// // publish
	/// myChannel.Publish(uri, webProxy, "POST", providerCredential);
	/// </code>
	/// <h4>Transform feeds</h4>
	/// This sample shows how to consume and transform (XSLT/CSS) the code project feed, where transform.xslt is a custom xslt file.
	/// <code>
	/// // consume rss feed
	/// RssChannel myChannel = new RssChannel(new Uri("http://www.codeproject.com/webservices/articlerss.aspx?cat=3"));
	/// // transform to stream
	///	System.IO.MemoryStream memoryStream = myChannel.Transform(new System.Xml.XmlTextReader("transform.xslt"));
	///	// transform to html output file
	///	myChannel.Transform(new System.Xml.XmlTextReader("transform.xslt"), "myChannel.htm");
	///	// transform to html and xml output file
	///	myChannel.Transform(new System.Xml.XmlTextReader("transform.xslt"), "channel.xml", "channel.htm");
	///	</code>
	/// </example>
	[System.Runtime.InteropServices.ComVisible(true), System.Runtime.InteropServices.Guid("026FF54F-98DF-4879-A355-880832C49A1C")]
	[System.Runtime.InteropServices.ClassInterface(System.Runtime.InteropServices.ClassInterfaceType.None)]
	[System.Runtime.InteropServices.ProgId("Raccoom.RssChannel")]
	//[System.Xml.Serialization.XmlRoot()]
	[System.Xml.Serialization.XmlTypeAttribute("channel")]
	[Serializable]
	[System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class RssChannel : ComponentModel.SyndicationObjectBase, IRssChannel, ISyndicationObjectRelationOwner, IParseableObject
	{
		#region fields
		private string _version;
		/// <summary>Title</summary>
		private string _title;
		/// <summary>Description</summary>
		private string _description;
		/// <summary>Link</summary>
		private string _link;
		/// <summary>Language</summary>
		private System.Globalization.CultureInfo _language;
		/// <summary>Copyright</summary>
		private string _copyright;
		/// <summary>ManagingEditor</summary>
		private string _managingEditor;
		/// <summary>WebMaster</summary>
		private string _webMaster;
		/// <summary>PubDate</summary>
		private DateTime _pubDate;
		/// <summary>LastBuildDate</summary>
		private DateTime _lastBuildDate;
		/// <summary>RssCategory</summary>
        private SyndicationObjectRelationCollection<RssCategory> _category;
		/// <summary>Generator</summary>
		private string _generator;
		/// <summary>Docs</summary>
		private string _docs;
		/// <summary>RssCloud</summary>
		private RssCloud _cloud;
		/// <summary>TTL</summary>
		private int _tTL;
		/// <summary>RssImage</summary>
		private RssImage _image;
		/// <summary>Rating</summary>
		private string _rating;
		/// <summary>RssTextInput</summary>
		private RssTextInput _textInput;		
		/// <summary>Items</summary>
        private SyndicationObjectRelationCollection<RssItem> _items;
        /// <summary>supress flag during parsing</summary>
        private bool _supressLastBuildTimeUpdate = false;
        /// <summary>holds the parsing errors</summary>
        private System.Collections.Generic.SortedList<string, Exception> _errors;
		#endregion
		
		#region constructors
		
		/// <summary>
		/// Initializes a new instance of the RssChannel class and set default values.
		/// </summary>		
		public RssChannel ()
		{
            this._supressLastBuildTimeUpdate = true;
            ((IRssChannel)this).Cloud = CreateCloud();
			((IRssChannel)this).Image = CreateImage();
			((IRssChannel)this).TextInput = CreateTextInput();
			Version = "2.0";
            this._supressLastBuildTimeUpdate = false;
		}
		#endregion
		
		#region public interface
		/// <summary>
		/// Gets the version if the version was specified
		/// </summary>
		[System.ComponentModel.Browsable(false), System.Xml.Serialization.XmlIgnore()]
		public string Version
		{
			get
			{
				return _version;
			}
            internal set
            {
                _version = value;
            }
		}
		
        ///// <summary>
        ///// Transforms the XML data using XSLT stylesheet.
        ///// </summary>
        ///// <param name="styleSheet">An XmlReader object that contains the XSLT stylesheet.</param>
        ///// <returns>An MemoryStream containing the results of the transform.</returns>
        //public System.IO.MemoryStream Transform (System.Xml.XmlReader styleSheet)
        //{
        //    System.IO.MemoryStream xmlStream = null;
        //    System.IO.MemoryStream xsltStream = null;
        //    try
        //    {
        //        xmlStream = new System.IO.MemoryStream();				
        //        // get xml content
        //        Save(xmlStream);				
        //        // transform
        //        xsltStream = Transform(styleSheet, xmlStream);
        //    } 
        //    finally
        //    {
        //        if(xmlStream!=null) xmlStream.Close();
        //    }
        //    return xsltStream;
		
        //}
		
        ///// <summary>
        ///// Transforms the XML data using XSLT stylesheet to an output file (html)
        ///// </summary>
        ///// <param name="styleSheet">An XmlReader object that contains the XSLT stylesheet.</param>
        ///// <param name="fileNameHtmlOutput">Filename of the html output file.</param>
        //public void Transform (System.Xml.XmlReader styleSheet, string fileNameHtmlOutput)
        //{
        //    Transform(styleSheet, null, fileNameHtmlOutput);
        //}
		
        ///// <summary>
        ///// Transforms the XML data using XSLT stylesheet to an output file (xml, html)
        ///// </summary>
        ///// <param name="styleSheet">An XmlReader object that contains the XSLT stylesheet.</param>
        ///// <param name="fileNameXmlOutput">Filename of the xml output file.</param>
        ///// <param name="fileNameHtmlOutput">Filename of the html output file.</param>
        //public void Transform (System.Xml.XmlReader styleSheet, string fileNameXmlOutput, string fileNameHtmlOutput)
        //{
        //    System.IO.FileStream xmlFileStream  = null;
        //    System.IO.FileStream htmlFileStream = null;
        //    //
        //    try
        //    {					
        //        using(System.IO.MemoryStream xmlStream = new System.IO.MemoryStream())
        //        {
        //            // get xml stream
        //            this.Save(xmlStream);	
        //            xmlStream.Seek(0, System.IO.SeekOrigin.Begin);
        //            // write xml stream to disk (xml)
        //            if(fileNameXmlOutput!=null)
        //            {						
        //                xmlFileStream = System.IO.File.Create(fileNameXmlOutput);
        //                xmlStream.WriteTo(xmlFileStream);
        //                xmlFileStream.Close();
						
        //            }
        //            // transform xml stream and write to disk (html)
        //            if(fileNameHtmlOutput!=null)
        //            {						
        //                htmlFileStream = System.IO.File.Create(fileNameHtmlOutput);
        //                using(System.IO.MemoryStream htmlStream = Transform(styleSheet, xmlStream))
        //                {
        //                    htmlStream.WriteTo(htmlFileStream);
        //                    htmlFileStream.Close();
        //                }						
        //            }
        //        }												
        //    }
        //    finally
        //    {			
        //        if(htmlFileStream!=null) htmlFileStream.Close();
        //        if(xmlFileStream!=null) xmlFileStream.Close();
        //    }
        //}
		
        ///// <summary>
        ///// Transforms the XML stream using XSLT stylesheet.
        ///// </summary>
        ///// <param name="styleSheet">An XmlReader object that contains the XSLT stylesheet.</param>
        ///// <param name="xmlStream">The data to transform</param>
        ///// <returns>An MemoryStream containing the results of the transform.</returns>
        ///// <example>
        ///// This sample shows how to consume and transform (XSLT/CSS) the code project feed.
        ///// <code>
        ///// // consume rss feed
        ///// RssChannel myChannel = new RssChannel(new Uri("http://www.codeproject.com/webservices/articlerss.aspx?cat=3"));
        ///// // transform to stream
        /////	System.IO.MemoryStream memoryStream = myChannel.Transform(new System.Xml.XmlTextReader("transform.xslt"));
        /////	// transform to html output file
        /////	myChannel.Transform(new System.Xml.XmlTextReader("transform.xslt"), "myChannel.htm");
        /////	// transform to html and xml output file
        /////	myChannel.Transform(new System.Xml.XmlTextReader("transform.xslt"), "channel.xml", "channel.htm");
        /////	</code>
        ///// </example>
        //public System.IO.MemoryStream Transform (System.Xml.XmlReader styleSheet, System.IO.Stream xmlStream)
        //{
        //    xmlStream.Seek(0, System.IO.SeekOrigin.Begin);
        //    // stream for transformed content
        //    System.IO.MemoryStream xsltStream = new System.IO.MemoryStream();			
        //    //
        //    try
        //    {	
        //        //Create a new XslTransform object.
        //        System.Xml.Xsl.XslTransform xslt = new System.Xml.Xsl.XslTransform();
        //        // Load the stylesheet.
        //        xslt.Load(styleSheet, new System.Xml.XmlUrlResolver(), null);
        //        // Create a new XPathDocument and load the XML data to be transformed.
        //        System.Xml.XPath.XPathDocument mydata = new System.Xml.XPath.XPathDocument(xmlStream);
        //        // Transform the data
        //        xslt.Transform(mydata,null,xsltStream, null);				
        //    } 
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if(xmlStream!=null) xmlStream.Close();				
        //    }			
        //    //
        //    return xsltStream;
        //}
		
		/// <summary>The name of the channel. It's how people refer to your service. If you have an HTML website that contains the same information as your RSS file, the title of your channel should be the same as the title of your website.</summary>
		[System.ComponentModel.Category("Required channel elements"), System.ComponentModel.Description("The name of the channel. It's how people refer to your service. If you have an HTML website that contains the same information as your RSS file, the title of your channel should be the same as the title of your website.")]
		[System.Xml.Serialization.XmlElementAttribute("title")]
		public string Title
		{
			get
			{
				return _title;
			}
			
			set
			{
				bool changed = !object.Equals(_title, value);
				_title = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Title));
			}
		}
		
		// end Title
		
		/// <summary>Phrase or sentence describing the channel.</summary>
		[System.ComponentModel.Category("Required channel elements"), System.ComponentModel.Description("Phrase or sentence describing the channel.")]
		[System.Xml.Serialization.XmlElementAttribute("description")]
		public string Description
		{
			get
			{
				return _description;
			}
			
			set
			{
				bool changed = !object.Equals(_description, value);
				_description = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Description));
			}
		}
		
		// end Description
		
		/// <summary>The URL to the HTML website corresponding to the channel.</summary>
		[System.ComponentModel.Category("Required channel elements"), System.ComponentModel.Description("The URL to the HTML website corresponding to the channel.")]
		[System.Xml.Serialization.XmlElementAttribute("link",DataType="anyURI")]
		public string Link
		{
			get
			{
				return _link;
			}
			
			set
			{
				bool changed = !object.Equals(_link, value);
				_link = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Link));
			}
		}
		
		// end Link
		
		/// <summary>The language the channel is written in. This allows aggregators to group all Italian language sites, for example, on a single page. A list of allowable values for this element, as provided by Netscape, is here. You may also use values defined by the W3C.</summary>
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("The language the channel is written in. This allows aggregators to group all Italian language sites, for example, on a single page. A list of allowable values for this element, as provided by Netscape, is here. You may also use values defined by the W3C.")]
		[System.Xml.Serialization.XmlIgnore]
		public System.Globalization.CultureInfo Language
		{
			get
			{
				return _language;
			}
			
			set
			{
				bool changed = !object.Equals(_language, value);
				_language = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Language));
			}
		}
		
		// end Language
		
		/// <summary>
		/// Internal, gets the CultureInfo ISO Code
		/// </summary>
		[System.Xml.Serialization.XmlElementAttribute("language",DataType="language")]
		[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public string LanguageIso
		{
			get
			{
                return this.Language != null ? this.Language.ToString() : string.Empty;
			}
			
			set
			{
                /* do nothing */
			}
		}
		
		/// <summary>Copyright notice for content in the channel.</summary>
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("Copyright notice for content in the channel.")]
		[System.Xml.Serialization.XmlElementAttribute("copyright")]
		public string Copyright
		{
			get
			{
				return _copyright;
			}
			
			set
			{
				bool changed = !object.Equals(_copyright, value);
				_copyright = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Copyright));
			}
		}
		
		// end Copyright
		
		/// <summary> Email address for person responsible for editorial content.</summary>
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description(" Email address for person responsible for editorial content.")]
		[System.Xml.Serialization.XmlElementAttribute("managingEditor")]
		public string ManagingEditor
		{
			get
			{
				return _managingEditor;
			}
			
			set
			{
				bool changed = !object.Equals(_managingEditor, value);
				_managingEditor = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.ManagingEditor));
			}
		}
		
		// end ManagingEditor
		
		/// <summary>Email address for person responsible for technical issues relating to channel.</summary>
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("Email address for person responsible for technical issues relating to channel.")]
		[System.Xml.Serialization.XmlElementAttribute("webMaster")]
		public string WebMaster
		{
			get
			{
				return _webMaster;
			}
			
			set
			{
				bool changed = !object.Equals(_webMaster, value);
				_webMaster = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.WebMaster));
			}
		}
		
		// end WebMaster
		
		/// <summary>The publication date for the content in the channel. For example, the New York Times publishes on a daily basis, the publication date flips once every 24 hours. That's when the pubDate of the channel changes. All date-times in RSS conform to the Date and Time Specification of RFC 822, with the exception that the year may be expressed with two characters or four characters (four preferred). </summary>
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("The publication date for the content in the channel. For example, the New York Times publishes on a daily basis, the publication date flips once every 24 hours. That's when the pubDate of the channel changes. All date-times in RSS conform to the Date and Time Specification of RFC 822, with the exception that the year may be expressed with two characters or four characters (four preferred). ")]		
		[System.Xml.Serialization.XmlIgnore]
		public DateTime PubDate
		{
			get
			{
				return _pubDate;
			}
			
			set
			{
				bool changed = !object.Equals(_pubDate, value);
				_pubDate = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.PubDate));
			}
		}
		
		// end PubDate
		
		/// <summary>
		/// Internal, gets the DateTime in RFC822 format
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		[System.Xml.Serialization.XmlElementAttribute("pubDate")]
		public string PubDateRfc
		{
			get
			{
				return this.PubDate.ToUniversalTime().ToString("r"); 
			}
			
			set
			{
                /* do nothing */
			}
		}
		
		/// <summary>The last time the content of the channel changed.</summary>
		/// <remarks>LastBuildDate is updated automatically every time the PropertyChanged event is fired.</remarks>
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("The last time the content of the channel changed."), System.ComponentModel.Browsable(false)]
		[System.Xml.Serialization.XmlIgnore]
		public DateTime LastBuildDate
		{
			get
			{
				return _lastBuildDate;
			}
			
			set
			{
				bool changed = !object.Equals(_lastBuildDate, value);
				_lastBuildDate = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.LastBuildDate));
			}
		}
		
		// end LastBuildDate
		
		/// <summary>
		/// Internal, gets the DateTime in RFC822 format
		/// </summary>				
		[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		[System.Xml.Serialization.XmlElementAttribute("lastBuildDate")]
		public string LastBuildDateRfc
		{
			get
			{
				return this.LastBuildDate.ToUniversalTime().ToString("r"); 
			}
			
			set
			{
                /* do nothing */
			}
		}
		
		/// <summary>Specify one or more categories that the channel belongs to. Follows the same rules as the item-level category element.</summary>
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("Specify one or more categories that the channel belongs to. Follows the same rules as the item-level category element.")]
		[System.Xml.Serialization.XmlElementAttribute("category")]        
		public SyndicationObjectRelationCollection<RssCategory> Category
		{
			get
			{
                if (_category == null)
                {
                    _category = new SyndicationObjectRelationCollection<RssCategory>(this);
                }
				return _category;
            }
		}
        System.Collections.ObjectModel.Collection<IRssCategory> IRssChannel.Category
        {
            get
            {                
                System.Collections.ObjectModel.Collection<IRssCategory>  cats = new System.Collections.ObjectModel.Collection<IRssCategory>();
                foreach (RssCategory cat in Category)
                {
                    cats.Add((IRssCategory)cat);
                }
                return cats;
            }
        }
        [System.ComponentModel.Browsable(false)]
        public bool CategorySpecified
        {
            get
            {
                return _category != null && _category.Count > 0;
            }
            set {/* do nothing */}
        }
		
		// end RssCategory
		
		/// <summary>A string indicating the program used to generate the channel.</summary>
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("A string indicating the program used to generate the channel.")]
		[System.Xml.Serialization.XmlElementAttribute("generator"), System.ComponentModel.Browsable(false)]
		public string Generator
		{
			get
			{
				return _generator;
			}
			
			set
			{
				bool changed = !object.Equals(_generator, value);
				_generator = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Generator));
			}
		}
		
		// end Generator
		
		/// <summary>A URL that points to the documentation for the format used in the RSS file. It's probably a pointer to this page. It's for people who might stumble across an RSS file on a Web server 25 years from now and wonder what it is.</summary>
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("A URL that points to the documentation for the format used in the RSS file. It's probably a pointer to this page. It's for people who might stumble across an RSS file on a Web server 25 years from now and wonder what it is.")]
		[System.Xml.Serialization.XmlElementAttribute("docs")]
		public string Docs
		{
			get
			{
				return _docs;
			}
			
			set
			{
				bool changed = !object.Equals(_docs, value);
				_docs = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Docs));
			}
		}
		
		// end Docs
		
		/// <summary>Allows processes to register with a cloud to be notified of updates to the channel, implementing a lightweight publish-subscribe protocol for RSS feeds. </summary>
		[System.Xml.Serialization.XmlElementAttribute("cloud")]
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("Allows processes to register with a cloud to be notified of updates to the channel, implementing a lightweight publish-subscribe protocol for RSS feeds. ")]
		public RssCloud Cloud
		{
			get
			{
				return _cloud;
			}
			
			set
			{
				bool changed = !object.Equals(_cloud, value);
				if(changed && _cloud!=null) _cloud.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(OnSubItemPropertyChanged);
				_cloud = value;
				if(changed)
				{
					OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Cloud));
					if(_cloud!=null) _cloud.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnSubItemPropertyChanged);
				}
			}
		}
		
		// end RssCloud
		
		/// <summary>Allows processes to register with a cloud to be notified of updates to the channel, implementing a lightweight publish-subscribe protocol for RSS feeds. </summary>
		[System.Xml.Serialization.XmlElementAttribute("cloud")]
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("Allows processes to register with a cloud to be notified of updates to the channel, implementing a lightweight publish-subscribe protocol for RSS feeds. ")]
		IRssCloud IRssChannel.Cloud
		{
			get
			{
				return this.Cloud;
			}
			
			set
			{
				this.Cloud = (RssCloud) value;
			}
		}
		
		// end RssCloud
		
		/// <summary>
		/// Instructs the XmlSerializer whether or not to generate the XML element
		/// </summary>
		[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		[System.Xml.Serialization.XmlIgnoreAttribute]
		public bool CloudSpecified
		{
			get
			{
                return _cloud.Specified;
			}
            set {/* do nothing */}
		}
		
		/// <summary>ttl stands for time to live. It's a number of minutes that indicates how long a channel can be cached before refreshing from the source.</summary>
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("ttl stands for time to live. It's a number of minutes that indicates how long a channel can be cached before refreshing from the source.")]
		[System.Xml.Serialization.XmlElementAttribute("ttl")]
		public int Ttl
		{
			get
			{
				return _tTL;
			}
			
			set
			{
				bool changed = !object.Equals(_tTL, value);
				_tTL = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Ttl));
			}
		}
		
		// end TTL
		
		/// <summary>
		/// Instructs the XmlSerializer whether or not to generate the XML element
		/// </summary>
		[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		[System.Xml.Serialization.XmlIgnoreAttribute]
		public bool TtlSpecified
		{
			get
			{
				return _tTL>0;
			}
            set { /* do nothing */ }
		}
		
		/// <summary>Specifies a GIF, JPEG or PNG image that can be displayed with the channel.</summary>
		[System.Xml.Serialization.XmlElementAttribute("image")]
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("Specifies a GIF, JPEG or PNG image that can be displayed with the channel.")]
		public RssImage Image
		{
			get
			{
				return _image;
			}
			
			set
			{
				bool changed = !object.Equals(_image, value);
				if(changed && _image!=null) _image.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(OnSubItemPropertyChanged);
				_image = value;
				if(changed)
				{
					OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Image));
					if(_image!=null) _image.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnSubItemPropertyChanged);
				}
			}
		}
		
		// end RssImage
		
		/// <summary>Specifies a GIF, JPEG or PNG image that can be displayed with the channel.</summary>
		[System.Xml.Serialization.XmlElementAttribute("image")]
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("Specifies a GIF, JPEG or PNG image that can be displayed with the channel.")]
		IRssImage IRssChannel.Image
		{
			get
			{
				return this.Image;
			}
			
			set
			{
				this.Image = value as RssImage;
			}
		}
		
		// end RssImage
		
		/// <summary>
		/// Instructs the XmlSerializer whether or not to generate the XML element
		/// </summary>
		[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		[System.Xml.Serialization.XmlIgnoreAttribute]
		public bool ImageSpecified
		{
			get
			{
                return _image.Specified;
			}
            set { /* do nothing */ }
		}
		
		/// <summary>The PICS rating for the channel</summary>
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("The PICS rating for the channel")]
		[System.Xml.Serialization.XmlElementAttribute("rating")]
		public string Rating
		{
			get
			{
				return _rating;
			}
			
			set
			{
				bool changed = !object.Equals(_rating, value);
				_rating = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Rating));
			}
		}
		
		// end Rating
		
		/// <summary>Specifies a text input box that can be displayed with the channel.</summary>
		[System.Xml.Serialization.XmlElementAttribute("textInput")]
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("Specifies a text input box that can be displayed with the channel.")]
		public RssTextInput TextInput
		{
			get
			{
				return _textInput;
			}
            set
            {
                if (_textInput != value)
                {
                    _textInput = value;
                    //
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.TextInput));
                }
            }
		}
		
		// end RssTextInput
		
		/// <summary>Specifies a text input box that can be displayed with the channel.</summary>
		[System.Xml.Serialization.XmlElementAttribute("textInput")]
		[System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("Specifies a text input box that can be displayed with the channel.")]
		IRssTextInput IRssChannel.TextInput
		{
			get
			{
				return this.TextInput;
			}
			
			set
			{
				this.TextInput = value as RssTextInput; 
			}
		}
		
		// end RssTextInput
		
		/// <summary>
		/// Instructs the XmlSerializer whether or not to generate the XML element
		/// </summary>
		[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		[System.Xml.Serialization.XmlIgnoreAttribute]
		public bool TextInputSpecified
		{
			get
			{
                return TextInput.Specified;
			}
			
			set
			{
                /* do nothing */
			}
		}
		
		/// <summary>A channel may contain any number of items. An item may represent a "story" -- much like a story in a newspaper or magazine; if so its description is a synopsis of the story, and the link points to the full story. An item may also be complete in itself, if so, the description contains the text (entity-encoded HTML is allowed), and the link and title may be omitted.</summary>
        [System.Xml.Serialization.XmlElementAttribute("item")]
        [System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("A channel may contain any number of items. An item may represent a \"story\" -- much like a story in a newspaper or magazine; if so its description is a synopsis of the story, and the link points to the full story. An item may also be complete in itself, if so, the description contains the text (entity-encoded HTML is allowed), and the link and title may be omitted.")]
        public SyndicationObjectRelationCollection<RssItem> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new SyndicationObjectRelationCollection<RssItem>(this);
                }
                return _items;
            }
        }
        
		/// <summary>
		/// Obtains the String representation of this instance. 
		/// </summary>
		/// <returns>The friendly name</returns>
		public override string ToString ()
		{
			return Title;
		}
        public override bool Specified
        {
            get
            {
                return true;
            }
        }
		#endregion

        #region internal interface
        /// <summary>
        /// Gets or sets the state of the flag to supress updates of LastBuildTime during parsing
        /// </summary>
        bool SupressUpdateLastBuildTime
        {
            get { return _supressLastBuildTimeUpdate; }
            set { _supressLastBuildTimeUpdate = value; }
        }

        //void ISyndicationObjectRelationOwner.RemoveItem(object item)
        //{
        //    if (item is RssItem)
        //    {
        //        _items.Remove((RssItem)item);
        //    }
        //    else if (item is RssCategory)
        //    {
        //        Category.Remove((RssCategory)item);
        //    }
        //}
        void ISyndicationObjectRelationOwner.NotifyCollectionChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }
        #endregion
		
		#region protected interface		
        protected internal virtual IRssCloud CreateCloud()
        {
            return new RssCloud(); 
        }
        protected internal virtual IRssImage CreateImage()
        {
            return new RssImage();
        }
        protected internal virtual IRssTextInput CreateTextInput()
        {
            return new RssTextInput();
        }
        protected internal virtual IRssCategory CreateCategory()
        {
            return new RssCategory();
        }
        public virtual IRssItem CreateItem()
        {
            return new RssItem();
        }
		#endregion
		
		#region nested classes
		
		/// <summary>
		/// public writeable class properties
		/// </summary>		
		internal struct Fields
		{
			public const string Title = "Title";
			public const string Description = "Description";
			public const string Link = "Link";
			public const string Language = "Language";
			public const string Copyright = "Copyright";
			public const string ManagingEditor = "ManagingEditor";
			public const string WebMaster = "WebMaster";
			public const string PubDate = "PubDate";
			public const string LastBuildDate = "LastBuildDate";
			public const string Category = "Category";
			public const string Generator = "Generator";
			public const string Docs = "Docs";
			public const string Cloud = "Cloud";
			public const string Ttl = "Ttl";
			public const string Image = "Image";
			public const string Rating = "Rating";
			public const string TextInput = "TextInput";
			public const string SkipHours = "SkipHours";
			public const string SkipDays = "SkipDays";
			public const string Items = "Items";
		}
		
		#endregion
		
		#region events
        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            //
            if(!_supressLastBuildTimeUpdate) _lastBuildDate = DateTime.Now;
        }
		///<summary>A PropertyChanged event is raised when a sub property is changed. A PropertyChangedEventArgs object specifies the name of the property that changed.</summary>
		protected internal virtual void OnSubItemPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
            OnPropertyChanged(e);
		}
		
		#endregion

        #region IParseableObject Members
        public void BeginParse()
        {
            SupressUpdateLastBuildTime = true;
        }
        public void EndParse()
        {
            SupressUpdateLastBuildTime = false;
        }
        [XmlIgnore, System.ComponentModel.Browsable(false)]
        public System.Collections.Generic.SortedList<string, Exception> Errors
        {
            get
            {
                if (_errors == null)
                {
                    _errors = new System.Collections.Generic.SortedList<string, Exception>();
                }
                return _errors;
            }
        }
        #endregion
    }
}