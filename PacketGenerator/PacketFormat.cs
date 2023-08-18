using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketGenerator
{
    internal class PacketFormat
    {
        //{0}이 패킷 이름
        //{1} 맴버 변수들
        //{2} 맴버 변수 Read
        //{2} 맴버 변수 Write
        public static string packetFormat =
 @"

class {0}
{{
    {1}


    public void Read(ArraySegment<byte> segmnet)
    {{
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segmnet.Array, segmnet.Offset, segmnet.Count);

        //ushort size = BitConverter.ToUInt16(s.Array, s.Offset);
        count += sizeof(ushort);
        //ushort id = BitConverter.ToUInt16(s.Array, s.Offset + count);
        count += sizeof(ushort);
        {2}
    }}

    public ArraySegment<byte> Write()
    {{
        //보낸다
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);

        //반드시 ushort버전으로 넣어야 2바이트가 입력된다
        ushort count = 0;
        bool success = true;
            
        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);

        //success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset, s.Count), packet.size);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)PacketID.{0});
        count += sizeof(ushort);
        {3}
        success &= BitConverter.TryWriteBytes(s, count);
        if (success == false)
            return null;    
        return SendBufferHelper.Close(count);
    }}
}}
";
        //{0} 변수 형식
        //{1} 변수 이름
        public static string memberFormat = 
@"public {0} {1}";

        //{0} 변수 이름
        //{1} To 변수 형식
        //{2} 변수 형식
        public static string readFormat =
@" this.{0} = BitConverter.{1}(s.Slice(count, s.Length - count));
count += sizeof({2});";

        //{0} 변수 이름
        public static string readStringFormat =
@"ushort {0}Len = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
count += sizeof(ushort);
this.{0} = Encoding.Unicode.GetString(s.Slice(count, {0}Len));
count += {0}Len;";

        //{0} 변수 이름
        //{1} 변수 형식
        public static string writeFormat =
@"success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.{0});
count += sizeof({1});";

        //{0} 변수 이름
        public static string writeStringFormat =
@"ushort {0}Len = (ushort)Encoding.Unicode.GetBytes(this.{0}, 0, this.{0}.Length, segment.Array, segment.Offset + count + sizeof(ushort));
success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), {0}Len);
count += sizeof(ushort);
count += {0}Len;";
    }
}
