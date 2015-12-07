using System;
using System.Collections.Generic;
using System.Text;
using Raccoom.Xml.ComponentModel;

namespace Raccoom.Xml.Atom
{
    /// <summary>
    /// The "atom:feed" element is the document (i.e., top-level) element of
    /// an Atom Feed OpmlDocument, acting as a container for metadata and data
    /// associated with the feed.  Its element children consist of metadata
    /// elements followed by zero or more atom:entry child elements.
    /// <para><![CDATA[
    /// atomFeed =
    ///    element atom:feed {
    ///       atomCommonAttributes,
    ///       (atomAuthor*
    ///        & atomCategory*
    ///        & atomContributor*
    ///        & atomGenerator?
    ///        & atomIcon?
    ///        & atomId
    ///        & atomLink*
    ///        & atomLogo?
    ///        & atomRights?
    ///        & atomSubtitle?
    ///        & atomTitle
    ///        & atomUpdated
    ///        & extensionElement*),
    ///       atomEntry*
    ///    }]]>
    /// </para>
    /// </summary>
    /// <remarks>
    /// This specification assigns no significance to the order of atom:entry
    /// elements within the feed.  
    /// <para>
    /// The following child elements are defined by this specification (note
    /// that the presence of some of these elements is required):
    /// </para><para>
    /// <list type="bullet">
    /// <item>atom:feed elements MUST contain one or more atom:author elements,
    ///    unless all of the atom:feed element's child atom:entry elements
    ///    contain at least one atom:author element.
    /// </item><item>atom:feed elements MAY contain any number of atom:category
    ///    elements.
    /// </item><item>atom:feed elements MAY contain any number of atom:contributor
    ///    elements.
    /// </item><item>atom:feed elements MUST NOT contain more than one atom:generator
    ///    element.
    /// </item><item>atom:feed elements MUST NOT contain more than one atom:icon
    ///    element.
    /// </item><item>atom:feed elements MUST NOT contain more than one atom:logo
    ///    element.
    /// </item><item>atom:feed elements MUST contain exactly one atom:id element.
    /// </item><item>atom:feed elements SHOULD contain one atom:link element with a rel
    ///    attribute value of "self".  This is the preferred URI for
    ///    retrieving Atom Feed Documents representing this Atom feed.
    /// </item><item>atom:feed elements MUST NOT contain more than one atom:link
    ///    element with a rel attribute value of "alternate" that has the
    ///    same combination of type and hreflang attribute values.
    /// </item><item>atom:feed elements MAY contain additional atom:link elements
    ///    beyond those described above.
    /// </item><item>atom:feed elements MUST NOT contain more than one atom:rights
    ///    element.
    /// </item><item>atom:feed elements MUST NOT contain more than one atom:subtitle
    ///    element.
    /// </item><item>atom:feed elements MUST contain exactly one atom:title element.
    /// </item><item>atom:feed elements MUST contain exactly one atom:updated element.</item></list>
    /// </para><para>
    /// If multiple atom:entry elements with the same atom:id value appear in
    /// an Atom Feed OpmlDocument, they represent the same entry.  Their
    /// atom:updated timestamps SHOULD be different.  If an Atom Feed
    /// OpmlDocument contains multiple entries with the same atom:id, Atom
    /// Processors MAY choose to display all of them or some subset of them.
    /// One typical behavior would be to display only the entry with the
    /// latest atom:updated timestamp.
    /// </para>
    /// </remarks>
    [System.Xml.Serialization.XmlTypeAttribute("feed",Namespace="http://www.w3.org/2005/Atom")]
    [Serializable]
    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class AtomFeed : AtomSyndicationObjectBase
    {
        #region fields
        SyndicationObjectRelationCollection<AtomEntry> _entries;
        AtomGenerator _generator;
        Uri _iconUri;
        Uri _logoUri;
        #endregion

        #region ctor
        public AtomFeed(){} 
        #endregion

        #region public interface
        /// <summary>
        /// The "atom:generator" element's content identifies the agent used to
        /// generate a feed, for debugging and other purposes.
        /// <para>
        /// atomGenerator = element atom:generator {
        ///    atomCommonAttributes,
        ///    attribute uri { atomUri }?,
        ///    attribute version { text }?,
        ///    text
        /// }
        /// </para><para>
        /// The content of this element, when present, MUST be a string that is a
        /// human-readable name for the generating agent.  Entities such as <![CDATA[
        /// "&amp;" and "&lt;" represent their corresponding characters ("&" and
        /// "<" respectively), not markup.]]>
        /// </para><para>
        /// The atom:generator element MAY have a "uri" attribute whose value
        /// MUST be an IRI reference [RFC3987].  When dereferenced, the resulting
        /// URI (mapped from an IRI, if necessary) SHOULD produce a
        /// representation that is relevant to that agent.
        /// </para><para>
        /// The atom:generator element MAY have a "version" attribute that
        /// indicates the version of the generating agent.
        /// </para>
        /// </summary>
        [System.Xml.Serialization.XmlElement("generator")]
        public AtomGenerator Generator
        {
            get
            {
                if (_generator == null) _generator = new AtomGenerator();
                return _generator;
            }
            set
            {
                if (_generator != value)
                {
                    _generator = value;
                    //
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Generator));
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore, System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool GeneratorSpecified { get { return _generator != null && _generator.Specified; } set { } }

        /// <summary>
        ///  The "atom:icon" element's content is an IRI reference [RFC3987] that
        /// identifies an image that provides iconic visual identification for a
        /// feed.
        /// <para>
        /// atomIcon = element atom:icon {
        ///    atomCommonAttributes,
        ///    (atomUri)
        /// }
        /// </para><para>
        /// The image SHOULD have an aspect ratio of one (horizontal) to one
        /// (vertical) and SHOULD be suitable for presentation at a small size.
        /// </para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Uri Icon
        {
            get { return _iconUri; }
            set 
            {
                if (_iconUri == value) return;
                //
                _iconUri = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Icon));
            }
        }
        [System.Xml.Serialization.XmlIgnore, System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool IconStringSpecified { get { return _iconUri != null; } set { } }
        [System.Xml.Serialization.XmlElement("icon")]
        [System.ComponentModel.Browsable(false)]
        public string IconString { get { return _iconUri.ToString().ToLower(); } set { } }
	
        /// <summary>
        /// The "atom:logo" element's content is an IRI reference [RFC3987] that
        /// identifies an image that provides visual identification for a feed.
        /// <para>
        /// atomLogo = element atom:logo {
        ///    atomCommonAttributes,
        ///    (atomUri)
        /// }
        /// </para><para>
        /// The image SHOULD have an aspect ratio of 2 (horizontal) to 1
        /// (vertical).
        /// </para><para>
        /// </para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Uri Logo
        {
            get
            {
                return _logoUri;
            }
            set
            {
                if (_logoUri == value) return;
                _logoUri = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Logo));
            }
        }
        [System.Xml.Serialization.XmlIgnore, System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool LogoStringSpecified { get { return _logoUri != null; } set { } }
        [System.Xml.Serialization.XmlElement("logo")]
        [System.ComponentModel.Browsable(false)]
        public string LogoString { get { return _logoUri.ToString().ToLower(); } set { } }

        /// <summary>
        /// The "atom:entry" element represents an individual entry, acting as a
        /// container for metadata and data associated with the entry.  This
        /// element can appear as a child of the atom:feed element, or it can
        /// appear as the document (i.e., top-level) element of a stand-alone
        /// Atom Entry OpmlDocument.
        /// <para>
        /// </para>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("entry")]
        public SyndicationObjectRelationCollection<AtomEntry> Entries
        {
            get
            {
                if (_entries == null) _entries = new SyndicationObjectRelationCollection<AtomEntry>(this);
                return _entries;
            }
        }

        public AtomEntry CreateEntry()
        {
            return new AtomEntry();
        }
        public AtomGenerator CreateGenerator()
        {
            return new AtomGenerator();
        }
        #endregion

        #region nested classes
        /// <summary>
        /// public writeable class properties
        /// </summary>		
        new internal struct Fields
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
