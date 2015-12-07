
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

namespace Raccoom.Xml.Opml
{
    /// <summary>OpmlOutline Processor Markup Language<para></para> Opml is an XML element, with a single required attribute, version; a head element and a body element, both of which are required. The version attribute is a version string, of the form, x.y, where x and y are both numeric strings.</summary>
	[System.Xml.Serialization.XmlRoot()]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName = "opml")]
	[Serializable]	
	[System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class OpmlDocument : Raccoom.Xml.ComponentModel.SyndicationObjectBase, IOpmlDocument, Raccoom.Xml.ComponentModel.IParseableObject
	{
		#region fields
		/// <summary>OpmlHead</summary>
		private OpmlHead _head;
		/// <summary>OpmlBody</summary>
		private OpmlBody _body;
        /// <summary>supress flag for last modified changed property update during parsing</summary>
        private bool _supressLastModifiedChanged = true;
        /// <summary>holds the parsing errors</summary>
        private System.Collections.Generic.SortedList<string, Exception> _errors;
		#endregion
		
		#region constructors
		
		/// <summary>Initializes a new instance of OpmlDocument with default values</summary>
		public OpmlDocument ()
		{
            _supressLastModifiedChanged = true;
            this.Head = new OpmlHead();			
			this.Body = new OpmlBody();
            _supressLastModifiedChanged = false;
		}
		#endregion
		
		#region public interface
        public override bool Specified
        {
            get
            {
                return true;
            }
        }
		
		/// <summary>
		/// Internal property used to generate readonly attribute version
		/// </summary>
		[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		[XmlAttribute("version")]
		public virtual string Version
		{
			get
			{
				return "2.0";
			}
			
			set
			{
				// do nothing
			}
		}
		
		/// <summary>A head contains zero or more optional elements</summary>
		[System.ComponentModel.Category("Required elements"), System.ComponentModel.Description("A head contains zero or more optional elements")]
		[System.Xml.Serialization.XmlElementAttribute("head")]
		public virtual OpmlHead Head
		{
			get
			{
				return _head;
			}
			
			set
			{
				bool changed = !object.Equals(_head, value);
				if(changed && _head!=null)
				{
					_head.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(OnSubItemPropertyChanged);					
					_head.SetDocument(null);
				}
				_head = value;
				if(changed)
				{
					OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Head));
					if(_head!=null)
					{						
						_head.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnSubItemPropertyChanged);
						_head.SetDocument(this);
					}
				}
			}
		}
		
		// end OpmlHead
		
		IOpmlHead IOpmlDocument.Head
		{
			get
			{
				return this.Head;
			}
			
			set
			{
				this.Head = value as OpmlHead;
			}
		}
		
		// end OpmlHead
		
		/// <summary>A body contains one or more outline elements</summary>
		[System.ComponentModel.Category("Required elements"), System.ComponentModel.Description("A body contains one or more outline elements")]
		[System.Xml.Serialization.XmlElementAttribute("body")]
		public virtual OpmlBody Body
		{
			get
			{
				return _body;
			}
			
			set
			{
				bool changed = !object.Equals(_body, value);
				if(changed && _body!=null)
				{
					_body.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(OnSubItemPropertyChanged);
					_body.SetDocument(null);
				}
				_body = value;
				if(changed)
				{
					OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Body));
					if(_body!=null)
					{
						_body.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnSubItemPropertyChanged);
						_body.SetDocument(this);
					}
				}
			}
		}
		
		// end OpmlBody
		
		IOpmlBody IOpmlDocument.Body
		{
			get
			{
				return this.Body;
			}
			
			set
			{
				this.Body = value as OpmlBody;
			}
		}
		
		/// <summary>
		/// Obtains the String representation of this instance. 
		/// </summary>
		/// <returns>The friendly name</returns>
		public override string ToString ()
		{
			return Head.Title;
		}
		
		#endregion
		
		#region protected interface
        /// <summary>
        /// Gets or sets the state of the supress flag during parsing the document
        /// </summary>
        private bool SupressLastModifiedDate
        {
            get
            {
                return _supressLastModifiedChanged;
            }
            set
            {
                _supressLastModifiedChanged = value;
            }
        }
		///<summary>A PropertyChanged event is raised when a property is changed on a component. A PropertyChangedEventArgs object specifies the name of the property that changed.</summary>
		protected override void OnPropertyChanged (System.ComponentModel.PropertyChangedEventArgs e)
		{
            base.OnPropertyChanged(e);
			// update modified date
            if (!_supressLastModifiedChanged && Head != null) this.Head._dateModified = DateTime.Now;
		}
		#endregion
		
		#region events
		///<summary>A PropertyChanged event is raised when a sub property is changed. A PropertyChangedEventArgs object specifies the name of the property that changed.</summary>
		protected internal virtual void OnSubItemPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			OnPropertyChanged(e);	
		}
		
		#endregion
		
		#region nested classes
		
		/// <summary>
		/// public writeable class properties
		/// </summary>		
		internal struct Fields
		{
			public const string Head = "Head";
			public const string Body = "Body";
		}
		
		#endregion

        #region IParseableObject Members
        public void BeginParse()
        {
            SupressLastModifiedDate = true;
        }
        public void EndParse()
        {
            SupressLastModifiedDate = false;
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