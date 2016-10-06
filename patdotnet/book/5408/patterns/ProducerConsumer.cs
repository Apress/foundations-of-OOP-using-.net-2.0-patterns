using System;
using System.Collections;
using System.Threading;

public class Data
{
	int _value;
}

public class SyncPoint
{
	Queue _queue = new Queue();

	public Data Buffer
	{
		get
		{
			Data returnBuffer;

			Monitor.Enter( this);
			if( _queue.Count == 0)
			{
				Monitor.Wait( this);
			}
			returnBuffer = (Data)_queue.Dequeue();
			//Monitor.Pulse( this);  NOT NECESSARY.....  Queue can more than the max, but you can add some max amount
			Monitor.Exit( this);
			return returnBuffer;
		}
		set
		{
			Monitor.Enter(this);
			_queue.Enqueue( value);
			Monitor.Pulse( this);
			Monitor.Exit( this);
		}
	}
}


public class ProducerThread
{
	SyncPoint _sync;
	public ProducerThread( SyncPoint sync)
	{
		_sync = sync;
	}
	public void Process()
	{
		int counter;

		for( counter = 0; counter < 10; counter++)
		{
			_sync.Buffer = new Data();
			Console.WriteLine( "Produced +1");
		}
	}
}

public class ConsumerThread
{
	SyncPoint _sync;
	public ConsumerThread( SyncPoint sync)
	{
		_sync = sync;
	}
	public void Process()
	{
		int counter;

		for( counter = 0; counter < 10; counter++)
		{
			Data data = _sync.Buffer;
			Console.WriteLine( "Consumed +1");
		}
	}
}

public class TestProducerConsumer
{
	public void Execute()
	{
		SyncPoint sync = new SyncPoint();
		ProducerThread producer = new ProducerThread(sync);
		ConsumerThread consumer = new ConsumerThread(sync);

		
		Thread thrd1 = new Thread( new ThreadStart( consumer.Process));
		thrd1.Start();

		Thread thrd2 = new Thread( new ThreadStart( producer.Process));
		thrd2.Start();
	}
}

