using System;
using System.Collections.Generic;
using System.Text;
using Raccoom.Xml.ComponentModel;

namespace Raccoom.Xml.Atom
{
    /// <summary>
    /// atomCommonAttributes =
    /// attribute xml:base { atomUri }?,
    /// attribute xml:lang { atomLanguageTag }?,
    /// undefinedAttribute*
    /// </summary>
    public class AtomCommonAttributes : SyndicationObjectBase, ISyndicationObjectRelationItem
    {
        #region fields
        ISyndicationObjectRelationOwner _parent;
        ISyndicationObjectCollectionInstance _collectionInstance;
        System.Globalization.CultureInfo _lang;
        Uri _baseUri;
        #endregion

        #region public interface
        [System.Xml.Serialization.XmlIgnore]
        public System.Globalization.CultureInfo XmlLanguage
        {
            get
            {
                return _lang;
            }
            set
            {
                if (_lang == value) return;
                _lang = value;
            }
        }
        [System.Xml.Serialization.XmlIgnore, System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool XmlLanguageStringSpecified{get{return _lang != null;}set{}}
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlAttribute("xml:lang")]
        public string XmlLanguageString{get{return _lang != null ? _lang.ToString() : string.Empty;}set{}}
        [System.Xml.Serialization.XmlIgnore]
        public Uri BaseUri
        {
            get
            {
                return _baseUri;
            }
            set
            {
                if (_baseUri == value) return;
                _baseUri = value;
            }
        }
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlIgnore]
        public bool BaseUriStringSpecified { get { return _baseUri != null; } set{}}
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlAttribute("xml:base")]
        public string BaseUriString { get { return _baseUri.ToString().ToLower(); } set { } }
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
    }
}
