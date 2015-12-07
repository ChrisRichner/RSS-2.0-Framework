
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
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Net;

namespace Raccoom.Xml.Rss
{	
	/// <summary>Optional sub-element of item. RssGuid stands for globally unique identifier. It's a string that uniquely identifies the item. When present, an aggregator may choose to use this string to determine if an item is new. isPermaLink is optional, its default value is true. If its value is false, the guid may not be assumed to be a url, or a url to anything in particular.</summary>
	[System.Runtime.InteropServices.ComVisible(true), System.Runtime.InteropServices.Guid("026FF52F-96DF-4879-A355-880832C49A2C")]
	[System.Runtime.InteropServices.ClassInterface(System.Runtime.InteropServices.ClassInterfaceType.None)]
	[System.Runtime.InteropServices.ProgId("Raccoom.RssGuid")]
	[System.Xml.Serialization.XmlTypeAttribute("guid")]
	[Serializable]	
	[System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class RssGuid : ComponentModel.SyndicationObjectBase, IRssGuid
	{
		#region fields
		/// <summary>IsPermaLink</summary>
		private bool _isPermaLink;
		/// <summary>RssGuid</summary>
		private string _guid;
		
		#endregion
		
		#region constructors
		
		/// <summary>Initializes a new instance of RssGuid with default values</summary>
		public RssGuid ()
		{
			this.IsPermaLink = true;
		}
		#endregion
		
		#region public interface
        public override bool Specified
        {
            get
            {
                return !string.IsNullOrEmpty(Value);
            }
        }
		/// <summary>If the guid element has an attribute named "isPermaLink" with a value of true, the reader may assume that it is a permalink to the item, that is, a url that can be opened in a Web browser, that points to the full item described by the item element.</summary>
		[System.ComponentModel.Category("RssGuid"), System.ComponentModel.Description("If the guid element has an attribute named isPermaLink with a value of true, the reader may assume that it is a permalink to the item, that is, a url that can be opened in a Web browser, that points to the full item described by the item element.")]
		[System.Xml.Serialization.XmlAttribute("isPermaLink")]
		public bool IsPermaLink
		{
			get
			{
				return _isPermaLink;
			}
			
			set
			{
				bool changed = !object.Equals(_isPermaLink, value);
				_isPermaLink = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.IsPermaLink));
			}
		}
		
		// end IsPermaLink
		
		/// <summary>RssGuid stands for globally unique identifier. It's a string that uniquely identifies the item. When present, an aggregator may choose to use this string to determine if an item is new.</summary>
		[System.ComponentModel.Category("RssGuid"), System.ComponentModel.Description("Guid stands for globally unique identifier. It's a string that uniquely identifies the item. When present, an aggregator may choose to use this string to determine if an item is new.")]
		[System.Xml.Serialization.XmlTextAttribute]
		public string Value
		{
			get
			{
				return _guid;
			}
			
			set
			{
				bool changed = !object.Equals(_guid, value);
				_guid = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Guid));
			}
		}
		
		// end RssGuid
		
		/// <summary>
		/// Obtains the String representation of this instance. 
		/// </summary>
		/// <returns>The friendly name</returns>
		public override string ToString ()
		{
			return Value;
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
			public const string IsPermaLink = "IsPermaLink";
			public const string Guid = "Guid";
		}
		
		#endregion
	}
}