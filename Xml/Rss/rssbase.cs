
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Net;

namespace Raccoom.Xml.Rss
{	
	/// <summary>RSS 2.0 channel
	/// RSS is a Web content syndication format. Its name is an acronym for Really Simple Syndication. RSS is a dialect of XML. All RSS files must conform to the XML 1.0 specification, as published on the World Wide Web Consortium (W3C) website. 
	/// At the top level, a RSS document is a rss element, with a mandatory attribute called version, that specifies the version of RSS that the document conforms to. If it conforms to this specification, the version attribute must be 2.0.
	/// Subordinate to the rss element is a single channel element, which contains information about the channel (metadata) and its contents.
	/// </summary>
	/// <remarks>RSS 2.0 Specification http://blogs.law.harvard.edu/tech/rss</remarks>	
	public interface IRssChannel
	{        
		/// <summary>The name of the channel. It's how people refer to your service. If you have an HTML website that contains the same information as your RSS file, the title of your channel should be the same as the title of your website.</summary>
		string Title
		{
			get; 
			set; 
		}
		
		// end Title
		
		/// <summary>Phrase or sentence describing the channel.</summary>
		string Description
		{
			get; 
			set; 
		}
		
		// end Description
		
		/// <summary>The URL to the HTML website corresponding to the channel.</summary>
		string Link
		{
			get; 
			set; 
		}
		
		// end Link
		
		/// <summary>The language the channel is written in. This allows aggregators to group all Italian language sites, for example, on a single page. A list of allowable values for this element, as provided by Netscape, is here. You may also use values defined by the W3C.</summary>
		System.Globalization.CultureInfo Language
		{
			get; 
			set; 
		}
		
		// end Language
		
		/// <summary>Copyright notice for content in the channel.</summary>
		string Copyright
		{
			get; 
			set; 
		}
		
		// end Copyright
		
		/// <summary> Email address for person responsible for editorial content.</summary>
		string ManagingEditor
		{
			get; 
			set; 
		}
		
		// end ManagingEditor
		
		/// <summary>Email address for person responsible for technical issues relating to channel.</summary>
		string WebMaster
		{
			get; 
			set; 
		}
		
		// end WebMaster
		
		/// <summary>The publication date for the content in the channel. For example, the New York Times publishes on a daily basis, the publication date flips once every 24 hours. That's when the pubDate of the channel changes. All date-times in RSS conform to the Date and Time Specification of RFC 822, with the exception that the year may be expressed with two characters or four characters (four preferred). </summary>
		DateTime PubDate
		{
			get; 
			set; 
		}
		
		// end PubDate
		
		/// <summary>The last time the content of the channel changed.</summary>
		DateTime LastBuildDate
		{
			get; 
			set; 
		}
		
		// end LastBuildDate
		
		/// <summary>Specify one or more categories that the channel belongs to. Follows the same rules as the item-level category element.</summary>
		System.Collections.ObjectModel.Collection<IRssCategory> Category
		{
			get; 
			//set; 
		}
		
		// end RssCategory
		
		/// <summary>A string indicating the program used to generate the channel.</summary>		
		string Generator
		{
			get; 
			set; 
		}
		
		// end Generator
		
		/// <summary>A URL that points to the documentation for the format used in the RSS file. It's probably a pointer to this page. It's for people who might stumble across an RSS file on a Web server 25 years from now and wonder what it is.</summary>
		string Docs
		{
			get; 
			set; 
		}
		
		// end Docs
		
		
		/// <summary>ttl stands for time to live. It's a number of minutes that indicates how long a channel can be cached before refreshing from the source.</summary>
        int Ttl
		{
			get; 
			set; 
		}
		
		// end TTL
		
		
		/// <summary>The PICS rating for the channel</summary>
		string Rating
		{
			get; 
			set; 
		}
		
		// end Rating
		
		
		/// <summary>A hint for aggregators telling them which hours they can skip. </summary>
		/// <remarks>
		/// Contains up to 24  sub-elements whose value is a number between 0 and 23, representing a time in GMT, when aggregators, if they support the feature, may not read the channel on hours listed in the skipHours element. The hour beginning at midnight is hour zero.
		/// </remarks>
		
        IRssCloud Cloud
        {
            get;
            set;
        }

