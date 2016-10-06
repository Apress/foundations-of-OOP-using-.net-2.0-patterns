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
using System.Collections;

namespace Devspace.Commons.Command {
    public interface ICommand<contexttype> {
        bool Execute( contexttype context);
    }
}

namespace Devspace.Commons.ChainOfResponsibility {
    public delegate bool Handler<contexttype>(contexttype context);
    
    public class ChainOfResponsibility< contexttype>{
        private Handler<contexttype> _handlers;
        
        public ChainOfResponsibility( Handler<contexttype> handlers) {
        }

        public bool HandleRequest( contexttype param) {
            Delegate[] handlers = _handlers.GetInvocationList();
            
            for(int c1 = 0; c1 < handlers.Length; c1++) {
                if((bool)handlers[c1].DynamicInvoke(new Object[] { param })) {
                    return true;
                }
            }
            return false;
        }
    }
}


/*
namespace ChainOfResponsibility {
    interface IContext {
    }
    interface IHandler {
        bool HandleRequest(IContext context);
    }
    
    class ContextForHandler1: IContext {
    }
    class ContextForHandler2: IContext {
    }
    class UnknownContext: IContext {
    }
    
    class ConcreteHandler1: IHandler {
        IHandler _next;
        
        public ConcreteHandler1(IHandler next) {
            _next = next;
        }
        public bool HandleRequest(IContext context) {
            if(context is ContextForHandler1) {
                return true;
            }
            else {
                return _next.HandleRequest(context);
            }
        }
    }
    class ConcreteHandler2: IHandler {
        public bool HandleRequest(IContext context) {
            if(context is ContextForHandler2) {
                return true;
            }
            else {
                return false;
            }
        }
    }
    
    delegate bool HandleRequest<ContextType>(ContextType context);
    
    delegate bool HandleRequest2(IContext context);
    
    class HandlerHub<ContextType> {
        private HandleRequest<ContextType> _handlers;
        private HandleRequest<ContextType> _localHandler;
        
        public HandleRequest<ContextType> AddHandler(HandleRequest<ContextType> handler) {
            if(_localHandler == null) {
                _localHandler = new HandleRequest<ContextType>(this.LocalHandleRequest);
            }
            _handlers += handler;
            return _localHandler;
        }
        private bool LocalHandleRequest(ContextType context) {
            Delegate[] handlers = _handlers.GetInvocationList();
            
            for(int c1 = 0; c1 < handlers.Length; c1++) {
                if((bool)handlers[c1].DynamicInvoke(new Object[] { context })) {
                    return true;
                }
            }
            return false;
        }
    }
    
    class HandlerHub2<DelegateType, ContextType> where DelegateType: class {
        private Delegate _handlers;
        private Delegate _localHandler;
        
        private bool LocalHandleRequest(params Object[] args) {
            Delegate[] handlers = _handlers.GetInvocationList();
            
            for(int c1 = 0; c1 < handlers.Length; c1++) {
                if((bool)handlers[c1].DynamicInvoke(args)) {
                    return true;
                }
            }
            return false;
        }
        
        public DelegateType AddHandler(Delegate handler) {
            if(_localHandler == null) {
                _localHandler = Delegate.CreateDelegate(handler.GetType(), this, "LocalHandleRequest");
            }
            _handlers = Delegate.Combine(_handlers, handler);
            return _localHandler as DelegateType;
        }
        
    }
    
    class ConcreteHandler {
        public void CallIt() {
            //Delegate handler = Delegate.CreateDelegate(typeof(HandleRequest), this, "HandleRequest2");
            //handler.DynamicInvoke(new Object[] { null });
            //_handlers(null);
        }
        public bool HandleRequest2(IContext context) {
            if(context is ContextForHandler1) {
                return true;
            }
            else {
                return false;
            }
        }
        public bool HandleRequest3(IContext context) {
            if(context is ContextForHandler2) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}


delegate void MyTestSomething(string arg);

class TestSomething {
    public static void TestMethod(params Object[] args) {
        Console.WriteLine("Args " + args[0]);
    }
    public static void TestMethod(string arg) {
    }
}
*/
/*
[TestFixture]
public class TestChainOfResponsibility {
    [Test]
     public void Test() {
     ChainOfResponsibility.ConcreteHandler1 handler =
     new ChainOfResponsibility.ConcreteHandler1( new ChainOfResponsibility.ConcreteHandler2());
     
     ChainOfResponsibility.ContextForHandler1 context1 = new ChainOfResponsibility.ContextForHandler1();
     ChainOfResponsibility.ContextForHandler2 context2 = new ChainOfResponsibility.ContextForHandler2();
     ChainOfResponsibility.UnknownContext context3 = new ChainOfResponsibility.UnknownContext();
     
     Assert.IsTrue(handler.HandleRequest(context1));
     Assert.IsTrue(handler.HandleRequest(context2));
     Assert.IsFalse(handler.HandleRequest(context3));
     }
    ChainOfResponsibility.HandleRequest<ChainOfResponsibility.IContext> CreateHandlers() {
        ChainOfResponsibility.ConcreteHandler cls = new ChainOfResponsibility.ConcreteHandler();
        ChainOfResponsibility.HandleRequest<ChainOfResponsibility.IContext> handlers;
        
        ChainOfResponsibility.HandlerHub<ChainOfResponsibility.IContext> hub =
        new ChainOfResponsibility.HandlerHub<ChainOfResponsibility.IContext>();
        
        hub.AddHandler(new ChainOfResponsibility.HandleRequest<ChainOfResponsibility.IContext>(cls.HandleRequest2));
        handlers = hub.AddHandler(new ChainOfResponsibility.HandleRequest<ChainOfResponsibility.IContext>(cls.HandleRequest3));
        return handlers;
    }
    [Test]
    public void TestDelegate() {
        ChainOfResponsibility.HandleRequest<ChainOfResponsibility.IContext> handlers;
        
        handlers = CreateHandlers();
        System.GC.Collect();
        GC.WaitForPendingFinalizers();
        System.GC.Collect();
        
        ChainOfResponsibility.ContextForHandler1 context1 = new ChainOfResponsibility.ContextForHandler1();
        ChainOfResponsibility.ContextForHandler2 context2 = new ChainOfResponsibility.ContextForHandler2();
        ChainOfResponsibility.UnknownContext context3 = new ChainOfResponsibility.UnknownContext();
        
        Assert.IsTrue(handlers(context1), "First");
        Assert.IsTrue(handlers(context2), "Second");
        Assert.IsFalse(handlers(context3), "Third");
    }
    [Test]
     public void TestGenerics() {
     MyTestSomething delg = new MyTestSomething(TestSomething.TestMethod);
     Console.WriteLine( delg.GetType().
     Console.WriteLine(delg.GetType().FullName);
     System.Reflection.MethodInfo methodInfo = delg.GetType().GetMethod( "Invoke");
     
     System.Reflection.ParameterInfo []parameters = methodInfo.GetParameters();
     foreach(System.Reflection.ParameterInfo parameter in parameters) {
     Console.WriteLine("(" + parameter.Name + ")(" + parameter.ToString() + ")");
     }
    ChainOfResponsibility.HandlerHub2< ChainOfResponsibility.HandleRequest2, ChainOfResponsibility.IContext> hub
     = new ChainOfResponsibility.HandlerHub2<ChainOfResponsibility.HandleRequest2, ChainOfResponsibility.IContext>();
     ChainOfResponsibility.HandleRequest2 handler = hub.AddHandler( new ChainOfResponsibility.HandleRequest2( TestChainOfResponsibility.Method));
     handler(new ChainOfResponsibility.ContextForHandler1());
    }
    static bool Method(ChainOfResponsibility.IContext context) {
        Console.WriteLine("hello world");
        return false;
    }
}
*/


