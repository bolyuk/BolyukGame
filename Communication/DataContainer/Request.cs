using BolyukGame.Shared;
using System;

namespace BolyukGame.Communication.DataContainer
{
    public class Request : IContainer<RequestType, Request>
    {
        public Guid UserID { get; set; }

        public Request(Guid? LobbyID, Guid? userID = null) : base(LobbyID)
        {
            UserID = userID ?? GameState.Config.UserId;
        }
    }

    public enum RequestType
    {
        ServerSearch = 0,

        //lobby
        Join = 1,
        ColorSelect = 2,

        //Game
        Ready = 3,
        MapGet = 4,
        PosChanged = 5,
    }

}

   

