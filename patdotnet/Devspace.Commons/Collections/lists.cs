// one line to give the library's name and an idea of what it does.
// Copyright (C) 2005  Christian Gross devspace.com (christianhgross@yahoo.ca)
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OrderedIterator = org.apache.commons.collections.OrderedIterator;

namespace Devspace.Commons.Collections {
    
    /// <summary> A <code>List</code> implementation with a <code>ListIterator</code> that
    /// allows concurrent modifications to the underlying list.
    /// <p>
    /// This implementation supports all of the optional {@link List} operations.
    /// It extends <code>AbstractLinkedList</code> and thus provides the
    /// stack/queue/dequeue operations available in {@link java.util.LinkedList}.
    /// <p>
    /// The main feature of this class is the ability to modify the list and the
    /// iterator at the same time. Both the {@link #listIterator()} and {@link #cursor()}
    /// methods provides access to a <code>Cursor</code> instance which extends
    /// <code>ListIterator</code>. The cursor allows changes to the list concurrent
    /// with changes to the iterator. Note that the {@link #iterator()} method and
    /// sublists  do <b>not</b> provide this cursor behaviour.
    /// <p>
    /// The <code>Cursor</code> class is provided partly for backwards compatibility
    /// and partly because it allows the cursor to be directly closed. Closing the
    /// cursor is optional because references are held via a <code>WeakReference</code>.
    /// For most purposes, simply modify the iterator and list at will, and then let
    /// the garbage collector to the rest.
    /// <p>
    /// <b>Note that this implementation is not synchronized.</b>
    ///
    /// </summary>
    /// <seealso cref="java.util.LinkedList">
    /// </seealso>
    /// <since> Commons Collections 1.0
    /// </since>
    /// <version>  $Revision: 1.5 $ $Date: 2004/02/18 01:12:26 $
    ///
    /// </version>
    /// <author>  Rodney Waldhoff
    /// </author>
    /// <author>  Janek Bogucki
    /// </author>
    /// <author>  Simon Kitching
    /// </author>
    /// <author>  Stephen Colebourne
    /// </author>
    [Serializable]
    public class CursorableLinkedList< type>: LinkedList< type>, System.Runtime.Serialization.ISerializable {
        
        /// <summary>A list of the cursor currently open on this list </summary>
        [NonSerialized]
        protected internal IList< type> cursors = new List<type>();
        
        //-----------------------------------------------------------------------
        /// <summary> Constructor that creates.</summary>
        public CursorableLinkedList():base() {
            Init(); // must call init() as use super();
        }
        
        /// <summary> Constructor that copies the specified collection
        ///
        /// </summary>
        /// <param name="coll"> the collection to copy
        /// </param>
        public CursorableLinkedList(System.Collections.Generic.IEnumerable<type > collection):base(collection) {
        }
        
        /// <summary> The equivalent of a default constructor called
        /// by any constructor and by <code>readObject</code>.
        /// </summary>
        protected internal override void  Init() {
            cursors = new List<type>();
        }
        
        //-----------------------------------------------------------------------
        /// <summary> Returns an iterator that does <b>not</b> support concurrent modification.
        /// <p>
        /// If the underlying list is modified while iterating using this iterator
        /// a ConcurrentModificationException will occur.
        /// The cursor behaviour is available via {@link #listIterator()}.
        ///
        /// </summary>
        /// <returns> a new iterator that does <b>not</b> support concurrent modification
        /// </returns>
        //UPGRADE_ISSUE: The equivalent in .NET for method 'java.util.List.iterator' returns a different type. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1224_3"'
        override System.Collections.Generic.IEnumerator<type> GetEnumerator() {
            return base.GetEnumerator();
        }
        
        /// <summary> Returns a cursor iterator that allows changes to the underlying list in parallel.
        /// <p>
        /// The cursor enables iteration and list changes to occur in any order without
        /// invalidating the iterator (from one thread). When elements are added to the
        /// list, an event is fired to all active cursors enabling them to adjust to the
        /// change in the list.
        /// <p>
        /// When the "current" (i.e., last returned by {@link ListIterator#next}
        /// or {@link ListIterator#previous}) element of the list is removed,
        /// the cursor automatically adjusts to the change (invalidating the
        /// last returned value such that it cannot be removed).
        ///
        /// </summary>
        /// <returns> a new cursor iterator
        /// </returns>
        //UPGRADE_ISSUE: The equivalent in .NET for method 'java.util.List.listIterator' returns a different type. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1224_3"'
        public override System.Collections.IEnumerator GetEnumerator() {
            return cursor(0);
        }
        
