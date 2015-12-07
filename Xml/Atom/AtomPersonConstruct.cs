using System;
using System.Collections.Generic;
using System.Text;

namespace Raccoom.Xml.Atom
{
    /// <summary>
    /// A Person construct is an element that describes a person,
    /// corporation, or similar entity (hereafter, 'person').
    /// <para><![CDATA[
    /// atomPersonConstruct =
    ///    atomCommonAttributes,
    ///    (element atom:name { text }
    ///     & element atom:uri { atomUri }?
    ///     & element atom:email { atomEmailAddress }?
    ///     & extensionElement*)]]>
    /// </para><para>
    /// This specification assigns no significance to the order of appearance
    /// of the child elements in a Person construct.  Person constructs allow
    /// extension Metadata elements 
    /// </para>
    /// </summary>
    public class AtomPersonConstruct : AtomCommonAttributes
    {
        #region fields
        string _email;
        Uri _uri;
        string _name; 
        #endregion

        #region public interface
        /// <summary>
        /// The "atom:email" element's content conveys an e-mail address
        /// associated with the person.  Person constructs MAY contain an
        /// atom:email element, but MUST NOT contain more than one.  Its content
        /// MUST conform to the "addr-spec" production in [RFC2822].
        /// </summary>
        [System.Xml.Serialization.XmlAttribute("email")]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (_email == value) return;
                _email = value;
            }
        }
        /// <summary>
        /// The "atom:uri" element's content conveys an IRI associated with the
        /// person.  Person constructs MAY contain an atom:uri element, but MUST
        /// NOT contain more than one.  The content of atom:uri in a Person
        /// construct MUST be an IRI reference 
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Uri Uri
        {
            get
            {
                return _uri;
            }
            set
            {
                if (_uri == value) return;
                _uri = value;
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        [System.ComponentModel.Browsable(false)]
        public bool UriStringSpecified { get { return _uri != null; } set { } }
        [System.ComponentModel.Browsable(false)]
        
        [System.Xml.Serialization.XmlAttribute("uri")]
        public string UriString { get { return _uri.ToString().ToLower(); } set { } }
        /// <summary>
        /// The "atom:name" element's content conveys a human-readable name for
        /// the person.  The content of atom:name is Language-Sensitive.  Person
        /// constructs MUST contain exactly one "atom:name" element.
        /// </summary>
        [System.Xml.Serialization.XmlAttribute("name")]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value) return;
                _name = value;
            }
        }
        #endregion

        //public override bool Specified
        //{
        //    get
        //    {
        //        return _email!=null || _name !=null|| _uri!=null;
        //    }
        //}
    }
}
