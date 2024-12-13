using BolyukGame.Shared;
using System;

namespace BolyukGame.Communication.DataContainer
{
    public abstract class IContainer<T, K> where T : Enum
        where K : IContainer<T,K>
    {
        public T Type { get; set; }

        public Guid? LobbyID { get; set; }

        public byte[] Body { get; set; }

        public IContainer(Guid? LobbyID)
        {
            this.LobbyID = LobbyID;
        }

        public K SetBody<F>(F body)
        {
            Body = ByteUtils.Serialize(body);
            return (K)this;
        }

        public F GetBody<F>()
        {
            return ByteUtils.Deserialize<F>(Body);
        }
    }
}
