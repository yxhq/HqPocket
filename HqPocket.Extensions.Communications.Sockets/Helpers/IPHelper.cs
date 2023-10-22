using System.Net;
using System.Net.Sockets;

namespace HqPocket.Extensions.Communications.Sockets.Helpers
{
    public static class IPHelper
    {
        public static IEnumerable<IPAddress> GetLocalIPAddresses()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(a => a.AddressFamily is AddressFamily.InterNetwork);
        }

        public static string[] GetLocalIPAddressStrings()
        {
            return GetLocalIPAddresses().Select(ip => ip.ToString()).ToArray();
        }
    }
}
