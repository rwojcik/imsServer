using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(IMSServer.Startup))]

namespace IMSServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Task.Factory.StartNew(StartDiscoveryServer);

            ConfigureAuth(app);

            app.MapSignalR();
        }

        private void StartDiscoveryServer()
        {
            var server = new UdpClient(8888);
            var responseData = Encoding.ASCII.GetBytes("SomeResponseData");

            while (true)
            {
                var clientEp = new IPEndPoint(IPAddress.Any, 0);
                var clientRequestData = server.Receive(ref clientEp);
                var clientRequest = Encoding.ASCII.GetString(clientRequestData);

                Console.WriteLine("Recived {0} from {1}, sending response", clientRequest, clientEp.Address.ToString());
                server.Send(responseData, responseData.Length, clientEp);
            }
        }
    }
}
