using System;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Net;

namespace Raccoom.Xml.Rss
{	
	/// <summary>A channel may optionally contain a textInput sub-element, which contains four required sub-elements.</summary>
	[System.Runtime.InteropServices.ComVisible(true), System.Runtime.InteropServices.Guid("026FF54F-94DF-4879-A355-880835C49A2C")]
	[System.Runtime.InteropServices.ClassInterface(System.Runtime.InteropServices.ClassInterfaceType.None)]
	[System.Runtime.InteropServices.ProgId("Raccoom.RssTextInput")]
	[System.Xml.Serialization.XmlTypeAttribute("textInput")]
	[Serializable]
	[System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class RssTextInput : ComponentModel.SyndicationObjectBase, IRssTextInput
	{
		#region fields
		/// <summary>Title</summary>
		private string _title;
		/// <summary>Description</summary>
		private string _description;
		/// <summary>Name</summary>
		private string _name;
		/// <summary>Link</summary>
		private string _link;
		
		#endregion
		
		#region constructors
		
		/// <summary>Initializes a new instance of RssTextInput with default values</summary>
		public RssTextInput ()
		{
		
		}
		#endregion
		
		#region public interface
        public override bool Specified
        {
            get
            {
                return !string.IsNullOrEmpty(Description) || !string.IsNullOrEmpty(Link) || !string.IsNullOrEmpty(Name) || !string.IsNullOrEmpty(Title);
            }
        }
		
		/// <summary>The label of the Submit button in the text input area. </summary>
		[System.ComponentModel.Category("RssTextInput"), System.ComponentModel.Description("The label of the Submit button in the text input area. ")]
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
		
		/// <summary>Explains the text input area. </summary>
		[System.ComponentModel.Category("RssTextInput"), System.ComponentModel.Description("Explains the text input area. ")]
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
		
		/// <summary> The name of the text object in the text input area. </summary>
		[System.ComponentModel.Category("RssTextInput"), System.ComponentModel.Description(" The name of the text object in the text input area. ")]
		[System.Xml.Serialization.XmlElementAttribute("name")]
		public string Name
		{
			get
			{
				return _name;
			}
			
			set
			{
				bool changed = !object.Equals(_name, value);
				_name = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Name));
			}
		}
		
		// end Name
		
		/// <summary>The URL of the CGI script that processes text input requests. </summary>
		[System.ComponentModel.Category("RssTextInput"), System.ComponentModel.Description("The URL of the CGI script that processes text input requests. ")]
		[System.Xml.Serialization.XmlElementAttribute("link")]
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
		
		/// <summary>
		/// Obtains the String representation of this instance. 
		/// </summary>
		/// <returns>The friendly name</returns>
		public override string ToString ()
		{
			return this.Description;
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
			public const string Title = "Title";
			public const string Description = "Description";
			public const string Name = "Name";
			public const string Link = "Link";
		}
		
		#endregion
	}
}