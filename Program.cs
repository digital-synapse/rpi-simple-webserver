using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RaspberryPiDotNet;
using System.Diagnostics;

namespace rpi_gpio_webserver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("A simple webserver to set/read gpio. Press a key to quit.");

            WebService.RegisterAndStart<GpioService>("/gpio");
            WebService.RegisterAndStart<CmdService>("/cmd");
            WebService.RegisterAndStart<TestService>("/test");

            // run forever (not sure how else to keep service alive when starting from init.d!)
            while (true) { Thread.Sleep(10); }
        }        
    }
}
