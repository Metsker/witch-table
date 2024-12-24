using System.Linq;
using Unity.Netcode;

namespace _Project.__Scripts.Core.WitchCard.Entities.Players.State
{
    public struct TurnInfo : INetworkSerializable
    {
        public int ActiveIndex;
        public int PassiveIndex;
        public int MaxPicks;

        public ulong ActiveId => NetworkManager.Singleton.ConnectedClients.Keys.ToList()[ActiveIndex];
        public ulong PassiveId => NetworkManager.Singleton.ConnectedClients.Keys.ToList()[PassiveIndex];
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ActiveIndex);
            serializer.SerializeValue(ref PassiveIndex);
        }
    }
}
