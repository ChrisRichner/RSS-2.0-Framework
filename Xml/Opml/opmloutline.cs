using System;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Net;

namespace Raccoom.Xml.Opml
{	
	/// <summary>
	/// <see cref="OpmlOutline"/> strong typed collecton.
	/// </summary>
	[Serializable]
	public class OpmlOutlineCollection	:	System.Collections.CollectionBase
	{
		#region fields
		
		private OpmlDocument _document;
		private OpmlBody _body;
		private OpmlOutline _outline;
		
		#endregion
		
		#region constructors
		
		/// <summary>Initializes a new instance of OpmlOutlineCollection</summary>
		public OpmlOutlineCollection ()
		{
		
		}
		
		/// <summary>Initializes a new instance of OpmlOutlineCollection</summary>
		public OpmlOutlineCollection (OpmlBody body)
		{
			_body = body;
		}
		
		/// <summary>Initializes a new instance of OpmlOutlineCollection</summary>
		public OpmlOutlineCollection (OpmlOutline outline)
		{
			_outline = outline;
		}
		
		#endregion
		
		#region internal interface
		
		internal void SetDocument (OpmlDocument document)
		{
			this._document = document;
			foreach(OpmlOutline item in this.List)
			{
				item.SetDocument(document);
			}
		}
		
		#endregion
		
		#region public interface
        /// <summary>Adds an item to the IOpmlOutlineCollection.</summary>
		public int Add (OpmlOutline value)
		{
			return base.List.Add(value as object);
		}
		
		/// <summary>Removes an item to the OpmlOutlineCollection.</summary>
		public void Remove (OpmlOutline value)
		{
			base.List.Remove(value as object);
		}
		
		/// <summary>Inserts an IOpmlOutline to the OpmlOutlineCollection at the specified position.</summary>
		public void Insert (int index, OpmlOutline value)
		{
			base.List.Insert(index, value as object);
		}
		
		/// <summary>Determines whether the OpmlOutlineCollection contains a specific value.</summary>
		public bool Contains (OpmlOutline value)
		{
			return base.List.Contains(value as object);
		}
		
		/// <summary>Gets the IOpmlOutline at the specified index.</summary>
		public OpmlOutline this [ int index ]
		{
			get
			{
				return (base.List[index] as OpmlOutline); 
			}
		}
		
		/// <summary>Determines the index of a specific item i</summary>
		public int IndexOf (IOpmlOutline value)
		{
			return( List.IndexOf( value ) );
		}
		
		/// <summary>Copies the elements of the Collection to an Array, starting at a particular Array index.</summary>
        public void CopyTo(IOpmlOutline[] array, int index)
		{
			List.CopyTo(array, index);
		}
		
		#endregion
		
		#region internal interface
		
		/// <summary>
		/// Performs additional custom processes
		/// </summary>
		protected override void OnInsertComplete (int index, object value)
		{
			base.OnInsertComplete (index, value);
			// attach item
			AttachItem(value as OpmlOutline);
			//if(item!=null && _parent!=null && !item.PropertyChangedEventAttached) item.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_parent.OnSubItemPropertyChanged);			
		}
		
		/// <summary>
		/// Performs additional custom processes
		/// </summary>
		protected override void OnRemoveComplete (int index, object value)
		{
			base.OnRemoveComplete (index, value);
			// attach item
			DetachItem(value as OpmlOutline);
			//if(item!=null && _parent!=null && item.PropertyChangedEventAttached) item.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(_parent.OnSubItemPropertyChanged);			
		}
		
		/// <summary>
		/// Performs additional custom processes
		/// </summary>
		protected override void OnClear ()
		{
			base.OnClear ();
			// detach all items
			foreach(OpmlOutline item in List)
			{
				DetachItem(item);
			}
			// Dirty state
			SetDirtyState();
		}
		
		#endregion
		
		#region private interface
		