        /// <summary> Returns a cursor iterator that allows changes to the underlying list in parallel.
        /// <p>
        /// The cursor enables iteration and list changes to occur in any order without
        /// invalidating the iterator (from one thread). When elements are added to the
        /// list, an event is fired to all active cursors enabling them to adjust to the
        /// change in the list.
        /// <p>
        /// When the "current" (i.e., last returned by {@link ListIterator#next}
        /// or {@link ListIterator#previous}) element of the list is removed,
        /// the cursor automatically adjusts to the change (invalidating the
        /// last returned value such that it cannot be removed).
        ///
        /// </summary>
        /// <param name="fromIndex"> the index to start from
        /// </param>
        /// <returns> a new cursor iterator
        /// </returns>
        public override System.Collections.IEnumerator listIterator(int fromIndex) {
            return cursor(fromIndex);
        }
        
        /// <summary> Returns a {@link Cursor} for iterating through the elements of this list.
        /// <p>
        /// A <code>Cursor</code> is a <code>ListIterator</code> with an additional
        /// <code>close()</code> method. Calling this method immediately discards the
        /// references to the cursor. If it is not called, then the garbage collector
        /// will still remove the reference as it is held via a <code>WeakReference</code>.
        /// <p>
        /// The cursor enables iteration and list changes to occur in any order without
        /// invalidating the iterator (from one thread). When elements are added to the
        /// list, an event is fired to all active cursors enabling them to adjust to the
        /// change in the list.
        /// <p>
        /// When the "current" (i.e., last returned by {@link ListIterator#next}
        /// or {@link ListIterator#previous}) element of the list is removed,
        /// the cursor automatically adjusts to the change (invalidating the
        /// last returned value such that it cannot be removed).
        /// <p>
        /// The {@link #listIterator()} method returns the same as this method, and can
        /// be cast to a <code>Cursor</code> if the <code>close</code> method is required.
        ///
        /// </summary>
        /// <returns> a new cursor iterator
        /// </returns>
        public virtual CursorableLinkedList.Cursor cursor() {
            return cursor(0);
        }
        
        /// <summary> Returns a {@link Cursor} for iterating through the elements of this list
        /// starting from a specified index.
        /// <p>
        /// A <code>Cursor</code> is a <code>ListIterator</code> with an additional
        /// <code>close()</code> method. Calling this method immediately discards the
        /// references to the cursor. If it is not called, then the garbage collector
        /// will still remove the reference as it is held via a <code>WeakReference</code>.
        /// <p>
        /// The cursor enables iteration and list changes to occur in any order without
        /// invalidating the iterator (from one thread). When elements are added to the
        /// list, an event is fired to all active cursors enabling them to adjust to the
        /// change in the list.
        /// <p>
        /// When the "current" (i.e., last returned by {@link ListIterator#next}
        /// or {@link ListIterator#previous}) element of the list is removed,
        /// the cursor automatically adjusts to the change (invalidating the
        /// last returned value such that it cannot be removed).
        /// <p>
        /// The {@link #listIterator(int)} method returns the same as this method, and can
        /// be cast to a <code>Cursor</code> if the <code>close</code> method is required.
        ///
        /// </summary>
        /// <param name="fromIndex"> the index to start from
        /// </param>
        /// <returns> a new cursor iterator
        /// </returns>
        /// <throws>  IndexOutOfBoundsException if the index is out of range </throws>
        /// <summary>      (index &lt; 0 || index &gt; size()).
        /// </summary>
        public virtual CursorableLinkedList.Cursor cursor(int fromIndex) {
            Cursor cursor = new Cursor(this, fromIndex);
            registerCursor(cursor);
            return cursor;
        }
        
        //-----------------------------------------------------------------------
        /// <summary> Updates the node with a new value.
        /// This implementation sets the value on the node.
        /// Subclasses can override this to record the change.
        ///
        /// </summary>
        /// <param name="node"> node to update
        /// </param>
        /// <param name="value"> new value of the node
        /// </param>
        protected internal override void  updateNode(Node node, System.Object value_Renamed) {
            base.updateNode(node, value_Renamed);
            broadcastNodeChanged(node);
        }
        
