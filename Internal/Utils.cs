using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace rpi_gpio_webserver
{
    public class Utils
    {
        private static string getLocalIPv4(NetworkInterfaceType? _type = null)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if ((_type == null || item.NetworkInterfaceType == _type) && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }

        public static string GetLocalIPv4()
        {
            var ip = getLocalIPv4(NetworkInterfaceType.Ethernet);
            if (string.IsNullOrEmpty(ip))            
                ip = getLocalIPv4(NetworkInterfaceType.Wireless80211);
            if (string.IsNullOrEmpty(ip))
                ip = getLocalIPv4();

            return ip;
        }

        public static string FormatRoute(string route)
        {
            if (route.StartsWith("/")) route = route.Substring(1);
            if (route.EndsWith("/")) route = route.Substring(0, route.Length - 1);
            //if (!route.StartsWith("/")) route = "/" + route;
            //if (!route.EndsWith("/")) route = route + "/";
            return route;
        }
    }
}
