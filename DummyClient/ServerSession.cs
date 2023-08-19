using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DummyClient
{
    //class PlayerInfoReq
    //{
    //    public long playerId;
    //    public string name;

    //    public struct SkillInfo
    //    {
    //        public int id;
    //        public short level;
    //        public float duration;

    //        public bool Write(Span<byte> s, ref ushort count)
    //        {
    //            bool success = true;
    //            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), id);
    //            count += sizeof(int);
    //            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), level);
    //            count += sizeof(short);
    //            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), duration);
    //            count += sizeof(float);

    //            return success;

    //        }

    //        public void Read(ReadOnlySpan<byte> s, ref ushort count)
    //        {
    //            id = BitConverter.ToInt32(s.Slice(count, s.Length - count));
    //            count += sizeof(int);
    //            level = BitConverter.ToInt16(s.Slice(count, s.Length - count));
    //            count += sizeof(short);
    //            duration = BitConverter.ToSingle(s.Slice(count, s.Length - count));
    //            count += sizeof(float);
    //        }
    //    }

    //    public List<SkillInfo> skills = new List<SkillInfo>();

    //    public void Read(ArraySegment<byte> segmnet)
    //    {
    //        ushort count = 0;

    //        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segmnet.Array, segmnet.Offset, segmnet.Count);

    //        //ushort size = BitConverter.ToUInt16(s.Array, s.Offset);
    //        count += sizeof(ushort);
    //        //ushort id = BitConverter.ToUInt16(s.Array, s.Offset + count);
    //        count += sizeof(ushort);
    //        //범위를 초과하는 값을 적용하려하면 에러가나는 안전한버전
    //        this.playerId = BitConverter.ToInt64(s.Slice(count, s.Length - count));
    //        count += sizeof(long);

    //        //string 추출
    //        ushort nameLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
    //        count += sizeof(ushort);
    //        this.name = Encoding.Unicode.GetString(s.Slice(count, nameLen));
    //        count += nameLen;

    //        //skill list
    //        skills.Clear();
    //        ushort skillLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
    //        count += sizeof(ushort);
    //        for(int i = 0; i < skillLen; i++)
    //        {
    //            SkillInfo skill = new SkillInfo();
    //            skill.Read(s, ref count);
    //            skills.Add(skill);
    //        }

    //    }

    //public ArraySegment<byte> Write()
    //    {
    //        //보낸다
    //        ArraySegment<byte> segment = SendBufferHelper.Open(4096);

    //        //반드시 ushort버전으로 넣어야 2바이트가 입력된다
    //        ushort count = 0;
    //        bool success = true;

    //        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);

    //        //success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset, s.Count), packet.size);
    //        count += sizeof(ushort);
    //        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)PacketID.PlayerInfoReq);
    //        count += sizeof(ushort);
    //        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.playerId);
    //        count += sizeof(long);   

    //        //SendBufferHelper 할당 공간에 문자열 복사하기
    //        ushort nameLen = (ushort)Encoding.Unicode.GetBytes(this.name, 0, this.name.Length, segment.Array, segment.Offset + count + sizeof(ushort));
    //        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), nameLen);
    //        count += sizeof(ushort);
    //        count += nameLen;

    //        //skill list
    //        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)skills.Count);
    //        count += sizeof(ushort);

    //        foreach(SkillInfo skill in skills)
    //            success &= skill.Write(s, ref count);


    //        success &= BitConverter.TryWriteBytes(s, count);

    //        if (success == false)
    //            return null;

    //        //이전버전
    //        {
    //            //byte[] size = BitConverter.GetBytes(packet.size);
    //            //byte[] packetId = BitConverter.GetBytes(packet.packetId);
    //            //byte[] playerId = BitConverter.GetBytes(packet.playerId);

    //            ////데이터 크기만큼 카운트 증가       
    //            //Array.Copy(size, 0, s.Array, s.Offset + count, 2);
    //            //count += 2;
    //            //Array.Copy(packetId, 0, s.Array, s.Offset + count, 2);
    //            //count += 2;
    //            //Array.Copy(playerId, 0, s.Array, s.Offset + count, 8);
    //            //count += 8;
    //        }

    //        return SendBufferHelper.Close(count);
    //    }
    //}


    //public enum PacketID
    //{
    //    PlayerInfoReq = 1,
    //    PlayerInfoOk = 2,   
    //}

    class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            C_PlayerInfoReq packet = new C_PlayerInfoReq() { playerId = 1001, name = "ABCD"};
            var skill = new C_PlayerInfoReq.Skill() { id = 101, level = 1, duration = 3.0f };
            skill.attributes.Add(new C_PlayerInfoReq.Skill.Attribute() { att = 77 });
            packet.skills.Add(skill);

            packet.skills.Add(new C_PlayerInfoReq.Skill() { id = 201, level = 2, duration = 4.0f });
            packet.skills.Add(new C_PlayerInfoReq.Skill() { id = 301, level = 3, duration = 5.0f });
            packet.skills.Add(new C_PlayerInfoReq.Skill() { id = 401, level = 4, duration = 6.0f });

            //for (int i = 0; i < 5; i++)
            {
                //보낸다
                ArraySegment<byte> s = packet.Write();
                 
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

                if(s != null)
                    Send(s);
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
