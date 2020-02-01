using System;
using System.Collections.Generic;

namespace MicroSliver
{
    /// <summary>
    /// Defines the MicroSliver container that manages mappings and instantiations.
    /// </summary>
    public interface IIoC
    {
        /// <summary>
        /// Creates a mapping
        /// </summary>
        IMap Map<TContract, TConcrete>() where TConcrete : TContract;
        /// <summary>
        /// Creates a mapping that specifies a ICreator object.
        /// </summary>
        IMap Map<TContract>(ICreator creator);
        /// <summary>
        /// Returns all mappings.
        /// </summary>
        IEnumerable<IMap> GetMappings();
        /// <summary>
        /// Returns a specific mapping.
        /// </summary>
        IMap GetMap<TContract>();
        /// <summary>
        /// Removes a specific mapping.
        /// </summary>
        void UnMap<TContract>();
        /// <summary>
        /// Returns true if a mapping exists for type.
        /// </summary>
        bool HasMap(Type T);
        /// <summary>
        /// Clears all mappings and caches.
        /// </summary>
        void Clear();
        /// <summary>
        /// Clears web request cache.
        /// </summary>
        void ClearRequests();
        /// <summary>
        /// Gets an instance of the object.
        /// </summary>
        T Get<T>();
        /// <summary>
        /// Gets an instance of the object by Type.
        /// </summary>
        object GetByType(Type T);
        /// <summary>
        /// Gets an instance of the object by Type if exists, otherwise null.
        /// </summary>
        object TryGetByType(Type T);
    }
}
