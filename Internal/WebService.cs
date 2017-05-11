using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace rpi_gpio_webserver
{
    public static class WebService
    {
        private static Dictionary<string, WebServer> services = new Dictionary<string, WebServer>();

        public static void RegisterAndStart<TRequestHandler>(string route) where TRequestHandler : IRequestHandler, new()
        {
            var requestHandler = (TRequestHandler)Activator.CreateInstance(typeof(TRequestHandler));
            services[route] = new WebServer(requestHandler.RequestListener, route);
            services[route].Run();
        }
    }

    public interface IRequestHandler
    {
        string RequestListener(HttpListenerRequest request);
    }
}
