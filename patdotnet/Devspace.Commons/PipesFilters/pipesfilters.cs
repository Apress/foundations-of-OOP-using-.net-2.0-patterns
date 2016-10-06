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
using System.Text;
using System.IO;
using Devspace.Commons.TDD;
using System.Collections;

namespace Devspace.Commons.PipesFilters {

#region IComponent definition
    public interface IComponent<ControlData> {
        void Process( ControlData controldata );
    }
#endregion

#region IComponent Streaming Interfaces
    public interface InputStream< type> : IEnumerable {
        bool Available();
        type Read();
        ulong Read( out type[] data );
        ulong Read( out type[] data, ulong offset, ulong length );
        void Reset();
        void Skip( ulong offset );
    }

    public interface OutputStream<type> {
        void Flush();
        void Write( type data );
        void Write( type[] data );
        void Write( type[] data, ulong offset, ulong length );
    }

    public interface IStreamingControl< type> {
        InputStream< type> Input {
            get; set;
        }
        OutputStream< type> Output {
            get; set;
        }
        InputStream< type> FactoryInputStream();
        OutputStream< type> FactoryOutputStream();
    }

    public interface IComponentStreaming< type> : IComponent< IStreamingControl< type> > {
        void Process( InputStream<type> input, OutputStream<type> output );
    }

#endregion

    #region StreamingControlImpl
    public class StreamingControlImpl<type> : MarshalByRefObject, IStreamingControl< type> where type : new() {
        #region BridgedStreams implementation of InputStream, and OutputStream
        [Serializable]
        public class BridgedStreams : InputStream<type>, OutputStream<type> {
            type[] _buffer = new type[ 0 ];
            ulong _currIndex;
            ulong _afterLastWrittenIndex;

            public BridgedStreams() {
                _currIndex = 0;
                _afterLastWrittenIndex = 0;
            }

            public bool Available() {
                if( _currIndex < _afterLastWrittenIndex ) {
                    return true;
                }
                else {
                    return false;
                }
            }
            public type Read() {
                if( _currIndex < _afterLastWrittenIndex ) {
                    _currIndex++;
                    return _buffer[ _currIndex - 1 ];
                }
                else {
                    throw new System.IO.EndOfStreamException();
                }
            }
            public ulong Read( out type[] data ) {
                ulong size = _afterLastWrittenIndex - _currIndex;
                data = new type[ size ];
                for( ulong c1 = _currIndex; c1 < _afterLastWrittenIndex; c1++ ) {
                    data[ c1 - _currIndex ] = _buffer[ c1 ];
                }
                return size;
            }
            public ulong Read( out type[] data, ulong offset, ulong length ) {
                if( _afterLastWrittenIndex - offset > length ) {
                    length = _afterLastWrittenIndex - offset;
                }
                data = new type[ length ];
                for( ulong c1 = offset; c1 < offset + length; c1++ ) {
                    data[ c1 - offset ] = _buffer[ c1 ];
                }
                return length;
            }
            public void Reset() {
                _currIndex = 0;
            }
            public void Skip( ulong offset ) {
                if( offset > _afterLastWrittenIndex ) {
                    _currIndex = _afterLastWrittenIndex;
                }
                else {
                    _currIndex = offset;
                }
            }

            public void Flush() {
                _currIndex = 0;
                _afterLastWrittenIndex = 0;
                return;
            }
            private void Resize( ulong length ) {
                if( length >= (ulong)_buffer.Length ) {
                    type[] temp;

                    if( _buffer.Length == 0 ) {
                        temp = new type[ length * 2 ];
                    }
                    else {
                        temp = new type[ _buffer.Length * 2 ];
                    }
                    for( int c1 = 0; c1 < _buffer.Length; c1++ ) {
                        temp[ c1 ] = _buffer[ c1 ];
                    }
                    _buffer = temp;
                }
            }
            public void Write( type data ) {
                Resize( _currIndex + 1 );
                _buffer[ _currIndex ] = data;
                _currIndex++;
                if( _currIndex > _afterLastWrittenIndex ) {
                    _afterLastWrittenIndex = _currIndex;
                }
                return;
            }
            public void Write( type[] data ) {
                foreach( type item in data ) {
                    Write( item );
                }
                return;
            }
            public void Write( type[] data, ulong offset, ulong length ) {
                Skip( offset );
                for( ulong c1 = 0; c1 < length; c1++ ) {
                    Write( data[ c1 ] );
                }
                return;
            }
            public System.Collections.IEnumerator GetEnumerator() {
                for( ulong c1 = 0; c1 < _afterLastWrittenIndex; c1++ ) {
                    yield return _buffer[ c1 ];
                }
            }
            public override string ToString() {
                MemoryTracer.Start( this );
                MemoryTracer.Variable( "CurrIndex", _currIndex );
                MemoryTracer.Variable( "LastWrittenIndex", _afterLastWrittenIndex );
                MemoryTracer.Variable( "Buffer size", _buffer.Length );
                MemoryTracer.StartArray( "Buffer content" );
                for( ulong c1 = 0; c1 < _afterLastWrittenIndex; c1++ ) {
                    MemoryTracer.Variable( "item", _buffer[ c1 ].ToString() );
                }
                MemoryTracer.EndArray();
                return MemoryTracer.End();
            }

        }
        #endregion

