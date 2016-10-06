using System;
using System.Threading;
using NUnit.Framework;

class StaticMethodThreads {
    public static void Thread1() {
        for(int i = 0; i < 10; i++) {
            Console.WriteLine("Thread1 {0}", i);
        }
    }
    
    public static void Thread2() {
        for(int i = 0; i < 10; i++) {
            Console.WriteLine("Thread2 {0}", i);
        }
    }
}

class DynamicMethodThreads {
    public void Thread1() {
        for(int i = 0; i < 10; i++) {
            Console.WriteLine("Thread1 {0}", i);
        }
    }
    
    public void Thread2() {
        for(int i = 0; i < 10; i++) {
            Console.WriteLine("Thread2 {0}", i);
        }
    }
}

class SleepMethodThreads {
    public void Thread1() {
        for(int i = 0; i < 10; i++) {
            Console.WriteLine("Thread1 {0}", i);
            Thread.Sleep(1);
        }
    }
    
    public void Thread2() {
        for(int i = 0; i < 10; i++) {
            Console.WriteLine("Thread2 {0}", i);
            Thread.Sleep(1);
        }
    }
}

class ErrorMethodThreads {
    public void Thread1() {
        for(int i = 0; i < 10; i++) {
            Thread thr = Thread.CurrentThread;
            Console.WriteLine(thr.ToString() + "=" + i);
            Thread.Sleep(new TimeSpan(0, 0, -1));
        }
    }
    
    public void Thread2() {
        for(int i = 0; i < 10; i++) {
            Thread thr = Thread.CurrentThread;
            Console.WriteLine(thr.ToString() + "=" + i);
            Thread.Sleep(new TimeSpan(0, 0, 1));
        }
    }
}

class PriorityMethodThreads {
    public void Thread1() {
        for(int i = 0; i < 10; i++) {
            Console.WriteLine("Thread1 {0}", i);
        }
    }
    
    public void Thread2() {
        for(int i = 0; i < 10; i++) {
            Console.WriteLine("Thread2 {0}", i);
        }
    }
}

[TestFixture]
public class TestSimpleThreading {
    [Test]
    public void RunStaticMethodTests() {
        Console.WriteLine("*** Start Static Method Tests ***");
        Console.WriteLine("Before start thread");
        
        Thread tid1 = new Thread(new ThreadStart(StaticMethodThreads.Thread1));
        Thread tid2 = new Thread(new ThreadStart(StaticMethodThreads.Thread2));
        
        tid1.Start();
        tid2.Start();
        Console.WriteLine("*** End Static Method Tests ***");
    }
    [Test]
    public void RunDynamicMethodTests() {
        Console.WriteLine("*** Start Dynamic Method Tests ***");
        
        DynamicMethodThreads thr = new DynamicMethodThreads();
        
        Thread tid1 = new Thread(new ThreadStart(thr.Thread1));
        Thread tid2 = new Thread(new ThreadStart(thr.Thread2));
        
        tid1.Start();
        tid2.Start();
        Console.WriteLine("*** End Dynamic Method Tests ***");
    }
    [Test]
    public void RunSleepMethodTests() {
        Console.WriteLine("*** Start Sleep Method Tests ***");
        
        SleepMethodThreads thr = new SleepMethodThreads();
        
        Thread tid1 = new Thread(new ThreadStart(thr.Thread1));
        Thread tid2 = new Thread(new ThreadStart(thr.Thread2));
        
        tid1.Start();
        tid2.Start();
        Console.WriteLine("*** End Sleep Method Tests ***");
    }
    [Test]
    
    public void RunErrorMethodTests() {
        Console.WriteLine("*** Start Error Method Tests ***");
        
        ErrorMethodThreads thr = new ErrorMethodThreads();
        
        Thread tid1 = new Thread(new ThreadStart(thr.Thread1));
        Thread tid2 = new Thread(new ThreadStart(thr.Thread2));
        
        tid1.Start();
        tid2.Start();
        Console.WriteLine("*** End Error Method Tests ***");
    }
    [Test]
    public void RunPriorityMethodTests() {
        Console.WriteLine("*** Start Priority Method Tests ***");
        
        PriorityMethodThreads thr = new PriorityMethodThreads();
        
        Thread tid1 = new Thread(new ThreadStart(thr.Thread1));
        Thread tid2 = new Thread(new ThreadStart(thr.Thread2));
        
        tid1.Priority = ThreadPriority.Lowest;
        tid2.Priority = ThreadPriority.Highest;
        
        tid1.Start();
        tid2.Start();
        Console.WriteLine("*** End Priority Method Tests ***");
    }
}
