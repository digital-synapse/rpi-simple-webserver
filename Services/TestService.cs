using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace rpi_gpio_webserver
{
    public class TestService : IRequestHandler
    {
        public string RequestListener(HttpListenerRequest request)
        {
            // Get the start time
            var startTime = DateTime.Now;

            // Wait for the requested time 
            int msTimeout = 0;
            int.TryParse(request.QueryString["timeout"], out msTimeout);            
            if (msTimeout > 0)
                Thread.Sleep(msTimeout);

            // Get the end time
            var endTime = DateTime.Now;

            return 
                "Request Received: " + startTime.ToString() + "\n"
               +"Response Sent:    " + endTime.ToString();
        }
    }
}
