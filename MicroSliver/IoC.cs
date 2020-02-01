using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSliver
{

    public enum Scope
    {
        Instance,
        Singleton,
        Request
    }

    /// <summary>
    /// Defines the MicroSliver container that manages mappings and instantiations.
    /// </summary>
    public class IoC : IIoC
    {

        #region Properties

        private readonly IDictionary<Type, IMap> _mappings;
        private readonly IDictionary<Type, ICtorInfo> _cachedCtors;
        private readonly IDictionary<Type, object> _cachedSingletons;
        private readonly IDictionary<Type, object> _cachedRequests;
        private readonly object _lock;

        #endregion

        #region Public Methods

        public IoC()
        {
            _mappings = new Dictionary<Type, IMap>();
            _cachedCtors = new Dictionary<Type, ICtorInfo>();
            _cachedSingletons = new Dictionary<Type, object>();
            _cachedRequests = new Dictionary<Type, object>();
            _lock = new object();

#if !SILVERLIGHT
            MicroSliverHttpRequestModule.ManageIoC(this);
#endif
        }

        /// <summary>
        /// Creates a mapping
        /// </summary>
        public IMap Map<TContract, TConcrete>() where TConcrete : TContract
        {
            return AddMap<TContract, TConcrete>();
        }

        /// <summary>
        /// Creates a mapping that specifies a ICreator object.
        /// </summary>
        public IMap Map<TContract>(ICreator creator)
        {
            return AddMap<TContract>(creator);
        }

        /// <summary>
        /// Gets an instance of the object.
        /// </summary>
        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        /// <summary>
        /// Gets an instance of the object by Type.
        /// </summary>
        public object GetByType(Type T)
        {
            return Get(T);
        }

        /// <summary>
        /// Gets an instance of the object by Type if exists, otherwise null.
        /// </summary>
        public object TryGetByType(Type T)
        {
            try
            {
                return Get(T);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Removes a specific mapping.
        /// </summary>
        public void UnMap<TContract>()
        {
            var contract = typeof(TContract);
            lock (_lock)
            {
                if (_mappings.ContainsKey(contract))
                {
                    _mappings.Remove(contract);
                }
            }
        }

        /// <summary>
        /// Returns a specific mapping.
        /// </summary>
        public IMap GetMap<TContract>()
        {
            var contract = typeof(TContract);
            lock (_lock)
            {
                if (_mappings.ContainsKey(contract))
                {
                    return _mappings[contract];
                }
            }
            throw new Exception("MicroSliver is unable to find a mapping for interface of type " + contract.Name + ".");
        }


        /// <summary>
        /// Returns true if a mapping exists for type.
        /// </summary>
        public bool HasMap(Type T)
        {
            return _mappings.ContainsKey(T);
        }

        /// <summary>
        /// Clears all mappings and caches.
        /// </summary>
        public void Clear()
        {
            _mappings.Clear();
            _cachedCtors.Clear();
            _cachedSingletons.Clear();
            _cachedRequests.Clear();
        }

        /// <summary>
        /// Clears web request cache.
        /// </summary>
        public void ClearRequests()
        {
            _cachedRequests.Clear();
        }

        /// <summary>
        /// Returns all mappings.
        /// </summary>
        public IEnumerable<IMap> GetMappings()
        {
            return _mappings.Values;
        }

        #endregion

        #region Infrastructure

        private IMap AddMap<TContract, TConcrete>()
        {
            var contract = typeof(TContract);
            var concrete = typeof(TConcrete);
            lock (_lock)
            {
                if (!_mappings.ContainsKey(contract))
                {
                    _mappings[contract] = new Map(concrete);
                }
                else
                {
                    throw new Exception("A mapping for interface of type " + contract.Name + " already exists.  MicroSliver does not allow for multiple bindings of the same type.");
                }
                return _mappings[contract];
            }
        }

        private IMap AddMap<TContract>(ICreator creator)
        {
            var contract = typeof(TContract);
            lock (_lock)
            {
                if (!_mappings.ContainsKey(contract))
                {
                    _mappings[contract] = new Map(null, creator);
                }
                else
                {
                    throw new Exception("A mapping for interface of type " + contract.Name + " already exists.  MicroSliver does not allow for multiple bindings of the same type.");
                }
                return _mappings[contract];
            }
        }

        private object Get(Type T)
        {
            if (T.IsInterface)
            {
                try
                {
                    var map = _mappings[T];
                    return (ProcessScope(T, map));
                }
                catch (KeyNotFoundException ex)
                {
                    throw new Exception("MicroSliver does not have a mapping for interface of type " + T.Name + ".", ex);
                }
            }
            else if (T.IsPrimitive || T.IsValueType)
            {
                throw new Exception("MicroSliver is unable to set values to primitive/value types in the constructor.");
            }

            CacheCtorInfo(T);
            var ctor = _cachedCtors[T];
            if (ctor.CtorParams.Length == 0)
            {
                return ctor.Ctor.Invoke(null);
            }

            return ProcessCtor(ctor);
        }

        private object ProcessScope(Type T, IMap map)
        {
            switch (map.Scope)
            {
                case Scope.Instance:
                    if (map.Creator != null)
                    {
                        return map.Creator.Create();
                    }
                    return Get(map.Concrete);
                case Scope.Singleton:
                    lock (_lock)
                    {
                        if (!_cachedSingletons.ContainsKey(T))
                        {
                            if (map.Creator != null)
                            {
                                _cachedSingletons.Add(T, map.Creator.Create());
                            }
                            else
                            {
                                _cachedSingletons.Add(T, Get(map.Concrete));
                            }
                        }
                        return _cachedSingletons[T];
                    }
                case Scope.Request:
                    lock (_lock)
                    {
                        if (!_cachedRequests.ContainsKey(T))
                        {
                            if (map.Creator != null)
                            {
                                _cachedRequests.Add(T, map.Creator.Create());
                            }
                            else
                            {
                                _cachedRequests.Add(T, Get(map.Concrete));
                            }
                        }

                        return _cachedRequests[T];
                    }
                default:
                    return null;
            }
        }

        private void CacheCtorInfo(Type T)
        {
            lock (_lock)
            {
                if (!_cachedCtors.ContainsKey(T))
                {
                    var concreteCtor = T.GetConstructors()[0];
                    var concreteCtorParams = concreteCtor.GetParameters();
                    _cachedCtors.Add(T, new CtorInfo(concreteCtor, concreteCtorParams));
                }
            }
        }

        private object ProcessCtor(ICtorInfo ctorInfo)
        {
            var parms = from p in ctorInfo.CtorParams select Get(p.ParameterType);
            return ctorInfo.Ctor.Invoke(parms.ToArray());
        }

        #endregion

    }
}