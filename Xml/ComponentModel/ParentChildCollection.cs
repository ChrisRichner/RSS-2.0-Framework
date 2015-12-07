
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
    public class SyndicationObjectRelationCollection<TItem> : System.Collections.ObjectModel.Collection<TItem>, ISyndicationObjectCollectionInstance
            where TItem : ISyndicationObjectRelationItem, new()
    {
        #region fields
        ISyndicationObjectRelationOwner _parent;
        #endregion

        #region ctor
        public SyndicationObjectRelationCollection(ISyndicationObjectRelationOwner parent)
        {
            _parent = parent;
        }
        #endregion

        #region public interface
        public TItem Add()
        {
            TItem item = new TItem();
            base.Add(item);
            return item;
        }
        #endregion

        #region internal interface
        protected override void InsertItem(int index, TItem item)
        {
            base.InsertItem(index, item);
            // connect child with parent
            item.Parent = _parent;
            item.Collection = this;
            // notify parent
            _parent.NotifyCollectionChanged(new System.ComponentModel.PropertyChangedEventArgs(typeof(TItem).Name + " added"));
            // add event handler
            if (item is System.ComponentModel.INotifyPropertyChanged)
            {
                ((System.ComponentModel.INotifyPropertyChanged)item).PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ParentChildCollection_PropertyChanged);
            }
        }
        protected override void RemoveItem(int index)
        {
            ISyndicationObjectRelationItem item = base[index] as ISyndicationObjectRelationItem;
            // remove child / parent connection
            base[index].Parent = null;
            base[index].Collection = null;
            // remove item from collection
            base.RemoveItem(index);
            // notify parent
            _parent.NotifyCollectionChanged(new System.ComponentModel.PropertyChangedEventArgs(typeof(TItem).Name + " removed"));
            // remove event handler
            if (item != null && item is System.ComponentModel.INotifyPropertyChanged)
            {
                ((System.ComponentModel.INotifyPropertyChanged)item).PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(ParentChildCollection_PropertyChanged);
            }
        }        
        void ParentChildCollection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ISyndicationObjectRelationItem child = sender as ISyndicationObjectRelationItem;
            if (child == null) return;
            //
            child.Parent.NotifyCollectionChanged(e);
        }        
        #endregion

        #region ISyndicationObjectCollectionInstance Members

        void ISyndicationObjectCollectionInstance.RemoveItem(object item)
        {
            this.Remove((TItem)item);
        }

        #endregion
    }
    public interface ISyndicationObjectCollectionInstance
    {
        void RemoveItem(object item);
    }
    public interface ISyndicationObjectRelationOwner : System.ComponentModel.INotifyPropertyChanged
    {        
        void NotifyCollectionChanged(System.ComponentModel.PropertyChangedEventArgs e);
    }
    public interface ISyndicationObjectRelationItem
    {
        ISyndicationObjectCollectionInstance Collection { get;set;}
        ISyndicationObjectRelationOwner Parent { get;set;}
    }
}
