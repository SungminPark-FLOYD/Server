using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DummyClient
{
    class Packet
    {
        public ushort size;
        public ushort packetId;

    }

    class PlayerInfoReq : Packet
    {
        public long playerId;
    }

    class PlayerInfoOk : Packet
    {
        public int hp;
        public int attack;
    }

    public enum PacketID
    {
        PlayerInfoReq = 1,
        PlayerInfoOk = 2,   
    }

    class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            PlayerInfoReq packet = new PlayerInfoReq() { size = 4, packetId = (ushort)PacketID.PlayerInfoReq, playerId = 1001};

            //for (int i = 0; i < 5; i++)
            {
                //보낸다
                ArraySegment<byte> s = SendBufferHelper.Open(4096);

                bool success = true;
                //반드시 ushort버전으로 넣어야 2바이트가 입력된다
                ushort count = 0;

                //success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset, s.Count), packet.size);
                count += 2;
                success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), packet.packetId);
                count += 2;
                success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), packet.playerId);
                count += 8;

                success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset, s.Count), count);
                //이전버전
                {
                    //byte[] size = BitConverter.GetBytes(packet.size);
                    //byte[] packetId = BitConverter.GetBytes(packet.packetId);
                    //byte[] playerId = BitConverter.GetBytes(packet.playerId);

                    ////데이터 크기만큼 카운트 증가       
                    //Array.Copy(size, 0, s.Array, s.Offset + count, 2);
                    //count += 2;
                    //Array.Copy(packetId, 0, s.Array, s.Offset + count, 2);
                    //count += 2;
                    //Array.Copy(playerId, 0, s.Array, s.Offset + count, 8);
                    //count += 8;
                }

                ArraySegment<byte> sendBuff = SendBufferHelper.Close(count);

                if(success)
                    Send(sendBuff);
            }
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Server] {recvData}");
            return buffer.Count;
        }

        public override void OnSend(int numOfByte)
        {
            Console.WriteLine($"Transferred bytes : {numOfByte}");
        }
    }
}
