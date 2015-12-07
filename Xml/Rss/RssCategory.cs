using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Raccoom.Xml.ComponentModel;

namespace Raccoom.Xml.Rss
{
    /// <summary>
    /// The value of the element is a forward-slash-separated string that identifies a hierarchic location in the indicated taxonomy.
    /// </summary>
    [Serializable]
    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    [System.Runtime.InteropServices.ComVisible(true), System.Runtime.InteropServices.Guid("EEF78F05-A8D4-42fa-8CF6-8694B9458597")]
    [System.Runtime.InteropServices.ClassInterface(System.Runtime.InteropServices.ClassInterfaceType.None)]
    [System.Runtime.InteropServices.ProgId("Raccoom.RssCategory")]
    public class RssCategory : Raccoom.Xml.ComponentModel.SyndicationObjectBase, IRssCategory, ISyndicationObjectRelationItem
    {
        #region fields
        string _value;
        string _domain;
        ISyndicationObjectRelationOwner _parent;
        ISyndicationObjectCollectionInstance _collectionInstance;
        #endregion

        #region ctor
        public RssCategory()
        {

        }
        public RssCategory(string value, string domain)
            : this()
        {
            _value = value;
            _domain = domain;
        } 
        #endregion

        #region public interface
        public override bool Specified
        {
            get
            {
                return !string.IsNullOrEmpty(Value);
            }
        }
        /// <summary>The value of the element is a forward-slash-separated string that identifies a hierarchic location in the indicated taxonomy. Processors may establish conventions for the interpretation of categories.</summary>
        [System.ComponentModel.Category("Required category elements"), System.ComponentModel.Description("The value of the element is a forward-slash-separated string that identifies a hierarchic location in the indicated taxonomy. Processors may establish conventions for the interpretation of categories.")]
        [System.Xml.Serialization.XmlTextAttribute]
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == value) return;
                //
                _value = value;
                //
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Value));
                
            }
        }
        /// <summary>A string that identifies a categorization taxonomy.</summary>
        [System.ComponentModel.Category("Optionale category elements"), System.ComponentModel.Description("A string that identifies a categorization taxonomy.")]
        [XmlAttribute("domain")]
        public string Domain
        {
            get
            {
                return _domain;
            }
            set
            {
                if (_domain == value) return;
                //
                _domain = value;
                //
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(Fields.Domain));
            }
        }
        public override string ToString()
        {
            return Value;
        }
        #endregion

        #region internal interface
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
        internal struct Fields
        {
            public const string Value = "Value";
            public const string Domain = "Domain";
        }
        #endregion
    }
}
