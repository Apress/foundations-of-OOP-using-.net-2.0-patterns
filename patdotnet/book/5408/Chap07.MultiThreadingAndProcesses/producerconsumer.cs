using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

interface IProducerConsumer {
    void Invoke( Delegate @delegate);
    void Invoke( Delegate @delegate, Object[] arguments);
}

class ThreadPoolProducerConsumer : IProducerConsumer {
    class Executor {
        public readonly Delegate _delegate;
        public readonly Object[] _arguments;
        
        public Executor( Delegate @delegate, Object[] arguments) {
            _delegate = @delegate;
            _arguments = arguments;
        }
    }
    
    private Queue< Executor> _queue = new Queue<Executor>();
    
    private void QueueProcessor( Object obj) {
        Monitor.Enter( _queue);
        while( _queue.Count == 0) {
            Monitor.Wait( _queue, -1);
        }
        Executor exec = _queue.Dequeue();
        Monitor.Exit( _queue);
        exec._delegate.DynamicInvoke( exec._arguments);
        ThreadPool.QueueUserWorkItem( new WaitCallback( QueueProcessor));
    }
    
    public ThreadPoolProducerConsumer() {
        ThreadPool.QueueUserWorkItem( new WaitCallback( QueueProcessor));
    }
    public void Invoke( Delegate @delegate, Object[] arguments) {
        Monitor.Enter( _queue);
        _queue.Enqueue( new Executor( @delegate, arguments));
        Monitor.Pulse( _queue);
        Monitor.Exit( _queue);
    }
    public void Invoke( Delegate @delegate) {
        Monitor.Enter( _queue);
        _queue.Enqueue( new Executor( @delegate, null));
        Monitor.Pulse( _queue);
        Monitor.Exit( _queue);
    }
}

[TestFixture]
public class TestProducerConsumer {
    delegate void TestMethod();
    
    void Method() {
        Console.WriteLine( "Processed in thread id (" + Thread.CurrentThread.ManagedThreadId + ")");
    }
    [Test]
    public void TestSimple() {
        IProducerConsumer producer = new ThreadPoolProducerConsumer();
        Console.WriteLine( "Sent in thread id (" + Thread.CurrentThread.ManagedThreadId + ")");
        producer.Invoke( new TestMethod( Method));
    }
}
