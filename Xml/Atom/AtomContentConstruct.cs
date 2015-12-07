using System;
using System.Collections.Generic;
using System.Text;

namespace Raccoom.Xml.Atom
{
    /// <summary>
    /// A Text construct contains human-readable text, usually in small
    /// quantities.  The content of Text constructs is Language-Sensitive.
    /// <para><![CDATA[
    /// atomPlainTextConstruct =
    ///    atomCommonAttributes,
    ///    attribute type { "text" | "html" }?,
    ///    text
    /// </para><para>
    /// atomXHTMLTextConstruct =
    ///    atomCommonAttributes,
    ///    attribute type { "xhtml" },
    ///    xhtmlDiv]]>
    /// </para><para>
    /// atomTextConstruct = atomPlainTextConstruct | atomXHTMLTextConstruct
    /// </para>
    /// </summary>
    public class AtomContentConstruct : AtomCommonAttributes
    {
        #region fields
        AtomContentType _types;
        string _value; 
        #endregion

        #region public interface
        /// <summary>
        /// Text constructs MAY have a "type" attribute.  When present, the value
        /// MUST be one of "text", "html", or "xhtml".  If the "type" attribute
        /// is not provided, Atom Processors MUST behave as though it were
        /// present with a value of "text".  Unlike the atom:content element
        /// defined in Section 4.1.3, MIME media types [MIMEREG] MUST NOT be used
        /// as values for the "type" attribute on Text constructs. 
        /// </summary>
        [System.Xml.Serialization.XmlAttribute("type")]
        public AtomContentType Type
        {
            get
            {
                return _types;
            }
            set
            {
                if (_types == value) return;
                _types = value;
            }
        }
        /// <summary>
        /// Example atom:title with text content:
        /// ...
        /// <example><![CDATA[<title type="text">
        ///   Less: &lt;
        /// </title>]]></example>
        /// ...
        /// <para>
        /// If the value is "text", the content of the Text construct MUST NOT
        /// contain child elements.  Such text is intended to be presented to
        /// humans in a readable fashion.  Thus, Atom Processors MAY collapse
        /// white space (including line breaks) and display the text using
        /// typographic techniques such as justification and proportional fonts.
        /// </para>
        /// </summary>
        [System.Xml.Serialization.XmlTextAttribute]
        public string Value
        {
            get
            {
                switch (_types)
                {
                    case AtomContentType.Html:                        
                        return System.Web.HttpUtility.HtmlEncode(_value);                        
                    case AtomContentType.XHtml:
                        return string.Format("{0}{1}{2}",@"<div xmlns=""http://www.w3.org/1999/xhtml"">", System.Web.HttpUtility.HtmlEncode(_value),"</div>");
                }
                return _value;
            }
            set
            {
                if (_value == value) return;
                _value = value;
            }
        } 
        #endregion
    }
}
