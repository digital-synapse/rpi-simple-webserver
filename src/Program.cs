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
            var gpioService = new WebServer(GPIOResponse, "/gpio");
            var cmdService = new WebServer(CMDResponse, "/cmd");
            gpioService.Run();
            cmdService.Run();

            Console.WriteLine("A simple webserver to set/read gpio. Press a key to quit.");

            // run forever (not sure how else to keep service alive when starting from init.d!)
            while (true) { Thread.Sleep(10); }
            cmdService.Stop();
            gpioService.Stop();
        }


        public static string CMDResponse(HttpListenerRequest request)
        {
            string rtnValue;
            StringBuilder sb = new StringBuilder();
            try
            {
                var cmd = request.QueryString["cmd"];
                var tokens = cmd.Split(' ');

                var proc = new Process { 
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = tokens[0],
                        Arguments = (tokens.Length <2) ? null : String.Join(" ", tokens.Skip(1)),
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                proc.Start();
                while (!proc.StandardOutput.EndOfStream)
                {
                    sb.AppendLine(proc.StandardOutput.ReadLine());                    
                }
                rtnValue = sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
                rtnValue = "ERROR";
            }
            return rtnValue;
        }


        public static string GPIOResponse(HttpListenerRequest request)
        {
            GPIOMem gpio = null;
            string rtnValue = null;
            try
            {
                // log out requests for debugging
                Console.WriteLine(request.Url);

                // set pin direction via "dir" argument
                GPIODirection dir = (GPIODirection)((new[] { "OUT", "TRUE", "1" }).Contains(request.QueryString["dir"].ToUpper()) ? 1 : 0);

                // set pin number via "pin" argument
                GPIOPins pin = (GPIOPins)int.Parse(request.QueryString["pin"]);

                gpio = new GPIOMem((GPIOPins)pin, dir);
                
                if (dir == GPIODirection.Out)
                {
                    // set pin state via "state" argument
                    bool state = (new[] { "HIGH", "1", "TRUE", "T" }).Contains(request.QueryString["state"].ToUpper());

                    gpio.Write(state);
                    rtnValue= state.ToString();
                }
                else
                {
                    rtnValue= (gpio.Read() == PinState.High).ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);                
                Console.WriteLine(ex.StackTrace);
                rtnValue= "ERROR";
            }
            finally
            {
                gpio.Dispose();
            }

            return rtnValue;
        }
        
    }
}
