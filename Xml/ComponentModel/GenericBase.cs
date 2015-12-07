using System;
using System.Collections.Generic;
using System.Text;

namespace Raccoom.Xml.ComponentModel
{
    /// <summary>
    /// Serves as base class for all syndication related classes (<see cref="Rss.RssChannel"/>,<see cref="Opml.OpmlDocument"/>,<see cref="Atom.AtomFeed"/>) and defines the most common interface and features.
    /// </summary>
    public abstract class SyndicationObjectBase : System.ComponentModel.INotifyPropertyChanged, IExtendableObject
    {
        #region fields
        /// <summary>Occurs when a property value changes.</summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        /// <summary>xml document namespaces</summary>
        System.Xml.Serialization.XmlSerializerNamespaces _xmlns;
        /// <summary>xml content that belongs to namespaces</summary>
        System.Xml.XmlDocumentFragment _docFragment;
        #endregion

        #region internal interface
        /// <summary>
        /// Indicates if the instance is specified and contains valid data (Xml Serialisation)
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
        public virtual bool Specified
        {
            get
            {
                return false;
            }
        }
        ///<summary>A PropertyChanged event is raised when a property is changed on a component. A PropertyChangedEventArgs object specifies the name of the property that changed.</summary>
        protected virtual void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        [System.ComponentModel.Category("Extensions")]
        [System.Xml.Serialization.XmlIgnore]
        System.Xml.Serialization.XmlSerializerNamespaces IExtendableObject.Xmlns
        {
            get
            {
                if (_xmlns == null) _xmlns = new System.Xml.Serialization.XmlSerializerNamespaces();
                return _xmlns;
            }
        }

        [System.ComponentModel.Category("Extensions")]
        [System.Xml.Serialization.XmlAnyElement]
        public System.Xml.XmlNode[] Extensions
        {
            get
            {                
                if (_docFragment == null) return null;
                System.Xml.XmlNode[] nodes = new System.Xml.XmlNode[_docFragment.ChildNodes.Count];                
                int i = 0;                
                foreach (System.Xml.XmlNode node in _docFragment.ChildNodes)
                {
                    nodes[i] = node;
                    i++;
                }
                return nodes; 
            }
            set
            {
                //
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlIgnore]
        public System.Xml.XmlDocumentFragment ExtendedContent
        {
            get
            {
                if (_docFragment == null)
                {
                    _docFragment = new System.Xml.XmlDocument().CreateDocumentFragment();                    
                }
                return _docFragment;
            }
        }
        #endregion
    }
}
