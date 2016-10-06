
using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections;

namespace Chap09.Refactorings {
    public abstract class LocalStream /*: System.MarshalByRefObject, System.IDisposable */{
    /*    public static readonly System.IO.Stream Null;
        
        protected LocalStream() {}
        
        void System.IDisposable.Dispose() {}
        
        public virtual void Close() {}
        
        protected virtual System.Threading.WaitHandle CreateWaitHandle() {}
        
        public abstract override void Flush();
        
        public abstract override int Read(byte [] buffer, int offset, int count);
        
        public virtual int ReadByte() {}
        
        public abstract override long Seek(long offset, System.IO.SeekOrigin origin);
        
        public abstract override void SetLength(long value);
        
        public abstract override void Write(byte [] buffer, int offset, int count);
        
        public virtual void WriteByte(byte value) {}
        
        public virtual System.IAsyncResult BeginRead(byte [] buffer, int offset, int count, System.AsyncCallback cback, object state) {
        }
        
        public virtual System.IAsyncResult BeginWrite(byte [] buffer, int offset, int count, System.AsyncCallback cback, object state) {
        }
        
        public virtual int EndRead(System.IAsyncResult async_result) {}
        
        public virtual void EndWrite(System.IAsyncResult async_result) {}
        
        
        public abstract bool CanSeek {
            get {}
        }
        
        public abstract long Position {
            get {}
            set {}
        }
        
        public abstract bool CanWrite {
            get {}
        }
        
        public abstract long Length {
            get {}
        }
        
        public abstract bool CanRead {
            get {}
        }
        */
    }
    
    public abstract class ReadStream {
        #region ReadStream members
        public static readonly System.IO.Stream Null;
        
        protected ReadStream() {}
        //void System.IDisposable.Dispose() {}
        public virtual void Close() {}
        public abstract int Read(byte [] buffer, int offset, int count);
        public abstract int ReadByte();
        public abstract long Position {
            get;
            set;
        }
        public abstract long Seek(long offset, System.IO.SeekOrigin origin);
        public abstract long Length {
            get;
        }
        #endregion
    }
    
    public abstract class WriteStream {
        /*public static readonly System.IO.Stream Null;
        
        public WriteStream() { }
        void System.IDisposable.Dispose() {}
        public virtual void Close() {}
        public abstract override void SetLength(long value);
        public abstract override void Write(byte [] buffer, int offset, int count);
        public virtual void WriteByte(byte value) {}
        public abstract override void Flush();
         */
    }
    
    public abstract class ASynchronousReadStream : ReadStream {
/*        protected virtual System.Threading.WaitHandle CreateWaitHandle() {}
        public virtual System.IAsyncResult BeginRead(byte [] buffer, int offset, int count, System.AsyncCallback cback, object state) {
        }
        public virtual int EndRead(System.IAsyncResult async_result) {}*/
    }
    
    public abstract class ASynchronousWriteStream : WriteStream {
        /*protected virtual System.Threading.WaitHandle CreateWaitHandle() {}
        public virtual System.IAsyncResult BeginWrite(byte [] buffer, int offset, int count, System.AsyncCallback cback, object state) {
        }
        public virtual void EndWrite(System.IAsyncResult async_result) {}*/
    }
    
    public class FileImp /*: ReadStream, WriteStream*/ {
        
    }
    
    public class InMemoryImp /*: WriteStream, IOriginator*/ {
        private Byte[] _data;
        
        public void GetState<type>(type data) { }
        public void SetState<type>(type data) { }
    }

