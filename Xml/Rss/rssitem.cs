
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
using Raccoom.Xml.ComponentModel;

namespace Raccoom.Xml.Rss
{
    /// <summary>An item may represent a "story" -- much like a story in a newspaper or magazine; if so its description is a synopsis of the story, and the link points to the full story. An item may also be complete in itself, if so, the description contains the text (entity-encoded HTML is allowed), and the link and title may be omitted. All elements of an item are optional, however at least one of title or description must be present.<see cref="IRssItem"/></summary>
    [System.Runtime.InteropServices.ComVisible(true), System.Runtime.InteropServices.Guid("026FF54F-94DF-4879-A355-880832C49A2C")]
    [System.Runtime.InteropServices.ClassInterface(System.Runtime.InteropServices.ClassInterfaceType.None)]
    [System.Runtime.InteropServices.ProgId("Raccoom.RssItem")]
    [Serializable]
    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class RssItem : ComponentModel.SyndicationObjectBase, IRssItem, Raccoom.Xml.ComponentModel.ISyndicationObjectRelationItem, Raccoom.Xml.ComponentModel.ISyndicationObjectRelationOwner
    {
        #region fields
        /// <summary>Title</summary>
        private string _title;
        /// <summary>Description</summary>
        private string _description;
        /// <summary>Link</summary>
        private string _link;
        /// <summary>Author</summary>
        private string _author;
        /// <summary>RssCategory</summary>
        private Raccoom.Xml.ComponentModel.SyndicationObjectRelationCollection<RssCategory> _category;
        /// <summary>PubDate</summary>
        private DateTime _pubDate;
        /// <summary>Comments</summary>
        private string _comments;
        /// <summary>RssEnclosure</summary>
        private RssEnclosure _enclosure;
        /// <summary>RssGuid</summary>
        private RssGuid _guid;
        /// <summary>RssSource</summary>
        private RssSource _source;
        /// <summary>Parent channel that the item is assigned to.</summary>
        Raccoom.Xml.ComponentModel.ISyndicationObjectRelationOwner _parent;
        ISyndicationObjectCollectionInstance _collectionInstance;
        #endregion

        #region constructors

        /// <summary>Initializes a new instance with default values</summary>
        public RssItem()
        {
            ((IRssItem)this).Enclosure = CreateEnclosure();
            ((IRssItem)this).Guid = CreateGuid();
            ((IRssItem)this).Source = CreateSource();
            this.PubDate = DateTime.Now;
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
        /// Removes the current item from the channel.
        /// </summary>
        /// <remarks>
        /// When the Remove method is called, the item is removed from the channel.
        /// </remarks>
        public void Remove()
        {
            this._collectionInstance.RemoveItem(this);
        }

        /// <summary>
        /// Gets the parent channel that the item is assigned to.
        /// </summary>
        [System.ComponentModel.Category("Data"), System.ComponentModel.Description("Gets the parent channel that the item is assigned to."), System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlIgnore]
        public RssChannel Channel
        {
            get
            {
                return _parent as RssChannel;
            }
        }

        /// <summary>
        /// Gets the parent channel that the item is assigned to.
        /// </summary>
        [System.ComponentModel.Category("Data"), System.ComponentModel.Description("Gets the parent channel that the item is assigned to.")]
        IRssChannel IRssItem.Channel
        {
            get
            {
                return Channel;
            }
        }

        /// <summary>The title of the item.</summary>
        [System.ComponentModel.Category("Required item elements"), System.ComponentModel.Description("The title of the item.")]
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
                if (changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Title));
            }
        }

        // end Title

        /// <summary>The item synopsis.</summary>
        [System.ComponentModel.Category("Required item elements"), System.ComponentModel.Description("The item synopsis.")]
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
                if (changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Description));
            }
        }

        // end Description

        /// <summary>The URL of the item.</summary>
        [System.ComponentModel.Category("Optional item elements"), System.ComponentModel.Description("The URL of the item.")]
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
                if (changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Link));
            }
        }

        // end Link

        /// <summary>Email address of the author of the item.</summary>
        [System.ComponentModel.Category("Optional item elements"), System.ComponentModel.Description("Email address of the author of the item.")]
        [System.Xml.Serialization.XmlElementAttribute("author")]
        public string Author
        {
            get
            {
                return _author;
            }

            set
            {
                bool changed = !object.Equals(_author, value);
                _author = value;
                if (changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Author));
            }
        }

        // end Author

        /// <summary>Includes the item in one or more categories.</summary>
        /// <summary>Specify one or more categories that the channel belongs to. Follows the same rules as the item-level category element.</summary>
        [System.ComponentModel.Category("Optional channel elements"), System.ComponentModel.Description("Specify one or more categories that the channel belongs to. Follows the same rules as the item-level category element.")]
        [System.Xml.Serialization.XmlElementAttribute("category")]
        public Raccoom.Xml.ComponentModel.SyndicationObjectRelationCollection<RssCategory> Category
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
        System.Collections.ObjectModel.Collection<IRssCategory> IRssItem.Category
        {
            get
            {
                System.Collections.ObjectModel.Collection<IRssCategory> cats = new System.Collections.ObjectModel.Collection<IRssCategory>();
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
        [System.ComponentModel.Category("Optional item elements"), System.ComponentModel.Description("Indicates when the item was published.")]
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
                if (changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.PubDate));
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
            set {/* do nothing */}
        }

        /// <summary>URL of a page for comments relating to the item. </summary>
        [System.ComponentModel.Category("Optional item elements"), System.ComponentModel.Description("URL of a page for comments relating to the item. ")]
        [System.Xml.Serialization.XmlElementAttribute("comments")]
        public string Comments
        {
            get
            {
                return _comments;
            }

            set
            {
                bool changed = !object.Equals(_comments, value);
                _comments = value;
                if (changed) OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Comments));
            }
        }

        // end Comments

        /// <summary>Describes a media object that is attached to the item. </summary>
        [System.Xml.Serialization.XmlElementAttribute("enclosure")]
        [System.ComponentModel.Category("Optional item elements"), System.ComponentModel.Description("Describes a media object that is attached to the item. ")]
        public RssEnclosure Enclosure
        {
            get
            {
                return _enclosure;
            }

            set
            {
                bool changed = !object.Equals(_enclosure, value);
                if (changed && _enclosure != null) _enclosure.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(OnSubItemPropertyChanged);
                _enclosure = value;
                if (changed)
                {
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Enclosure));
                    if (_enclosure != null) _enclosure.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnSubItemPropertyChanged);
                }
            }
        }

        // end RssEnclosure

        /// <summary>Describes a media object that is attached to the item. </summary>
        [System.Xml.Serialization.XmlElementAttribute("enclosure")]
        [System.ComponentModel.Category("Optional item elements"), System.ComponentModel.Description("Describes a media object that is attached to the item. ")]
        IRssEnclosure IRssItem.Enclosure
        {
            get
            {
                return Enclosure;
            }

            set
            {
                Enclosure = value as RssEnclosure;
            }
        }

        // end RssEnclosure

        /// <summary>
        /// Instructs the XmlSerializer whether or not to generate the XML element
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool EnclosureSpecified
        {
            get
            {
                return (_enclosure.Url != null && _enclosure.Url.Length > 0) || (_enclosure.Type != null && _enclosure.Type.Length > 0) || _enclosure.LengthSpecified;
            }
            set {/* do nothing */}
        }

        /// <summary>A string that uniquely identifies the item.</summary>
        [System.ComponentModel.Category("Optional item elements"), System.ComponentModel.Description("A string that uniquely identifies the item.")]
        [System.Xml.Serialization.XmlElementAttribute("guid")]
        public RssGuid Guid
        {
            get
            {
                return _guid;
            }

            set
            {
                bool changed = !object.Equals(_guid, value);
                if (changed && _guid != null) _guid.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(OnSubItemPropertyChanged);
                _guid = value;
                if (changed)
                {
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Guid));
                    if (_guid != null) _guid.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnSubItemPropertyChanged);
                }
            }
        }

        // end RssGuid

        /// <summary>A string that uniquely identifies the item.</summary>
        [System.ComponentModel.Category("Optional item elements"), System.ComponentModel.Description("A string that uniquely identifies the item.")]
        [System.Xml.Serialization.XmlElementAttribute("guid")]
        IRssGuid IRssItem.Guid
        {
            get
            {
                return this.Guid;
            }

            set
            {
                this.Guid = value as RssGuid;
            }
        }

        /// <summary>
        /// Instructs the XmlSerializer whether or not to generate the XML element
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool GuidSpecified
        {
            get
            {
                return _guid.Value != null && _guid.Value.Length > 0;
            }
            set {/* do nothing */}
        }

        /// <summary>The RSS channel that the item came from.</summary>
        [System.ComponentModel.Category("Optional item elements"), System.ComponentModel.Description("The RSS channel that the item came from.")]
        [System.Xml.Serialization.XmlElementAttribute("source")]
        public RssSource Source
        {
            get
            {
                return _source;
            }

            set
            {
                bool changed = !object.Equals(_source, value);
                if (changed && _source != null) _source.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(OnSubItemPropertyChanged);
                _source = value;
                if (changed)
                {
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Source));
                    if (_source != null) _source.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnSubItemPropertyChanged);
                }
            }
        }

        // end RssSource

        /// <summary>The RSS channel that the item came from.</summary>
        [System.ComponentModel.Category("Optional item elements"), System.ComponentModel.Description("The RSS channel that the item came from.")]
        [System.Xml.Serialization.XmlElementAttribute("source")]
        IRssSource IRssItem.Source
        {
            get
            {
                return Source;
            }

            set
            {
                Source = value as RssSource;
            }
        }

        /// <summary>
        /// Instructs the XmlSerializer whether or not to generate the XML element
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool SourceSpecified
        {
            get
            {
                return (_source.Url != null && _source.Url.Length > 0) || (_source.Value != null && _source.Value.Length > 0);
            }
            set {/* do nothing */}
        }

        /// <summary>
        /// Obtains the String representation of this instance. 
        /// </summary>
        /// <returns>The friendly name</returns>
        public override string ToString()
        {
            return Title;
        }

        #endregion

        #region internal interface
        protected virtual IRssEnclosure CreateEnclosure()
        {
            return new RssEnclosure();
        }
        IRssEnclosure IRssItem.CreateEnclosure()
        {
            return this.CreateEnclosure();
        }
        protected virtual IRssGuid CreateGuid()
        {
            return new RssGuid();
        }
        IRssGuid IRssItem.CreateGuid()
        {
            return CreateGuid();
        }
        protected virtual internal IRssSource CreateSource()
        {
            return new RssSource();
        }
        IRssSource IRssItem.CreateSource()
        {
            return this.CreateSource();
        }
        IRssCategory IRssItem.CreateCategory()
        {
            return this.CreateCategory();
        }
        protected internal virtual IRssCategory CreateCategory()
        {
            return new RssCategory();
        }
        Raccoom.Xml.ComponentModel.ISyndicationObjectRelationOwner Raccoom.Xml.ComponentModel.ISyndicationObjectRelationItem.Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }
        ISyndicationObjectCollectionInstance ISyndicationObjectRelationItem.Collection
        {
            get
            {
                return _collectionInstance;
            }
            set
            {
                _collectionInstance = value;
            }
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
            public const string Author = "Author";
            public const string Category = "Category";
            public const string PubDate = "PubDate";
            public const string Comments = "Comments";
            public const string Enclosure = "Enclosure";
            public const string Guid = "Guid";
            public const string Source = "Source";
        }

        #endregion

        #region events

        ///<summary>A PropertyChanged event is raised when a sub property is changed. A PropertyChangedEventArgs object specifies the name of the property that changed.</summary>
        protected internal void OnSubItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }

        #endregion

        #region IParent Members

        void ISyndicationObjectRelationOwner.NotifyCollectionChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Category));
        }

        #endregion
    }
}