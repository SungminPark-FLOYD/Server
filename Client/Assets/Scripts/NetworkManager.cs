using DummyClient;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    ServerSession _session = new ServerSession();

    public void Send(ArraySegment<byte> sendBuff)
    {
        _session.Send(sendBuff);
    }
    
    void Start()
    {
        //DNS -> ������ �ּҸ� ���ؼ� ip�ּ� ã��
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        Connecter connecter = new Connecter();

        connecter.Connect(endPoint,
            () => { return _session; },
            1);       
    }

   
    void Update()
    {
        //�ѹ��� ��Ŷ ó��
        List<IPacket> list = PacketQueue.Instance.PopAll();
        foreach(IPacket packet in list)
            PacketManager.Instance.HandlePacket(_session, packet);

        ////��Ŷ�� ���� ������� �Űܼ� ó���ϵ��� �����
        //IPacket packet = PacketQueue.Instance.Pop();
        //if(packet != null)
        //{
        //    PacketManager.Instance.HandlePacket(_session, packet);
        //}
    }    
}
