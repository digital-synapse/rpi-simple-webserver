using RaspberryPiDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace rpi_gpio_webserver
{
    public class GpioService : IRequestHandler
    {
        public string RequestListener(HttpListenerRequest request)
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
                    rtnValue = state.ToString();
                }
                else
                {
                    rtnValue = (gpio.Read() == PinState.High).ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
                rtnValue = "ERROR";
            }
            finally
            {
                gpio.Dispose();
            }

            return rtnValue;
        }
    }
}
