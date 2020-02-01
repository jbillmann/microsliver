using System.Reflection;

namespace MicroSliver
{
    /// <summary>
    /// Provides a summary regarding constructor information for a mapping.
    /// </summary>
    public class CtorInfo : ICtorInfo
    {
        public ConstructorInfo Ctor { get; set; }
        public ParameterInfo[] CtorParams { get; set; }

        public CtorInfo(ConstructorInfo ctor, ParameterInfo[] ctorParams)
        {
            Ctor = ctor;
            CtorParams = ctorParams;
        }
    }
}
