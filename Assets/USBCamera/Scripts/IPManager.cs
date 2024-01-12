using System.Net;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ChaosIkaros
{
    public class IPManager
    {
        public static string GetIPList()
        {
            string output = "";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    output += ip.ToString() + ",";
            }
            return output;
        }
    }
}