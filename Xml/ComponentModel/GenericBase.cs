
// Copyright © 2009 by Christoph Richner. All rights are reserved.
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//
// website http://www.raccoom.net, email support@raccoom.net, msn chrisdarebell@msn.com

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
