using System;
using System.Web;
using System.Web.Mvc;

namespace MicroSliver.Web.Extensions
{
    /// <summary>
    /// Implements the default HttpApplication to control plumbing for controller factory.
    /// </summary>
    public abstract class MicroSliverHttpApplication : HttpApplication
    {
        private static IIoC _ioc;
        public static IIoC IoC
        {
            get
            {
                return _ioc;
            }
        }

        public override void Init()
        {
            _ioc = LoadIIoC();
            ControllerBuilder.Current.SetControllerFactory(new MicroSliverControllerFactory(_ioc));
        }

        /// <summary>
        /// Called before setting controller factory as a means to fetch mappings.
        /// </summary>
        protected abstract IIoC LoadIIoC();
        protected abstract void Application_Start();

        protected void Application_Start(object sender, EventArgs e)
        {
            Application_Start();
        }

    }


}
