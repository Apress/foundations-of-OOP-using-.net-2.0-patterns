using System;
using System.Collections.Generic;
using System.Text;

class SynchronizedList<type>: ICollection<type> {
    private ICollection<type> _parent;
    
    public SynchronizedList(ICollection<type> parent) {
        _parent = parent;
    }
    
    #region ICollection<type> Members
    
    public void Add(type item) {
        lock(this) {
            _parent.Add(item);
        }
    }
    
    public void Clear() {
        throw new System.Exception("The method or operation is not implemented.");
    }
    
    public bool Contains(type item) {
        throw new System.Exception("The method or operation is not implemented.");
    }
    
    public void CopyTo(type[] array, int arrayIndex) {
        throw new System.Exception("The method or operation is not implemented.");
    }
    
    public int Count {
        get { throw new System.Exception("The method or operation is not implemented."); }
    }
    
    public bool IsReadOnly {
        get { throw new System.Exception("The method or operation is not implemented."); }
    }
    
    public bool Remove(type item) {
        throw new System.Exception("The method or operation is not implemented.");
    }
    
    #endregion
    
    #region IEnumerable<type> Members
    
    public IEnumerator<type> GetEnumerator() {
        throw new System.Exception("The method or operation is not implemented.");
    }
    
    #endregion
    
    #region IEnumerable Members
    
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
        throw new System.Exception("The method or operation is not implemented.");
    }
    
    #endregion
}

class Proxy {
    public static void Method() {
        IList<string> list = new System.Collections.Generic.List<string>() as IList<string>;
        
        List<string> list2 = new List<string>();
        
        //List<string> synchronized = List<string>.Synchronize( new List());
    }
    public static void OtherExample() {
        ICollection<string> collection = new SynchronizedList<string>(new List<string>());
    }
}

