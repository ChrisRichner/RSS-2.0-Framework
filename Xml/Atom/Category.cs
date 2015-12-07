using System;
using System.Collections.Generic;
using System.Text;

namespace Raccoom.Xml.Atom
{
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
    public class AtomCategory : AtomCommonAttributes
    {
        string _term;
        Uri _scheme;
        string _label;
        /// <summary>
        /// The "term" attribute is a string that identifies the category to which the entry or feed belongs.  RssCategory elements MUST have a "term" attribute.
        /// </summary>
        [System.Xml.Serialization.XmlAttribute("term")]
        public string Term
        {
            get
            {
                return _term;
            }
            set
            {
                if (_term == value) return;
                _term = value;
            }
        }
        /// <summary>
        /// The "scheme" attribute is an IRI that identifies a categorization scheme.  RssCategory elements MAY have a "scheme" attribute.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Uri Scheme
        {
            get
            {
                return _scheme;
            }
            set
            {
                if (_scheme == value) return;
                _scheme = value;
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        [System.ComponentModel.Browsable(false)]
        public bool SchemeSpecified { get { return _scheme != null; } set { } }
        [System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlAttribute("scheme")]
        public string SchemeString { get { return _scheme.ToString().ToLower(); } set { } }
        /// <summary>
        /// <![CDATA[The "label" attribute provides a human-readable label for display in  end-user applications.  The content of the "label" attribute is Language-Sensitive.  Entities such as "&amp;" and "&lt;" represent their corresponding characters ("&" and ">", respectively), not markup.  RssCategory elements MAY have a "label" attribute.]]>
        /// </summary>
        [System.Xml.Serialization.XmlAttribute("label")]        
        public string Label
        {
            get
            {
                return _label;
            }
            set
            {
                if (_label == value) return;
                _label = value;
            }
        }        
    }
}
