using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MicroSliver.Web.Extensions
{
    /// <summary>
    /// Implements the default controller factory for handling ASP.NET MVC controller equests.
    /// </summary>
    public class MicroSliverControllerFactory : DefaultControllerFactory
    {
        private IIoC IoC;

        /// <summary>
        /// Constructor for the controller factory which sets its internal field for use with controller instantiation.
        /// </summary>
        /// <param name="ioc">The container containing the mappings.</param>
        public MicroSliverControllerFactory(IIoC ioc)
        {
            IoC = ioc;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }

            var controller = (IController)IoC.GetByType(controllerType);
            if (controller == null)
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }

            return controller;
        }
    }
}
