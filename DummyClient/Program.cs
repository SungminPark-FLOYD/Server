using ServerCore;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyClient
{  

    class Program
    {
        static void Main(string[] args)
        {
            //DNS -> 도메인 주소를 통해서 ip주소 찾기
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            Connecter connecter = new Connecter();

            connecter.Connect(endPoint, () => { return new ServerSession(); });

            while(true)
            {
               
                try
                {
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Thread.Sleep(100);
            }
                       
        }
    }
}