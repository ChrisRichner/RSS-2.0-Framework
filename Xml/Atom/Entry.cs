using System;
using System.Collections.Generic;
using System.Text;
using Raccoom.Xml.ComponentModel;

namespace Raccoom.Xml.Atom
{
    /// <summary>
    /// The "atom:entry" element represents an individual entry, acting as a
    /// container for metadata and data associated with the entry.  This
    /// element can appear as a child of the atom:feed element, or it can
    /// appear as the document (i.e., top-level) element of a stand-alone
    /// Atom Entry OpmlDocument.
    /// <para><![CDATA[
    /// atomEntry =
    ///    element atom:entry {
    ///       atomCommonAttributes,
    ///       (atomAuthor*
    ///        & atomCategory*
    ///        & atomContent?
    ///        & atomContributor*
    ///        & atomId
    ///        & atomLink*
    ///        & atomPublished?
    ///        & atomRights?
    ///        & atomSource?
    ///        & atomSummary?
    ///        & atomTitle
    ///        & atomUpdated
    ///        & extensionElement*)
    ///    }]]>
    /// </para>
    /// </summary>
    /// <remarks>
    /// This specification assigns no significance to the order of appearance
    /// of the child elements of atom:entry.
    /// <para>
    /// The following child elements are defined by this specification (note
    /// that it requires the presence of some of these elements):
    /// </para><para>
    /// <list type="bullet">
    /// <item>atom:entry elements MUST contain one or more atom:author elements,
    ///    unless the atom:entry contains an atom:source element that
    ///    contains an atom:author element or, in an Atom Feed OpmlDocument, the
    ///    atom:feed element contains an atom:author element itself.</item>
    /// <item>atom:entry elements MAY contain any number of atom:category
    ///    elements.</item>
    /// <item>atom:entry elements MUST NOT contain more than one atom:content
    ///    element.</item>
    /// <item>atom:entry elements MAY contain any number of atom:contributor
    ///    elements.      </item> 
    /// <item>atom:entry elements MUST contain exactly one atom:id element.</item>
    /// <item>atom:entry elements that contain no child atom:content element
    ///    MUST contain at least one atom:link element with a rel attribute
    ///    value of "alternate".</item>
    /// <item>atom:entry elements MUST NOT contain more than one atom:link
    ///    element with a rel attribute value of "alternate" that has the
    ///    same combination of type and hreflang attribute values.</item>
    /// <item>atom:entry elements MAY contain additional atom:link elements
    ///    beyond those described above.</item>
    /// <item>atom:entry elements MUST NOT contain more than one atom:published
    ///    element.</item>
    /// <item>atom:entry elements MUST NOT contain more than one atom:rights
    ///    element.</item>
    /// <item>atom:entry elements MUST NOT contain more than one atom:source
    ///    element.</item>
    /// <item>atom:entry elements MUST contain an atom:summary element in either
    ///    of the following cases:
    ///    *  the atom:entry contains an atom:content that has a "src"
    ///       attribute (and is thus empty).
    ///    *  the atom:entry contains content that is encoded in Base64;
    ///       i.e., the "type" attribute of atom:content is a MIME media type
    ///       [MIMEREG], but is not an XML media type [RFC3023], does not
    ///       begin with "text/", and does not end with "/xml" or "+xml".</item>
    /// <item>atom:entry elements MUST NOT contain more than one atom:summary
    ///    element.</item>
    /// <item>atom:entry elements MUST contain exactly one atom:title element.</item>
    /// <item>atom:entry elements MUST contain exactly one atom:updated element.</item></list>
    /// </para>
    /// </remarks>
    [System.Xml.Serialization.XmlTypeAttribute("entry")]
    [Serializable]
    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class AtomEntry : AtomSyndicationObjectBase, ISyndicationObjectRelationOwner, ISyndicationObjectRelationItem
    {
        #region fields
        ISyndicationObjectRelationOwner _parent;
        ISyndicationObjectCollectionInstance _collectionInstance;
        AtomContentConstruct _summary;
        AtomContentConstruct _content;
        Uri _published;
        #endregion

        #region public interface        
        /// <summary>
        /// The "atom:content" element either contains or links to the content of
        /// the entry.  The content of atom:content is Language-Sensitive.
        /// <para>
        /// atomInlineTextContent =
        ///    element atom:content {
        ///       atomCommonAttributes,
        ///       attribute type { "text" | "html" }?,
        ///       (text)*
        ///    }
        /// </para><para>
        /// atomInlineXHTMLContent =
        ///    element atom:content {
        ///       atomCommonAttributes,
        ///       attribute type { "xhtml" },
        ///       xhtmlDiv
        ///    }
        /// </para><para>
        /// atomInlineOtherContent =
        ///    element atom:content {
        ///       atomCommonAttributes,
        ///       attribute type { atomMediaType }?,
        ///       (text|anyElement)*
        ///    }
        /// </para><para>
        /// atomOutOfLineContent =
        ///    element atom:content {
        ///       atomCommonAttributes,
        ///       attribute type { atomMediaType }?,
        ///       attribute src { atomUri },
        ///       empty
        ///    }
        /// </para><para>
        /// atomContent = atomInlineTextContent
        ///  | atomInlineXHTMLContent
        ///  | atomInlineOtherContent
        ///  | atomOutOfLineContent
        /// </para><para>
        /// .3.1.  The "type" Attribute
        /// </para><para>
        /// On the atom:content element, the value of the "type" attribute MAY be
        /// one of "text", "html", or "xhtml".  Failing that, it MUST conform to
        /// the syntax of a MIME media type, but MUST NOT be a composite type
        /// (see Section 4.2.6 of [MIMEREG]).  If neither the type attribute nor
        /// the src attribute is provided, Atom Processors MUST behave as though
        /// the type attribute were present with a value of "text".
        /// </para><para>
        /// .3.2.  The "src" Attribute
        /// </para><para>
        /// atom:content MAY have a "src" attribute, whose value MUST be an IRI
        /// reference [RFC3987].  If the "src" attribute is present, atom:content
        /// MUST be empty.  Atom Processors MAY use the IRI to retrieve the
        /// content and MAY choose to ignore remote content or to present it in a
        /// different manner than local content.
        /// </para><para>
        /// If the "src" attribute is present, the "type" attribute SHOULD be
        /// provided and MUST be a MIME media type [MIMEREG], rather than "text",
        /// "html", or "xhtml".  The value is advisory; that is to say, when the
        /// corresponding URI (mapped from an IRI, if necessary) is dereferenced,
        /// if the server providing that content also provides a media type, the
        /// server-provided media type is authoritative.
        /// </para><para>
        /// .3.3.  Processing Model
        /// </para><para>
        /// Atom Documents MUST conform to the following rules.  Atom Processors
        /// MUST interpret atom:content according to the first applicable rule.
        /// </para><para>
        /// 1.  If the value of "type" is "text", the content of atom:content
        ///     MUST NOT contain child elements.  Such text is intended to be
        ///     presented to humans in a readable fashion.  Thus, Atom Processors
        ///     MAY collapse white space (including line breaks), and display the
        ///     text using typographic techniques such as justification and
        ///     proportional fonts.
        /// </para><para><![CDATA[
        /// 2.  If the value of "type" is "html", the content of atom:content
        ///     MUST NOT contain child elements and SHOULD be suitable for
        ///     handling as HTML [HTML].  The HTML markup MUST be escaped; for
        ///     example, "<br>" as "&lt;br>".  The HTML markup SHOULD be such
        ///     that it could validly appear directly within an HTML <DIV>
        ///     element.  Atom Processors that display the content MAY use the
        ///     markup to aid in displaying it.]]>
        /// </para><para><![CDATA[
        /// 3.  If the value of "type" is "xhtml", the content of atom:content
        ///     MUST be a single XHTML div element [XHTML] and SHOULD be suitable
        ///     for handling as XHTML.  The XHTML div element itself MUST NOT be
        ///     considered part of the content.  Atom Processors that display the
        ///     content MAY use the markup to aid in displaying it.  The escaped
        ///     versions of characters such as "&" and ">" represent those
        ///     characters, not markup.]]>
        /// </para><para>
        /// 4.  If the value of "type" is an XML media type [RFC3023] or ends
        ///     with "+xml" or "/xml" (case insensitive), the content of
        ///     atom:content MAY include child elements and SHOULD be suitable
        ///     for handling as the indicated media type.  If the "src" attribute
        ///     is not provided, this would normally mean that the "atom:content"
        ///     element would contain a single child element that would serve as
        ///     the root element of the XML document of the indicated type.
        /// </para><para>
        /// 5.  If the value of "type" begins with "text/" (case insensitive),
        ///     the content of atom:content MUST NOT contain child elements.
        /// </para><para>
        /// 6.  For all other values of "type", the content of atom:content MUST
        ///     be a valid Base64 encoding, as described in [RFC3548], section 3.
        ///     When decoded, it SHOULD be suitable for handling as the indicated
        ///     media type.  In this case, the characters in the Base64 encoding
        ///     MAY be preceded and followed in the atom:content element by white
        ///     space, and lines are separated by a single newline (U+000A)
        ///     character.        
        /// </para><para>
        /// .3.4.  Examples
        /// </para><para>
        /// XHTML inline:
        /// </para><para><![CDATA[
        /// ...
        /// <content type="xhtml">
        ///    <div xmlns="http://www.w3.org/1999/xhtml">
        ///       This is <b>XHTML</b> content.
        ///    </div>
        /// </content>
        /// ...
        /// <content type="xhtml">
        ///    <xhtml:div xmlns:xhtml="http://www.w3.org/1999/xhtml">
        ///       This is <xhtml:b>XHTML</xhtml:b> content.
        ///    </xhtml:div>
        /// </content>
        /// ...]]>
        /// </para><para>
        /// The following example assumes that the XHTML namespace has been bound
        /// to the "xh" prefix earlier in the document:
        /// </para><para><![CDATA[
        /// ...
        /// <content type="xhtml">
        ///    <xh:div>
        ///       This is <xh:b>XHTML</xh:b> content.
        ///    </xh:div>
        /// </content>
        /// ...]]>
        /// </para><para>
        /// </para>
        /// </summary>
        public AtomContentConstruct Content
        {
            get
            {
                if (_content == null) _content = new AtomContentConstruct();
                return _content;
            }
            set
            {
                if (_content == value) return;
                _content = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Content));
            }
        }
        /// <summary>
        /// The "atom:published" element is a Date construct indicating an
        /// instant in time associated with an event early in the life cycle of
        /// the entry.
        /// <para>
        /// atomPublished = element atom:published { atomDateConstruct }
        /// </para><para>
        /// Typically, atom:published will be associated with the initial
        /// creation or first availability of the resource.
        /// </para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Uri Published
        {
            get { return _published; }
            set { _published = value; }
        }
        [System.Xml.Serialization.XmlIgnore, System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool PublishedStringSpecified { get { return _published != null; } set { } }
        [System.Xml.Serialization.XmlElement("id")]
        [System.ComponentModel.Browsable(false)]
        public string PublishedString { get { return _published.ToString().ToLower(); } set { } }
	
        /// <summary>
        /// The "atom:summary" element is a Text construct that conveys a short
        /// summary, abstract, or excerpt of an entry.
        /// <para>
        /// atomSummary = element atom:summary { atomTextConstruct }
        /// </para><para>
        /// It is not advisable for the atom:summary element to duplicate
        /// atom:title or atom:content because Atom Processors might assume there
        /// is a useful summary when there is none.
        /// </para>
        /// </summary>
        [System.Xml.Serialization.XmlElement("summary")]
        public AtomContentConstruct Summary
        {
            get
            {
                if (_summary == null) _summary = new AtomContentConstruct();
                return _summary;
            }
            set
            {
                if (_summary != value)
                {
                    _summary = value;
                    //
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Summary));
                }
            }
        }

        public void Remove()
        {
            if (_collectionInstance == null) return;
            _collectionInstance.RemoveItem(this);
        }
        #endregion

        #region IChildItem Members

        ISyndicationObjectRelationOwner ISyndicationObjectRelationItem.Parent
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
        new internal struct Fields
        {
            public const string Content = "Content";
            public const string Summary = "Summary";
        }
        #endregion
    }
}
