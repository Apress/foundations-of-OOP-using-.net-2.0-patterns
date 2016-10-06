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
using System.Text;
using System.Reflection;
using NUnit.Framework;
using Devspace.Commons.Loader;

public interface MyInterface {
    void Something();
}


[TestFixture]
public class TestPlugin {
    private string _pluginsPath;
    private string _commonsAssembly;
    private string _implAssembly;
    private string _interfaceAssembly;

    private void LinuxConfiguration() {
        _pluginsPath = "/home/cgross/documents/active/devspace.commons/lib/plugins";
    }
    private void WindowsConfiguration() {
        _pluginsPath = @"c:\cygwin\home\cgross\desktop\documents\active\devspace.commons\lib\plugins";
    }
    [TestFixtureSetUp]
    public void Setup() {
        //LinuxConfiguration();
        WindowsConfiguration();
        _commonsAssembly = "Devspace.Commons.Loader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        _implAssembly = "Loader.Tests.Implementation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        _interfaceAssembly = "Loader.Tests.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
    }

    [Test]
    public void TestResolverPlugins() {
        IResolver<Identifier> resolver = new ResolverDynamicAssemblyDirectory( "testapplication",
                                                                               AppDomain.CurrentDomain.BaseDirectory, true, _pluginsPath );
        resolver.Load();
        Console.WriteLine( "Test TestResolverPlugins (\n" + resolver.ToString() + ")" );
    }
    [Test]
    public void TestResolverPluginsCreate() {
        IResolver<Identifier> resolver = new ResolverDynamicAssemblyDirectory( "testapplication",
                                                                               AppDomain.CurrentDomain.BaseDirectory, true, _pluginsPath );
        resolver.Load();
        Assert.IsTrue( resolver.CanCreate( new Identifier( "ExampleClass" ) ), "Resolver cannot find type" );
        ExampleInterface obj = resolver.CreateInstance<ExampleInterface>( new Identifier( "ExampleClass" ) );
        Assert.IsNotNull( obj, "Created ExampleClass object is null" );
        obj.Message();
        Console.WriteLine( "Test  TestResolverPluginsCreate (\n" + resolver.ToString() + ")" );
    }
    [Test]
    public void TestResolverAssemblyCreate() {
        ResolverStaticAssemblies resolver = new ResolverStaticAssemblies( "testapplication" );
        resolver.AppendPath( _pluginsPath );
        resolver.Load();
        Identifier identifier = new Identifier( _implAssembly, "ExampleClass" );
        Console.WriteLine( "Before Test  TestResolverAssemblyCreate (\n" + resolver.ToString() + ")" );
        ExampleInterface obj = resolver.CreateInstance< ExampleInterface>( identifier );
        Assert.IsNotNull( obj, "Created ExampleClass object is null" );
        obj.Message();
        Console.WriteLine( "After Test  TestResolverAssemblyCreate (\n" + resolver.ToString() + ")" );
    }
}

