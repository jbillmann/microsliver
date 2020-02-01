using System;

namespace MicroSliver
{
    /// <summary>
    /// Provides a summary regarding constructor information for a concrete mapping.
    /// </summary>
    public class Map : IMap
    {
        /// <summary>
        /// Gets the concrete type of the mapping.
        /// </summary>
        public Type Concrete { get; private set; }
        /// <summary>
        /// Gets the life cycle scope of the mapping.
        /// </summary>
        public Scope Scope { get; private set; }
        /// <summary>
        /// Gets the object defining custom creation.
        /// </summary>
        public ICreator Creator { get; private set; }

        /// <summary>
        /// Initializes the mapping and sets the mapping type.
        /// </summary>
        /// <param name="concrete">The concrete type.</param>
        public Map(Type concrete)
        {
            Concrete = concrete;
            Scope = Scope.Instance;
        }
        
        /// <summary>
        /// Initializes the mapping and sets the mapping type and creator.
        /// </summary>
        /// <param name="concrete">The concrete type.</param>
        /// <param name="creator">The creator to control instantiation of the concrete type.</param>
        public Map(Type concrete, ICreator creator)
            : this(concrete)
        {
            Creator = creator;
        }

        /// <summary>
        /// Sets life cycle of the mapping to a singleton scope.
        /// </summary>
        public void ToSingletonScope()
        {
            Scope = Scope.Singleton;
        }
        /// <summary>
        /// Sets life cycle of the mapping to a non-permanent scope.
        /// </summary>
        public void ToInstanceScope()
        {
            Scope = Scope.Instance;
        }
        /// <summary>
        /// Sets life cycle of the mapping to a web request scope.
        /// </summary>
        public void ToRequestScope()
        {
            Scope = Scope.Request;
        }

    }
}
