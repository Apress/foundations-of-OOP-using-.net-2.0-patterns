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
using Devspace.Commons.TDD;

namespace Devspace.Commons.Pool {

//    public interface IFlyWeight {
//        bool IsDescriptor< desctype>( desctype descriptor);
//    }
    
    
    public interface IPoolableObjectFactory< type> {
        type MakeObject( IObjectPoolBase<type> parent);
        void ActivateObject(type obj);
        void PassivateObject(type obj);
    }

    public interface IObjectPoolBase< type> {
        bool ReturnObject( type obj);
    }
    
    public interface IObjectPool< type> : IObjectPoolBase< type> {
        int NumIdle {
            get;
        }
        int NumActive{
            get;
        }
        type GetObject();
        void SetFactory( IPoolableObjectFactory< type> factory);
    }
    
    /*
    
    public interface IObjectPool< type> {
        type BorrowObject();
        void ReturnObject(type obj);
        void InvalidateObject(type obj);
        void PreCreateObject();
        int GetNumIdle();
        int GetNumActive();
        void Clear();
        void Close();
        void SetFactory(IPoolableObjectFactory<type> factory);
    }

    public interface IObjectPoolFactory< type> {
        IObjectPool<type> CreatePool();
    }*/

    
}



