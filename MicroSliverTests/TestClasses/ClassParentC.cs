using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroSliverTests.TestClasses
{
    public class ClassParentC
    {
        public IDependencyC C { get; set; }

        public ClassParentC(IDependencyC c)
        {
            C = c;
        }
    }
}