        private InputStream< type> _inputStream;
        private OutputStream< type> _outputStream;
        public virtual InputStream< type> Input {
            get {
                return _inputStream;
            }
            set {
                _inputStream = value;
            }
        }
        public virtual OutputStream< type> Output {
            get {
                return _outputStream;
            }
            set {
                _outputStream = value;
            }
        }
        public virtual InputStream< type> FactoryInputStream() {
            return new BridgedStreams();
        }
        public virtual OutputStream< type> FactoryOutputStream() {
            return new BridgedStreams();
        }
    }
    
    public abstract class StreamingComponentBase< type> : MarshalByRefObject, IComponentStreaming< type> where type : new() {
        public virtual void Process( InputStream< type> input, OutputStream< type> output) {
            foreach( type element in input) {
                output.Write( element);
            }
        }
        public void Process( IStreamingControl< type> controlData) {
            if( controlData.Input == null) {
                controlData.Input = controlData.FactoryInputStream();
            }
            if( controlData.Output == null) {
                controlData.Output = controlData.FactoryOutputStream();
            }
            Process( controlData.Input, controlData.Output);
            if( controlData.Output is StreamingControlImpl<type>.BridgedStreams ) {
                controlData.Input = (InputStream< type>)controlData.Output;
                controlData.Input.Reset();
                controlData.Output = null;
            }
            else {
                controlData.Input = null;
                controlData.Output = null;
            }
        }
    }
    public abstract class InputSink<type> : StreamingComponentBase<type> where type : new() {
        public abstract void Process( OutputStream<type> output );

        public override void Process( InputStream<type> input, OutputStream<type> output ) {
            foreach( type item in input ) {
                output.Write( item );
            }
            Process( output );
        }
    }

    public abstract class OutputSink<type> : StreamingComponentBase<type> where type : new() {
        public abstract void Process( InputStream<type> input );

        public override void Process( InputStream<type> input, OutputStream<type> output ) {
            Process( input );
        }
    }

    public abstract class ComponentInputIterator<type> : StreamingComponentBase<type> where type : new() {
        public abstract void Process( type item, OutputStream<type> output );
        public override void Process( InputStream<type> input, OutputStream<type> output ) {
            foreach( type item in input ) {
                Process( item, output );
            }
        }
    }
    #endregion

    public class Chain<ControlData> where ControlData : new() {
        IList<IComponent<ControlData>> _links = new List<IComponent<ControlData>>();

        public void AddLink( IComponent<ControlData> link ) {
            _links.Add( link );
        }
        
        public void Process( ControlData controldata) {
            foreach( IComponent< ControlData> element in _links) {
                element.Process( controldata);
            }
        }
        public override string ToString() {
            throw new Exception( "The method or operation is not implemented." );
        }
    }

}


/*
 *         private void Transfer( InputStream<type> inputStream, OutputStream<type> outputStream ) {
        type[] buffer;

        inputStream.Reset();
        inputStream.Read( out buffer );
        outputStream.Write( buffer );
    }
    public void Process( InputStream< type> input, OutputStream<type> output ) {
        InputStream<type> tempInput = input;
        OutputStream<type> tempOutput = new BufferedStreams();
        foreach( IComponent< type> component in _links ) {
            component.Process( tempInput, tempOutput );
            tempInput = (InputStream<type>)tempOutput;
            tempInput.Reset();
            tempOutput = new BufferedStreams();
        }
        Transfer( tempInput, output );
    }
*/


