using System;
using System.ServiceModel.DomainServices.Server;

namespace MicroSliver.Web.Extensions
{
    /// <summary>
    /// Implements the domain service factory interface that allows for control over domain service instantiation.
    /// </summary>
    public class MicroSliverDomainFactory : IDomainServiceFactory
    {
        private IIoC IoC;

        /// <summary>
        /// Initializes the domain factory and sets its internal IIoC container for use with domain service instantiation.
        /// </summary>
        /// <param name="ioc">The container containing the mappings.</param>
        public MicroSliverDomainFactory(IIoC ioc)
        {
            IoC = ioc;
        }

        /// <summary>
        /// Handles the creation of domain service objects.
        /// </summary>
        public DomainService CreateDomainService(Type domainServiceType, DomainServiceContext context)
        {
            var domainService = (DomainService)IoC.GetByType(domainServiceType);

            if (domainService != null)
            {
                domainService.Initialize(context);
            }

            return domainService;
        }

        public void ReleaseDomainService(DomainService domainService)
        {
            domainService.Dispose();
        }
    }
}
