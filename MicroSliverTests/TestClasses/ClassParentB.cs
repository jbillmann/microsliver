using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroSliverTests.TestClasses
{
    public class ClassParentB
    {
        public IDependencyA A { get; set; }
        public IDependencyB B { get; set; }

        public ClassParentB(IDependencyA a, IDependencyB b)
        {
            A = a;
            B = b;
        }
    }
}
