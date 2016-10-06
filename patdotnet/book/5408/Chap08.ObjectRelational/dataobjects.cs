using System;
using System.Collections;

namespace Chap08.ObjectRelational
{
    public class BookUpdater {
        public static void UpdateAuthor( Book book, string author) {
            book.Author = author;
        }
    }
    public class Book {
        private string _isbn;
        private string _title;
        private string _author;
        private IList _comments;

        public Book() { }
        public Book( string isbn, string title, string author) {
            _isbn = isbn;
            _title = title;
            _author = author;
        }
        public string ISBN {
            get {
                return _isbn;
            }
            internal set {
                _isbn = value;
            }
        }
        public string Title {
            get {
                return _title;
            }
            internal set {
                _title = value;
            }
        }
        public string Author {
            get {
                return _author;
            }
            internal set {
                _author = value;
            }
        }
        public IList Comments {
            get {
                return _comments;
            }
            internal set {
                _comments = value;
            }
        }
        public void AddComment( Comment comment) {
            _comments.Add( comment);
            comment.Parent = this;
        }
    }
     
    public class Comment {
        private string _comment;
        private string _whoMadeComment;
        private Book _parent;
        private string _ID;
        
        public Comment() { }
        public Comment( string comment, string whoMadeComment) {
            _comment = comment;
            _whoMadeComment = whoMadeComment;
        }
        public Comment( string comment, string whoMadeComment, Book parent) {
            _comment = comment;
            _whoMadeComment = whoMadeComment;
            _parent = parent;
        }
        public string ID {
            get {
                return _ID;
            }
            set {
                _ID = value;
            }
        }
        public string Text {
            get {
                return _comment;
            }
            internal set {
                _comment = value;
            }
        }
        public string WhoMadeComment {
            get {
                return _whoMadeComment;
            }
            internal set {
                _whoMadeComment = value;
            }
        }
        public Book Parent {
            get {
                return _parent;
            }
            internal set {
                _parent = value;
            }
        }
    }
}



