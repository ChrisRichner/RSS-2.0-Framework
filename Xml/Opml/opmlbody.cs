using System;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Net;

namespace Raccoom.Xml.Opml
{	
	/// <summary>A body contains one or more outline elements</summary>
	[Serializable]
	[System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class OpmlBody : Raccoom.Xml.ComponentModel.SyndicationObjectBase, IOpmlBody
	{
		#region fields
		/// <summary>Items</summary>
		private OpmlOutlineCollection _items;
		/// <summary>the document that the body is assigned to.</summary>
		private OpmlDocument _document;
		
		#endregion
		
		#region constructors
		
		/// <summary>Initializes a new instance of OpmlBody</summary>
		public OpmlBody ()
		{
			this._items = new OpmlOutlineCollection(this);
		}
		
		#endregion
		
		#region public interface
        public override bool Specified
        {
            get
            {
                return _items!= null && _items.Count > 0;
            }
        }

        /// <summary>Gets the document that the body is assigned to.</summary>
		#if DEBUG
			[System.ComponentModel.Browsable(true)]
		#else
			[System.ComponentModel.Browsable(false)]
		#endif		
		[System.Xml.Serialization.XmlIgnore]
		[System.ComponentModel.Category("Data"), System.ComponentModel.Description("Gets the document that the body is assigned to.")]        
		public OpmlDocument Document
		{
			get
			{
				return _document;
			}
		}
		
		internal void SetDocument (OpmlDocument value)
		{
			this._document = value;
			this._items.SetDocument(value);
		}
		
		/// <summary>Gets the document that the outline is assigned to.</summary>
		[System.Xml.Serialization.XmlIgnore]
		IOpmlDocument IOpmlBody.Document
		{
			get
			{
				return Document;
			}
		}
		
		/// <summary>OpmlOutline elements.</summary>
		[System.ComponentModel.Category("OpmlBody"), System.ComponentModel.Description("Outline elements.")]
		[System.Xml.Serialization.XmlElementAttribute("outline")]
		public virtual OpmlOutlineCollection Items
		{
			get
			{
				return _items;
			}
		}
		
		// end Items
        public IOpmlOutline CreateOutline()
        {
            return new OpmlOutline();
        }
		System.Collections.IList IOpmlBody.Items
		{
			get
			{
				return Items as System.Collections.IList;
			}
		}
		
		// end Items
		
		/// <summary>
		/// Obtains the String representation of this instance. 
		/// </summary>
		/// <returns>The friendly name</returns>
		public override string ToString ()
		{
			return "Outlines: " +_items.Count.ToString();
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
			public const string Items = "Items";
		}
		
		#endregion
		
		#region events
		
		///<summary>A PropertyChanged event is raised when a sub property is changed. A PropertyChangedEventArgs object specifies the name of the property that changed.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
        protected internal virtual void OnSubItemPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			OnPropertyChanged(e);	
		}
		
		#endregion
	}
}