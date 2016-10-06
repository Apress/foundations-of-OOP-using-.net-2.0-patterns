using System;
using System.Collections.Generic;

namespace Library {
    class Book {
        Shelf _shelf;
        public void AssignShelf( Shelf shelf) {
            _shelf = shelf;
            _shelf.AssignBook( this);
        }
    }
    class Shelf {
        List< Book> _books;
        public void AssignBook( Book book) {
            _books.Add( book);
            book.AssignShelf( this);
        }
    }
}

class Book {
    Shelf _shelf;
    public void AssignShelf( Shelf shelf) {
        _shelf = shelf;
        _shelf.AssignBook( this);
    }
}
class Shelf {
    List< Book> _books;
    public void AssignBook( Book book) {
        _books.Add( book);
        book.AssignShelf( this);
    }
}
