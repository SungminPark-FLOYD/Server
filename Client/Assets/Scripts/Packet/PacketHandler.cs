﻿using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PacketHandler
{
    public static void S_BroadcastEnterGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastEnterGame pkt = packet as S_BroadcastEnterGame;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.EnterGame(pkt);

        //if(chatPacket.playerId == 1)
        //Console.WriteLine(chatPacket.chat);

    }

    public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastLeaveGame pkt = packet as S_BroadcastLeaveGame;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.LeaveGame(pkt);
    }

    public static void S_PlayerListHandler(PacketSession session, IPacket packet)
    {
        S_PlayerList pkt = packet as S_PlayerList;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.Add(pkt);
    }

    public static void S_BroadcastMoveHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastMove pkt = packet as S_BroadcastMove;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.Move(pkt);
    }
}