    /*
     interface ISerialization {
        Channel ReadChannel();
        void WriteChannel( Channel channel);
    }
    
    class RssSerialization : ISerialization { }
    class AtomSerialization : ISerialization { }
    
    
    public class Item {
        private Item _next = null;
        
        public Item() { }
        public Item( Item next) {
            _next = next;
        }
        public bool Unread = true;
        public void MarkAsRead() {
            if( _next != null) {
                _next.MarkAsRead();
            }
            Unread = false;
        }
    }
    public class Channel {
        private Item _items;
    }
    
    public class Item {
        [XmlAttribute] public string   Id = "";
        [XmlAttribute] public bool     Unread = true;
        
        [XmlAttribute] public string   Title = "";
        [XmlAttribute] public string   Text = "";
        [XmlAttribute] public string   Link = "";
        [XmlAttribute] public DateTime PubDate;
        [XmlAttribute] public string   Author = "";
        
        [XmlIgnore]    public Channel  Channel;
        private  Item _next;
        
        public Item( Item next) {
            _next = next;
        }
        
        public Item(string id, RssItem rssItem) {
            this.Id = id;
            this.Title = HtmlUtils.StripHtml(rssItem.Title.Trim());
            this.Text = rssItem.Description.Trim();
            if(rssItem.Link != null) {
                this.Link = rssItem.Link.ToString().Trim();
            }
            else {
                this.Link = "";
            }
            this.Author = rssItem.Author.Trim();
            
            this.PubDate = rssItem.PubDate;
        }
        
        public Item(string id, AtomEntry entry) {
            this.Id = id;
            this.Title = HtmlUtils.StripHtml(entry.Title.Content.Trim());
            
            if(entry.Author != null && entry.Author.Name != "") {
                this.Author = entry.Author.Name.Trim();
            }
            */
            /* Content is optional in Atom feeds, or there may be multiple
             * contents of different media types.  Use the summary if there
             * is no content, otherwise use HTML if it's available.
             */
            /*if(entry.Contents == null || entry.Contents.Count == 0) {
                this.Text = entry.Summary.Content.Trim();
            }
            else {
                foreach(AtomContent content in entry.Contents) {
                    if(content.Type == MediaType.TextHtml ||
                       content.Type == MediaType.ApplicationXhtmlXml) {
                        this.Text = content.Content.Trim();
                        break;
                    }
                }
            }
            */
            /* Atom entries must have at least one link with a relationship
             * type of "alternate," but may have more.  Again, prefer HTML if
             * more than one is available.
             */
            /*if(entry.Links.Count == 1) {
                this.Link = entry.Links[0].HRef.ToString().Trim();
            }
            else {
                foreach(AtomLink link in entry.Links) {
                    if(link.Rel == Relationship.Alternate) {
                        if(link.Type == MediaType.TextHtml ||
                           link.Type == MediaType.ApplicationXhtmlXml) {
                            this.Link = link.HRef.ToString().Trim();
                            break;
                        }
                    }
                }
            }
            
            this.PubDate = entry.Modified.DateTime;
        }
        */
        /* This is called in the middle of an refresh so the channel will be
         * updated when refresh is done
         */
        /*public bool Update(RssItem rssItem) {
            if(this.Title != HtmlUtils.StripHtml(rssItem.Title.Trim())) {
                this.Title = HtmlUtils.StripHtml(rssItem.Title.Trim());
                return true;
            }
            
            if(this.Text.Equals(rssItem.Description.Trim()) == false) {
                this.Text = rssItem.Description.Trim();
                return true;
            }
            
            return false;
        }
        
        public bool Update(AtomEntry entry) {
            if(this.Title != HtmlUtils.StripHtml(entry.Title.Content.Trim())) {
                this.Title = HtmlUtils.StripHtml(entry.Title.Content.Trim());
                return true;
            }
            
            string entryText = "";
            if(entry.Contents == null || entry.Contents.Count == 0) {
                entryText = entry.Summary.Content.Trim();
            }
            else {
                foreach(AtomContent content in entry.Contents) {
                    if(content.Type == MediaType.TextHtml ||
                       content.Type == MediaType.ApplicationXhtmlXml) {
                        entryText = content.Content.Trim();
                        break;
                    }
                }
            }
            
            if(this.Text.Equals(entryText) == false) {
                this.Text = entryText;
                return true;
            }
            
            return false;
        }
        
        public void SetUnread(bool unread, bool inAllChannels) {
            if(Unread != unread) {
                Unread = unread;
                Application.TheApp.CCollection.Update(this.Channel);
                Application.TheApp.ItemList.Update(this);
                
                if(inAllChannels) {
                    Application.TheApp.CCollection.MarkItemIdAsReadInAllChannels(this.Channel,
                                                                                 this.Id);
                }
            }
        }
    }
    
    public class Channel {
        [XmlAttribute] public string Name = "";
        [XmlAttribute] public string Url = "";
        
        // Used when updating the feed
        [XmlAttribute] public string LastModified = "";
        [XmlAttribute] public string ETag = "";
        [XmlAttribute] public string Type = "";
        
        public int NrOfItems {
            get {
                return mItems.Count;
            }
        }
        
        public int NrOfUnreadItems {
            get {
                int unread = 0;
                
                foreach(Item item in mItems) {
                    if(item.Unread == true) {
                        unread++;
                    }
                }
                
                return unread;
            }
        }
        
        ArrayList mItems;
        [XmlElement ("Item", typeof (Item))]
        public ArrayList Items {
            get {
                return mItems;
            }
            set {
                mItems = value;
            }
        }
        
        public Channel() {
            mItems = new ArrayList();
        }
        
        public Channel(string name, string url) {
            mItems = new ArrayList();
            Name = name;
            Url = url;
        }
        
        public void Setup() {
            foreach(Item item in mItems) {
                item.Channel = this;
            }
        }
        
        public Item GetItem(string id) {
            foreach(Item item in mItems) {
                if(item.Id == id) {
                    return item;
                }
            }
            
            return null;
        }
        
        public bool MarkAsRead() {
            bool updated = false;
            
            foreach(Item item in mItems) {
                if(item.Unread) {
                    item.SetUnread(false, false);
                    updated = true;
                }
            }
            
            return updated;
        }
        
        private ArrayList mUnupdatedItems;
        
        // Sets the channel in update mode.
        public void StartRefresh() {
            mUnupdatedItems = (ArrayList) mItems.Clone();
        }
        
        // Removes any items not being part of the RSS feed any more
        public void FinishRefresh() {
            // Remove old items
            foreach(Item item in mUnupdatedItems) {
                mItems.Remove(item);
            }
        }
        
        public bool UpdateItem(string id, RssItem rssItem) {
            Item item = GetItem(id);
            
            if(item == null) {
                item = new Item(id, rssItem);
                item.Channel = this;
                
                mItems.Add(item);
                return true;
            }
            else {
                bool updated = item.Update(rssItem);
                mUnupdatedItems.Remove(item);
                return updated;
            }
        }
        
        public bool UpdateItem(string id, AtomEntry entry) {
            Item item = GetItem(id);
            
            if(item == null) {
                item = new Item(id, entry);
                item.Channel = this;
                
                mItems.Add(item);
                return true;
            }
            else {
                bool updated = item.Update(entry);
                mUnupdatedItems.Remove(item);
                return updated;
            }
        }
        */
        /* Used to cross-mark as read */
        /*public void MarkItemIdAsRead(string id) {
            foreach(Item item in mItems) {
                if(item.Id.Equals(id)) {
                    if(item.Unread) {
                        item.Unread = false;
                        Application.TheApp.CCollection.Update(this);
                    }
                    break;
                }
            }
        }
    }*/
    
    
    
}


