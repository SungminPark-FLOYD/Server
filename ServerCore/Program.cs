using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerCore
{
    class Program
    {
        //문지기
        static Listener _listener = new Listener();

        static void OnAcceptHandler(Socket clientSocket)
        {
            try
            {
                //받는다
                byte[] recvBuff = new byte[1024];
                int recvBytes = clientSocket.Receive(recvBuff);
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                Console.WriteLine($"[From Client] {recvData}");

                //보낸다
                byte[] senBuff = Encoding.UTF8.GetBytes("Welcom to MMORPG Server!");
                clientSocket.Send(senBuff);

                //쫒아낸다
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }            
        }
        static void Main(string[] args)
        {
            //DNS -> 도메인 주소를 통해서 ip주소 찾기
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);          
            
          
            _listener.Init(endPoint, OnAcceptHandler);
            Console.WriteLine("Listening...");
            while (true)
            {
                //손님입장
                //Socket clientSocket = _listener.Accept();                   
            }
                     
        }
    }
    
}

