using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectCopyHelper;

namespace ObjectCopyHelperTest
{
    [TestClass]
    public class CopyHelperTest
    {
        [TestMethod]
        public void CopyToTest()
        {
            var c = new Class1("aaa" , 1 , new [] { 1,2,3 });
            c.StringProperty = "bbb";
            c.IntProperty = 2;
            var x = c.CopyTo<Class1>();

            x.IntProperty.Is(2);
            x.StringProperty.Is("bbb");
            x.ReadOnlyStringProperty.Is("aaa");
            x.ReadOnlyIntProperty.Is(1);
            x.ReadOnlyListProperty.Is(new[] { 1, 2, 3 });
        }

        [TestMethod]
        public void CopyToTest2()
        {
            var c = new Class1("aaa", 1, new[] { 1, 2, 3 });
            c.StringProperty = "bbb";
            c.IntProperty = 2;
            var x = c.CopyTo<Class1>( nameof(c.ReadOnlyIntProperty) , 100 );

            x.IntProperty.Is(2);
            x.StringProperty.Is("bbb");
            x.ReadOnlyStringProperty.Is("aaa");
            x.ReadOnlyIntProperty.Is(100);
            x.ReadOnlyListProperty.Is(new[] { 1, 2, 3 });
        }

        [TestMethod]
        public void CopyToTest3()
        {
            var c = new Class1("aaa", 1, new[] { 1, 2, 3 });
            c.StringProperty = "bbb";
            c.IntProperty = 2;
            var x = c.CopyTo<Class1>(nameof(c.StringProperty), "ccc");

            x.IntProperty.Is(2);
            x.StringProperty.Is("ccc");
            x.ReadOnlyStringProperty.Is("aaa");
            x.ReadOnlyIntProperty.Is(1);
            x.ReadOnlyListProperty.Is(new[] { 1, 2, 3 });
        }

        [TestMethod]
        public void CopyToTest4()
        {
            var c = new Class1("aaa", 1, new[] { 1, 2, 3 });
            c.StringProperty = "bbb";
            c.IntProperty = 2;

            var propertyAndValues = new (string , object)[] {(nameof(c.ReadOnlyIntProperty), 100),
                                                             (nameof(c.IntProperty), 200)};
            var x = c.CopyTo<Class1>(propertyAndValues);

            x.IntProperty.Is(200);
            x.StringProperty.Is("bbb");
            x.ReadOnlyStringProperty.Is("aaa");
            x.ReadOnlyIntProperty.Is(100);
            x.ReadOnlyListProperty.Is(new[] { 1, 2, 3 });
        }
    }

    [Serializable()]
    public class Class1
    {
        public string StringProperty { get; set; }
        public string ReadOnlyStringProperty { get; }

        public int IntProperty { get; set; }
        public int ReadOnlyIntProperty { get;   }

        public IReadOnlyList<int> ReadOnlyListProperty { get; }

        public Class1( string s , int i , IEnumerable<int> c)
        {
            ReadOnlyStringProperty = s;
            ReadOnlyIntProperty = i;
            ReadOnlyListProperty = c.ToList();
        }
    }
}
