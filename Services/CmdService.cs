using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;

namespace rpi_gpio_webserver
{
    public class CmdService : IRequestHandler
    {
        public string RequestListener(HttpListenerRequest request)
        {
            string rtnValue;
            StringBuilder sb = new StringBuilder();
            try
            {
                var cmd = request.QueryString["cmd"];
                var tokens = cmd.Split(' ');

                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = tokens[0],
                        Arguments = (tokens.Length < 2) ? null : String.Join(" ", tokens.Skip(1)),
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
    }
}
