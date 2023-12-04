using System;
using System.Net;

namespace DTAClient.Domain.Multiplayer.LAN;

public class LANLobbyUser
{
    public LANLobbyUser(string name, IPEndPoint endPoint)
    {
        Name = name;
        EndPoint = endPoint;
    }

    public string Name { get; private set; }
    public IPEndPoint EndPoint { get; private set; }
    public TimeSpan TimeWithoutRefresh { get; set; }
}