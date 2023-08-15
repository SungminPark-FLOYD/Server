using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ServerCore;

namespace Server
{
    class Packet
    {
        public ushort size;
        public ushort packetId;

    }
    class GameSession : PacketSession
    {
        
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            Packet packet = new Packet() { size = 100, packetId = 10 };          

            ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
            byte[] buffer = BitConverter.GetBytes(packet.size);
            byte[] buffer2 = BitConverter.GetBytes(packet.packetId);
            Array.Copy(buffer, 0, openSegment.Array, openSegment.Offset, buffer.Length);
            Array.Copy(buffer2, 0, openSegment.Array, openSegment.Offset + buffer.Length, buffer2.Length);
            ArraySegment<byte> sendBuff = SendBufferHelper.Close(buffer.Length + buffer2.Length);

            Send(sendBuff);

            Thread.Sleep(1000);

            Disconnect();
        }
        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + 2);
            Console.WriteLine($"RecvPacketId: {id}, Size { size}");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnSend(int numOfByte)
        {
            Console.WriteLine($"Transferred bytes : {numOfByte}");
        }
    }
    class Program
    {
        //문지기
        static Listener _listener = new Listener();

        #region Session 이전의 처리방식
        //static void OnAcceptHandler(Socket clientSocket)
        //{
        //    try
        //    {
        //        GameSession session = new GameSession();
        //        session.Start(clientSocket);

        //        byte[] senBuff = Encoding.UTF8.GetBytes("Welcom to MMORPG Server!");
        //        session.Send(senBuff);

        //        Thread.Sleep(1000);

        //        session.Disconnect();

        //        ////받는다
        //        //byte[] recvBuff = new byte[1024];
        //        //int recvBytes = clientSocket.Receive(recvBuff);
        //        //string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
        //        //Console.WriteLine($"[From Client] {recvData}");

        //        ////보낸다
        //        //byte[] senBuff = Encoding.UTF8.GetBytes("Welcom to MMORPG Server!");
        //        //clientSocket.Send(senBuff);

        //        ////쫒아낸다
        //        //clientSocket.Shutdown(SocketShutdown.Both);
        //        //clientSocket.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }            
        //}
        #endregion
        static void Main(string[] args)
        {
            //DNS -> 도메인 주소를 통해서 ip주소 찾기
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            //GameSession만들기
            _listener.Init(endPoint, () => { return new GameSession(); });
            Console.WriteLine("Listening...");
            while (true)
            {
                //손님입장
                //Socket clientSocket = _listener.Accept();
                ;
            }

        }
    }

}