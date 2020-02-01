using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroSliverTests.TestClasses
{
    public class ClassDependencyC : IDependencyC
    {
        public IDependencyE E { get; set; }
        public ClassDependencyC(IDependencyE e)
        {
            E = e;
        }
    }
}
