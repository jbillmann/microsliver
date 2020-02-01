using System.Reflection;

namespace MicroSliver
{
    /// <summary>
    /// Provides a summary regarding constructor information for a mapping.
    /// </summary>
    public interface ICtorInfo
    {
        ConstructorInfo Ctor { get; set; }
        ParameterInfo[] CtorParams { get; set; }
    }
}