        // end RssCloud
        /// <summary>Specifies a GIF, JPEG or PNG image that can be displayed with the channel.<see cref="IRssImage"/></summary>
        IRssImage Image
        {
            get;
            set;
        }

        // end RssImage

        /// <summary>Specifies a text input box that can be displayed with the channel.<see cref="IRssTextInput"/></summary>
        IRssTextInput TextInput
        {
            get;
            set;
        }
	}
	
	/// <summary>Is an optional sub-element of channel.  It specifies a web service that supports the rssCloud interface which can be implemented in HTTP-POST, XML-RPC or SOAP 1.1.</summary>
	public interface IRssCloud
	{
		/// <summary></summary>
		string Domain
		{
			get; 
			set; 
		}
		
		// end Domain
		
		/// <summary></summary>
		int Port
		{
			get; 
			set; 
		}
		
		// end Port
		
		/// <summary></summary>
		string Path
		{
			get; 
			set; 
		}
		
		// end Path
		
		/// <summary></summary>
		string RegisterProcedure
		{
			get; 
			set; 
		}
		
		// end RegisterProcedure
		
		/// <summary></summary>
		CloudProtocol Protocol
		{
			get; 
			set; 
		}
		
		// end Protocol
	}
	
	/// <summary>RssEnclosure is an optional sub-element of item.</summary>
	public interface IRssEnclosure
	{
		/// <summary> The url must be an http url.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        string Url
		{
			get; 
			set; 
		}
		
		// end Url
		
		/// <summary>length says how big it is in bytes</summary>
		int Length
		{
			get; 
			set; 
		}
		
		// end Length
		
		/// <summary>type says what its type is, a standard MIME type.</summary>
		string Type
		{
			get; 
			set; 
		}
		
		// end Type
	}
	
	/// <summary>Optional sub-element of item. RssGuid stands for globally unique identifier. It's a string that uniquely identifies the item. When present, an aggregator may choose to use this string to determine if an item is new. isPermaLink is optional, its default value is true. If its value is false, the guid may not be assumed to be a url, or a url to anything in particular.</summary>
	public interface IRssGuid
	{
		/// <summary>If the guid element has an attribute named "isPermaLink" with a value of true, the reader may assume that it is a permalink to the item, that is, a url that can be opened in a Web browser, that points to the full item described by the item element.</summary>
		bool IsPermaLink
		{
			get; 
			set; 
		}
		
		// end IsPermaLink
		
		/// <summary>RssGuid stands for globally unique identifier. It's a string that uniquely identifies the item. When present, an aggregator may choose to use this string to determine if an item is new.</summary>
		string Value
		{
			get; 
			set; 
		}
		
		// end Value
	}
	
	/// <summary>RssImage is an optional sub-element of channel, which contains three required and three optional sub-elements.</summary>
	public interface IRssImage
	{
		/// <summary>Describes the image, it's used in the ALT attribute of the HTML img tag when the channel is rendered in HTML. </summary>
		string Title
		{
			get; 
			set; 
		}
		
		// end Title
		
		/// <summary>The URL of a GIF, JPEG or PNG image. </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        string Url
		{
			get; 
			set; 
		}
		
		// end Url
		
		/// <summary>the URL of the site, when the channel is rendered, the image is a link to the site.</summary>
		string Link
		{
			get; 
			set; 
		}
		
		// end Link
		
		/// <summary>Contains text that is included in the TITLE attribute of the link formed around the image in the HTML rendering.</summary>
		string Description
		{
			get; 
			set; 
		}
		
		// end Description
		
		/// <summary>Maximum value for width is 144, default value is 88.</summary>
		int Width
		{
			get; 
			set; 
		}
		
		// end Width
		
		/// <summary> Maximum value for height is 400, default value is 31.</summary>
		int Height
		{
			get; 
			set; 
		}
		
		// end Height
	}

    /// <summary>An item may represent a "story" -- much like a story in a newspaper or magazine; if so its description is a synopsis of the story, and the link points to the full story. An item may also be complete in itself, if so, the description contains the text (entity-encoded HTML is allowed), and the link and title may be omitted. All elements of an item are optional, however at least one of title or description must be present.</summary>
    public interface IRssItem
    {
        /// <summary>Gets the parent channel that the item is assigned to.</summary>
        IRssChannel Channel
        {
            get;
        }
        IRssEnclosure CreateEnclosure();
        IRssGuid CreateGuid();
        IRssSource CreateSource();
        IRssCategory CreateCategory();

