using System;
using System.Collections.Generic;
using System.Text;

namespace Raccoom.Xml.Atom
{
    /// <summary>
    /// The "atom:generator" element's content identifies the agent used to generate a feed, for debugging and other purposes.
    /// </summary>    
    public class AtomGenerator : AtomCommonAttributes
    {
        string _uri;
        int _version;
        /// <summary>
        /// The atom:generator element MAY have a "version" attribute that
        /// indicates the version of the generating agent.
        /// </summary>
        [System.Xml.Serialization.XmlAttribute("version")]
        public int Version
        {
            get
            {
                return _version;
            }
            set
            {
                if (_version == value) return;
                //
                _version = value;
            }
        }
        /// <summary>
        /// The content of this element, when present, MUST be a string that is a
        /// human-readable name for the generating agent.  Entities such as<![CDATA[
        /// "&amp;" and "&lt;" represent their corresponding characters ("&" and
        /// "<" respectively), not markup.]]>
        /// <para>
        /// The atom:generator element MAY have a "uri" attribute whose value
        /// MUST be an IRI reference [RFC3987].  When dereferenced, the resulting
        /// URI (mapped from an IRI, if necessary) SHOULD produce a
        /// representation that is relevant to that agent.
        /// </para>
        /// </summary>
        public string Uri
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
    }
}
