using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Xml;

namespace PacketGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlReaderSettings settings = new XmlReaderSettings()
            {
                IgnoreComments = true,
                IgnoreWhitespace = true
            };

            //using을 사용해서 해당 범위 벗어나면 종료
            using (XmlReader r = XmlReader.Create("PDL.xml", settings))
            {
                r.MoveToContent();

               while(r.Read())
                {
                    if (r.Depth == 1 && r.NodeType == XmlNodeType.Element)
                        ParsePacket(r);

                    //Console.WriteLine(r.Name + " " + r["name"]);
                }
            }
        }

        public static void ParsePacket(XmlReader r)
        {
            if (r.NodeType == XmlNodeType.Element)
                return;

            //다 소문자로 변환해서 파일과 다르면 return
            if (r.Name.ToLower() != "packet")
            {
                Console.WriteLine("Invalid packet node");
                return;
            }
                

            string packetName = r["name"];
            if (string.IsNullOrEmpty(packetName))
            {
                Console.WriteLine("Packet without name");
                return;
            }

            ParseMembers(r);  

        }
        
        public static void ParseMembers(XmlReader r)
        {
            string packetName = r["name"];

            //파싱할 곳의 정보
            int depth = r.Depth + 1;
            while (r.Read())
            {
                if (r.Depth != depth)
                    break;

                string memberNmae = r["name"];
                if (string.IsNullOrEmpty(memberNmae))
                {
                    Console.WriteLine("Memver without name");
                    return;
                }

                string memberType = r.Name.ToLower();
                switch (memberType)
                {
                    case "bool":
                    case "byte":
                    case "short":
                    case "ushort":
                    case "int":
                    case "long":
                    case "float":
                    case "double":
                    case "string":
                    case "list":
                        break;
                    default:
                        break;
                }
            }
        }
    }
}