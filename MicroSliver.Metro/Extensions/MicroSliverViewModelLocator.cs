using System;
using System.Reflection;

namespace MicroSliver.Metro.Extensions
{
    /// <summary>
    /// Manages and handles view model instantiation. 
    /// </summary>
    public abstract class MicroSliverViewModelLocator
    {
        private string assemblyName;
        private static IIoC _ioc;
        public IIoC IoC
        {
            get
            {
                return _ioc;
            }
        }

        /// <summary>
        /// Initializes the viewmodel locator and sets its internal IIoC container for use with view model instantiation.  Requires assembly for object instantiation.
        /// </summary>
        /// <param name="ioc">The container containing the mappings.</param>
        /// <param name="ExecutingAssembly">The executing assembly of the class implementing this view model locator.</param>
        public void LoadIoC(IIoC ioc, Assembly ExecutingAssembly)
        {
            _ioc = ioc;
            assemblyName = ExecutingAssembly.FullName;
        }

        /// <summary>
        /// Uses Silverlight indexer binding to instantiate the the viewmodel from its internal IIoC container.  Must be the fully qualified name of the viewModel.
        /// </summary>
        /// <param name="viewModel">Must be the fully qualified name of the viewModel..</param>
        public object this[string viewModel]
        {
            get
            {
                var fullname = viewModel + ", " + assemblyName;
                return IoC.GetByType(Type.GetType(fullname));
            }
        }
    }
}