        /// <summary> Inserts a new node into the list.
        ///
        /// </summary>
        /// <param name="nodeToInsert"> new node to insert
        /// </param>
        /// <param name="insertBeforeNode"> node to insert before
        /// </param>
        /// <throws>  NullPointerException if either node is null </throws>
        protected internal override void  addNode(Node nodeToInsert, Node insertBeforeNode) {
            base.addNode(nodeToInsert, insertBeforeNode);
            broadcastNodeInserted(nodeToInsert);
        }
        
        /// <summary> Removes the specified node from the list.
        ///
        /// </summary>
        /// <param name="node"> the node to remove
        /// </param>
        /// <throws>  NullPointerException if <code>node</code> is null </throws>
        protected internal override void  removeNode(Node node) {
            base.removeNode(node);
            broadcastNodeRemoved(node);
        }
        
        /// <summary> Removes all nodes by iteration.</summary>
        protected internal override void  removeAllNodes() {
            if(Count > 0) {
                // superclass implementation would break all the iterators
                System.Collections.IEnumerator it = GetEnumerator();
                //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilIteratorhasNext_3"'
                while(it.MoveNext()) {
                    //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilIteratornext_3"'
                    System.Object generatedAux = it.Current;
                    //UPGRADE_ISSUE: Method 'java.util.Iterator.remove' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javautilIteratorremove_3"'
                    it.remove();
                }
            }
        }
        
        //-----------------------------------------------------------------------
        /// <summary> Registers a cursor to be notified of changes to this list.
        ///
        /// </summary>
        /// <param name="cursor"> the cursor to register
        /// </param>
        protected internal virtual void  registerCursor(Cursor cursor) {
            // We take this opportunity to clean the cursors list
            // of WeakReference objects to garbage-collected cursors.
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilIteratorhasNext_3"'
            for(System.Collections.IEnumerator it = cursors.GetEnumerator(); it.MoveNext();) {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilIteratornext_3"'
                System.WeakReference ref_Renamed = (System.WeakReference) it.Current;
                //UPGRADE_ISSUE: Method 'java.lang.ref.Reference.get' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangrefReference_3"'
                if(ref_Renamed.get_Renamed() == null) {
                    //UPGRADE_ISSUE: Method 'java.util.Iterator.remove' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javautilIteratorremove_3"'
                    it.remove();
                }
            }
            cursors.Add(new System.WeakReference(cursor));
        }
        
        /// <summary> Deregisters a cursor from the list to be notified of changes.
        ///
        /// </summary>
        /// <param name="cursor"> the cursor to deregister
        /// </param>
        protected internal virtual void  unregisterCursor(Cursor cursor) {
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilIteratorhasNext_3"'
            for(System.Collections.IEnumerator it = cursors.GetEnumerator(); it.MoveNext();) {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilIteratornext_3"'
                System.WeakReference ref_Renamed = (System.WeakReference) it.Current;
                //UPGRADE_ISSUE: Method 'java.lang.ref.Reference.get' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangrefReference_3"'
                Cursor cur = (Cursor) ref_Renamed.get_Renamed();
                if(cur == null) {
                    // some other unrelated cursor object has been
                    // garbage-collected; let's take the opportunity to
                    // clean up the cursors list anyway..
                    //UPGRADE_ISSUE: Method 'java.util.Iterator.remove' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javautilIteratorremove_3"'
                    it.remove();
                }
                else if(cur == cursor) {
                    //UPGRADE_ISSUE: Method 'java.lang.ref.Reference.clear' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangrefReference_3"'
                    ref_Renamed.clear();
                    //UPGRADE_ISSUE: Method 'java.util.Iterator.remove' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javautilIteratorremove_3"'
                    it.remove();
                    break;
                }
            }
        }
        
        //-----------------------------------------------------------------------
        /// <summary> Informs all of my registered cursors that the specified
        /// element was changed.
        ///
        /// </summary>
        /// <param name="node"> the node that was changed
        /// </param>
        protected internal virtual void  broadcastNodeChanged(Node node) {
            System.Collections.IEnumerator it = cursors.GetEnumerator();
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilIteratorhasNext_3"'
            while(it.MoveNext()) {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilIteratornext_3"'
                System.WeakReference ref_Renamed = (System.WeakReference) it.Current;
                //UPGRADE_ISSUE: Method 'java.lang.ref.Reference.get' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangrefReference_3"'
                Cursor cursor = (Cursor) ref_Renamed.get_Renamed();
                if(cursor == null) {
                    //UPGRADE_ISSUE: Method 'java.util.Iterator.remove' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javautilIteratorremove_3"'
                    it.remove(); // clean up list
                }
                else {
                    cursor.nodeChanged(node);
                }
            }
        }
        
