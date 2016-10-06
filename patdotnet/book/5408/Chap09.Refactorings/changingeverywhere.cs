using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Chap09.Refactorings {
    
    class RefactoredIterator< type> {
        private IList< type> _list;
        
        public RefactoredIterator( IList< type> list) {
            _list = list;
        }
        public void Iterate() {
            foreach( type item in _list) {
                ProcessItem( item);
            }
        }
        public virtual void ProcessItem( type item) { }
    }
    
    class Item {
        public int Id;
        public bool MarkedFlag = false;
        
        public Item( int id) {
            Id = id;
        }
        public override string ToString() {
            return "ID (" + Id + ")";
        }
    }
    
    class DuplicatedInternal2 {
        public IList< Item> _list = new List< Item>();
        
        public DuplicatedInternal2() {
            
        }
        private class InternalGetItem : RefactoredIterator< Item> {
            public int Id;
            public Item FoundItem;
            
            public InternalGetItem( IList< Item> list, int id) : base( list) {
                FoundItem = null;
                Id = id;
                Iterate();
            }
            
            public override void ProcessItem( Item item) {
                if( item.Id == Id) {
                    FoundItem = item;
                }
            }
        }
        public Item GetItem(int id) {
            return new InternalGetItem( _list, id).FoundItem;
        }
    }
    class DuplicatedInternal {
        private IList _items = new ArrayList();
        
        public Item GetItem(int id) {
            foreach(Item item in _items) {
                if(item.Id == id) {
                    return item;
                }
            }
            
            return null;
        }
        public bool MarkItems() {
            bool updated = false;
            
            foreach(Item item in _items) {
                if(!item.MarkedFlag) {
                    item.MarkedFlag = true;
                    updated = true;
                }
            }
            return updated;
        }
    }

    delegate bool Processor<type>( type element);
    
    class LocalIterator< type> {
        IList _list;
        Processor< type> _processor;
        
        public LocalIterator( IList list, Processor< type> processor) {
            _list = list;
            _processor = processor;
        }
        public bool Iterate() {
            foreach( type element in _list) {
                if( !_processor( element)) {
                    return false;
                }
            }
            return true;
        }
    }
    
    class RefactoredInternal {
        public IList _items = new ArrayList();

        public Item GetItem( int id) {
            Item found = null;
            new LocalIterator< Item>( _items, new Processor< Item>( delegate( Item item) {
                                                                       if( item.Id == id) {
                                                                           found = item;
                                                                           return false;
                                                                       }
                                                                       return true;
                                                                   }
                                                                   )).Iterate();
            return found ;
        }
        
        public bool MarkAsRead() {
            return false;
        }
    }
    
    [TestFixture]
    public class TestRefactored {
        [Test]
        public void TestExample() {
            RefactoredInternal refactored = new RefactoredInternal();
            refactored._items.Add( new Item( 10));
            refactored._items.Add( new Item( 20));
            
            Item item = refactored.GetItem( 10);
            Assert.IsNotNull( item);
            Assert.AreEqual( 10, item.Id);
        }
        [Test]
        public void TestExample2() {
            DuplicatedInternal2 refactored = new DuplicatedInternal2();
            
            refactored._list.Add( new Item( 10));
            refactored._list.Add( new Item( 20));
        
            Item item = refactored.GetItem( 10);
            Assert.IsNotNull( item);
            Assert.AreEqual( 10, item.Id);
        }
    }
}
