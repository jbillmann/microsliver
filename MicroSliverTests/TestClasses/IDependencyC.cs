using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroSliverTests.TestClasses
{
    public interface IDependencyC
    {
        IDependencyE E { get; set; }
    }
}