        /// <summary> Informs all of my registered cursors that the specified
        /// element was just removed from my list.
        ///
        /// </summary>
        /// <param name="node"> the node that was changed
        /// </param>
        protected internal virtual void  broadcastNodeRemoved(Node node) {
            System.Collections.IEnumerator it = cursors.GetEnumerator();
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilIteratorhasNext_3"'
            while(it.MoveNext()) {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilIteratornext_3"'
                System.WeakReference ref_Renamed = (System.WeakReference) it.Current;
                //UPGRADE_ISSUE: Method 'java.lang.ref.Reference.get' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangrefReference_3"'
                Cursor cursor = (Cursor) ref_Renamed.get_Renamed();
                if(cursor == null) {
                    //UPGRADE_ISSUE: Method 'java.util.Iterator.remove' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javautilIteratorremove_3"'
                    it.remove(); // clean up list
                }
                else {
                    cursor.nodeRemoved(node);
                }
            }
        }
        
        /// <summary> Informs all of my registered cursors that the specified
        /// element was just added to my list.
        ///
        /// </summary>
        /// <param name="node"> the node that was changed
        /// </param>
        protected internal virtual void  broadcastNodeInserted(Node node) {
            System.Collections.IEnumerator it = cursors.GetEnumerator();
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilIteratorhasNext_3"'
            while(it.MoveNext()) {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javautilIteratornext_3"'
                System.WeakReference ref_Renamed = (System.WeakReference) it.Current;
                //UPGRADE_ISSUE: Method 'java.lang.ref.Reference.get' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangrefReference_3"'
                Cursor cursor = (Cursor) ref_Renamed.get_Renamed();
                if(cursor == null) {
                    //UPGRADE_ISSUE: Method 'java.util.Iterator.remove' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javautilIteratorremove_3"'
                    it.remove(); // clean up list
                }
                else {
                    cursor.nodeInserted(node);
                }
            }
        }
        
        //-----------------------------------------------------------------------
        /// <summary> Serializes the data held in this object to the stream specified.</summary>
        //UPGRADE_TODO: Class 'java.io.ObjectOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaioObjectOutputStream_3"'
        //UPGRADE_TODO: Method 'writeObject' was converted to 'GetObjectData' and its parameters were changed. The code must be reviewed in order to assure that no calls to non-member methods of the converted parameters are made. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1192_3"'
        public virtual void  GetObjectData(System.Runtime.Serialization.SerializationInfo out_Renamed, System.Runtime.Serialization.StreamingContext context) {
            SupportClass.DefaultWriteObject(out_Renamed, context, this);
            //UPGRADE_ISSUE: Serialization code was not used in the conversion of the method. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1193_3"'
            doWriteObject(out_Renamed);
        }
        
        /// <summary> Deserializes the data held in this object to the stream specified.</summary>
        //UPGRADE_TODO: Class 'java.io.ObjectInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaioObjectInputStream_3"'
        //UPGRADE_TODO: Method 'readObject' was converted to 'CursorableLinkedList' and its parameters were changed. The code must be reviewed in order to assure that no calls to non-member methods of the converted parameters are made. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1192_3"'
        protected CursorableLinkedList(System.Runtime.Serialization.SerializationInfo in_Renamed, System.Runtime.Serialization.StreamingContext context) {
            SupportClass.DefaultReadObject(in_Renamed, context, this);
            //UPGRADE_ISSUE: Serialization code was not used in the conversion of the method. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1193_3"'
            doReadObject(in_Renamed);
        }
        
        //-----------------------------------------------------------------------
        /// <summary> An extended <code>ListIterator</code> that allows concurrent changes to
        /// the underlying list.
        /// </summary>
        public class Cursor:AbstractLinkedList.LinkedListIterator {
            /// <summary>Is the cursor valid (not closed) </summary>
            internal bool valid = true;
            /// <summary>Is the next index valid </summary>
            internal bool nextIndexValid = true;
            
            /// <summary> Constructs a new cursor.
            ///
            /// </summary>
            /// <param name="index"> the index to start from
            /// </param>
            protected internal Cursor(CursorableLinkedList parent, int index):base(parent, index) {
                valid = true;
            }
            
            /// <summary> Adds an object to the list.
            /// The object added here will be the new 'previous' in the iterator.
            ///
            /// </summary>
            /// <param name="obj"> the object to add
            /// </param>
            public override void  add(System.Object obj) {
                base.add(obj);
                // add on iterator does not return the added element
                next_Renamed_Field = next_Renamed_Field.next;
            }
            
