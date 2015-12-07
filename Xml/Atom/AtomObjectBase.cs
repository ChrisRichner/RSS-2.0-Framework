using System;
using System.Collections.Generic;
using System.Text;
using Raccoom.Xml.ComponentModel;

namespace Raccoom.Xml.Atom
{

    /// <summary>
    /// Serves as base class for all syndication related classes (<see cref="Atom.AtomFeed"/>,<see cref="Atom.AtomEntry"/>) and defines the most common interface and features.
    /// </summary>
    [Serializable]
    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public abstract class AtomSyndicationObjectBase : Raccoom.Xml.ComponentModel.SyndicationObjectBase, Raccoom.Xml.ComponentModel.ISyndicationObjectRelationOwner
    {
        #region fields
        SyndicationObjectRelationCollection<AtomPersonConstruct> _authors;
        SyndicationObjectRelationCollection<AtomPersonConstruct> _contributors;
        SyndicationObjectRelationCollection<AtomContentConstruct> _links;
        SyndicationObjectRelationCollection<AtomCategory> _categories;
        AtomContentConstruct _rights;
        AtomContentConstruct _subTitle;
        AtomContentConstruct _title;
        DateTime _updated;
        Uri _id;
        #endregion

        #region ctor
        public AtomSyndicationObjectBase()
        {
            _updated = DateTime.Now;
        }
        #endregion

        #region public interface
        /// <summary>
        /// The "atom:author" element is a Person construct that indicates the
        /// author of the entry or feed.
        /// <para>
        /// atomAuthor = element atom:author { atomPersonConstruct }
        /// </para><para>
        /// If an atom:entry element does not contain atom:author elements, then
        /// the atom:author elements of the contained atom:source element are
        /// considered to apply.  In an Atom Feed OpmlDocument, the atom:author
        /// elements of the containing atom:feed element are considered to apply
        /// to the entry if there are no atom:author elements in the locations
        /// described above.
        /// </para>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("author")]
        public SyndicationObjectRelationCollection<AtomPersonConstruct> Authors
        {
            get
            {
                if (_authors == null) _authors = new SyndicationObjectRelationCollection<AtomPersonConstruct>(this);
                return _authors;
            }
        }

        /// <summary>
        /// The "atom:category" element conveys information about a category
        /// associated with an entry or feed.  This specification assigns no
        /// meaning to the content (if any) of this element.
        /// <para><![CDATA[
        /// atomCategory =
        ///    element atom:category {
        ///       atomCommonAttributes,
        ///       attribute term { text },
        ///       attribute scheme { atomUri }?,
        ///       attribute label { text }?,
        ///       undefinedContent
        ///    }]]>
        /// </para>
        /// </summary>
        /// <remarks>
        /// .2.1.  The "term" Attribute
        /// <para>
        /// The "term" attribute is a string that identifies the category to
        /// which the entry or feed belongs.  RssCategory elements MUST have a
        /// "term" attribute.
        /// </para><para>
        /// .2.2.  The "scheme" Attribute
        /// </para><para>
        /// The "scheme" attribute is an IRI that identifies a categorization
        /// scheme.  RssCategory elements MAY have a "scheme" attribute.
        /// </para><para>
        /// .2.3.  The "label" Attribute
        /// </para><para>
        /// The "label" attribute provides a human-readable label for display in
        /// end-user applications.  The content of the "label" attribute is
        /// Language-Sensitive.  Entities such as "&amp;" and "&lt;" represent<![CDATA[
        /// their corresponding characters ("&" and "<", respectively), not
        /// markup.  RssCategory elements MAY have a "label" attribute.]]>
        /// </para>
        /// </remarks>
        [System.Xml.Serialization.XmlElementAttribute("category")]
        public SyndicationObjectRelationCollection<AtomCategory> Categories
        {
            get
            {
                if (_categories == null) _categories = new SyndicationObjectRelationCollection<AtomCategory>(this);
                return _categories;
            }
        }

        /// <summary>
        ///  The "atom:contributor" element is a Person construct that indicates a
        /// person or other entity who contributed to the entry or feed.
        /// <para>
        /// atomContributor = element atom:contributor { atomPersonConstruct }
        /// </para>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("contributor")]
        public SyndicationObjectRelationCollection<AtomPersonConstruct> Contributors
        {
            get
            {
                if (_contributors == null) _contributors = new SyndicationObjectRelationCollection<AtomPersonConstruct>(this);
                return _contributors;
            }
        }

        /// <summary>
        /// The "atom:id" element conveys a permanent, universally unique
        /// identifier for an entry or feed.
        /// <para>
        /// atomId = element atom:id {
        ///    atomCommonAttributes,
        ///    (atomUri)
        /// }
        /// </para><para>
        /// Its content MUST be an IRI, as defined by [RFC3987].  Note that the
        /// definition of "IRI" excludes relative references.  Though the IRI
        /// might use a dereferencable scheme, Atom Processors MUST NOT assume it
        /// can be dereferenced.        
        /// </para><para>
        /// When an Atom OpmlDocument is relocated, migrated, syndicated,
        /// republished, exported, or imported, the content of its atom:id
        /// element MUST NOT change.  Put another way, an atom:id element
        /// pertains to all instantiations of a particular Atom entry or feed;
        /// revisions retain the same content in their atom:id elements.  It is
        /// suggested that the atom:id element be stored along with the
        /// associated resource.
        /// </para><para>
        /// The content of an atom:id element MUST be created in a way that
        /// assures uniqueness.
        /// </para><para>
        /// Because of the risk of confusion between IRIs that would be
        /// equivalent if they were mapped to URIs and dereferenced, the
        /// following normalization strategy SHOULD be applied when generating
        /// atom:id elements:
        /// </para><para>
        /// <list type="bullet">
        /// <item>Provide the scheme in lowercase characters.
        /// </item><item>Provide the host, if any, in lowercase characters.
        /// </item><item>Only perform percent-encoding where it is essential.
        /// </item><item>Use uppercase A through F characters when percent-encoding.
        /// </item><item>Prevent dot-segments from appearing in paths.
        /// </item><item>For schemes that define a default authority, use an empty
        ///    authority if the default is desired.
        /// </item><item>For schemes that define an empty path to be equivalent to a path
        ///    of "/", use "/".
        /// </item><item>For schemes that define a port, use an empty port if the default
        ///    is desired.
        /// </item><item>Preserve empty fragment identifiers and queries.
        /// </item><item>Ensure that all components of the IRI are appropriately character
        ///    normalized, e.g., by using NFC or NFKC.</item></list>
        /// </para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Uri Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id == value) return;
                //
                _id = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Id));
            }
        }
        [System.Xml.Serialization.XmlIgnore, System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool IdStringSpecified { get { return _id != null; } set { } }
        [System.Xml.Serialization.XmlElement("id")]
        [System.ComponentModel.Browsable(false)]
        public string IdString { get { return _id.ToString().ToLower(); } set { } }

        /// <summary>
        /// The "atom:link" element defines a reference from an entry or feed to
        /// a Web resource.  This specification assigns no meaning to the content
        /// (if any) of this element.
        /// <para><![CDATA[
        /// atomLink =
        ///    element atom:link {
        ///       atomCommonAttributes,
        ///       attribute href { atomUri },
        ///       attribute rel { atomNCName | atomUri }?,
        ///       attribute type { atomMediaType }?,
        ///       attribute hreflang { atomLanguageTag }?,
        ///       attribute title { text }?,
        ///       attribute length { text }?,
        ///       undefinedContent
        ///    }]]>
        /// </para>
        /// </summary>
        /// <remarks>
        /// .7.1.  The "href" Attribute
        /// <para>
        /// The "href" attribute contains the link's IRI. atom:link elements MUST
        /// have an href attribute, whose value MUST be a IRI reference
        /// [RFC3987].
        /// </para><para>
        /// .7.2.  The "rel" Attribute
        /// </para><para>
        /// atom:link elements MAY have a "rel" attribute that indicates the link
        /// relation type.  If the "rel" attribute is not present, the link
        /// element MUST be interpreted as if the link relation type is
        /// "alternate".
        /// </para><para>
        /// The value of "rel" MUST be a string that is non-empty and matches
        /// either the "isegment-nz-nc" or the "IRI" production in [RFC3987].
        /// Note that use of a relative reference other than a simple name is not
        /// allowed.  If a name is given, implementations MUST consider the link
        /// relation type equivalent to the same name registered within the IANA
        /// </para><para>
        /// Registry of Link Relations (Section 7), and thus to the IRI that
        /// would be obtained by appending the value of the rel attribute to the
        /// string "http://www.iana.org/assignments/relation/".  The value of
        /// "rel" describes the meaning of the link, but does not impose any
        /// behavioral requirements on Atom Processors.
        /// </para><para>
        /// This document defines five initial values for the Registry of Link
        /// Relations:
        /// </para><para>
        /// 1.  The value "alternate" signifies that the IRI in the value of the
        ///     href attribute identifies an alternate version of the resource
        ///     described by the containing element.
        /// </para><para>
        /// 2.  The value "related" signifies that the IRI in the value of the
        ///     href attribute identifies a resource related to the resource
        ///     described by the containing element.  For example, the feed for a
        ///     site that discusses the performance of the search engine at
        ///     "http://search.example.com" might contain, as a child of
        ///     atom:feed:
        /// </para><para>
        ///     <link rel="related" href="http://search.example.com/"/>
        /// </para><para>
        ///     An identical link might appear as a child of any atom:entry whose
        ///     content contains a discussion of that same search engine.
        /// </para><para>
        /// 3.  The value "self" signifies that the IRI in the value of the href
        ///     attribute identifies a resource equivalent to the containing
        ///     element.
        /// </para><para>
        /// 4.  The value "enclosure" signifies that the IRI in the value of the
        ///     href attribute identifies a related resource that is potentially
        ///     large in size and might require special handling.  For atom:link
        ///     elements with rel="enclosure", the length attribute SHOULD be
        ///     provided.
        /// </para><para>
        /// 5.  The value "via" signifies that the IRI in the value of the href
        ///     attribute identifies a resource that is the source of the
        ///     information provided in the containing element.
        /// </para><para>
        /// .7.3.  The "type" Attribute
        /// </para><para>
        /// On the link element, the "type" attribute's value is an advisory
        /// media type: it is a hint about the type of the representation that is
        /// expected to be returned when the value of the href attribute is
        /// dereferenced.  Note that the type attribute does not override the
        /// actual media type returned with the representation.  Link elements
        /// MAY have a type attribute, whose value MUST conform to the syntax of
        /// a MIME media type [MIMEREG].       
        /// </para><para>
        /// .7.4.  The "hreflang" Attribute
        /// </para><para>
        /// The "hreflang" attribute's content describes the language of the
        /// resource pointed to by the href attribute.  When used together with
        /// the rel="alternate", it implies a translated version of the entry.
        /// Link elements MAY have an hreflang attribute, whose value MUST be a
        /// language tag [RFC3066].
        /// </para><para>
        /// .7.5.  The "title" Attribute
        /// </para><para>
        /// The "title" attribute conveys human-readable information about the
        /// link.  The content of the "title" attribute is Language-Sensitive.
        /// Entities such as "&amp;" and "&lt;" represent their corresponding
        /// <![CDATA[characters ("&" and "<",]]> respectively), not markup.  Link elements
        /// MAY have a title attribute.
        /// </para><para>
        /// .7.6.  The "length" Attribute
        /// </para><para>
        /// The "length" attribute indicates an advisory length of the linked
        /// content in octets; it is a hint about the content length of the
        /// representation returned when the IRI in the href attribute is mapped
        /// to a URI and dereferenced.  Note that the length attribute does not
        /// override the actual content length of the representation as reported
        /// by the underlying protocol.  Link elements MAY have a length
        /// attribute.
        /// </para>
        /// </remarks>
        [System.Xml.Serialization.XmlElementAttribute("link")]
        public SyndicationObjectRelationCollection<AtomContentConstruct> Links
        {
            get
            {
                if (_links == null) _links = new SyndicationObjectRelationCollection<AtomContentConstruct>(this);
                return _links;
            }
        }

        /// <summary>
        /// The "atom:rights" element is a Text construct that conveys
        /// information about rights held in and over an entry or feed.
        /// <para>
        /// atomRights = element atom:rights { atomTextConstruct }
        /// </para><para>
        /// The atom:rights element SHOULD NOT be used to convey machine-readable
        /// licensing information.
        /// </para><para>
        /// If an atom:entry element does not contain an atom:rights element,
        /// then the atom:rights element of the containing atom:feed element, if
        /// present, is considered to apply to the entry.
        /// </para>
        /// </summary>
        [System.Xml.Serialization.XmlElement("rights")]
        public AtomContentConstruct Rights
        {
            get
            {
                if (_rights == null) _rights = new AtomContentConstruct();
                return _rights;
            }
            set
            {
                if (_rights != value)
                {
                    _rights = value;
                    //
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Rights));
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore, System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool RightsSpecified { get { return _rights != null && _rights.Specified; } set { } }

        /// <summary>
        /// The "atom:subtitle" element is a Text construct that conveys a human-
        /// readable description or subtitle for a feed.
        /// <para>
        /// atomSubtitle = element atom:subtitle { atomTextConstruct }
        /// </para>
        /// </summary>
        [System.Xml.Serialization.XmlElement("subtitle")]
        public AtomContentConstruct SubTitle
        {
            get
            {
                if (_subTitle == null) _subTitle = new AtomContentConstruct();
                return _subTitle;
            }
            set
            {
                if (_subTitle != value)
                {
                    _subTitle = value;
                    //
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.SubTitle));
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore, System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool SubTitleSpecified { get { return _subTitle != null && _subTitle.Specified; } set { } }

        /// <summary>
        /// The "atom:title" element is a Text construct that conveys a human-
        /// readable title for an entry or feed.
        /// <para>
        /// atomTitle = element atom:title { atomTextConstruct }
        /// </para>
        /// </summary>
        [System.Xml.Serialization.XmlElement("title")]
        public AtomContentConstruct Title
        {
            get
            {
                if (_title == null) _title = new AtomContentConstruct();
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    //
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Title));
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore, System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool TitleSpecified { get { return _title != null && _title.Specified; } set { } }

        /// <summary>
        /// The "atom:updated" element is a Date construct indicating the most
        /// recent instant in time when an entry or feed was modified in a way
        /// the publisher considers significant.  Therefore, not all
        /// modifications necessarily result in a changed atom:updated value.
        /// <para>
        /// atomUpdated = element atom:updated { atomDateConstruct }
        /// </para><para>
        /// Publishers MAY change the value of this element over time.
        /// </para><para>
        /// </para>
        /// </summary>
        public DateTime Updated
        {
            get
            {
                return _updated;
            }
            set
            {
                if (_updated == value) return;
                _updated = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Updated));
            }
        }
        [System.Xml.Serialization.XmlIgnore, System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool UpdatedSpecified { get { return _updated != DateTime.MinValue; } set { } }

        public AtomContentConstruct CreateLink()
        {
            return new AtomContentConstruct();
        }
        public AtomPersonConstruct CreateAuthor()
        {
            return new AtomPersonConstruct();
        }
        public AtomPersonConstruct CreateContributor()
        {
            return CreateAuthor();
        }
        #endregion

        #region internal interface
        void ISyndicationObjectRelationOwner.NotifyCollectionChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(e.PropertyName));
        }
        #endregion

        #region nested classes
        /// <summary>
        /// public writeable class properties
        /// </summary>		
        internal struct Fields
        {
            public const string Rights = "Rights";
            public const string SubTitle = "SubTitle";
            public const string Title = "Title";
            public const string Generator = "Generator";
            public const string Icon = "Icon";
            public const string Id = "Id";
            public const string Link = "Link";
            public const string Logo = "Logo";
            public const string Updated = "Updated";
            public const string ExtensionElement = "ExtensionElement";
        }
        #endregion
    }


}
