using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroSliverTests.TestClasses
{
    public class ClassParentA
    {
        public IDependencyA A { get; set; }

        public ClassParentA(IDependencyA a)
        {
            A = a;
        }
    }
}
