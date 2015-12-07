using System;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Net;

namespace Raccoom.Xml.Rss
{	
	/// <summary>RssEnclosure is an optional sub-element of item.</summary>
	[System.Runtime.InteropServices.ComVisible(true), System.Runtime.InteropServices.Guid("026FF52F-96DF-4879-A355-880832C49A1C")]
	[System.Runtime.InteropServices.ClassInterface(System.Runtime.InteropServices.ClassInterfaceType.None)]
	[System.Runtime.InteropServices.ProgId("Raccoom.RssEnclosure")]
	[System.Xml.Serialization.XmlTypeAttribute("enclosure")]
	[Serializable]
	[System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class RssEnclosure : ComponentModel.SyndicationObjectBase, IRssEnclosure
	{
		#region fields
		/// <summary>Url</summary>
		private string _url;
		/// <summary>Length</summary>
		private int _length;
		/// <summary>Type</summary>
		private string _type;
		
		#endregion
		
		#region constructors		
		/// <summary>Initializes a new instance of RssEnclosure with default values</summary>
		public RssEnclosure ()
		{
		
		}
		#endregion
		
		#region public interface
        public override bool Specified
        {
            get
            {
                return !string.IsNullOrEmpty(Url) || !string.IsNullOrEmpty(Type) || LengthSpecified;
            }
        }
		
		/// <summary> The url must be an http url.</summary>
		[System.ComponentModel.Category("RssEnclosure"), System.ComponentModel.Description(" The url must be an http url.")]
		[System.Xml.Serialization.XmlAttribute("url", DataType="anyURI")]
		public string Url
		{
			get
			{
				return _url;
			}
			
			set
			{
				bool changed = !object.Equals(_url, value);
				_url = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Url));
			}
		}
		
		// end Url
		
		/// <summary>length says how big it is in bytes,</summary>
		[System.ComponentModel.Category("RssEnclosure"), System.ComponentModel.Description("length says how big it is in bytes,")]
		[System.Xml.Serialization.XmlAttribute("length")]
		public int Length
		{
			get
			{
				return _length;
			}
			
			set
			{
				bool changed = !object.Equals(_length, value);
				_length = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Length));
			}
		}
		
		// end Length
		
		/// <summary>
		/// Instructs the XmlSerializer whether or not to generate the XML element
		/// </summary>
		[System.Xml.Serialization.XmlIgnore]
		[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool LengthSpecified
		{
			get
			{
				return _length>0;
			}
            set { /* do nothing */ }
		}
		
		/// <summary>type says what its type is, a standard MIME type.</summary>
		[System.ComponentModel.Category("RssEnclosure"), System.ComponentModel.Description("type says what its type is, a standard MIME type.")]
		[System.Xml.Serialization.XmlAttribute("type")]
		public string Type
		{
			get
			{
				return _type;
			}
			
			set
			{
				bool changed = !object.Equals(_type, value);
				_type = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Type));
			}
		}
		
		// end Type
		
		/// <summary>
		/// Obtains the String representation of this instance. 
		/// </summary>
		/// <returns>The friendly name</returns>
		public override string ToString ()
		{
			return this.Url;
		}
		
		#endregion
		
		#region protected interface

		#endregion
		
		#region nested classes
		
		/// <summary>
		/// public writeable class properties
		/// </summary>		
		internal struct Fields
		{
			public const string Url = "Url";
			public const string Length = "Length";
			public const string Type = "Type";
			public const string Path = "Path";
		}
		
		#endregion
	}
}