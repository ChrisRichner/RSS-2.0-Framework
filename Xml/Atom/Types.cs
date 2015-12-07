using System;
using System.Collections.Generic;
using System.Text;

namespace Raccoom.Xml.Atom
{
    /// <summary>
    /// Experience teaches that feeds that contain textual content are in
    /// general more useful than those that do not.  Some applications (one
    /// example is full-text indexers) require a minimum amount of text or
    /// (X)HTML to function reliably and predictably.  Feed producers should
    /// be aware of these issues.  It is advisable that each atom:entry
    /// element contain a non-empty atom:title element, a non-empty
    /// <para>
    /// atom:content element when that element is present, and a non-empty
    /// atom:summary element when the entry contains no atom:content element.
    /// However, the absence of atom:summary is not an error, and Atom
    /// Processors MUST NOT fail to function correctly as a consequence of
    /// such an absence.
    /// </para>
    /// </summary>
    public enum AtomContentType
    {
        /// <summary>
        /// Example atom:title with text content:
        ///
        /// ... <![CDATA[
        /// <title type="text">
        /// Less: &lt;
        /// </title>
        /// ...]]>
        ///
        /// If the value is "text", the content of the Text construct MUST NOT
        /// contain child elements.  Such text is intended to be presented to
        /// humans in a readable fashion.  Thus, Atom Processors MAY collapse
        /// white space (including line breaks) and display the text using
        /// typographic techniques such as justification and proportional fonts.
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "text")]
        Text,
        /// <summary>
        /// Example atom:title with XHTML content:
        ///
        /// ...<![CDATA[
        /// <title type="xhtml" xmlns:xhtml="http://www.w3.org/1999/xhtml">
        /// <xhtml:div>
        /// Less: <xhtml:em> &lt; </xhtml:em>
        /// </xhtml:div>
        /// </title>
        /// ...]]>
        ///
        /// If the value of "type" is "xhtml", the content of the Text construct
        /// MUST be a single XHTML div element [XHTML] and SHOULD be suitable for
        /// handling as XHTML.  The XHTML div element itself MUST NOT be
        /// considered part of the content.  Atom Processors that display the
        /// content MAY use the markup to aid in displaying it.  The escaped
        /// versions of characters such as <![CDATA["&" and ">"]]> represent those
        /// characters, not markup.
        ///
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "xhtml")]
        XHtml,
        /// <summary>
        /// Example atom:title with HTML content:
        ///
        /// ...<![CDATA[
        /// <title type="html">
        /// Less: &lt;em> &amp;lt; &lt;/em>
        /// </title>
        /// ...]]>
        ///
        /// If the value of "type" is "html", the content of the Text construct
        /// MUST NOT contain child elements and SHOULD be suitable for handling
        /// as HTML [HTML].  Any markup within MUST be escaped; for example,
        /// <![CDATA["<br>"]]> as "&lt;br>".  HTML markup within SHOULD be such that it could
        /// validly appear directly within an HTML <![CDATA[<DIV>]]> element, after
        /// unescaping.  Atom Processors that display such content MAY use that
        /// markup to aid in its display.
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "html")]
        Html,
    }
}