        /// <summary>The title of the item.</summary>
        string Title
        {
            get;
            set;
        }

        // end Title

        /// <summary>The item synopsis.</summary>
        string Description
        {
            get;
            set;
        }

        // end Description

        /// <summary>The URL of the item.</summary>
        string Link
        {
            get;
            set;
        }

        // end Link

        /// <summary>Email address of the author of the item.</summary>
        string Author
        {
            get;
            set;
        }

        // end Author

        /// <summary>Includes the item in one or more categories.</summary>
        System.Collections.ObjectModel.Collection<IRssCategory> Category
        {
            get;
        }

        // end RssCategory

        /// <summary>Indicates when the item was published.</summary>
        DateTime PubDate
        {
            get;
            set;
        }

        // end PubDate

        /// <summary>URL of a page for comments relating to the item. </summary>
        string Comments
        {
            get;
            set;
        }

        // end Comments

        /// <summary>Describes a media object that is attached to the item. </summary>
        IRssEnclosure Enclosure
        {
            get;
            set;
        }

        // end RssEnclosure

        /// <summary>A string that uniquely identifies the item.</summary>
        IRssGuid Guid
        {
            get;
            set;
        }

        // end RssGuid

        /// <summary>The RSS channel that the item came from.</summary>
        IRssSource Source
        {
            get;
            set;
        }

        // end RssSource
    }
	
	/// <summary>Optional sub-element of item. Its value is the name of the RSS channel that the item came from, derived from its title. It has one required attribute, url, which links to the XMLization of the source.The purpose of this element is to propagate credit for links, to publicize the sources of news items. It can be used in the Post command of an aggregator. It should be generated automatically when forwarding an item from an aggregator to a weblog authoring tool.</summary>
	public interface IRssSource
	{
		/// <summary>The RSS channel url that the item came from.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        string Url
		{
			get; 
			set; 
		}
		
		// end Uri
		
		/// <summary>The RSS channel name that the item came from.</summary>
		string Value
		{
			get; 
			set; 
		}
		
		// end Value
	}
	
	/// <summary>A channel may optionally contain a textInput sub-element, which contains four required sub-elements.</summary>
	public interface IRssTextInput
	{
		/// <summary>The label of the Submit button in the text input area. </summary>
		string Title
		{
			get; 
			set; 
		}
		
		// end Title
		
		/// <summary>Explains the text input area. </summary>
		string Description
		{
			get; 
			set; 
		}
		
		// end Description
		
		/// <summary> The name of the text object in the text input area. </summary>
		string Name
		{
			get; 
			set; 
		}
		
		// end Name
		
		/// <summary>The URL of the CGI script that processes text input requests. </summary>
		string Link
		{
			get; 
			set; 
		}
		
		// end Link
	}
    /// <summary>
    /// The value of the element is a forward-slash-separated string that identifies a hierarchic location in the indicated taxonomy.
    /// </summary>
    public interface IRssCategory
    {
        /// <summary>The value of the element is a forward-slash-separated string that identifies a hierarchic location in the indicated taxonomy. Processors may establish conventions for the interpretation of categories.</summary>
		string Value
		{
			get; 
			set; 
		}
		
		// end Value

        /// <summary>A string that identifies a categorization taxonomy.</summary>
        string Domain
        {
            get;
            set;
        }

        // end Domain
    }
	
	/// <summary>
	/// Specifies a web service which can be implemented in HTTP-POST, XML-RPC or SOAP 1.1. 
	/// </summary>
	public enum CloudProtocol
	{
		/// <summary>No protocol is used.</summary>
		None,
        [XmlEnum(Name = "Xml-RPC")]
		XmlRpc,
        [XmlEnum(Name = "HTTP-POST")]
		HttpPost,
        [XmlEnum(Name = "SOAP 1.1")]
		Soap
	}
    /// <summary>
    /// IRssWriter defines the write operations needed
    /// </summary>
    public interface IRssWriter
    {
        void Write(System.IO.Stream stream, IRssChannel channel);
        void Write(string feed, IRssChannel channel);
    }
    /// <summary>
    /// IRssReader defines the read operations needed
    /// </summary>
    public interface IRssReader
    {
        IRssChannel Read(string feed);
    }
}