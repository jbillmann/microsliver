using System;

namespace MicroSliver
{
    /// <summary>
    /// Defines information regarding a mapped concrete class
    /// </summary>
    public interface IMap
    {
        /// <summary>
        /// Gets the concrete type of the mapping.
        /// </summary>
        Type Concrete { get; }
        /// <summary>
        /// Gets the life cycle scope of the mapping.
        /// </summary>
        Scope Scope { get; }
        /// <summary>
        /// Gets the object defining custom creation.
        /// </summary>
        ICreator Creator { get; }

        /// <summary>
        /// Sets life cycle of the mapping to a singleton scope.
        /// </summary>
        void ToSingletonScope();
        /// <summary>
        /// Sets life cycle of the mapping to a non-permanent scope.
        /// </summary>
        void ToInstanceScope();
        /// <summary>
        /// Sets life cycle of the mapping to a web request scope.
        /// </summary>
        void ToRequestScope();
    }
}