		private void SetDirtyState ()
		{
			if(_body!=null)
			{
				_body.OnSubItemPropertyChanged(_body, new System.ComponentModel.PropertyChangedEventArgs(OpmlBody.Fields.Items));
			} 
			else if(_outline!=null)
			{
				_outline.OnSubItemPropertyChanged(_outline, new System.ComponentModel.PropertyChangedEventArgs(OpmlBody.Fields.Items));
			}
		}
		
		private void AttachItem (OpmlOutline item)
		{
			System.Diagnostics.Debug.Assert(item!=null);
			//
			if(_body!=null)
			{	
				item.SetDocument(_document);
				item.SetParent(null);
				item.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_body.OnSubItemPropertyChanged);			
			} 
			else
			{
				item.SetParent(this._outline);
				item.SetDocument(this._outline.Document);
				item.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_outline.OnSubItemPropertyChanged);
			}
			SetDirtyState();			
		}
		
		private void DetachItem (OpmlOutline item)
		{
			System.Diagnostics.Debug.Assert(item!=null);
			//
			if(_body!=null)
			{					
				item.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(_body.OnSubItemPropertyChanged);			
			} 
			else
			{
				item.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(_outline.OnSubItemPropertyChanged);
			}
			item.SetDocument(null);
			item.SetParent(null);
			//
			SetDirtyState();
		}
		
		#endregion
}
	
	/// <summary>An outline is an XML element, possibly containing one or more attributes, and containing any number of outline sub-elements.</summary>
	[System.Xml.Serialization.XmlTypeAttribute("outline")]
	[Serializable]
	[System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class OpmlOutline : Raccoom.Xml.ComponentModel.SyndicationObjectBase, IOpmlOutline
	{
		#region fields
		/// <summary>Text</summary>
		private string _text;
		/// <summary>Type</summary>
		private string _type;
		/// <summary>Description</summary>
		private string _description;
		/// <summary>XmlUrl</summary>
		private string _xmlUrl;
		/// <summary>HtmlUrl</summary>
		private string _htmlUrl;
		/// <summary>IsComment</summary>
		private bool _isComment;
		/// <summary>IsBreakpoint</summary>
		private bool _isBreakpoint;
		/// <summary>Items</summary>
		private OpmlOutlineCollection _items;
		/// <summary>the document that the outline is assigned to.</summary>
		private OpmlDocument _document;
		/// <summary>the parent outline that the outline is assigned to.</summary>
		private OpmlOutline _outline;
        /// <summary>Date created</summary>
        private DateTime _created;
        /// <summary>categories</summary>
        private string _category;
        /// <summary>version</summary>
        private string _version;
        /// <summary>language</summary>
        private System.Globalization.CultureInfo _language;
        /// <summary>holds the associated inclusion document</summary>
        OpmlDocument _inclusionDocument;
		#endregion
		
		#region constructors
		
		/// <summary>Initializes a new instance of OpmlOutline with default values.</summary>
		public OpmlOutline ()
		{
			_items = new OpmlOutlineCollection(this);
		}		
	    #endregion
		
		#region public interface        
        [XmlIgnore, System.ComponentModel.Browsable(true), System.ComponentModel.Category("Inclusion")]
        public OpmlDocument InclusionDocument
        {
            get
            {
                return _inclusionDocument;
            }
            set
            {
                if (_inclusionDocument != value)
                {
                    _inclusionDocument = value;                    
                }
            }

        }
        /// <summary>
        ///Determines if this outline contains an inclusion link
        /// <br></br>
        ///An outline element whose type is link must have a url attribute whose value is an http
        ///address. The text element is, as usual, what's displayed in the outliner; it's also what is
        ///displayed in an HTML rendering.
        ///When a link element is expanded in an outliner, if the address ends with ".opml", the outline
        ///expands in place. This is called inclusion.
        ///If the address does not end with ".opml" the link is assumed to point to something that can be
        ///displayed in a web browser.
        ///In OPML 2.0 a new type is introduced. An outline element whose type is include must have a
        ///url attribute that points to the OPML file to be included. The text attribute is, as usual, what's
        ///displayed in the outliner, and it's also what is displayed in an HTML rendering.
        ///The difference between link and include is that link may point to something that is displayed
        ///in a web browser, and include always points to an OPML file.
        /// </summary>
        public bool IsInclusion
        {
            get
            {
                return Type != null && ((string.Compare(Type, "include",true) == 0 || (string.Compare(Type,"link", true)== 0 && System.IO.Path.GetExtension(XmlUrl) == ".opml")));
            }
        }
        /// <summary>
        /// Indicates if the instance is specified and contains valid data (Xml Serialisation)
        /// </summary>
        public override bool Specified
        {
            get
            {
                return true;
            }
        }
        public IOpmlOutline CreateOutline()
        {
            return new OpmlOutline();
        }
		/// <summary>
		/// Removes the current outline from the document.
		/// </summary>
		/// <remarks>
		/// When the Remove method is called, the outline and any child outline items assigned to the document are removed from the document. The removed child outlines are removed from the document , but are still attached to this outline item.
		/// </remarks>
		public void Remove ()
		{
			if(this.Parent != null) Parent.Items.Remove(this);
			else if(this.Document!=null) this.Document.Body.Items.Remove(this);
		}
		
		/// <summary>Gets the document that the outline is assigned to.</summary>
		[System.ComponentModel.Category("Data"), System.ComponentModel.Description("Gets the document that the outline is assigned to.")]		
		[System.Xml.Serialization.XmlIgnore]
        [System.ComponentModel.Browsable(false)]
		public OpmlDocument Document
		{
			get
			{
				return _document;
			}
		}
		
		/// <summary>Gets the document that the outline is assigned to.</summary>
		[System.ComponentModel.Category("Data"), System.ComponentModel.Description("Gets the document that the outline is assigned to.")]		
		[System.Xml.Serialization.XmlIgnore]
		IOpmlDocument IOpmlOutline.Document
		{
			get
			{
				return this.Document;
			}
		}
		
		/// <summary>Sets the document that the outline is assigned to.</summary>
		internal void SetDocument (OpmlDocument value)
		{
			this._document= value;
			this._items.SetDocument(value);
		}
		
		/// <summary>Gets the outline that this outline is assigned to.</summary>
		/// <remarks>
		/// If the outline is at the root level, the Parent property returns null. 
		/// </remarks>		
		[System.ComponentModel.Category("Data"), System.ComponentModel.Description("Gets the outline that this outline is assigned to.")]
		#if DEBUG
			[System.ComponentModel.Browsable(true)]
		#else
			[System.ComponentModel.Browsable(false)]
		#endif
		[System.Xml.Serialization.XmlIgnore]
		public OpmlOutline Parent
		{
			get
			{
				return _outline;
			}
		}
		
		/// <summary>Gets the outline that this outline is assigned to.</summary>
		/// <remarks>
		/// If the outline is at the root level, the Parent property returns null. 
		/// </remarks>		
		[System.ComponentModel.Category("Data"), System.ComponentModel.Description("Gets the outline that this outline is assigned to."), System.ComponentModel.Browsable(false)]
		[System.Xml.Serialization.XmlIgnore]
		IOpmlOutline IOpmlOutline.Parent
		{
			get
			{
				return this.Parent;
			}
		}
		
		/// <summary>Sets the outline that this outline is assigned to.</summary>
		internal void SetParent (OpmlOutline value)
		{
			this._outline = value;
		}
		
		/// <summary>Text is the string of characters that's displayed when the outline is being browsed or edited. There is no specific limit on the length of the text attribute.</summary>
		[System.ComponentModel.Category("Required outline elements"), System.ComponentModel.Description("Text is the string of characters that's displayed when the outline is being browsed or edited. There is no specific limit on the length of the text attribute.")]
		[System.Xml.Serialization.XmlAttribute("text")]
		public string Text
		{
			get
			{
				return _text;
			}
			
			set
			{
				bool changed = !object.Equals(_text, value);
				_text = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Text));
			}
		}
		
		// end Text
		
		/// <summary>Type is a string, it says how the other attributes of the outline are interpreted</summary>
		[System.ComponentModel.Category("Optional outline elements"), System.ComponentModel.Description("Type is a string, it says how the other attributes of the outline are interpreted")]
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
		
		/// <summary></summary>
		[System.ComponentModel.Category("Optional outline elements"), System.ComponentModel.Description("")]
		[System.Xml.Serialization.XmlAttribute("description")]
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
		
		/// <summary>Gets or sets the favorite url.</summary>

        private string _uri;
        /// <summary>When outline type is link url must have a value that is an http address.</summary>
        [System.ComponentModel.Category("Optional outline elements"), System.ComponentModel.Description("When outline type is link url must have a value that is an http address.")]
        [System.Xml.Serialization.XmlAttribute("url")]		
        public string Url
        {
            get { return _uri; }
            set 
            {
                if (_uri != value)
                {
                    _uri = value;
                    //
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Url));
                }
            }
        }
	
		[System.ComponentModel.Category("Required outline elements"), System.ComponentModel.Description("Gets or sets the favorite url.")]
		[System.Xml.Serialization.XmlAttribute("xmlUrl")]
		public string XmlUrl
		{
			get
			{
				return _xmlUrl;
			}
			
			set
			{
				bool changed = !object.Equals(_xmlUrl, value);
				_xmlUrl = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.XmlUrl));
			}
		}
		
		// end XmlUrl
		
		/// <summary></summary>
		[System.ComponentModel.Category("Optional outline elements"), System.ComponentModel.Description("")]
		[System.Xml.Serialization.XmlAttribute("htmlUrl")]
		public string HtmlUrl
		{
			get
			{
				return _htmlUrl;
			}
			
			set
			{
				bool changed = !object.Equals(_htmlUrl, value);
				_htmlUrl = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.HtmlUrl));
			}
		}
		
		// end HtmlUrl
		
		/// <summary>IsComment is a string, either true or false, indicating whether the outline is commented or not. By convention if an outline is commented, all subordinate outlines are considered to be commented as well. If it's not present, the value is false.</summary>
		[System.ComponentModel.Category("Optional outline elements"), System.ComponentModel.Description("IsComment is a string, either true or false, indicating whether the outline is commented or not. By convention if an outline is commented, all subordinate outlines are considered to be commented as well. If it's not present, the value is false.")]
		[System.Xml.Serialization.XmlAttribute("isComment")]
		public bool IsComment
		{
			get
			{
				return _isComment;
			}
			
			set
			{
				bool changed = !object.Equals(_isComment, value);
				_isComment = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.IsComment));
			}
		}
		
		// end IsComment
		
		/// <summary>IsBreakpoint is a string, either true or false, indicating whether a breakpoint is set on this outline. This attribute is mainly necessary for outlines used to edit scripts that execute. If it's not present, the value is false.</summary>
		[System.ComponentModel.Category("Optional outline elements"), System.ComponentModel.Description("IsBreakpoint is a string, either true or false, indicating whether a breakpoint is set on this outline. This attribute is mainly necessary for outlines used to edit scripts that execute. If it's not present, the value is false.")]
		[System.Xml.Serialization.XmlAttribute("isBreakpoint")]
		public bool IsBreakpoint
		{
			get
			{
				return _isBreakpoint;
			}
			
			set
			{
				bool changed = !object.Equals(_isBreakpoint, value);
				_isBreakpoint = value;
				if(changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.IsBreakpoint));
			}
		}
        [System.ComponentModel.Category("Optional outline elements"), System.ComponentModel.Description("The date-time that the outline node was created.")]
        [XmlIgnore]
        public DateTime Created
        {
            get
            {
                return _created;
            }
            set
            {
                if (_created != value)
                {
                    _created = value;
                    //
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Created));
                }
            }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value"), System.Xml.Serialization.XmlAttribute("created"), System.ComponentModel.Browsable(false)]
        public string CreatedRfc
        {
            get
            {
                return _created.ToUniversalTime().ToString("r");
            }
            set
            {
                // do nothing
            }
        }
        [System.ComponentModel.Category("Optional outline elements"), System.ComponentModel.Description("A string of comma-separated slash-delimited category strings, in the format defined by the RSS 2.0 category element. To represent a 'tag,' the category string should contain no slashes.")]
        [System.Xml.Serialization.XmlAttribute("category")]
        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                if (_category != value)
                {
                    _category = value;
                    //
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Category));
                }
            }
        }
        [System.ComponentModel.Category("Optional outline elements"), System.ComponentModel.Description("The value of the top-level language element. title is probably the same as text, it should not be omitted. title contains the top-level title element from the feed.")]
        [XmlIgnore]
        public System.Globalization.CultureInfo Language
        {
            get
            {
                return _language;
            }
            set
            {
                if (_language != value)
                {
                    _language = value;
                    //
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Language));
                }
            }
        }
        // end Language

        /// <summary>
        /// Internal, gets the CultureInfo ISO Code
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value"), System.Xml.Serialization.XmlElementAttribute("language", DataType = "language")]
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public string LanguageIso
        {
            get
            {
                return this.Language != null ? this.Language.ToString() : string.Empty;
            }
            set {/* do nothing */}
        }
        [System.ComponentModel.Category("Optional outline elements"), System.ComponentModel.Description("Varies depending on the version of RSS that's being supplied. It was invented at a time when we thought there might be some processors that only handled certain versions, but that hasn't turned out to be a major issue. The values it can have are: RSS1 for RSS 1.0; RSS for 0.91, 0.92 or 2.0; scriptingNews for scriptingNews format. There are no known values for Atom feeds, but they certainly could be provided.")]
        [System.Xml.Serialization.XmlAttribute("version")]
        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                if (_version != value)
                {
                    _version = value;
                    //
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Version));
                }
            }
        }
		
		// end IsBreakpoint
		
		/// <summary>OpmlOutline elements.</summary>
		[System.ComponentModel.Category("Optional outline elements"), System.ComponentModel.Description("Outline elements.")]
		[System.Xml.Serialization.XmlElementAttribute("outline")]
		public OpmlOutlineCollection Items
		{
			get
			{
                if (_inclusionDocument != null) return _inclusionDocument.Body.Items;
				return _items;
			}
		}
        [XmlIgnore, System.ComponentModel.Browsable(false)]
        public bool ItemsSpecified
        {
            get
            {
                return _inclusionDocument == null;
            }
            set {/* do nothing */}
        }
		
		// end Items
		
		System.Collections.IList IOpmlOutline.Items
		{
			get
			{
				return _items;
			}
		}
		
		/// <summary>
		/// Obtains the String representation of this instance. 
		/// </summary>
		/// <returns>The friendly name</returns>
		public override string ToString ()
		{
			return Text;
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
			public const string Text = "Text";
			public const string Type = "Type";
			public const string Description = "Description";
            public const string Url = "Url";
			public const string XmlUrl = "XmlUrl";
			public const string HtmlUrl = "HtmlUrl";
			public const string IsComment = "IsComment";
			public const string IsBreakpoint = "IsBreakpoint";
			public const string Items = "Items";
            public const string Created = "Created";
            public const string Language = "Language";
            public const string Version = "Version";
            public const string Category = "Category";
		}
		
		#endregion
		
		#region events
		
		///<summary>A PropertyChanged event is raised when a sub property is changed. A PropertyChangedEventArgs object specifies the name of the property that changed.</summary>
		protected internal void OnSubItemPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			OnPropertyChanged(e);	
		}
		
		#endregion
    }
}