            /// <summary> Gets the index of the next element to be returned.
            ///
            /// </summary>
            /// <returns> the next index
            /// </returns>
            public override int nextIndex() {
                if(nextIndexValid == false) {
                    if(next_Renamed_Field == parent.header) {
                        nextIndex_Renamed_Field = parent.Count;
                    }
                    else {
                        int pos = 0;
                        Node temp = parent.header.next;
                        while(temp != next_Renamed_Field) {
                            pos++;
                            temp = temp.next;
                        }
                        nextIndex_Renamed_Field = pos;
                    }
                    nextIndexValid = true;
                }
                return nextIndex_Renamed_Field;
            }
            
            /// <summary> Handle event from the list when a node has changed.
            ///
            /// </summary>
            /// <param name="node"> the node that changed
            /// </param>
            protected internal virtual void  nodeChanged(Node node) {
                // do nothing
            }
            
            /// <summary> Handle event from the list when a node has been removed.
            ///
            /// </summary>
            /// <param name="node"> the node that was removed
            /// </param>
            protected internal virtual void  nodeRemoved(Node node) {
                if(node == next_Renamed_Field) {
                    next_Renamed_Field = node.next;
                }
                else if(node == current) {
                    current = null;
                    nextIndex_Renamed_Field--;
                }
                else {
                    nextIndexValid = false;
                }
            }
            
            /// <summary> Handle event from the list when a node has been added.
            ///
            /// </summary>
            /// <param name="node"> the node that was added
            /// </param>
            protected internal virtual void  nodeInserted(Node node) {
                if(node.previous == current) {
                    next_Renamed_Field = node;
                }
                else if(next_Renamed_Field.previous == node) {
                    next_Renamed_Field = node;
                }
                else {
                    nextIndexValid = false;
                }
            }
            
            /// <summary> Override superclass modCount check, and replace it with our valid flag.</summary>
            protected internal override void  checkModCount() {
                if(!valid) {
                    throw new System.Exception("Cursor closed");
                }
            }
            
            /// <summary> Mark this cursor as no longer being needed. Any resources
            /// associated with this cursor are immediately released.
            /// In previous versions of this class, it was mandatory to close
            /// all cursor objects to avoid memory leaks. It is <i>no longer</i>
            /// necessary to call this close method; an instance of this class
            /// can now be treated exactly like a normal iterator.
            /// </summary>
            public virtual void  close() {
                if(valid) {
                    ((CursorableLinkedList) parent).unregisterCursor(this);
                    valid = false;
                }
            }
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1232_3"'
        override public void  RemoveAt(System.Int32 index) {
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1232_3"'
        override public void  Remove(System.Object value) {
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1232_3"'
        override public void  Insert(System.Int32 index, System.Object value) {
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1232_3"'
        override public System.Int32 IndexOf(System.Object value) {
            return 0;
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1232_3"'
        override public void  Clear() {
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1232_3"'
        override public System.Boolean Contains(System.Object value) {
            return false;
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1232_3"'
        override public System.Int32 Add(System.Object value) {
            return 0;
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1232_3"'
        override public void  CopyTo(System.Array array, System.Int32 index) {
        }
        //UPGRADE_TODO: The following property was automatically generated and it must be implemented in order to preserve the class logic. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1232_3"'
        override public System.Object this[System.Int32 index] {
            get {
                return null;
            }
            
            set {
            }
            
        }
        //UPGRADE_TODO: The following property was automatically generated and it must be implemented in order to preserve the class logic. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1232_3"'
        override public System.Boolean IsReadOnly {
            get {
                return false;
            }
            
        }
        //UPGRADE_TODO: The following property was automatically generated and it must be implemented in order to preserve the class logic. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1232_3"'
        override public System.Boolean IsFixedSize {
            get {
                return false;
            }
            
        }
        //UPGRADE_TODO: The following property was automatically generated and it must be implemented in order to preserve the class logic. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1232_3"'
        override public System.Int32 Count {
            get {
                return 0;
            }
            
        }
        //UPGRADE_TODO: The following property was automatically generated and it must be implemented in order to preserve the class logic. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1232_3"'
        override public System.Object SyncRoot {
            get {
                return null;
            }
            
        }
        //UPGRADE_TODO: The following property was automatically generated and it must be implemented in order to preserve the class logic. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1232_3"'
        override public System.Boolean IsSynchronized {
            get {
                return false;
            }
            
        }
    }
}
