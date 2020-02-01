using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicroSliver;
using MicroSliverTests.TestClasses;

namespace TestProject1
{
    [TestClass]
    public class MicroSliverTests
    {
        [TestMethod]
        public void TestShallowInstancesWithoutCreator()
        {
            var ioc = new IoC();
            ioc.Map<IDependencyA, ClassDependencyA>();

            var classA1 = ioc.Get<ClassParentA>();
            var classA2 = ioc.Get<ClassParentA>();

            Assert.AreNotEqual(classA1.A, classA2.A);
        }

        [TestMethod]
        public void TestShallowRequestsWithoutCreator()
        {
            MakeWebRequest();
            var ioc = new IoC();
            ioc.Map<IDependencyA, ClassDependencyA>().ToRequestScope();

            var classA1 = ioc.Get<ClassParentA>();
            var classA2 = ioc.Get<ClassParentA>();

            //Assert.AreEqual(classA1.A, classA2.A);

            MakeWebRequest();

            var classA3 = ioc.Get<ClassParentA>();

            //Assert.AreNotEqual(classA1.A, classA3.A);
        }

        [TestMethod]
        public void TestShallowSingletonsWithoutCreator()
        {
            var ioc = new IoC();
            ioc.Map<IDependencyA, ClassDependencyA>().ToSingletonScope();

            var classA1 = ioc.Get<ClassParentA>();
            var classA2 = ioc.Get<ClassParentA>();

            Assert.AreEqual(classA1.A, classA2.A);
        }

        [TestMethod]
        public void TestShallowMultipleInstancesWithoutCreator()
        {
            var ioc = new IoC();
            ioc.Map<IDependencyA, ClassDependencyA>();
            ioc.Map<IDependencyB, ClassDependencyB>();


            var classB1 = ioc.Get<ClassParentB>();
            var classB2 = ioc.Get<ClassParentB>();

            Assert.AreNotEqual(classB1.A, classB2.A);
            Assert.AreNotEqual(classB1.B, classB2.B);
        }

        [TestMethod]
        public void TestShallowMultipleSingletonsWithoutCreator()
        {
            var ioc = new IoC();
            ioc.Map<IDependencyA, ClassDependencyA>().ToSingletonScope();
            ioc.Map<IDependencyB, ClassDependencyB>().ToSingletonScope();

            var classB1 = ioc.Get<ClassParentB>();
            var classB2 = ioc.Get<ClassParentB>();

            Assert.AreEqual(classB1.A, classB2.A);
            Assert.AreEqual(classB1.B, classB2.B);
        }

        [TestMethod]
        public void TestShallowMultipleSingletonsWithCreator()
        {
            var ioc = new IoC();
            ioc.Map<IDependencyA>(new ClassDependencyACreator()).ToSingletonScope();
            ioc.Map<IDependencyB>(new ClassDependencyBCreator()).ToSingletonScope();

            var classB1 = ioc.Get<ClassParentB>();
            var classB2 = ioc.Get<ClassParentB>();

            Assert.AreEqual(classB1.A, classB2.A);
            Assert.AreEqual(classB1.B, classB2.B);
        }

        [TestMethod]
        public void TestShallowMultipleDifferentsWithCreator()
        {
            var ioc = new IoC();
            ioc.Map<IDependencyA>(new ClassDependencyACreator()).ToSingletonScope();
            ioc.Map<IDependencyB>(new ClassDependencyBCreator());

            var classB1 = ioc.Get<ClassParentB>();
            var classB2 = ioc.Get<ClassParentB>();

            Assert.AreEqual(classB1.A, classB2.A);
            Assert.AreNotEqual(classB1.B, classB2.B);
        }


        [TestMethod]
        public void TestDeepSingleInstanceWithoutCreator()
        {
            var ioc = new IoC();
            ioc.Map<IDependencyC, ClassDependencyC>();
            ioc.Map<IDependencyE, ClassDependencyE>();

            var classC1 = ioc.Get<ClassParentC>();
            var classC2 = ioc.Get<ClassParentC>();

            Assert.AreNotEqual(classC1.C, classC2.C);
            Assert.AreNotEqual(classC1.C.E, classC2.C.E);
        }

        [TestMethod]
        public void TestDeepSingleSingletonsWithoutCreator()
        {
            var ioc = new IoC();
            ioc.Map<IDependencyC, ClassDependencyC>().ToSingletonScope();
            ioc.Map<IDependencyE, ClassDependencyE>().ToSingletonScope();

            var classC1 = ioc.Get<ClassParentC>();
            var classC2 = ioc.Get<ClassParentC>();

            Assert.AreEqual(classC1.C, classC2.C);
            Assert.AreEqual(classC1.C.E, classC2.C.E);
        }

        [TestMethod]
        public void TestDeepSingleSingletonsWithCreator()
        {
            var ioc = new IoC();
            ioc.Map<IDependencyC, ClassDependencyC>().ToSingletonScope();
            ioc.Map<IDependencyE>(new ClassDependencyECreator()).ToSingletonScope();

            var classC1 = ioc.Get<ClassParentC>();
            var classC2 = ioc.Get<ClassParentC>();

            Assert.AreEqual(classC1.C, classC2.C);
            Assert.AreEqual(classC1.C.E, classC2.C.E);
        }

        [TestMethod]
        public void TestDeepSingleDifferentWithCreator()
        {
            var ioc = new IoC();
            ioc.Map<IDependencyC, ClassDependencyC>();
            ioc.Map<IDependencyE>(new ClassDependencyECreator()).ToSingletonScope();

            var classC1 = ioc.Get<ClassParentC>();
            var classC2 = ioc.Get<ClassParentC>();

            Assert.AreNotEqual(classC1.C, classC2.C);
            Assert.AreEqual(classC1.C.E, classC2.C.E);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void TestMultipleMappings()
        {
            var ioc = new IoC();
            ioc.Map<IDependencyA, ClassDependencyA>();
            ioc.Map<IDependencyA, ClassDependencyA1>();
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void TestMissingMapping()
        {
            var ioc = new IoC();
            ioc.Get<IDependencyA>();
        }

        [TestMethod]
        public void TestHasMapping()
        {
            var ioc = new IoC();
            ioc.Map<IDependencyA, ClassDependencyA>();

            Assert.AreEqual(true, ioc.HasMap(typeof(IDependencyA)));
            Assert.AreEqual(false, ioc.HasMap(typeof(IDependencyB)));
        }

        [TestMethod]
        public void TestTryGetByType()
        {
            var ioc = new IoC();
            ioc.Map<IDependencyA, ClassDependencyA>();

            Assert.AreNotEqual(null, ioc.TryGetByType(typeof(IDependencyA)));
            Assert.AreEqual(null, ioc.TryGetByType(typeof(IDependencyB)));
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void TestClassWithPrimitiveValues()
        {
            var ioc = new IoC();
            ioc.Get<ClassWithPrimitiveArguments>();
        }

        public void MakeWebRequest()
        {
            var request = new HttpRequest("", "http://www.w3.org", string.Empty);
            var response = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(request, response);
        }
    }

    public class ClassDependencyACreator : ICreator
    {
        public object Create()
        {
            return new ClassDependencyA();
        }
    }

    public class ClassDependencyBCreator : ICreator
    {
        public object Create()
        {
            return new ClassDependencyB();
        }
    }

    public class ClassDependencyECreator : ICreator
    {
        public object Create()
        {
            return new ClassDependencyE();
        }
    }
}
