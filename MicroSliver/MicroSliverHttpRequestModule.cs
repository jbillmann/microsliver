using System.Web;

namespace MicroSliver
{
    /// <summary>
    /// Dynamically registers the MicroSliverHttpModule.
    /// </summary>
    public class PreApplicationStartCode
    {
        public static void Start()
        {
            Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(MicroSliverHttpRequestModule));
        }
    }

    /// <summary>
    /// Handles clearing the web requests each time the HttpApplication invokes the EndRequest.
    /// </summary>
    public class MicroSliverHttpRequestModule : IHttpModule
    {
        private static IIoC IoC;
        public void Init(HttpApplication context)
        {
            context.EndRequest += new System.EventHandler(ClearRequests);
        }

        /// <summary>
        /// Holds a reference to an IIoC objects to clear web requests cache.
        /// </summary>
        public static void ManageIoC(IIoC ioc)
        {
            IoC = ioc;
        }

        /// <summary>
        /// Clears the web request cache for the referenced IIoC container.
        /// </summary>
        private static void ClearRequests(object sender, System.EventArgs e)
        {
            if (IoC != null)
            {
                IoC.ClearRequests();
            }
        }

        public void Dispose()
        {
        }
    }
}
