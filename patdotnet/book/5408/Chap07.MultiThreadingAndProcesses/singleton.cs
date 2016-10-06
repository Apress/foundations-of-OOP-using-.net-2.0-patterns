using System;
using System.Collections;
using NUnit.Framework;
using System.Threading;
using Devspace.Commons.State;
using Devspace.Commons.Functors;
using System.IO;


class Singleton {
    public static Singleton Instance() {
        if(_instance == null) {
            lock(typeof(Singleton)) {
                if(_instance == null) {
                    _instance = new Singleton();
                }
            }
        }
        return _instance;
    }
    
    protected Singleton() { }
    
    private static volatile Singleton _instance = null;
}

sealed class SimpleSingleton {
 
    private SimpleSingleton() { }
 
    public static readonly SimpleSingleton Instance = new SimpleSingleton();
 
}

// *****************************************************************

[Serializable]
class SingletonData {
    public readonly string FileData;
    
    public SingletonData( string filedata) {
        FileData = filedata;
    }
}

class SingletonBuilder : BaseSingletonDelegation< SingletonData> {
    private string _path;
    public SingletonBuilder() {
        //_path = some configuration file entry
    }
    protected override bool IsObjectValid( SingletonData obj) {
        FileInfo file = new FileInfo( _path);
        if( file.Exists) {
            return false;
        }
        else {
            return true;
        }
    }
    protected override SingletonData InstantiateNewObject( object descriptor) {
        FileStream filestream = new FileStream( _path, FileMode.Open, FileAccess.Read, FileShare.None);
        StreamReader stream = new StreamReader( filestream);
        SingletonData retval = new SingletonData( stream.ReadLine());
        stream.Close();
        FileInfo info = new FileInfo( _path);
        info.Delete();
        return retval;
    }
}

class SingletonMultiAppDomainBuilder : BaseSingletonDelegation< SingletonData> {
    private string _path;
    private static SingletonData _data;
    private static bool _needUpdate;
    private static ReaderWriterLock _lock = new ReaderWriterLock();
    
    public SingletonMultiAppDomainBuilder( string path) {
        _path = path;
    }
    protected override bool IsObjectValid( SingletonData obj) {
        try {
            _lock.AcquireReaderLock( -1);
            FileInfo file = new FileInfo( _path);
            if( file.Exists) {
                _lock.UpgradeToWriterLock( -1);
                _needUpdate = true;
                return false;
            }
            else {
                return true;
            }
        }
        finally {
            _lock.ReleaseLock();
        }
    }
    protected override SingletonData InstantiateNewObject( object descriptor) {
        try {
            _lock.AcquireWriterLock( -1);
            if( _needUpdate) {
                FileStream filestream = new FileStream( _path, FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader stream = new StreamReader( filestream);
                _data = new SingletonData( stream.ReadLine());
                stream.Close();
            }
            return _data;
        }
        finally {
            _lock.ReleaseLock();
        }
    }
}

[TestFixture]
public class TestSingleton {
    [Test]
    public void TestSimpleSingleton() {
        NUnit.Framework.Assert.AreEqual( "hello", Singleton< SingletonBuilder, SingletonData>.Instance( "").FileData);
        Thread.Sleep( 1000);
    }
}



